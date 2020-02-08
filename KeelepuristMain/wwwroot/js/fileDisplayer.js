
const exerciseLinks = document.getElementById("linkContainer").children;

function displayFiles() {
    for (link of exerciseLinks) {
        let path = link.textContent;

        if (path.includes(".txt")) {
            let rootUlPath = path.split("/").slice(0, -1);
            let rootUl = getOrCreateUlPathById(rootUlPath);

            let li = document.createElement("li");
            let newLink = link.cloneNode();
            newLink.textContent = path.slice(path.lastIndexOf("/"));
            newLink.classList.add("btn", "btn-link", "btn-small");

            li.appendChild(newLink);
            rootUl.appendChild(li);
        }
    }
}

function getOrCreateUlPathById(ulPath) {
    // Input is a string[] containing the path.
    // The whole system goes inside rootUl.

    // Attempts to find the ul path from the DOM.
    let ul = document.getElementById(`${ulPath.join("/")}/`);

    // If a result is found, it gets returned.
    if (ul !== null) {
        return ul;
    }

    // Div doesn't exist so it starts to go up the path given in the argument.
    // For every step in the input path:
    let id = "";
    for (i = 0; i < ulPath.length; i++) {
        // Appends the ID to search for by a step in the input path.
        id += `${ulPath[i]}/`;
        // Attempts to find the path so far from the DOM.
        let newUl = document.getElementById(id);
        // If nothing is found, creates a new ul element with an id matching the path so far.
        if (newUl === null) {
            newUl = document.createElement("ul");
            newUl.id = id;
            newUl.classList.add("file-system");

            // Creates a button to show/hide the ul created.
            let btn = document.createElement("button");
            btn.id = ulPath[i];
            btn.innerHTML = `<i class="material-icons">folder</i>${btn.id}`;
            btn.classList.add("btn", "btn-light", "btn-small");
            btn.addEventListener("click", function () {
                if (newUl.parentNode.style.display === "none") {
                    newUl.parentNode.style.display = "block";
                    btn.innerHTML = `<i class="material-icons">folder_open</i>${btn.id}`;
                }
                else {
                    newUl.parentNode.style.display = "none";
                    btn.innerHTML = `<i class="material-icons">folder</i>${btn.id}`;
                }
            });

            let li1 = document.createElement("li");
            let li2 = document.createElement("li");
            // Appends this div to the last div in the DOM mathcing the input path.
            li1.appendChild(btn);
            li2.appendChild(newUl);
            ul.appendChild(li1);
            ul.appendChild(li2);

            // Hides the new item by default.
            li2.style.display = "none";
        }

        // Sets the root ul to the ul found or created to restart the loop.
        ul = newUl;
    }

    return ul;
}


displayFiles();