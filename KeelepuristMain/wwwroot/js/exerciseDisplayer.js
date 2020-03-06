let blankSpaces = document.getElementsByTagName("input");

for (bs of blankSpaces) {
    let firstCorrectAnswerLength = bs.name.split("|")[0].length;
    bs.setAttribute("size", (firstCorrectAnswerLength > 4 ? firstCorrectAnswerLength.toString() : "4"))
}
