using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private player mage;
    private float cooldown = 1f;
    private static bool inCooldown = false;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Mage" && inCooldown == false)
        {
            mage.TakeDamage(1);
            inCooldown = true;
            Invoke("Reset", cooldown);
        }
    }

    private void Reset()
    {
        inCooldown = false;
    }
}
