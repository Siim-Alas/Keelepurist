
function checkAnswers() {
    let blankSpaces = document.getElementsByTagName("input");

    for (bs of blankSpaces) {
        let correctAnswers = bs.name.split("|");
        bs.classList.add(correctAnswers.includes(bs.value) ? "answer-correct" : "answer-wrong");
    }
}

function resetBlankSpace(id) {
    let blankSpace = document.getElementById(id);
    blankSpace.classList.remove("answer-correct", "answer-wrong");
}
