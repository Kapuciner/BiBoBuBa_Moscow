using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private float cooldown = 1f;
    private static bool inCooldown = false;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Mage" && inCooldown == false)
        {
            collision.gameObject.GetComponent<player>().TakeDamage(1);
            inCooldown = true;
            Invoke("Reset", cooldown);
        }
    }

    private void Reset()
    {
        inCooldown = false;
    }
}
