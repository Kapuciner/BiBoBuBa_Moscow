using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ConnectManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager _playerInputManager;
    

    
    public void OnConnect()
    {
        
        
        
        if (_playerInputManager.playerCount == _playerInputManager.maxPlayerCount)
        {
            _playerInputManager.DisableJoining();
        }

    }
}
