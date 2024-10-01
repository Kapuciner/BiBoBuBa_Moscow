using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArenaTimer : MonoBehaviour
{
    public float timeRemaining = 300f;
    public TMP_Text timerText;

    private bool timerIsRunning = false;

    void Start()
    {
        // ������ �������
        timerIsRunning = true;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

