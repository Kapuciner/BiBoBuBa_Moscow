using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AireProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private float timePassed = 0;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage = 2;
    [SerializeField] private int PushForce = 20;
    private Vector3 direction = Vector3.zero;
    bool workedOnce = false;

    private GameObject playerImmune;
    private bool immuneTimePassed = false;
    [SerializeField] float immuneTime = 0;

    public void Activate(Vector3 direction, GameObject playerWhoCast)
    {
        StartCoroutine(AirHit(direction));
        playerImmune = playerWhoCast;
    }

    IEnumerator AirHit(Vector3 direction)
    {
        while (timePassed < lifetime)
        {
            if (timePassed >= immuneTime)
                immuneTimePassed = true;
            timePassed += Time.fixedDeltaTime;
            transform.position += direction * projectileSpeed;
            yield return new WaitForFixedUpdate();
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !workedOnce)
        {
            if (other.gameObject != playerImmune || immuneTimePassed)
            {
                other.GetComponent<ArenaPlayerManager>().PlayOnHit("air");
                workedOnce = true;
                direction = other.transform.position - this.gameObject.transform.position;
                other.GetComponent<Rigidbody>().AddForce(direction.normalized * PushForce, ForceMode.Impulse);
                other.GetComponent<ArenaPlayerManager>().TakeDamage(damage);
                Destroy(this.gameObject, 0.05f);
            }
        }

        if (other.tag == "Block")
        {
            other.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
        }
    }
}
