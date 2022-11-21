using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToCompleteQuestion = 15f;
    [SerializeField] float timeToShowCorrectAnswer = 5f;

    [SerializeField] Image timerImage;

    public bool loadNextQuestion = false;
    public bool isAnsweringQuestion = false;

    float timerValue;
    float fillFraction;

    bool timerEnabled = true;

    void Start()
    {
        loadNextQuestion = false;
        isAnsweringQuestion = false;
        timerEnabled = true;
        timerValue = timeToCompleteQuestion;
    }

    void Update()
    {
        UpdateTimer();
        UpdateTimerImage();
    }

    public void CancelTimer()
    {
        timerValue = 0;
    }

    public void DisableTimer()
    {
        timerValue = 0;
        timerEnabled = false;
        loadNextQuestion = false;
        isAnsweringQuestion = false;
    }

    void UpdateTimer()
    {
        if (timerEnabled)
        {
            timerValue -= Time.deltaTime;

            if (isAnsweringQuestion)
            {
                if (timerValue <= 0)
                {
                    isAnsweringQuestion = false;
                    timerValue = timeToShowCorrectAnswer;
                }
            }
            else
            {
                if (timerValue <= 0)
                {
                    isAnsweringQuestion = true;
                    timerValue = timeToCompleteQuestion;
                    loadNextQuestion = true;
                }
            }
        }
    }
    void UpdateTimerImage()
    {
        fillFraction = isAnsweringQuestion ? (timerValue / timeToCompleteQuestion) : (timerValue / timeToShowCorrectAnswer);
        timerImage.fillAmount = fillFraction;
    }
}
