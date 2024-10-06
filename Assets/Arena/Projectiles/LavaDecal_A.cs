using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDecal_A : MonoBehaviour
{
    [SerializeField] private int damagePerCooldown;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player") 
        {
            ArenaPlayerManager apm = other.GetComponent<ArenaPlayerManager>();

            if (apm.lavaDamageCooldownUp)
            {
                apm.OnFire();
                apm.lavaDamageCooldownUp = false;
                apm.cooldownPass("lava");
                apm.TakeDamage(damagePerCooldown);
                Debug.Log("lavaDamage");
            }

        }
    }


}
