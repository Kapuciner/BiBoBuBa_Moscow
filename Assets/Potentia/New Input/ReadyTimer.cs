using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ReadyTimer : MonoBehaviour
{
    public float timeRemaining = 3; 
    public float timeOnStart = 4; 
    public TMP_Text timerText;
    public GameManager gameManager;

    void Start()
    {
        timeRemaining = timeOnStart;
        timerText.text = Math.Floor(timeRemaining).ToString();
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = Math.Floor(timeRemaining).ToString();
        }
        else
        {
            timerText.text = "0";
            gameManager.gameCanStart = true;
        }
    }
}