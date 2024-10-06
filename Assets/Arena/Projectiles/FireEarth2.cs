using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEarth2 : MonoBehaviour
{
    [SerializeField] private FireEarth fe;
    [SerializeField] private float lifetime;
    public void Activate(Vector3 direction)
    {
        fe.Activate(direction);
        Destroy(this.gameObject, lifetime);
    }
}
