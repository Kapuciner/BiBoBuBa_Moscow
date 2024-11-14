using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultScreenArena : MonoBehaviour
{
    [SerializeField] private GameManagerArena gma;
    [SerializeField] private TMP_Text[] placesText;

    [SerializeField] private GameObject[] playerIMG;

    [SerializeField] private GameObject[] firstPlayersStars;
    [SerializeField] private GameObject[] secondPlayersStars;
    [SerializeField] private GameObject[] thirdPlayersStars;
    [SerializeField] private GameObject[] fourthPlayersStars;
    [SerializeField] private List<GameObject[]> allPlayersStars;

    [SerializeField] private Animator[] animators;

    [SerializeField] private TMP_Text timerTXT;
    [SerializeField] private GameObject whoWonTXT;
    [SerializeField] private GameObject pressAnyButton;
    [SerializeField] private float time = 3;

    private void Awake()
    {
        allPlayersStars = new List<GameObject[]> {firstPlayersStars, secondPlayersStars, thirdPlayersStars, fourthPlayersStars};
    }
    private void OnEnable()
    {
        if (FindObjectOfType<pauseManager>() != null)
            FindObjectOfType<pauseManager>().canPause = false;
        if (gma.gameOver)
            StartCoroutine(ShowEndResult());
        else
            StartCoroutine(ShowResult());
    }

    private void Update()
    {
        animators[0].Update(Time.unscaledDeltaTime);
        animators[1].Update(Time.unscaledDeltaTime);
        animators[2].Update(Time.unscaledDeltaTime);
        animators[3].Update(Time.unscaledDeltaTime);
    }

    IEnumerator ShowResult()
    {
        ShowStars();
        for (int i = 0; i < gma.getPlayerCount(); i++)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            if (i == GameManagerArena.winnerID)
                placesText[i].text = "+1";
            else
                placesText[i].text = "+0";
        }

        yield return new WaitForSecondsRealtime(0.2f);

        //the last star shows up (the new one)
        for (int j = 0; j < GameManagerArena.winCounts[GameManagerArena.winnerID]; j++)
        {
            allPlayersStars[GameManagerArena.winnerID][j].SetActive(true);
        }

        yield return new WaitUntil(() => Input.anyKey);
        StartCoroutine(timerRoutine());
    }


    void ShowStars()
    {
        for (int i = 0; i < gma.getPlayerCount(); i++)
        {
            if (i == GameManagerArena.winnerID)
            {
                for (int j = 0; j < GameManagerArena.winCounts[i] - 1; j++)
                {
                    allPlayersStars[i][j].SetActive(true);
                }
            }
            else
            {
                for (int j = 0; j < GameManagerArena.winCounts[i]; j++)
                {
                    allPlayersStars[i][j].SetActive(true);
                }
            }//
        }
    }

    IEnumerator timerRoutine()
    {
        pressAnyButton.SetActive(false);
        while (true)
        {
            if (time > 0)
            {
                time -= Time.unscaledDeltaTime;
                timerTXT.text = $"{Mathf.Round(time)}";
            }
            else
            {
                StartCoroutine(gma.StartGame());
                if (FindObjectOfType<pauseManager>() != null)
                    FindObjectOfType<pauseManager>().canPause = true;
                this.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator ShowEndResult()
    {
        Clear();

        timerTXT.gameObject.SetActive(false);



        for (int i = 0; i < gma.getPlayerCount(); i++)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            if (i == GameManagerArena.winnerID)
                placesText[i].text = "+1";
            else
                placesText[i].text = "+0";
        }

        for (int i = 0; i < gma.getPlayerCount(); i++)
        {
            for (int j = 0; j < GameManagerArena.winCounts[i]; j++)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                allPlayersStars[i][j].SetActive(true);
            }
        }

        yield return new WaitForSecondsRealtime(1);

        StartCoroutine(RotateWinner());
        StartCoroutine(FallGuys());

        yield return new WaitForSecondsRealtime(2);
        whoWonTXT.SetActive(true);
        pressAnyButton.SetActive(true);

        yield return new WaitUntil(() => Input.anyKey);
        SceneManager.LoadScene(0);
    }

    IEnumerator RotateWinner()
    {
        int i = GameManagerArena.winnerID;

        while (true)
        {
            playerIMG[i].transform.Rotate(new Vector3(0,180,0));
            yield return new WaitForSecondsRealtime(Random.Range(0.3f, 0.6f));
        }
    }

    IEnumerator FallGuys()
    {
        float currentFallSpeed = 50;

        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i != GameManagerArena.winnerID)
                {
                    playerIMG[i].GetComponent<RectTransform>().anchoredPosition += Vector2.down * currentFallSpeed * Time.unscaledDeltaTime;

                    if (playerIMG[i].GetComponent<RectTransform>().anchoredPosition.y < -Screen.height)
                    {
                        playerIMG[i].gameObject.SetActive(false);
                    }
                }
            }

            currentFallSpeed += 3;

            yield return null; 
        }
    }

    void Clear()
    {
        for (int i = 0; i < gma.getPlayerCount(); i++)
        {
            placesText[i].text = $"-";
        }

        for (int i = 0; i < gma.getPlayerCount(); i++)
        {
            for (int j = 0; j < GameManagerArena.winCounts[i]; j++)
            {
                allPlayersStars[i][j].SetActive(false);
            }
        }
    }
}
