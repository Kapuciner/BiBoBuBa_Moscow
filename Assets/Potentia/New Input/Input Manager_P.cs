using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputManager_P : MonoBehaviour
{
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private ConnectionData _connectionData;
    private void Start()
    {
        ConnectPlayers();
    }

    public void ConnectPlayers()
    {
        print("Connect?");
        bool keyboardJoined = false;
        print(_connectionData.ConnectedPlayersData().Count);
        foreach (var player in _connectionData.ConnectedPlayersData())//
        {
            if (player.Device is Keyboard)
            {
                if (keyboardJoined)
                {
                    continue;
                }
                keyboardJoined = true;
            }
            print("Connect");
            print(player.Device);
            _playerInputManager.JoinPlayer(player.playerID, -1, null, player.Device);
        }
    }
    
    public void OnConnect()
    {
        //if (_playerInputManager.playerCount == _playerInputManager.maxPlayerCount)
        //{
        //    _playerInputManager.DisableJoining();
        //}
    }
}
