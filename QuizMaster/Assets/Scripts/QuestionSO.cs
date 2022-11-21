using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuizQuestion", fileName = "newQuestion")]

public class QuestionSO : ScriptableObject
{
    [TextArea(1, 6)]
    [SerializeField] private string question = "Enter new question text here";
    [SerializeField] private string[] answerChoices = new string[4];
    [SerializeField] private int correctIndex = 0;

    public string GetQuestionText()
    {
        return question;
    }

    public string GetAnswerChoice(int index)
    {
        return answerChoices[index];
    }

    public int GetCorrectIndex()
    {
        return correctIndex;
    }
}
