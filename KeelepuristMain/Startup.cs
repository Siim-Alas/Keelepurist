using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Azure;
using KeelepuristMain.Services;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Security.Principal;
using System.Security.Claims;
using System.Net;
using Newtonsoft.Json.Linq;

namespace KeelepuristMain
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                    .AddAzureAD(options => Configuration.Bind("AzureAd", options))
                    .AddCookie();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin-only", p => {
                    p.RequireClaim("groups", "493a2009-e41c-4395-8270-175e59e83efc");
                });
            });

            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Admin", "admin-only");
                    options.Conventions.AuthorizeFolder("/TestExercises");
                });

            services.AddSingleton<IAzureStorageService, AzureStorageService>();
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(Configuration["ConnectionStrings:DefaultEndpointsProtocol=https;AccountName=keelepuristdata;AccountKey=z5hPZ7XrLm2W0U6oKWI3+FsQPysaSDFPVTbW+Kgh4z2h1UCMxSHaQRYzLEi+rnss8ZbBcgSGp9wTlmy8be83hg==;EndpointSuffix=core.windows.net"]);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            //TODO: look if an official way to polulate ClaimsPrincipal has been released.
            // Current code from https://stackoverflow.com/questions/42260708/azure-apps-easyauth-claims-with-net-core/42270669#42270669
            app.Use(async (context, next) =>
            {
                // Create a user on current thread from provided header
                if (context.Request.Headers.ContainsKey("X-MS-CLIENT-PRINCIPAL-ID"))
                {
                    // Read headers from Azure
                    var azureAppServicePrincipalIdHeader = context.Request.Headers["X-MS-CLIENT-PRINCIPAL-ID"][0];
                    var azureAppServicePrincipalNameHeader = context.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"][0];

                    #region extract claims via call /.auth/me
                    //invoke /.auth/me
                    var cookieContainer = new CookieContainer();
                    HttpClientHandler handler = new HttpClientHandler()
                    {
                        CookieContainer = cookieContainer
                    };
                    string uriString = $"{context.Request.Scheme}://{context.Request.Host}";
                    foreach (var c in context.Request.Cookies)
                    {
                        cookieContainer.Add(new Uri(uriString), new Cookie(c.Key, c.Value));
                    }
                    string jsonResult = string.Empty;
                    using (HttpClient client = new HttpClient(handler))
                    {
                        var res = await client.GetAsync($"{uriString}/.auth/me");
                        jsonResult = await res.Content.ReadAsStringAsync();
                    }

                    //parse json
                    var obj = JArray.Parse(jsonResult);
                    string user_id = obj[0]["user_id"].Value<string>(); //user_id

                    // Create claims id
                    List<Claim> claims = new List<Claim>();
                    foreach (var claim in obj[0]["user_claims"])
                    {
                        claims.Add(new Claim(claim["typ"].ToString(), claim["val"].ToString()));
                    }

                    // Set user in current context as claims principal
                    var identity = new GenericIdentity(azureAppServicePrincipalIdHeader);
                    identity.AddClaims(claims);
                    #endregion

                    // Set current thread user to identity
                    context.User = new GenericPrincipal(identity, null);
                };

                await next.Invoke();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
