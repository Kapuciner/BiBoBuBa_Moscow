using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public GameObject VictoryPanel;
    public Image image;

    private float delay = 1f;
    private bool canExit = false;
    public void FinishGame(Player winner)
    {
        VictoryPanel.SetActive(true);
        Time.timeScale = 1;
        image.sprite = winner.VictoryScreen;
        StartCoroutine(Delay());
    }

    private void Update()
    {
        if (canExit && Input.anyKey && VictoryPanel.activeSelf)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(delay);
        canExit = true;
    }
}
