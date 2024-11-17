using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ConnectManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private ConnectionData _connectionData;

    public int GetPlayerCount()
    {
        return _connectionData.GetPlayerCount();
    }
    
    private void Start()
    {
        ConnectPlayers();
    }

    public void ConnectPlayers()
    {
        bool keyboardJoined = false;
        foreach (var player in _connectionData.ConnectedPlayersData())
        {
            if (player.Device is Keyboard)
            {
                if (keyboardJoined)
                {
                    continue;
                }
                keyboardJoined = true;
            }
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
