using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    // [SerializeField] private Transform P1_spawner;
    // [SerializeField] private Transform P2_spawner;
    // [SerializeField] private Transform P3_spawner;
    // [SerializeField] private Transform P4_spawner;

    [SerializeField] private List<GameObject> _spawners;
    
    [SerializeField] private ConnectionData _connectionData;

    [SerializeField] private GameObject _playerPrefab;

    public List<R_Player> players = new List<R_Player>();
    
    private int _keyboardCount;

    private void Start()
    {
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        int i = 0;
        R_Player P1 = null;
        R_Player P2 = null;
        R_Player P3 = null;
        R_Player P4 = null;
        foreach (var p in _connectionData.ConnectedPlayersData())
        {
            GameObject player_obj = Instantiate(_playerPrefab);
            var player = player_obj.GetComponent<R_Player>();
            if (i==0)
            {
                P1 = player;
            }
            else if (i==1)
            {
                P2 = player;
            }
            else if (i==2)
            {
                P3 = player;
            }
            else if (i==3)
            {
                P4 = player;
            }
            players.Add(player);
            
            player.SpawnPoint = _spawners[i];
            player.transform.position = player.SpawnPoint.transform.position;
            // player.SetIndex(p.playerID);
            var p_input = PlayerInput.FindFirstPairedToDevice(p.Device);
            var controller = p_input.gameObject.GetComponent<R_InputHandler>();
            if (p.Device is Keyboard)
            {
                _keyboardCount++;
                if (_keyboardCount == 1)
                {
                    controller._keyboardPlayer1 = player;
                }
                else if (_keyboardCount == 2)
                {
                    controller._keyboardPlayer2 = player;
                
                    controller.IsDoubleKeyboard = true;
                }
            }
            controller.controlledPlayer = player;
            i++;
        }
        FindObjectOfType<CameraScaler>().SetPlayers(P1, P2, P3, P4);
    }
    
    private void sssSpawnPlayers()
    {
        var players = _connectionData.ConnectedPlayersData();
        foreach (var player in players)
        {
            // GameObject p = Instantiate(DummyPrefab);
            // var d = p.GetComponent<LobbyDummy>();
            // var p_input = PlayerInput.FindFirstPairedToDevice(player.Device);
            // Players.Add(d);
            // var controller = p_input.gameObject.GetComponent<Connector>();
            //
            // if (player.Device is Keyboard)
            // {
            //     _keyboardCount++;
            //
            //     if (_keyboardCount == 1)
            //     {
            //         controller._keyboardPlayer1 = d;
            //     }
            //     else if (_keyboardCount == 2)
            //     {
            //         controller._keyboardPlayer2 = d;
            //
            //         controller.IsDoubleKeyboard = true;
            //     }
            // }
            // controller.controlledPlayer = d;
            // p.transform.position = _spawnPoints[player.playerID].transform.position;
            // p.transform.rotation = Quaternion.identity;
            
            print("spawned " + player.Device);
        }
    }
}
