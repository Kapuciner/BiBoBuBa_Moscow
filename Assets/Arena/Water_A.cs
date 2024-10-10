using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_A : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<ArenaPlayerManager>().ExtinguishFire();
        }
    }
}
