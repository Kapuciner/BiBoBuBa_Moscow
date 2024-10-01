using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Animator txt;

    public bool gameOver = false;
    public bool mageWon = false;
    public CloudScript cs;
    public TMP_Text startTXT;
    private void Start()
    {
        Time.timeScale = 0;
        cs.canMove = false;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        int countdown = 5; // Таймер на 3 секунды

        while (countdown > 0)
        {
            startTXT.text = countdown.ToString(); 
            yield return new WaitForSecondsRealtime(1);
            this.GetComponent<AudioSource>().Play();
            countdown--; 
        }

        startTXT.text = "Go!"; // Сообщение о старте игры
        yield return new WaitForSecondsRealtime(1); // Ожидание дополнительной секунды, если нужно показать сообщение "Go!"

        Time.timeScale = 1; // Снимаем игру с паузы
        cs.canMove = true; // Разрешаем движение
        startTXT.text = ""; // Очищаем текст таймера
    }

    public void MageWon()
    {
        winPanel.SetActive(true);
        txt.Play("TxTappear");
        mageWon = true;
        cs.canMove = false;
    }

    private void Update()
    {
        if (gameOver || mageWon)
        {
            if (Input.GetKeyDown(KeyCode.R))
                RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
