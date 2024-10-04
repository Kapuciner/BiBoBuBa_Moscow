using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MageInputScript : MonoBehaviour
{
    PlayerManager mageToInput;
    PotentiaInput mageInput;

    // Start is called before the first frame update
    void Start()
    {
        mageInput = new PotentiaInput();
        mageInput.Enable();
        mageToInput = GameObject.Find("Player").GetComponent<PlayerManager>();
        mageInput.Player.Move.performed += OnMove;
        mageInput.Player.Move.canceled += OnMove;
        mageInput.Player.Dash.performed += OnDashUse;
        mageInput.Player.Fireball.performed += OnFireballUse;
        mageInput.Player.Shield.performed += OnShieldUse;
        

    }

    public void OnMove(InputAction.CallbackContext cntx) {
        mageToInput.MoveMage(cntx.ReadValue<Vector2>());
        print(cntx.control.device);
        print(mageInput.devices);
    }

    public void OnDashUse(InputAction.CallbackContext cntx) {
        mageToInput.TryDash();
    }
    public void OnFireballUse(InputAction.CallbackContext cntx) {
        mageToInput.TryFireball();
    }
    public void OnShieldUse(InputAction.CallbackContext cntx) {
        mageToInput.TryShield();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
