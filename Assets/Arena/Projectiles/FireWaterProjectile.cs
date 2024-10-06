using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWaterProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private float timePassed = 0;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage = 2;
    [SerializeField] bool extinguishFire = true;

    public void Activate(Vector3 direction)
    {
        StartCoroutine(SteamHit(direction));
    }

    IEnumerator SteamHit(Vector3 direction)
    {
        while (timePassed < lifetime)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position += direction * projectileSpeed;
            yield return new WaitForFixedUpdate();
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            ArenaPlayerManager apm = other.GetComponent<ArenaPlayerManager>();

            if (apm.steamDamageCooldownUp)
            {
                apm.TakeDamage(damage);
                apm.steamDamageCooldownUp = false;
                apm.cooldownPass("steam");
            }
        }
    }

}
