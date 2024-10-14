using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameManagerArena : MonoBehaviour
{
    private int currentCountdown = 5;
    [SerializeField] int maxCountdown = 5;
    private List<GameObject> playersList = new List<GameObject>();
    [SerializeField] private TMP_Text[] scoreCounters;
    public static List<int> winCounts = new List<int>() { 0, 0, 0, 0 };
    [SerializeField] private Transform[] spawnPoints;

    public TMP_Text startTXT;

    [SerializeField] private int maxWinNum = 4;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text playerNicknameTXT;

    bool gameOver = false;

    [SerializeField] private GameObject[] playerAbilitiesUI;
    [SerializeField] private List<Image> abilityImage1; //первый скилл
    [SerializeField] private List<Image> abilityImage2; //второй скилл
    [SerializeField] private List<TMP_Text> skillName1;
    [SerializeField] private List<TMP_Text> skillName2;
    [SerializeField] private List<Slider> hpBar;
    [SerializeField] private List<TMP_Text> hpTXT;
    [SerializeField] private List<TMP_Text> readyTXT;


    [SerializeField] private ConnectionData _connectionData;
    [SerializeField] private GameObject[] playerPrefab;

    [SerializeField] private CameraScaler cameraScaler;

    public static int playersReady = 0;
    [SerializeField] private GameObject canvaREADY;
    [SerializeField] private GameObject canvaUI;
    [SerializeField] private Animator animator1;
    [SerializeField] private Animator animator2;
    bool firstgame = true;

    private void Start()
    {
        Physics.defaultMaxDepenetrationVelocity = 20;
        SpawnPlayers();

        for (int i = 0; i < playersList.Count; i++)
        {
            if (i == 0)   //камера пока что таргетит только двоих игроков 
                cameraScaler.Player1 = playersList[i];
            else if (i == 1)
                cameraScaler.Player2 = playersList[i];
        }

        currentCountdown = maxCountdown;
        Time.timeScale = 0;

        firstgame = true;
        for (int i = 0; i < 4; i++)
        {
            if (i < scoreCounters.Length)
                scoreCounters[i].text = $"{winCounts[i]}";
            if (winCounts[i] > 0)
                firstgame = false;
        }

        if (!firstgame)
        {
            StartCoroutine(StartGame());
        }
        else
        {
            playersReady = 0;
            StartCoroutine(whileNotStarted());
        }
    }

    private void Update()
    {
        if (canvaREADY.activeSelf)
        {
            animator1.Update(Time.unscaledDeltaTime);
            animator2.Update(Time.unscaledDeltaTime);
        }
    }

    IEnumerator whileNotStarted()
    {
        while (true)
        {
            if (playersReady == playersList.Count)
            {
                StartCoroutine(StartGame());
                if (FindObjectOfType<pauseManager>() != null)
                    FindObjectOfType<pauseManager>().canPause = true;
                break;
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
    private void SpawnPlayers()
    {
        bool firstKeyboardTaken = false;
        var players = _connectionData.ConnectedPlayersData();

        if (_connectionData.GetPlayerCount() > 0) //пока не проверено, но должно работать между сценами
        {
            int j = 0;
            foreach (var player in players)
            {
                GameObject pl = Instantiate(playerPrefab[j], spawnPoints[playersList.Count].transform.position, playerPrefab[j].transform.rotation);
                j++;
                pl.GetComponent<ArenaPlayerManager>().playerID = playersList.Count;
                AddPlayer(pl);
                var playerInput = pl.GetComponent<PlayerInput>();

                if (player.Device is Keyboard && firstKeyboardTaken == false)
                {
                    playerInput.SwitchCurrentControlScheme("Keyboard1", player.Device);
                    firstKeyboardTaken = true;
                }
                else if (player.Device is Keyboard && firstKeyboardTaken == true)
                {
                    playerInput.SwitchCurrentControlScheme("Keyboard2", player.Device);
                }
                else
                {
                    playerInput.SwitchCurrentControlScheme("GamePad", player.Device);
                    print("shouldWork");
                }

            }
        }
        else //по умолчанию (если без переключения между сценами)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject pl = Instantiate(playerPrefab[i], spawnPoints[playersList.Count].transform.position, playerPrefab[i].transform.rotation);
                pl.GetComponent<ArenaPlayerManager>().playerID = playersList.Count;
                AddPlayer(pl);
                var playerInput = pl.GetComponent<PlayerInput>();

                if (i == 0)
                {
                    playerInput.SwitchCurrentControlScheme("Keyboard1", Keyboard.current);
                }
                else if (i == 1)
                {
                    playerInput.SwitchCurrentControlScheme("Keyboard2", Keyboard.current);
                }
                //else if (i == 1)
                //{
                //  playerInput.SwitchCurrentControlScheme("GamePad", Gamepad.current);
                //}

                // для теста геймпада, поменять последний if на тот, что выше


            }
        }
        
    }

    IEnumerator StartGame()
    {
        canvaREADY.SetActive(false);
        canvaUI.SetActive(true);

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
        for (int i = 0; i < playersList.Count; i++)
        {
            if (playersList[i].GetComponent<ArenaPlayerManager>().dead == true)
            {
                count++;
            }
            else
            {
                winnerID = i;
            }
        }

        if (playersList.Count - count == 1)
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
                Invoke("Restart", 1f);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator EndGame(int winnerID)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < scoreCounters.Length)
                scoreCounters[i].text = $"{winCounts[i]}";
        }

        gameOver = true;
        endScreen.SetActive(true);
        playerNicknameTXT.text = $"{playersList[winnerID].GetComponent<ArenaPlayerManager>().nickname} победил!";

        for (int i = 0; i < winCounts.Count; i++)
        {
            winCounts[i] = 0;
        }

        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            if (Input.anyKey && gameOver)
            {
                SceneManager.LoadScene(0); //переход в меню (нужно сделать в buildSettings)
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void AddPlayer(GameObject player)
    {
        playersList.Add(player);
        player.transform.position = spawnPoints[playersList.Count - 1].transform.position;
        ShowUI(playersList.Count - 1);
        player.GetComponent<ArenaPlayerManager>().abilityImage.Add(abilityImage1[playersList.Count - 1]); 
        player.GetComponent<ArenaPlayerManager>().abilityImage.Add(abilityImage2[playersList.Count - 1]); 
        player.GetComponent<ArenaPlayerManager>().skillName.Add(skillName1[playersList.Count - 1]);
        player.GetComponent<ArenaPlayerManager>().skillName.Add(skillName2[playersList.Count - 1]);
        player.GetComponent<ArenaPlayerManager>().hpBar = hpBar[playersList.Count - 1];
        player.GetComponent<ArenaPlayerManager>().hpTXT = hpTXT[playersList.Count - 1];
        player.GetComponent<ArenaPlayerManager>().readyTXT = readyTXT[playersList.Count - 1];
    }

    void ShowUI(int playerID)
    {
        playerAbilitiesUI[playerID].SetActive(true);
        skillName1[playerID].gameObject.SetActive(true);
        skillName2[playerID].gameObject.SetActive(true);
        hpBar[playerID].gameObject.SetActive(true);
        hpTXT[playerID].gameObject.SetActive(true);
    }
}
