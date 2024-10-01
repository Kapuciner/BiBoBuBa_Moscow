using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerArena : MonoBehaviour
{
    [SerializeField] int countdown = 5;

    public TMP_Text startTXT;
    private void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {

        while (countdown > 0)
        {
            startTXT.text = countdown.ToString();
            yield return new WaitForSecondsRealtime(1);
            this.GetComponent<AudioSource>().Play();
            countdown--;
        }

        startTXT.text = "Go!";
        yield return new WaitForSecondsRealtime(1);

        Time.timeScale = 1; 
        startTXT.text = "";
    }
}
