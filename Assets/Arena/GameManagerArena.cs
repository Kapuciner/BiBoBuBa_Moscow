using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameManagerArena : MonoBehaviour
{
    [SerializeField] int maxCountdown = 5;
    public List<GameObject> playersList = new List<GameObject>();
    [SerializeField] private TMP_Text[] scoreCounters;
    public static List<int> winCounts = new List<int>() { 0, 0, 0, 0 };
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private ZoneNewShrinking zone;

    [SerializeField] private int maxWinNum = 4;
    [SerializeField] private TMP_Text playerNicknameTXT;
    static public int winnerID = 0;

    public bool gameOver = false;

    [SerializeField] private GameObject[] readyCrosses;
    [SerializeField] private Sprite[] labels;

    [SerializeField] private GameObject[] playerAbilitiesUI;
    [SerializeField] private List<Image> abilityImage1; //первый скилл
    [SerializeField] private List<Image> abilityImage2; //второй скилл
    [SerializeField] private List<TMP_Text> skillName1;
    [SerializeField] private List<TMP_Text> skillName2;
    [SerializeField] private List<Slider> hpBar;
    [SerializeField] private List<TMP_Text> hpTXT;
    [SerializeField] private List<TMP_Text> readyTXT;
    [SerializeField] private TMP_Text timerForReady;
    [SerializeField] private GameObject[] crosses;
    [SerializeField] private GameObject[] fireUI;


    [SerializeField] private ConnectionData _connectionData;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private CameraScaler cameraScaler;

    public static int playersReady = 0;
    [SerializeField] private GameObject canvaREADY;
    [SerializeField] private GameObject canvaUI;
    [SerializeField] private Animator[] animators;
    [SerializeField] private GameObject resultScreen;
    bool firstgame = true;

    [SerializeField] private GameObject mobileUICanva;
    [SerializeField] private Button mobileHitButton;
    [SerializeField] private Button mobileCastButton;
    private void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            mobileUICanva.SetActive(true);
            canvaREADY.SetActive(false);
            StartCoroutine(StartGame());
        }
        
        Physics.defaultMaxDepenetrationVelocity = 20;
        SpawnPlayers();


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
            canvaREADY.SetActive(false);
            canvaUI.SetActive(true);
            resultScreen.SetActive(true);
            if (FindObjectOfType<pauseManager>() != null)
                FindObjectOfType<pauseManager>().canPause = true;
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
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].Update(Time.unscaledDeltaTime);
            }
        }
    }

    IEnumerator whileNotStarted() //this code is running while readyCanva is open
    {
        float timer = 3f;
        while (true)
        {
            if (playersReady == playersList.Count)
            {
                if (timer > 0)
                {
                    timer -= Time.unscaledDeltaTime;
                    timerForReady.text = $"{Mathf.Round(timer)}";
                }
                else
                {
                    StartCoroutine(StartGame());
                    if (FindObjectOfType<pauseManager>() != null)
                        FindObjectOfType<pauseManager>().canPause = true;
                    break;
                }
            }
            else
            {
                timer = 3f;
            }

            timerForReady.text = $"{Mathf.Round(timer)}";
            yield return new WaitForEndOfFrame();
        }
    }
    private void SpawnPlayers()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            OnMobile();
        }
        else
        {
            OnOneComputer();
        }
    }

    private void OnMobile()
    {
        GameObject pl = Instantiate(playerPrefab, spawnPoints[playersList.Count].transform.position, playerPrefab.transform.rotation);
        pl.GetComponent<ArenaPlayerManager>().playerID = playersList.Count;
        AddPlayer(pl);
        mobileHitButton.onClick.RemoveAllListeners();
        mobileCastButton.onClick.RemoveAllListeners();
        mobileHitButton.onClick.AddListener(pl.GetComponent<ArenaPlayerManager>().Hit);
        mobileCastButton.onClick.AddListener(pl.GetComponent<ArenaPlayerManager>().Cast);
    }

    private void OnOneComputer()
    {
        bool firstKeyboardTaken = false;
        var players = _connectionData.ConnectedPlayersData();

        if (_connectionData.GetPlayerCount() > 0) //пока не проверено, но должно работать между сценами
        {
            int j = 0;
            foreach (var player in players)
            {
                GameObject pl = Instantiate(playerPrefab, spawnPoints[playersList.Count].transform.position, playerPrefab.transform.rotation);
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

                switch (j)
                {
                    case 1: 
                        pl.GetComponent<ArenaPlayerManager>().nickname = "Синий";
                        break;
                    case 2:
                        pl.GetComponent<ArenaPlayerManager>().nickname = "Красный";
                        break;
                    case 3:
                        pl.GetComponent<ArenaPlayerManager>().nickname = "Жёлтый";
                        break;
                    case 4:
                        pl.GetComponent<ArenaPlayerManager>().nickname = "Зелёный";
                        break;
                }
            }
        }
        else //по умолчанию (если без переключения между сценами) DEFAULT (IF YOU START BY NOT ENTERING LOBBY, E.G. WITH UNITY
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    GameObject pl = Instantiate(playerPrefab, spawnPoints[playersList.Count].transform.position, playerPrefab.transform.rotation);
                    pl.GetComponent<ArenaPlayerManager>().playerID = i % 2; // change later to pl.GetComponent<ArenaPlayerManager>().playerID = i;.
                    AddPlayer(pl);
                    var playerInput = pl.GetComponent<PlayerInput>();

                    playerInput.SwitchCurrentControlScheme("Keyboard1", Keyboard.current);
                    pl.GetComponent<ArenaPlayerManager>().nickname = "Синий";
                }
                else if (i == 1)
                {
                    GameObject pl = Instantiate(playerPrefab, spawnPoints[playersList.Count].transform.position, playerPrefab.transform.rotation);
                    pl.GetComponent<ArenaPlayerManager>().playerID = i % 2; // change later to pl.GetComponent<ArenaPlayerManager>().playerID = i;
                    AddPlayer(pl);
                    var playerInput = pl.GetComponent<PlayerInput>();

                    playerInput.SwitchCurrentControlScheme("Keyboard2", Keyboard.current);
                    pl.GetComponent<ArenaPlayerManager>().nickname = "Красный";
                }
                else if (i == 2) // Третий игрок - геймпад 1
                {
                    var gamepad1 = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
                    if (gamepad1 != null)
                    {
                        GameObject pl = Instantiate(playerPrefab, spawnPoints[playersList.Count].transform.position, playerPrefab.transform.rotation);
                        pl.GetComponent<ArenaPlayerManager>().playerID = 2; 
                        AddPlayer(pl);
                        var playerInput = pl.GetComponent<PlayerInput>();

                        playerInput.SwitchCurrentControlScheme("Gamepad", gamepad1);
                        pl.GetComponent<ArenaPlayerManager>().nickname = "Жёлтый";
                    }
                    else
                    {
                        Debug.LogWarning("Геймпад 1 не подключён!");
                    }
                }
                else if (i == 3) // Четвёртый игрок - геймпад 2
                {
                    var gamepad2 = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;
                    if (gamepad2 != null)
                    {
                        GameObject pl = Instantiate(playerPrefab, spawnPoints[playersList.Count].transform.position, playerPrefab.transform.rotation);
                        pl.GetComponent<ArenaPlayerManager>().playerID = 3; // change later to pl.GetComponent<ArenaPlayerManager>().playerID = i;
                        AddPlayer(pl);
                        var playerInput = pl.GetComponent<PlayerInput>();

                        playerInput.SwitchCurrentControlScheme("Gamepad", gamepad2);
                        pl.GetComponent<ArenaPlayerManager>().nickname = "Зелёный";
                    }
                    else
                    {
                        Debug.LogWarning("Геймпад 2 не подключён!");
                    }
                }


            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < playersList.Count)
            {
                readyCrosses[i].SetActive(false);
            }
        }
    }
    public IEnumerator StartGame()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < playersList.Count)
            {
                if (i > cameraScaler.Players.Count - 1)
                    cameraScaler.Players.Add(playersList[i]);
                else
                    cameraScaler.Players[i] = playersList[i];
            }
        }

        canvaREADY.SetActive(false);
        canvaUI.SetActive(true);
        Time.timeScale = 1;
        zone.StartShrinking();
        yield return null;
    }



    public void CheckIfRoundEnd()
    {
        int count = 0;
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
        Time.timeScale = 0;
        for (int i = 0; i < 4; i++)
        {
            if (i < scoreCounters.Length)
                scoreCounters[i].text = $"{winCounts[i]}";
        }

        gameOver = true;
        resultScreen.SetActive(true);
        playerNicknameTXT.text = $"{playersList[winnerID].GetComponent<ArenaPlayerManager>().nickname} победил!";


        yield return new WaitForSeconds(5f);

        for (int i = 0; i < winCounts.Count; i++)
        {
            winCounts[i] = 0;
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
        player.GetComponent<ArenaPlayerManager>().cross = crosses[playersList.Count - 1];
        player.GetComponent<ArenaPlayerManager>().fireUI = fireUI[playersList.Count - 1];
        player.GetComponent<ArenaPlayerManager>().label.sprite = labels[playersList.Count - 1];
        scoreCounters[playersList.Count - 1].gameObject.SetActive(true);
    }

    void ShowUI(int playerID)
    {
        playerAbilitiesUI[playerID].SetActive(true);
        skillName1[playerID].gameObject.SetActive(true);
        skillName2[playerID].gameObject.SetActive(true);
        hpBar[playerID].gameObject.SetActive(true);
        hpTXT[playerID].gameObject.SetActive(true);
    }

    public int getPlayerCount()
    {
        return playersList.Count;
    }

    public int getAlivePlayerCount()
    {
        int count = 0;
        for (int i = 0; i < playersList.Count; i++)
        {
            if (playersList[i].GetComponent<ArenaPlayerManager>().dead == false)
            {
                count++;
            }
        }

        return count;
    }
    public void ChangeCameraTarget(GameObject deadPlayer)
    {
        for (int i = 0; i < playersList.Count; i++)
        {
            if (playersList[i] != deadPlayer)
            {
                cameraScaler.Players[deadPlayer.GetComponent<ArenaPlayerManager>().playerID] = playersList[i];
                print(deadPlayer.GetComponent<ArenaPlayerManager>().playerID);
                break;
            }
        }
    }

}
