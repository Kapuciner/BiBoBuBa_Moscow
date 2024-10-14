using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class R_InputHandler : MonoBehaviour
{
    public R_Player controlledPlayer;
    private PlayerSelection _playerSelection;
    private int index;
    private bool _notMoving = false;

    private bool _canSwipeSelection = true;
    
    public R_Player _keyboardPlayer1;
    public R_Player _keyboardPlayer2;
    public bool IsDoubleKeyboard = false;
    private bool _notMoving_KB2 = false;
    private void Start()
    {
        index = GetComponent<PlayerInput>().playerIndex;
        _playerSelection = FindObjectOfType<PlayerSelection>();
        AssignPlayer();
    }

    private void Update()
    {
        if (IsDoubleKeyboard)
        {
            if (_notMoving)
            {
                _keyboardPlayer1.controller.Move(Vector2.zero);
            }
            if (_notMoving_KB2)
            {
                _keyboardPlayer2.controller.Move(Vector2.zero);
            }
        }
        else
        {
            if (_notMoving)
            {
                controlledPlayer.controller.Move(Vector2.zero);
            }
        }
    }

    private void AssignPlayer()
    {
        var players = FindObjectsOfType<R_Player>();
        
        foreach (var player in players)
        {
            if (player.GetIndex() == index)
            {
                controlledPlayer = player;
                return;
            }
        }
        throw new IndexOutOfRangeException("Failed to find player with index " + index.ToString());
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
        _targetPlayer.controller.Move(value);
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
        _targetPlayer.controller.Move(value);
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        var _targetPlayer = TargetPlayer(context, false);
        
        FindObjectOfType<PlayerSelection>()?.SelectPlayer(_targetPlayer.GetIndex());
        FindObjectOfType<Restart>()?.OnAttackPressed(_targetPlayer.GetIndex());
        _targetPlayer.GetController().Attack();
    }
    
    public void OnAttack_KB2(InputAction.CallbackContext context)
    {
        var _targetPlayer = TargetPlayer(context, true);
        
        FindObjectOfType<PlayerSelection>()?.SelectPlayer(_targetPlayer.GetIndex());
        FindObjectOfType<Restart>()?.OnAttackPressed(_targetPlayer.GetIndex());
        _targetPlayer.GetController().Attack();
    }

    public R_Player TargetPlayer(InputAction.CallbackContext context, bool KB2)
    {
        var _targetPlayer = controlledPlayer;
        if (context.control.device is Keyboard)
        {
            if (KB2)
            {
                _targetPlayer = _keyboardPlayer2;
            }
            else _targetPlayer = _keyboardPlayer1;
        }

        return _targetPlayer;
    }
    public void OnAbility(InputAction.CallbackContext context)
    {
        var _targetPlayer = TargetPlayer(context, false);
        _targetPlayer.GetController().Cast();
    }
    public void OnAbility_KB2(InputAction.CallbackContext context)
    {
        var _targetPlayer = TargetPlayer(context, true);
        _targetPlayer.GetController().Cast();
    }
    public void OnSwipe(InputAction.CallbackContext context)
    {
        var _targetPlayer = TargetPlayer(context, false);
        if (context.control.device is Gamepad)
        {
            if (context.started && _playerSelection.isActiveAndEnabled && _canSwipeSelection)
            {
                if (context.ReadValue<float>() > 0)
                {
                    _playerSelection.ShiftSelection(_targetPlayer.GetIndex(), 1);
                    return;
                }
                if (context.ReadValue<float>() < 0)
                {
                    _playerSelection.ShiftSelection(_targetPlayer.GetIndex(), -1);
                
                }
                _canSwipeSelection = false;
                return;
            }
        }
        else if (context.control.device is Keyboard)
        {
            if (context.started && _playerSelection.isActiveAndEnabled && _canSwipeSelection)
            {
                if (context.ReadValue<Vector2>().x > 0)
                {
                    _playerSelection.ShiftSelection(_targetPlayer.GetIndex(), 1);
                    return;
                }
                if (context.ReadValue<Vector2>().x < 0)
                {
                    _playerSelection.ShiftSelection(_targetPlayer.GetIndex(), -1);
                
                }
                _canSwipeSelection = false;
                return;
            }
        }
        

        if (context.canceled)
        {
            _canSwipeSelection = true;
        }
    }
    public void OnSwipe_KB2(InputAction.CallbackContext context)
    {
        var _targetPlayer = TargetPlayer(context, true);
        if (context.control.device is Gamepad)
        {
            if (context.started && _playerSelection.isActiveAndEnabled && _canSwipeSelection)
            {
                if (context.ReadValue<float>() > 0)
                {
                    _playerSelection.ShiftSelection(_targetPlayer.GetIndex(), 1);
                    return;
                }
                if (context.ReadValue<float>() < 0)
                {
                    _playerSelection.ShiftSelection(_targetPlayer.GetIndex(), -1);
                
                }
                _canSwipeSelection = false;
                return;
            }
        }
        else if (context.control.device is Keyboard)
        {
            if (context.started && _playerSelection.isActiveAndEnabled && _canSwipeSelection)
            {
                if (context.ReadValue<Vector2>().x > 0)
                {
                    _playerSelection.ShiftSelection(_targetPlayer.GetIndex(), 1);
                    return;
                }
                if (context.ReadValue<Vector2>().x < 0)
                {
                    _playerSelection.ShiftSelection(_targetPlayer.GetIndex(), -1);
                
                }
                _canSwipeSelection = false;
                return;
            }
        }
        

        if (context.canceled)
        {
            _canSwipeSelection = true;
        }
    }
    public void OnRestart(InputAction.CallbackContext context)
    {
        return;
        FindObjectOfType<Restart>()?.OnRestartPressed();
    }
}
