
function submitFillInBlanks() {
    let blankSpaces = document.getElementsByTagName("input");

    if (blankSpaces.length > 0) {
        let correctAnswersPercentageTag = document.getElementById("correctAnswersPercentage");
        let numOfCorrectAnswers = 0;

        for (bs of blankSpaces) {
            let correctAnswers = bs.name.split("|");
            if (correctAnswers.includes(bs.value)) {
                bs.classList.add("answer-correct");
                numOfCorrectAnswers += 1;
            } else {
                bs.classList.add("answer-wrong");
            }
        }

        correctAnswersPercentageTag.textContent = `${Math.round(100 * (numOfCorrectAnswers / blankSpaces.length))}% õige`
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

        document.addEventListener("keypress", event => {
            if (event.keyCode === 13) {
                document.getElementById("newWordLink").click();
            }
        });
    } else {
        inputTag.classList.add("answer-wrong");
    }

    inputTag.blur();
}