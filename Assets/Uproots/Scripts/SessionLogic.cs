using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SessionLogic : MonoBehaviour
{
    public float PointsToWin;
    public float PointsPerSecond;

    public R_Player P1;
    public R_Player P2;
    
    private RootZone zone;

    public TextMeshProUGUI Timer;
    private float _time;
    public float GameTime = 240f;

    private bool _gameFinished = false;
    private void Start()
    {
        P1 = FindObjectOfType<PlayerSpawner>().players[0];
        P2 = FindObjectOfType<PlayerSpawner>().players[1];
        
        zone = GameObject.FindObjectOfType<RootZone>();
        P1.PointsBar.SetMaxValue(PointsToWin);
        P2.PointsBar.SetMaxValue(PointsToWin);

        P1.PlayerDies += () => { StartCoroutine(DeathRoutine(P1));};
        P2.PlayerDies += () => { StartCoroutine(DeathRoutine(P2));};

        _time = GameTime;
    }

    private void Update()
    {
        P1.PointsBar.SetValue(P1.Points);
        P2.PointsBar.SetValue(P2.Points);
        if (zone.RootedPlayer != null)
        {
            zone.RootedPlayer.Points += PointsPerSecond * Time.deltaTime;
            if (zone.RootedPlayer.Points >= PointsToWin)
            {
                FinishGame(zone.RootedPlayer);
            }
        }

        if (_gameFinished == false)
        {
            _time -= Time.deltaTime;
            Timer.text = Mathf.RoundToInt(_time).ToString();
            if (_time <= 0)
            {
                FinishGame(GetPlayerWithMaximumPoints());
            }
        }
    }

    private void FinishGame(R_Player winner)
    {
        _gameFinished = true;
        GameObject.FindObjectOfType<Victory>().FinishGame(winner);
    }

    private R_Player GetPlayerWithMaximumPoints()
    {
        if (P1.Points >= P2.Points)
        {
            return P1;
        }

        return P2;
    }
    private IEnumerator DeathRoutine(R_Player player)
    {
        TextMeshProUGUI text = player.RespawnTimer.GetComponent<TextMeshProUGUI>();
        player.RespawnTimer.SetActive(true);
        float elapsed = 0;
        while (elapsed < 5f)
        {
            text.text = Mathf.RoundToInt(5 - elapsed).ToString();
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        player.RespawnTimer.SetActive(false);
        player.gameObject.SetActive(true);
        player.Reset(player.VegetableType);
    }

    public void StartGame(VegetableType _P1, VegetableType _P2)
    {
        P1.Reset(_P1);
        P2.Reset(_P2);
    }
}
