using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public Vector3 offset;
    
    public virtual void OnPicked(Player player)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            OnPicked(player);
            Destroy(gameObject);
        }
    }
}
