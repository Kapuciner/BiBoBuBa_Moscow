using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using Microsoft.Unity.VisualStudio.Editor;

public class ActionTimerCircle : MonoBehaviour
{
    // public float timeRemaining = 300f; 
    float timeRemaining = 0; 
    public TMP_Text timerText;
    public UnityEngine.UI.Image circle;

    private bool timerIsRunning = false;

    void Start()
    {

    }

    public void StartTimer(float newTime, int numberToDisplay) {
        timerText.text = numberToDisplay.ToString();
        timerIsRunning = true;
        timeRemaining = newTime;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                circle.fillAmount = timeRemaining / 1;
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay();
                transform.gameObject.SetActive(false);
            }
        }
        transform.LookAt(Camera.main.transform.position);
    } 

    public void StopTimer()
    {
        timerIsRunning = false;
    }
    void UpdateTimerDisplay()
    {
        
    }

}
