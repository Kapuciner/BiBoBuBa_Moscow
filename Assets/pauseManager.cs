using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class pauseManager : MonoBehaviour
{
    private static pauseManager pauseCanvaInstance;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainWindow;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private Button primaryButton;
    [SerializeField] private Button primaryButtonSettings;
    bool inPause = false;
    public bool canPause = false;
    
    private void Awake()
    {
        if (pauseCanvaInstance != null && pauseCanvaInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        pauseCanvaInstance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        print("works");
        pauseMenu.SetActive(false);
        inPause = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pauseMenu.SetActive(false);
        inPause = false;
        canPause = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Left Shoulder"))
        {

            if (canPause)
            {
                inPause = !inPause;
                pauseMenu.SetActive(inPause);
                if (pauseMenu.activeSelf)
                    primaryButton.Select();
                
                mainWindow.SetActive(true);
                settingsWindow.SetActive(false);
            }
        }
    }

    public void showSettings()
    {
        primaryButtonSettings.Select();
        settingsWindow.SetActive(true);
        mainWindow.SetActive(false);
    }

    public void settingsBack()
    {
        primaryButton.Select();
        settingsWindow.SetActive(false);
        mainWindow.SetActive(true);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void inPauseReverse()
    {
        inPause = !inPause;
    }
}
