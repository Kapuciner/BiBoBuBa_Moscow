using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerArena : MonoBehaviour
{
    private int currentCountdown = 5;
    [SerializeField] int maxCountdown = 5;
    [SerializeField] private GameObject[] players;
    private List<Vector3> startPositions = new List<Vector3>();
    [SerializeField] private TMP_Text[] scoreCounters;
    private static List<int> winCounts = new List<int>() { 0, 0, 0, 0 };

    public TMP_Text startTXT;

    [SerializeField] private int maxWinNum = 4;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text playerNicknameTXT;

    bool gameOver = false;

    private void Awake()
    {
      Physics.defaultMaxDepenetrationVelocity = 20;
    }
    private void Start()
    {


        currentCountdown = maxCountdown;
        Time.timeScale = 0;
        StartCoroutine(StartGame());

        for (int i = 0; i < players.Length; i++)
        {
            startPositions.Add(players[i].transform.position);
        }

        for (int i = 0; i < players.Length; i++)
        {
            scoreCounters[i].text = $"{winCounts[i]}";
        }
    }

    void Update()
    {

    }

    IEnumerator StartGame()
    {

        while (currentCountdown > 0)
        {
            startTXT.text = currentCountdown.ToString();
            yield return new WaitForSecondsRealtime(1);
            this.GetComponent<AudioSource>().Play();
            currentCountdown--;
        }

        startTXT.text = "Go!";
        yield return new WaitForSecondsRealtime(1);

        Time.timeScale = 1; 
        startTXT.text = "";
    }

    public void CheckIfRoundEnd()
    {
        int count = 0;
        int winnerID = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<ArenaPlayerManager>().dead == true)
            {
                count++;
            }
            else
            {
                winnerID = i;
            }
        }

        if (players.Length - count == 1)
        {
            winCounts[winnerID]++;

            for (int i = 0; i < winCounts.Count; i++)
            {
                if (winCounts[i] >= maxWinNum)
                {
                    StartCoroutine(EndGame(winnerID));
                }
            }

            if (!gameOver)
                Invoke("Restart", 0.5f);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator EndGame(int winnerID)
    {
        gameOver = true;
        endScreen.SetActive(true);
        playerNicknameTXT.text = $"{players[winnerID].GetComponent<ArenaPlayerManager>().nickname} WON!";

        for (int i = 0; i < winCounts.Count; i++)
        {
            winCounts[i] = 0;
        }

        yield return new WaitForSeconds(1);

        while (true)
        {
            if (Input.anyKey && gameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
