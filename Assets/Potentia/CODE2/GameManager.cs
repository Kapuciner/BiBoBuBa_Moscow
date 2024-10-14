using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameTimer timer;
    [SerializeField] private Animator txt;
    [SerializeField] private ConnectionData _connectionData;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject magePrefab;
    [SerializeField] private PlayerManager mageManagerScript;

    [SerializeField] private GameObject dashImage;

    [SerializeField] private TMP_Text dashCooldownTimer;
    [SerializeField] private TMP_Text attackCooldownTimer;
    [SerializeField] private TMP_Text shieldCooldownTimer;
    [SerializeField] private GameObject shieldImage;
    [SerializeField] private GameObject attackImage;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Animator gameOverTXT;
    [SerializeField] public TMP_Text livesCounter;
    [SerializeField] public GameObject readyCanvas;
    [SerializeField] public GameObject mainCanvas;

    [SerializeField] public TMP_Text mageReadyText;
    [SerializeField] public TMP_Text cloudReadyText;
    [SerializeField] public GameObject[] hearts;  

    [SerializeField] public Falliant[] faliants;
    [SerializeField] private GameObject cloudPrefab;

    [SerializeField] CloudTargetScript cloudTarget;

    [SerializeField] CloudInterface cloudInterface;
    [SerializeField] CameraScaler cameraScaler;

    public bool mageReady = false;

    public bool cloudReady = false;
    public bool gameOver = false;
    public bool gameStarted = false;
    public bool gameCanStart = false;

    public ReadyTimer readyTimer;
    public bool mageWon = false;
    public CloudScript cs;
    public TMP_Text startTXT;
    private void Awake()
    {
        SpawnPlayers();
        //Time.timeScale = 0;
        cs.canMove = false;
        
        StartCoroutine(StartGame());
        StartCoroutine(StartGameAdditionalTimer());
    }

    private void SpawnPlayers()
    {
        bool firstKeyboardTaken = false;
        var players = _connectionData.ConnectedPlayersData();

        GameObject tempPlayer = Instantiate(magePrefab, spawnPoints[0].transform.position, magePrefab.transform.rotation);
        AddMage(tempPlayer);

        var playerInput = tempPlayer.GetComponent<PlayerInput>();
        if (players[0].Device is Keyboard && firstKeyboardTaken == false)
        {
            playerInput.SwitchCurrentControlScheme("Keyboard1", Keyboard.current);
            firstKeyboardTaken = true;
        }
        else if (players[0].Device is Keyboard && firstKeyboardTaken == true)
        {
            playerInput.SwitchCurrentControlScheme("Keyboard2", Keyboard.current);
        }
        else
        {
            playerInput.SwitchCurrentControlScheme("GamePad", Gamepad.current);
        }

        GameObject tempCs = Instantiate(cloudPrefab, spawnPoints[1].transform.position, cloudPrefab.transform.rotation);
        AddCloud(tempCs);

        playerInput = tempCs.GetComponent<PlayerInput>();
        if (players[1].Device is Keyboard && firstKeyboardTaken == false)
        {
            print("CLOUD KEY1");
            playerInput.SwitchCurrentControlScheme("Mouse", Mouse.current);
            firstKeyboardTaken = true;
        }
        else if (players[1].Device is Keyboard && firstKeyboardTaken == true)
        {
            print("CLOUD KEY2");
            playerInput.SwitchCurrentControlScheme("Mouse", Mouse.current);
        }
        else
        {
            print("CLOUD GAMEPAD");
            playerInput.SwitchCurrentControlScheme("GamePad", Gamepad.current);
        }
    }

    public void AddMage(GameObject mage)
    {
        PlayerManager mageManager = mage.GetComponent<PlayerManager>();
        mageManager.dashImage = dashImage; 
        mageManagerScript = mageManager;
        mageManager.dashCooldownTimer = dashCooldownTimer;
        mageManager.attackCooldownTimer = attackCooldownTimer;
        mageManager.shieldCooldownTimer = shieldCooldownTimer;
        mageManager.shieldCooldownTimer = shieldCooldownTimer;

        mageManager.shieldImage = shieldImage;
        mageManager.attackImage = attackImage;

        player magePlayer = mage.GetComponent<player>();
        magePlayer.gm = this;
        magePlayer.gameOverScreen = gameOverScreen;
        magePlayer.gameOverTXT = gameOverTXT;
        magePlayer.livesCounter = livesCounter;
        magePlayer.hearts = hearts;
        magePlayer.start = spawnPoints[0];

        cameraScaler.Player1 = mage;

        foreach (Falliant faliant in faliants) {
            faliant.playerManager = mageManager;
            faliant.player = mageManager.transform;
        }

    }

    public void AddCloud(GameObject cloud)
    {
        
        CloudScript cloudScript = cloud.GetComponent<CloudScript>();
        cs = cloudScript;
        cloudScript.gm = this;
        cloudScript.cloudTarget = cloudTarget;
        cloudScript.cloudInterface = cloudInterface;
        cloudInterface.clousScript = cs;
        if(_connectionData.ConnectedPlayersData()[1].Device is Gamepad) cloudScript.isMouse = false;

        cameraScaler.Player2 = cloud;
    }

    IEnumerator StartGame()
    {
        while(!gameCanStart) {
            if(mageReady) mageReadyText.text = "Готов";
            else mageReadyText.text = "Не готов";

            if(cloudReady) cloudReadyText.text = "Готов";
            else cloudReadyText.text = "Не готов";
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 0;
        mageManagerScript.canMove = true;
        cs.canCast = true;
        readyCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        int countdown = 5; // ������ �� 3 �������

        while (countdown > 0)
        {
            startTXT.text = countdown.ToString(); 
            yield return new WaitForSecondsRealtime(1);
            this.GetComponent<AudioSource>().Play();
            countdown--; 
        }

        startTXT.text = "Go!"; // ��������� � ������ ����
        yield return new WaitForSecondsRealtime(1); // �������� �������������� �������, ���� ����� �������� ��������� "Go!"

        Time.timeScale = 1; // ������� ���� � �����
        cs.canMove = true; // ��������� ��������
        startTXT.text = ""; // ������� ����� �������
    }

    IEnumerator StartGameAdditionalTimer()
    {
        
        while(!gameCanStart) {
            if (mageReady && cloudReady && readyTimer.gameObject.activeSelf == false) {
                readyTimer.gameObject.SetActive(true);
                yield return new WaitForEndOfFrame();
            } else if (!(mageReady && cloudReady)) {
                readyTimer.gameObject.SetActive(false);
                yield return new WaitForEndOfFrame();
            } else {
                yield return new WaitForEndOfFrame();
            }
        }  
        
    }

    public void MageWon()
    {
        winPanel.SetActive(true);
        txt.Play("TxTappear");
        mageWon = true;
        cs.canMove = false;
        timer.StopTimer();
    }

    private void Update()
    {
        if (gameOver || mageWon)
        {
            if (Input.GetKeyDown(KeyCode.R))
                RestartGame();
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
