using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler_D : MonoBehaviour
{
    private PlayerScript_D controlledPlayer;
    private int index;
    private bool _notMoving = false;
    private void Start()
    {
        index = GetComponent<PlayerInput>().playerIndex;
        AssignPlayer();
    }

    private void Update()
    {
        if (_notMoving)
        {
            controlledPlayer.Move(Vector2.zero);
        }
    }

    private void AssignPlayer()
    {
        var players = FindObjectsOfType<PlayerScript_D>();
        print(players);

        foreach (var player in players)
        {
            if (player.index == index)
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
        controlledPlayer.Move(context.ReadValue<Vector2>());
    }
    
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started) {
            controlledPlayer.Attack();
        }     
    }
    public void OnPickaxeAction(InputAction.CallbackContext context)
    {
        if(context.started) {
            controlledPlayer.PickaxeAction();
        }
    }
    
}
