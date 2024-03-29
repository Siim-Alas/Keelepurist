﻿
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

        correctAnswersPercentageTag.textContent = `${Math.round(100 * (numOfCorrectAnswers / blankSpaces.length))}% õigesti vastatud.`
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

        setTimeout(r => {
            document.addEventListener("keypress", event => {
                if (event.key === "Enter") {
                    document.getElementById("newWordLink").click();
                }
            });
        }, 250);
    } else {
        inputTag.classList.add("answer-wrong");
    }

    inputTag.blur();
}