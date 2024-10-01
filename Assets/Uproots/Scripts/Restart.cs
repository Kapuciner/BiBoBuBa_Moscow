using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    public GameObject choose;

    public GameObject restart;

    public Player P1;
    public Player P2;

    public Image check1;
    public Image check2;

    public Sprite checked1;
    public Sprite checked2;
    public Sprite uncheck;
    private void Update()
    {
        if (choose.activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            restart.SetActive(!restart.activeSelf);
        }
        if (!restart.activeSelf)
        {
            return;
        }
        if (Input.GetKeyDown(P1.controller.attack))
        {
            if (check1.sprite == uncheck)
            {
                check1.sprite = checked1;
            }
            else check1.sprite = uncheck;
        }
        if (Input.GetKeyDown(P2.controller.attack))
        {
            if (check2.sprite == uncheck)
            {
                check2.sprite = checked2;
            }
            else check2.sprite = uncheck;
        }

        if (check1.sprite == checked1 && check2.sprite == checked2)
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
