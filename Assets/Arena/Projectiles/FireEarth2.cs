using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEarth2 : MonoBehaviour
{
    [SerializeField] private FireEarth fe;
    [SerializeField] private FireEarth_DecalVER fe2;
    [SerializeField] private float lifetime;
    public void Activate(Vector3 direction)
    {
        if (fe != null)
            fe.Activate(direction);
        else
            fe2.Activate(direction);
        Destroy(this.gameObject, lifetime);
    }
}
