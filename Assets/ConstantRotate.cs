using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeedX = 0;
    [SerializeField] private float rotationSpeedY = 0;
    [SerializeField] private float rotationSpeedZ = 0;

    void FixedUpdate()
    {
        this.gameObject.transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ));
    }
}
