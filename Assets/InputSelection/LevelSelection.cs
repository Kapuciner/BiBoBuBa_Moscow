using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    public string SceneToLoad;

    private int _playersInZone = 0;
    private int _connectedPlayersCount;

    private Coroutine _readyRoutine;

    private bool _enabled;

    [SerializeField] private TMP_Text playersAmount;
    [SerializeField] private GameObject playersAmountInfo;
    [SerializeField] private TMP_Text timeTXT;
    [SerializeField] private GameObject timeInfo;
    public void Load()
    {
        _connectedPlayersCount = FindObjectOfType<DeviceConnectManager>().GetPlayerCount();
        _enabled = true;
    }

    private void Update()
    {
        if (_enabled && _playersInZone == _connectedPlayersCount && _readyRoutine == null)
        {
            StartReadyCountdown();
        }
    }
    
    private void StartReadyCountdown()
    {
        playersAmountInfo.SetActive(false);
        _readyRoutine = StartCoroutine(ReadyRoutine());
    }

    private IEnumerator ReadyRoutine()
    {
        float elapsed = 0;
        float time = 3;
        while (elapsed < time)
        {
            //GameStartingText.text = "All ready, starting in " + (time-elapsed);
            timeInfo.gameObject.SetActive(true);
            timeTXT.text = "Начало через " + (time-elapsed).ToString("F1") + "с";
            if (_playersInZone < _connectedPlayersCount)
            {
                playersAmountInfo.SetActive(true);
                playersAmount.text = $"Количество игроков {_playersInZone}/{_connectedPlayersCount}";
                timeInfo.gameObject.SetActive(false);
                _readyRoutine = null;
                //GameStartingText.gameObject.SetActive(false);
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //GameStartingText.gameObject.SetActive(false);
        timeInfo.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneToLoad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lobby_Dummy"))
        {
            _playersInZone++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lobby_Dummy"))
        {
            _playersInZone--;
            playersAmountInfo.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Lobby_Dummy"))
        {
        if (_playersInZone > 0 && _readyRoutine is null)
            {
                playersAmountInfo.SetActive(true);
                playersAmount.text = $"Количество игроков {_playersInZone}/{_connectedPlayersCount}";
            }
            else
                playersAmountInfo.SetActive(false);
        }
    }
}
