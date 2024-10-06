using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private float timePassed = 0;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage = 2;
    [SerializeField] bool extinguishFire = true;
    private bool workedOnce = false;

    public void Activate(Vector3 direction)
    {
        StartCoroutine(WaterHit(direction));
    }

    IEnumerator WaterHit(Vector3 direction)
    {
        while (timePassed < lifetime)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position += direction * projectileSpeed;
            yield return new WaitForFixedUpdate();
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !workedOnce)
        {
            workedOnce = true;
            other.GetComponent<ArenaPlayerManager>().TakeDamage(damage);
            if (extinguishFire)
                other.GetComponent<ArenaPlayerManager>().ExtinguishFire();
            Destroy(this.gameObject, 0.05f);
        }
    }
}
