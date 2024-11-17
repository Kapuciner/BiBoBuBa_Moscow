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
    public R_Player P3;
    public R_Player P4;
    
    private RootZone zone;

    public TextMeshProUGUI Timer;
    private float _time;
    public float GameTime = 240f;

    private bool _gameFinished = false;

    public List<R_Player> Players;
    private void Start()
    {
        Players = FindObjectOfType<PlayerSpawner>().players;
        
        // P1 = FindObjectOfType<PlayerSpawner>().players[0];
        // P2 = FindObjectOfType<PlayerSpawner>().players[1];
        // P3 = FindObjectOfType<PlayerSpawner>().players[2];
        // P4 = FindObjectOfType<PlayerSpawner>().players[3];
        
        
        
        zone = GameObject.FindObjectOfType<RootZone>();
        // P1.PointsBar.SetMaxValue(PointsToWin);
        // P2.PointsBar.SetMaxValue(PointsToWin);
        // P3.PointsBar.SetMaxValue(PointsToWin);
        // P4.PointsBar.SetMaxValue(PointsToWin);

        foreach (var player in Players)
        {
            player.PointsBar.SetMaxValue(PointsToWin);
            player.PlayerDies += () => { StartCoroutine(DeathRoutine(player));};
        }

        // P1.PlayerDies += () => { StartCoroutine(DeathRoutine(P1));};
        // P2.PlayerDies += () => { StartCoroutine(DeathRoutine(P2));};
        // P3.PlayerDies += () => { StartCoroutine(DeathRoutine(P3));};
        // P4.PlayerDies += () => { StartCoroutine(DeathRoutine(P4));};

        _time = GameTime;
    }

    private void Update()
    {
        // P1.PointsBar.SetValue(P1.Points);
        // P2.PointsBar.SetValue(P2.Points);
        // P3.PointsBar.SetValue(P2.Points);
        // P4.PointsBar.SetValue(P2.Points);
        foreach (var player in Players)
        {
            player.PointsBar.SetValue(player.Points);
        }
            
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
        var result = Players[0];
        float max = 0;
        foreach (var player in Players)
        {
            if (player.Points > max)
            {
                max = player.Points;
                result = player;
            }
        }

        return result;
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

    public void StartGame(List<PlayerSelection.Selection> selections)
    {
        int i = 0;
        foreach (var selection in selections)
        {
            Players[i].Reset(selection.selectionInterface.SelectedType);
            Players[i].controller.SetCanMove(true);
            i++;
        }
    }
}
