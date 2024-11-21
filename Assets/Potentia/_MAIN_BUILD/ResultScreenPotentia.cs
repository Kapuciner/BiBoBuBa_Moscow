using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ResultScreenPotentia : MonoBehaviour
{
    [SerializeField] private Image[] playersSpries;
    [SerializeField] private List<int> typesList = new List<int> ();
    [SerializeField] private GameManager gm;
    [SerializeField] TMP_Text vsTXT;
    [SerializeField] TMP_Text powerUPStxt;
    [SerializeField] TMP_Text startTXT;

    public int magesCount = 0;
    public int cloudsCount = 0;

    [SerializeField] private GameObject UIcanva;
    [SerializeField] private GameObject[] bg;
    [SerializeField] private GameObject[] labels;
    [SerializeField] private GameObject[] crosses;

    [SerializeField] TMP_Text resultTXT;
    [SerializeField] TMP_Text pressButtonTXT;
    private void OnEnable()
    {
        if (gm.gameOver)
        {
            vsTXT.enabled = false;
            powerUPStxt.enabled = false;
            startTXT.enabled = false;
            UIcanva.SetActive(false);
            StartCoroutine(GameOverResults());
        }
    }

    IEnumerator GameOverResults()
    {
        for (int i = 0; i < playersSpries.Length; i++)
        {
            playersSpries[i].gameObject.GetComponent<Animator>().SetInteger("playerID", i);
            playersSpries[i].gameObject.GetComponent<Animator>().SetInteger("type", typesList[i]);
        }

        resultTXT.text = "Результат:";
        yield return new WaitForSeconds(1);
        int winType = 0;
        if (gm.mageWon)
        {
            winType = 1;
            string whoWon = " Победа за стороной магов";
            for (int i = 0; i < whoWon.Length; i++)
            {
                resultTXT.text += whoWon[i];
                yield return new WaitForSeconds(0.25f);
            }
        }
        if (gm.cloudWon)
        {
            winType = 2;
            string whoWon = " Рок победил";
            for (int i = 0; i < whoWon.Length; i++)
            {
                resultTXT.text += whoWon[i];
                yield return new WaitForSeconds(0.25f);
            }
        }
        yield return new WaitForSeconds(1);

        StartCoroutine(FallGuys());
        for (int i = 0; i < 4; i++)
        {
            if (typesList[i] == winType)
                StartCoroutine(RotateWinner(i));
        }
        yield return new WaitForSeconds(1.5f);

        pressButtonTXT.text = "Нажмите любую кнопку, чтобы вернуться в лобби";
        yield return new WaitUntil(() => Input.anyKey);
        SceneManager.LoadScene(0);
    }

    public void StartScreen()
    {
        StartCoroutine(GameStartScreen());
    }

    IEnumerator GameStartScreen()
    {
        for (int i = 0; i < 4; i++)
        {
            typesList.Add(playersSpries[i].gameObject.GetComponent<Animator>().GetInteger("type"));
            crosses[i].SetActive(true);
        }
        for (int i = 0; i < bg.Length; i++)
        {
            bg[i].SetActive(true);
        }
        for (int i = 0; i < gm.readyCount; i++)
        {
            playersSpries[i].enabled = true;
            crosses[i].SetActive(false);
            labels[i].SetActive(true);
        }

        string vs = $"{magesCount} VS {cloudsCount}";

        for (int i = 0; i < vs.Length; i++)
        {
            vsTXT.text += vs[i];
            if (i != 1 && i != 4)
                yield return new WaitForSeconds(0.75f);
        }

        if (cloudsCount <= magesCount - 1)
        {
            powerUPStxt.text = "Облака получают усиление: УРОН X2";
        }
        else if (cloudsCount < magesCount)
        {
            powerUPStxt.text = "Облака получают усиление: УРОН X3";
        }
        else if (magesCount <= cloudsCount - 1)
        {
            powerUPStxt.text = "Маги получают усиление: СКОРОСТЬ +50%";
        }
        else if (magesCount < cloudsCount )
        {
            powerUPStxt.text = "Маги получают усиление: СКОРОСТЬ +100%";
        }
        else
        {
            powerUPStxt.text = "Силы сторон равны.";
        }

        yield return new WaitForSeconds(2f);

        startTXT.text = "Начинаем игру";
        yield return new WaitForSeconds(0.75f);
        startTXT.text = "Начинаем игру.";
        yield return new WaitForSeconds(0.75f);
        startTXT.text = "Начинаем игру..";
        yield return new WaitForSeconds(0.75f);
        startTXT.text = "Начинаем игру...";
        yield return new WaitForSeconds(0.25f);
        this.gameObject.SetActive(false);
        UIcanva.SetActive(true);

        gm.SpawnPlayers();
        if (FindObjectOfType<pauseManager>() != null)
            FindObjectOfType<pauseManager>().canPause = true;

    }

    IEnumerator FallGuys()
    {
        float currentFallSpeed = 50;

        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                if (gm.cloudWon && typesList[i] == 1)
                {
                    playersSpries[i].gameObject.GetComponent<RectTransform>().anchoredPosition += Vector2.down * currentFallSpeed * Time.unscaledDeltaTime;
                    labels[i].gameObject.SetActive(false);

                    if (playersSpries[i].gameObject.GetComponent<RectTransform>().anchoredPosition.y < -Screen.height)
                    {
                        playersSpries[i].gameObject.SetActive(false);
                    }
                }
                else if (gm.mageWon && typesList[i] == 2)
                {
                    playersSpries[i].gameObject.GetComponent<RectTransform>().anchoredPosition += Vector2.down * currentFallSpeed * Time.unscaledDeltaTime;
                    labels[i].gameObject.SetActive(false);

                    if (playersSpries[i].gameObject.GetComponent<RectTransform>().anchoredPosition.y < -Screen.height)
                    {
                        playersSpries[i].gameObject.SetActive(false);
                    }
                }
            }

            currentFallSpeed += 3;

            yield return null;
        }
    }

    IEnumerator RotateWinner(int id)
    {
        while (true)
        {
            playersSpries[id].transform.Rotate(new Vector3(0, 180, 0));
            yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.6f));
        }
    }
}
