
function appendPlusSignsToAnswers(numOfBlanks){
    for (i = 0; i < numOfBlanks; i++) {
        document.getElementById(`UserAnswers_${i}_`).value += "+";
    }
}