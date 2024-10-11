using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDecal_A : MonoBehaviour
{
    [SerializeField] private int damagePerCooldown;
    [SerializeField] private FireEarth fe;
    private float timer;
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }
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

        if (other.tag == "Block" || other.tag == "Finish")
        {
            if (fe != null)
            {
                fe.stopLava = true;
            }
            if (timer < 0.02)
            {
                Destroy(this.gameObject);
            }
        }


    }


}
