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
    private bool workedOnce = false;

    private GameObject playerImmune;
    private bool immuneTimePassed = false;
    [SerializeField] float immuneTime = 0;

    Vector3 pushDirection;
    [SerializeField] float pushForce = 5f;


    public void Activate(Vector3 direction, GameObject playerWhoCast)
    {
        StartCoroutine(EarthHit(direction));
        playerImmune = playerWhoCast;
    }

    IEnumerator EarthHit(Vector3 direction)
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
                workedOnce = true;
                pushDirection = other.transform.position - this.gameObject.transform.position;
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
                other.GetComponent<ArenaPlayerManager>().TakeDamage(damage);
                other.GetComponent<ArenaPlayerManager>().Stan(stanTime);
                Destroy(this.gameObject, 0.05f);
            }
        }
        if (other.tag == "Block")
        {
            Destroy(this.gameObject);
        }
    }
}
