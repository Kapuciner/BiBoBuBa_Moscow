using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArenaTimer : MonoBehaviour
{
    float timePassed = 0;
    public TMP_Text timerText;

    private bool timerIsRunning = false;

    void Start()
    {
        // Запуск таймера
        timerIsRunning = true;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            timePassed += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timePassed / 60);
        int seconds = Mathf.FloorToInt(timePassed % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

