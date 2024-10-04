using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerTEST : MonoBehaviour
{
    private int index;
    private TestPlayer targetPlayer;
    void Awake()
    {
        index = GetComponent<PlayerInput>().playerIndex;
        targetPlayer = GetPlayer();
        print(index);
    }

    TestPlayer GetPlayer()
    {
        var players = GameObject.FindObjectsOfType<TestPlayer>();
        foreach (var p in players)
        {
            if (p.GetIndex() == index)
            {
                return p;
            }
        }

        throw new Exception();
    }

    void Update()
    {

    }

    public void OnTest(InputAction.CallbackContext context)
    {
        print(index.ToString() + context.action.ReadValue<Vector2>());
    }

    
    
    
}
