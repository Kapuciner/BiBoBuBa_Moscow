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
    private void Start()
    {
        ConnectPlayers();
    }

    public void ConnectPlayers()
    {
        foreach (var player in _connectionData.ConnectedPlayersData())
        {
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
