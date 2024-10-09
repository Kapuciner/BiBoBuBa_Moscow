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

    public R_Player P1;
    public R_Player P2;

    public Image check1;
    public Image check2;

    public Sprite checked1;
    public Sprite checked2;
    public Sprite uncheck;

    public void OnRestartPressed()
    {
        if (choose.activeSelf)
        {
            return;
        }
        restart.SetActive(!restart.activeSelf);
    }

    public void OnAttackPressed(int playerIndex)
    {
        if (!restart.activeSelf)
        {
            return;
        }
        if (playerIndex == 0)
        {
            if (check1.sprite == uncheck)
            {
                check1.sprite = checked1;
            }
            else check1.sprite = uncheck;
        }
        if (playerIndex == 1)
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
