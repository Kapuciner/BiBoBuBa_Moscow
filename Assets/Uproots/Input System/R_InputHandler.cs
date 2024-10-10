using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class R_InputHandler : MonoBehaviour
{
    private R_Player controlledPlayer;
    private PlayerSelection _playerSelection;
    private int index;
    private bool _notMoving = false;

    private bool _canSwipeSelection = true;
    private void Awake()
    {
        index = GetComponent<PlayerInput>().playerIndex;
        _playerSelection = FindObjectOfType<PlayerSelection>();
        AssignPlayer();
    }

    private void Update()
    {
        if (_notMoving)
        {
            controlledPlayer.GetController().Move(Vector2.zero);
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
        if (context.canceled)
        {
            _notMoving = true;
            return;
        }

        _notMoving = false;
        controlledPlayer.GetController().Move(context.ReadValue<Vector2>());
    }
    
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        FindObjectOfType<PlayerSelection>()?.SelectPlayer(index);
        FindObjectOfType<Restart>()?.OnAttackPressed(index);
        controlledPlayer.GetController().Attack();
    }
    public void OnAbility(InputAction.CallbackContext context)
    {
        controlledPlayer.GetController().Cast();
    }

    public void OnSwipe(InputAction.CallbackContext context)
    {
        if (context.started && _playerSelection.isActiveAndEnabled && _canSwipeSelection)
        {
            if (context.ReadValue<float>() > 0)
            {
                _playerSelection.ShiftSelection(index, 1);
                return;
            }
            if (context.ReadValue<float>() < 0)
            {
                _playerSelection.ShiftSelection(index, -1);
                
            }
            _canSwipeSelection = false;
            return;
        }

        if (context.canceled)
        {
            _canSwipeSelection = true;
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        FindObjectOfType<Restart>()?.OnRestartPressed();
    }
}
