let blankSpaces = document.getElementsByTagName("input");

for (bs of blankSpaces) {
    bs.setAttribute("size", bs.name.split("|")[0].length + 1)
}
