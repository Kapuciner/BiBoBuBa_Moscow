using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public Vector3 offset;
    
    public virtual void OnPicked(R_Player player)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            R_Player player = other.GetComponent<R_Player>();
            OnPicked(player);
            Destroy(gameObject);
        }
    }
}
