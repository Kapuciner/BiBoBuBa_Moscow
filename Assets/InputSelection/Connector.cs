using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Connector : MonoBehaviour
{
// Это только для окошка выбора контроля
    public void OnPress(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            FindObjectOfType<DeviceConnectManager>().OnGamepadPress(callbackContext.control.device);
        }
    }

    public void OnKeyboardPress1(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            FindObjectOfType<DeviceConnectManager>().OnKeyboardPress1(callbackContext.control.device);
        }
    }
    public void OnKeyboardPress2(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            FindObjectOfType<DeviceConnectManager>().OnKeyboardPress2(callbackContext.control.device);
        }
    }
// Это только для окошка выбора контроля


    // только при 1 игроке на девайс 
    public LobbyDummy controlledPlayer;
    
    public LobbyDummy _keyboardPlayer1;
    public LobbyDummy _keyboardPlayer2;
    
    private int index;

    public bool IsDoubleKeyboard = false;
    
    private bool _notMoving = false;
    private bool _notMoving_KB2 = false;
    
    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (IsDoubleKeyboard)
        {
            if (_notMoving)
            {
                _keyboardPlayer1.Move(Vector2.zero);
            }
            if (_notMoving_KB2)
            {
                _keyboardPlayer2.Move(Vector2.zero);
            }
        }
        else
        {
            if (_notMoving)
            {
                controlledPlayer.Move(Vector2.zero);
            }
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = Vector2.zero;
        var _targetPlayer = controlledPlayer;
        if (context.control.device is Keyboard)
        {
            _targetPlayer = _keyboardPlayer1;
        }
        if (context.canceled)
        {
            _notMoving = true;
            return;
        }
        _notMoving = false;
        value = context.ReadValue<Vector2>();
        _targetPlayer.Move(value);
    }

    public void OnMove_KB2(InputAction.CallbackContext context)
    {
        if (_keyboardPlayer2 == null)
        {
            return;
        }
        Vector2 value = Vector2.zero;
        var _targetPlayer = _keyboardPlayer2;


        if (context.canceled)
        {
            _notMoving_KB2 = true;
            return;
        }
        _notMoving_KB2 = false;
        value = context.ReadValue<Vector2>();
        //print(_targetPlayer);
        _targetPlayer.Move(value);
    }
}
