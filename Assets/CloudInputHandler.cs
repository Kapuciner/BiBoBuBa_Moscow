using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloudInputScript : MonoBehaviour
{
    CloudTargetScript cloudToInput;
    CloudInterface cloudInterfaceToInput;
    PotentiaInput cloudInput;

    // Start is called before the first frame update
    void Start()
    {
        cloudInput = new PotentiaInput();
        cloudInput.Enable();
        
        cloudToInput = GameObject.Find("Cloud target").GetComponent<CloudTargetScript>();
        //cloudInterfaceToInput = GameObject.Find("cloudInterface").GetComponent<CloudInterface>();

        cloudInput.Cloud.Move.performed += OnMove;
        cloudInput.Cloud.Move.canceled += OnMove;
    }

    public void OnInput(InputAction.CallbackContext cntx) {
        //cloudToInput.SetDirection(cntx.ReadValue<Vector2>());
        //print(cntx.control.device);
        //print(cloudInput.devices);
    }

    public void OnMove(InputAction.CallbackContext cntx) {
        cloudToInput.SetDirection(cntx.ReadValue<Vector2>());
        //print(cntx.control.device);
        //print(cloudInput.devices);
    }
}
