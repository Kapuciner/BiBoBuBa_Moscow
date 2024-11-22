using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject DummyPrefab;
    [SerializeField] private ConnectionData _connectionData;
    [SerializeField] private List<GameObject> _spawnPoints;
    
    
    private List<LobbyDummy> Players;
    private int _keyboardCount = 0;
    public void OnStarted()
    {
        Players = new List<LobbyDummy>();
        SpawnPlayers();
        var levels = FindObjectsOfType<LevelSelection>();
        foreach (var level in levels)
        {
            level.Load();
        }
    }

    private void SpawnPlayers()
    {
        int count = 0;
        var players = _connectionData.ConnectedPlayersData();
        print("spawning: " + _connectionData.ConnectedPlayersData().Count + "players:");
        for (int i = 0; i < _connectionData.ConnectedPlayersData().Count; i++)
        {
            print("player " + i.ToString() + ": " + _connectionData.ConnectedPlayersData()[i].Device.ToString());
        }
        foreach (var player in players)
        {
            if (PlayerInput.FindFirstPairedToDevice(player.Device) == null)
            {
                continue;
            }
            GameObject p = Instantiate(DummyPrefab);
            var d = p.GetComponent<LobbyDummy>();
            var p_input = PlayerInput.FindFirstPairedToDevice(player.Device);
            Players.Add(d);
            d.PlayerIndex = count;
            d._animator.SetInteger("playerID", count);
            count++;
            
            
            
            var controller = p_input.gameObject.GetComponent<Connector>();

            if (player.Device is Keyboard)
            {
                _keyboardCount++;

                if (_keyboardCount == 1)
                {
                    controller._keyboardPlayer1 = d;
                }
                else if (_keyboardCount == 2)
                {
                    controller._keyboardPlayer2 = d;

                    controller.IsDoubleKeyboard = true;
                }
            }
            controller.controlledPlayer = d;
            p.transform.position = _spawnPoints[player.playerID].transform.position;
            
            print("spawned " + player.Device);

            if (FindObjectOfType<pauseManager>() != null)
            {
                FindObjectOfType<pauseManager>().canPause = true;
            }

        }
    }

}
