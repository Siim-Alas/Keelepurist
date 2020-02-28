
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

function submitDictation() {
    let inputTag = document.getElementById("0");
    let correctAnswers = inputTag.name.split("|");

    if (correctAnswers.includes(inputTag.value)) {
        inputTag.classList.add("answer-correct");
        document.getElementById("newWordLink").classList.replace("btn-light", "btn-primary");
    } else {
        inputTag.classList.add("answer-wrong");
    }
}