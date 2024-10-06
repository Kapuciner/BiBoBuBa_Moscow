using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthProjectiles : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private float timePassed = 0;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage = 2;
    [SerializeField] private int stanTime = 2;

    public void Activate(Vector3 direction)
    {
        StartCoroutine(EarthHit(direction));
    }

    IEnumerator EarthHit(Vector3 direction)
    {
        while (timePassed < lifetime)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position += direction * projectileSpeed;
            yield return new WaitForFixedUpdate();
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<ArenaPlayerManager>().TakeDamage(damage);
            collision.gameObject.GetComponent<ArenaPlayerManager>().Stan(stanTime);
            Destroy(this.gameObject, 0.05f);
        }
    }
}
