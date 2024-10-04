using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Player controlledPlayer;
    private int index;
    private bool _notMoving = false;
    private void Awake()
    {
        index = GetComponent<PlayerInput>().playerIndex;
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
        var players = FindObjectsOfType<Player>();

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
        FindObjectOfType<PlayerChoose>()?.SelectPlayer(index);
        controlledPlayer.GetController().Attack();
    }
    public void OnAbility(InputAction.CallbackContext context)
    {
        controlledPlayer.GetController().Cast();
    }
    
}
