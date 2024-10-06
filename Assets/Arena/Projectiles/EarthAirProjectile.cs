using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAirProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private float timePassed = 0;
    [SerializeField] private float lifetime;
    [SerializeField] public int damage = 2;
    private bool workedOnce = false;
    public void Activate(Vector3 direction)
    {
        StartCoroutine(Hit(direction));


    }
    IEnumerator Hit(Vector3 direction)
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
            Destroy(this.gameObject);
        }
    }
}
