using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : MonoBehaviour
{
    public AnimationCurve Curve;

    public float power;
    public float speed;
    private Vector3 position;
    public GameObject shadow;

    private void Start()
    {
        position = transform.position;
    }

    private void Update()
    {
        Vector3 old = shadow.transform.position;
        Vector3 newPos = position;
        Vector3 delta = new Vector3(0, Mathf.Sin(speed * Time.timeSinceLevelLoad) * power, 0);
        newPos = position + delta;
        transform.position = newPos;
        shadow.transform.position = old;
    }
}
