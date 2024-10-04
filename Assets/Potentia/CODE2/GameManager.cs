using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameTimer timer;
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
