using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    [SerializeField] string name = "water";
    [SerializeField] private float projectileSpeed;
    private float timePassed = 0;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage = 2;
    [SerializeField] bool extinguishFire = true;
    private bool workedOnce = false;

    private GameObject playerImmune;
    private bool immuneTimePassed = false;
    [SerializeField] float immuneTime = 0;

    Vector3 pushDirection;
    [SerializeField] float pushForce = 2f;
    public void Activate(Vector3 direction, GameObject playerWhoCast)
    {
        StartCoroutine(WaterHit(direction));
        playerImmune = playerWhoCast;
    }

    IEnumerator WaterHit(Vector3 direction)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !workedOnce)
        {
            if (other.gameObject != playerImmune || immuneTimePassed)
            {
                if (name == "water")
                    other.GetComponent<ArenaPlayerManager>().PlayOnHit("water");
                if (name == "waterAir")
                    other.GetComponent<ArenaPlayerManager>().PlayOnHit("waterAir");
                if (name == "fireAir")
                    other.GetComponent<ArenaPlayerManager>().PlayOnHit("fireAir");
                pushDirection = other.transform.position - this.gameObject.transform.position;
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
                workedOnce = true;
                other.GetComponent<ArenaPlayerManager>().TakeDamage(damage);
                if (extinguishFire)
                    other.GetComponent<ArenaPlayerManager>().ExtinguishFire();
                Destroy(this.gameObject, 0.05f);
                if (name == "fireAir")
                    other.GetComponent<ArenaPlayerManager>().OnFire();
            }
        }

        if (other.tag == "Block")
        {
            other.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
        }
    }
}
