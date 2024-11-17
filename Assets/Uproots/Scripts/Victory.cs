using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public GameObject VictoryPanel;
    public Image image;

    private float delay = 1f;
    private bool canExit = false;

    public Sprite P1win;
    public Sprite P2win;
    public void FinishGame(R_Player winner)
    {
        VictoryPanel.SetActive(true);
        Time.timeScale = 1;

        Sprite screen;
        screen = FindObjectOfType<UI_Manager>().GetVictoryScreen(winner.GetIndex());
        
        image.sprite = screen;
        StartCoroutine(Delay());
    }

    private void Update()
    {
        if (canExit && Input.anyKey && VictoryPanel.activeSelf)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(delay);
        canExit = true;
    }
}
