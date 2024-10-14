using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

//sorry for lower-case :/
public class pauseManager : MonoBehaviour
{
    private static pauseManager pauseCanvaInstance;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainWindow;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private VolumeSettings _volumeSettings;
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
        _volumeSettings.SetCurrentVolumes();
        print("works");
        pauseMenu.SetActive(false);
        inPause = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
        if (MenuManager.alreadyChosenControl)
        {
            canPause = true;
        }
        else
        {
            canPause = false; 
        }
        inPause = false;
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("option") || Input.GetButtonDown("option"))
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

    public void ExitTheGame()
    {
        Application.Quit();
    }
}
