using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameTimer timer;
    [SerializeField] private Animator txt;
    [SerializeField] private ConnectionData _connectionData;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject magePrefab;
    [SerializeField] private GameObject cloudPrefab;

    private List<GameObject> players = new List<GameObject>() ;
    public List<GameObject> mages = new List<GameObject>() ;
    public List<GameObject> clouds = new List<GameObject>() ;
    public GameObject[] magesUI;
    [SerializeField] private GameObject[] cloudsUI;
    [SerializeField] private GameObject[] readyCrosses;
    [SerializeField] private Sprite[] labels;
    [SerializeField] private GameObject[] dashImage;
    [SerializeField] private GameObject[] shieldImage;
    [SerializeField] private GameObject[] attackImage;
    [SerializeField] private GameObject[] hpImage;
    [SerializeField] private TMP_Text[] dashCooldownTimer;
    [SerializeField] private TMP_Text[] attackCooldownTimer;
    [SerializeField] private TMP_Text[] shieldCooldownTimer;
    [SerializeField] public TMP_Text[] livesCounter;
    [SerializeField] public GameObject[] heartsPlayer1;  
    [SerializeField] public GameObject[] heartsPlayer2;  
    [SerializeField] public GameObject[] heartsPlayer3;  
    [SerializeField] public GameObject[] heartsPlayer4; 
    private List<GameObject[]> hearts;
    [SerializeField] private TMP_Text[] faliantCounters;
    [SerializeField] private GameObject[] deathCrosses;

    [SerializeField] public Image[] emptyEnergyPlayer1;
    [SerializeField] public Image[] emptyEnergyPlayer2;
    [SerializeField] public Image[] emptyEnergyPlayer3;
    [SerializeField] public Image[] emptyEnergyPlayer4;
    private List<Image[]> allEnergyPlayers;

    [SerializeField] CloudTargetScript[] cloudTarget;
    [SerializeField] CloudInterface[] cloudInterface;

    [SerializeField] public Falliant[] faliants;
    [SerializeField] CameraScaler cameraScaler;

    public bool gameOver = false;
    public bool gameStarted = false;
    public bool gameCanStart = false;


    [SerializeField] private GameObject readyCanvas;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject resultCanvas;


    public bool playersChosen = false;
    public bool mageWon = false;
    public bool cloudWon = false;
    public CloudScript[] cs;

    //----------------------
    [SerializeField] private List<GameObject> inputs = new List<GameObject>();
    [SerializeField] private GameObject inputPrefab;
    public int readyCount = 0;
    public int readyMagesCount = 0;
    public int readyCloudsCount = 0;
    public TMP_Text timerText;

    [SerializeField] private TMP_Text allFaliantsCounter;
    public static int faliantsCarriedAmount = 0;
    static public int maxFaliantsNeeded;
    [SerializeField] private MagesWin winAnimationSequence;

    [SerializeField] private GameTimer commonTimer;
    [SerializeField] private GameObject Goal;
    [SerializeField] private MeshRenderer BG;
    [SerializeField] private TMP_Text warning;
    int deadCount = 0;
    private void Awake()
    {
        SpawnInputsForChoice();
        
        StartCoroutine(StartGame());
    }

    private void Start()
    {
        Cursor.visible = false;
        hearts = new List<GameObject[]> {heartsPlayer1, heartsPlayer2, heartsPlayer3, heartsPlayer4};
        allEnergyPlayers = new List<Image[]> {emptyEnergyPlayer1, emptyEnergyPlayer2, emptyEnergyPlayer3, emptyEnergyPlayer4};
        faliantsCarriedAmount = 0;
        maxFaliantsNeeded = 0;
        BG.sortingOrder = -4;
    }

    void SpawnInputsForChoice()
    {
        bool firstKeyboardTaken = false;
        var players = _connectionData.ConnectedPlayersData();

        if (_connectionData.GetPlayerCount() > 0) 
        {
            int j = 0;
            foreach (var player in players)
            {
                GameObject inp = Instantiate(inputPrefab);
                inputs.Add(inp);

                var playerInput = inp.GetComponent<PlayerInput>();
                inp.GetComponent<CanvaReadyControl>().playerID = j;

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
                }

                j++;
            }
        }
        else //по умолчанию (если без переключения между сценами) DEFAULT (IF YOU START BY NOT ENTERING LOBBY/
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    GameObject inp = Instantiate(inputPrefab);
                    inp.GetComponent<CanvaReadyControl>().playerID = i;
                    inputs.Add(inp);
                    var playerInput = inp.GetComponent<PlayerInput>();
                    playerInput.SwitchCurrentControlScheme("Keyboard1", Keyboard.current);
                }
                else if (i == 1)
                {
                    GameObject inp = Instantiate(inputPrefab);
                    inp.GetComponent<CanvaReadyControl>().playerID = i;
                    inputs.Add(inp);
                    var playerInput = inp.GetComponent<PlayerInput>();
                    playerInput.SwitchCurrentControlScheme("Keyboard2", Keyboard.current);
                }
                else if (i == 2) // Третий игрок - геймпад 1
                {
                    var gamepad1 = Gamepad.all.Count > 0 ? Gamepad.all[0] : null;
                    if (gamepad1 != null)
                    {
                        GameObject inp = Instantiate(inputPrefab);
                        inp.GetComponent<CanvaReadyControl>().playerID = i;
                        inputs.Add(inp);
                        var playerInput = inp.GetComponent<PlayerInput>();
                        playerInput.SwitchCurrentControlScheme("Gamepad", gamepad1);
                    }
                }
                else if (i == 3) // Четвёртый игрок - геймпад 2
                {
                    var gamepad2 = Gamepad.all.Count > 1 ? Gamepad.all[1] : null;
                    if (gamepad2 != null)
                    {
                        GameObject inp = Instantiate(inputPrefab);
                        inp.GetComponent<CanvaReadyControl>().playerID = i;
                        inputs.Add(inp);
                        var playerInput = inp.GetComponent<PlayerInput>();
                        playerInput.SwitchCurrentControlScheme("Gamepad", gamepad2);
                    }
                }
            }
        }

        //здесь настроить UI в зависимости от устройств
        // сделать 3 версии объяснения управления в потенции? если только клава, если только геймпады и если оба

        for (int i = 0; i < 4; i++)
        {
            if (i < inputs.Count)
            {
                readyCrosses[i].SetActive(false);
            }
        }
    }

    public void SpawnPlayers()
    {

        for (int i = 0; i < inputs.Count; i++)
        {
            GameObject tempPlayer;

            if (inputs[i].gameObject.GetComponent<CanvaReadyControl>().chosenCharacter == "mage")
            {
                tempPlayer = Instantiate(magePrefab, spawnPoints[i].transform.position, magePrefab.transform.rotation);
                PlayerManager pm = tempPlayer.GetComponent<PlayerManager>();
                pm.playerID = i;
                pm.label.sprite = labels[i];
                if (readyCloudsCount == 3)
                    pm.GetSecondBonus();
                if (readyCloudsCount == 2 && readyMagesCount == 1)
                    pm.GetFirstBonus();
                var playerInput = tempPlayer.GetComponent<PlayerInput>();
                AddMage(tempPlayer);
                playerInput.SwitchCurrentControlScheme(inputs[i].GetComponent<PlayerInput>().currentControlScheme, inputs[i].GetComponent<PlayerInput>().devices[0]);
            }
            else //cloud
            {
                tempPlayer = Instantiate(cloudPrefab, spawnPoints[i].transform.position, cloudPrefab.transform.rotation);
                CloudScript cls = tempPlayer.GetComponent<CloudScript>();
                cls.playerID = i;
                cls._animator.SetInteger("playerID", i);
                cls.label.sprite = labels[i];
                if (readyMagesCount == 3)
                    cls.GetSecondBonus();
                if (readyMagesCount == 2 && readyCloudsCount == 1)
                    cls.GetFirstBonus();

                var playerInput = tempPlayer.GetComponent<PlayerInput>();
                if (inputs[i].GetComponent<PlayerInput>().currentControlScheme == "GamePad")
                {
                    print(1);
                    tempPlayer.GetComponent<CloudScript>().isMouse = false;//
                }
                AddCloud(tempPlayer);

                if (inputs[i].GetComponent<PlayerInput>().currentControlScheme == "GamePad")
                    playerInput.SwitchCurrentControlScheme(inputs[i].GetComponent<PlayerInput>().currentControlScheme, inputs[i].GetComponent<PlayerInput>().devices[0]);
                else
                    playerInput.SwitchCurrentControlScheme("Mouse", Mouse.current);
            }
        }

        allFaliantsCounter.text = $"{faliantsCarriedAmount} / {maxFaliantsNeeded}";

        foreach (Falliant fal in FindObjectsByType<Falliant>(sortMode: default))
        {
            fal.limitFalliantsAmount(maxFaliantsNeeded);
        }

        for (int i = 0; i < players.Count; i++)
        {
            cameraScaler.Players[i] = players[i];
        }

        for (int i = 0; i < inputs.Count; i++)
        {
            Destroy(inputs[i]);
        }
    }

    public void AddMage(GameObject mage)
    {
        players.Add(mage);
        mages.Add(mage);
        magesUI[players.Count - 1].SetActive(true);
        PlayerManager mageManager = mage.GetComponent<PlayerManager>();
        
        //видимо что-то удалил, хз что это
        //mageManagerScript = mageManager;
        mageManager.dashCooldownTimer = dashCooldownTimer[players.Count - 1];
        mageManager.attackCooldownTimer = attackCooldownTimer[players.Count - 1];
        mageManager.shieldCooldownTimer = shieldCooldownTimer[players.Count - 1];
        mageManager.dashImage = dashImage[players.Count - 1];
        mageManager.shieldImage = shieldImage[players.Count - 1];
        mageManager.attackImage = attackImage[players.Count - 1];
        mageManager.hpImage = hpImage[players.Count - 1];
        mageManager.faliantCounterTXT = faliantCounters[players.Count - 1];
        mageManager.mageEmptyEnergy = allEnergyPlayers[players.Count - 1];

        player magePlayer = mage.GetComponent<player>();
        magePlayer.gm = this;
        magePlayer.livesCounter = livesCounter[players.Count - 1];
        magePlayer.hearts = hearts[players.Count - 1];
        magePlayer.start = spawnPoints[players.Count - 1];
        magePlayer.deathCross = deathCrosses[players.Count - 1];
        if (players.Count >= 3)
            cameraScaler.Players.Add(mage);
        maxFaliantsNeeded += 8;
        
    }

    public void AddCloud(GameObject cloud)
    {
        players.Add(cloud);
        clouds.Add(cloud);
        cloudsUI[players.Count - 1].SetActive(true);
        CloudScript cloudScript = cloud.GetComponent<CloudScript>();
        cs[players.Count - 1] = cloudScript;
        cloudScript.gm = this;
        cloudScript.cloudTarget = cloudTarget[players.Count - 1];
        cloudScript.cloudInterface = cloudInterface[players.Count - 1];
        cloudInterface[players.Count - 1].clousScript = cs[players.Count - 1];

        if (players.Count >= 3)
            cameraScaler.Players.Add(cloud);

    }

    IEnumerator StartGame()
    {
        float timer = 3f;
        while (true) {

            if (readyCount == inputs.Count)
            {
                warning.text = "";
                if (timer > 0)
                {
                    timer -= Time.unscaledDeltaTime;
                    timerText.text = $"{Mathf.Round(timer)}";
                }
                else if (CheckConditions()) 
                {
                    readyCanvas.SetActive(false);
                    ResultScreenPotentia rsp = resultCanvas.GetComponent<ResultScreenPotentia>();
                    rsp.magesCount = readyMagesCount;
                    rsp.cloudsCount = readyCloudsCount;
                    rsp.StartScreen();
                    playersChosen = true;
                    break;
                }
            }
            else
            {
                timer = 3f;
            }

            timerText.text = $"{Mathf.Round(timer)}";
            yield return new WaitForEndOfFrame();
        }

       
    }

    bool CheckConditions()
    {
        if (readyMagesCount == 0)
        {
            warning.text = "В игре должен присутстовать хотя бы один маг!";
            return false;
        }
        else if (readyCloudsCount == 0)
        {
            warning.text = "В игре должна присутстовать хотя бы одна туча!";//
            return false;
        }

        int countKeyboardCloud = 0;
        for (int i = 0; i < inputs.Count; i++)
        {
            if (inputs[i].gameObject.GetComponent<CanvaReadyControl>().chosenCharacter == "cloud" && (inputs[i].GetComponent<PlayerInput>().currentControlScheme == "Keyboard1" || 
                inputs[i].GetComponent<PlayerInput>().currentControlScheme == "Keyboard2"))
            {
                countKeyboardCloud++;
            }
        }

        if (countKeyboardCloud >= 2)
        {
            warning.text = "Только один игрок может играть на туче на 1-ой клавиатуре!";
            return false;
        }
        return true;
    }
    public void RestartCoroutine()
    {
        StartCoroutine(RestartCoroutine_P());
    }
    private IEnumerator RestartCoroutine_P()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateFalliantCounter()
    {
        allFaliantsCounter.text = $"{faliantsCarriedAmount}/{maxFaliantsNeeded}";
        if (faliantsCarriedAmount == maxFaliantsNeeded)
        {
            GameOver("mages");
        }
    }
    public void ChangeCameraTarget(GameObject deadMage)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != deadMage)
            {
                cameraScaler.Players[deadMage.GetComponent<PlayerManager>().playerID] = players[i];
                break;
            }
        }
    }
    public void isGameOver()
    {
        deadCount++;
        if (deadCount == mages.Count)
        {
            cloudWon = true;
            GameOver("clouds");
        }
    }

    public void GameOver(string winner)
    {
        if (gameOver) //because it should work only once
            return;

        for (int i = 0; i < cameraScaler.Players.Count; i++)
        {
            cameraScaler.Players[i] = Goal;
        }

        winAnimationSequence.StartAnimationSequence(winner);

        gameOver = true;
        if (winner == "mages")
            mageWon = true;
        if (winner == "clouds")
            cloudWon = true;

        commonTimer.StopTimer();
    }

    public void MageWon()
    {
        winPanel.SetActive(true);
        txt.Play("TxTappear");
        mageWon = true;
        timer.StopTimer();
        RestartCoroutine();
    }
}
