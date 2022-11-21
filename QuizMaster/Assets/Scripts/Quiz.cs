using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO[] questions;
    QuestionSO question;

    [Header("Answer")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly;

    [Header("Buttons")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    Timer timer;

    [Header("ScoreKeeper")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scorekeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;

    int currentQuestionIndex;


    void Start()
    {
        isComplete = false;
        timer = FindObjectOfType<Timer>();
        scorekeeper = FindObjectOfType<ScoreKeeper>();

        progressBar.maxValue = questions.Length;
        progressBar.value = 0;

        currentQuestionIndex = -1;
        GetNextQuestion();
    }

    private void Update()
    {
        if (timer.loadNextQuestion)
        {
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswers(-1);
        }
    }

    public void OnAnswerSelected(int index)
    {
        Debug.Log("Button clicked");
        hasAnsweredEarly = true;
        timer.CancelTimer();
        if (currentQuestionIndex >= questions.Length)
        {
            Debug.Log("Game Over! No more questions. (OAS)");
            SetButtonState(false);
            timer.DisableTimer();
        }
        else
        {
            DisplayAnswers(index);
            SetButtonState(false);
        }
        scoreText.text = "Score: " + scorekeeper.CalculateScore() + "%";
    }

    void GetNextQuestion()
    {
        if (progressBar.value == progressBar.maxValue)
        {
            isComplete = true;
        }
        currentQuestionIndex++;
        timer.isAnsweringQuestion = true;
        SetButtonState(true);
        SetDefaultButtonSprites();
        DisplayQuestion();
        progressBar.value++;
        scorekeeper.IncrementQuestionsSeen();
    }

    void DisplayQuestion()
    {
        Debug.Log("Displaying Question index " + currentQuestionIndex);
        hasAnsweredEarly = false;
        if (currentQuestionIndex >= questions.Length)
        {
            Debug.Log("Game over!");
            SetButtonState(false);
            timer.DisableTimer();
        }
        else
        {
            question = questions[currentQuestionIndex];
            questionText.text = question.GetQuestionText();
            correctAnswerIndex = question.GetCorrectIndex();

            for (int i = 0; i < answerButtons.Length; i++)
            {
                TextMeshProUGUI buttonTextMeshProUGUI = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonTextMeshProUGUI.text = question.GetAnswerChoice(i);
            }
        }
    }

    void DisplayAnswers(int index)
    {
        Image buttonImage;

        question = questions[currentQuestionIndex];

        if (index == question.GetCorrectIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scorekeeper.IncrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = question.GetCorrectIndex();
            questionText.text = "Incorrect, the answer was \n" + question.GetAnswerChoice(correctAnswerIndex);
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
