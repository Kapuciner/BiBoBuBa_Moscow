using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public string SceneToLoad;

    private int _playersInZone = 0;
    private int _connectedPlayersCount;

    private Coroutine _readyRoutine;

    private bool _enabled;
    
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
        _readyRoutine = StartCoroutine(ReadyRoutine());
    }

    private IEnumerator ReadyRoutine()
    {
        float elapsed = 0;
        float time = 3;
        while (elapsed < time)
        {
            print(time - elapsed);
            //GameStartingText.text = "All ready, starting in " + (time-elapsed);
            if (_playersInZone < _connectedPlayersCount)
            {
                _readyRoutine = null;
                //GameStartingText.gameObject.SetActive(false);
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //GameStartingText.gameObject.SetActive(false);
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
        }
    }
}
