
function checkAnswers() {
    let blankSpaces = document.getElementsByTagName("input");

    for (bs of blankSpaces) {
        let correctAnswers = bs.name.split("|");
        bs.classList.add(correctAnswers.includes(bs.value) ? "text-info" : "text-danger");
    }
}

function resetBlankSpace(id) {
    let blankSpace = document.getElementById(id);
    blankSpace.classList.remove("text-info", "text-danger");
}
