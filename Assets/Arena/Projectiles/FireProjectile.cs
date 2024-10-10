using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private float timePassed = 0;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage = 1;
    private bool workedOnce = false;

    private GameObject playerImmune;
    private bool immuneTimePassed = false;
    [SerializeField] float immuneTime = 0;

    Vector3 pushDirection;
    [SerializeField] float pushForce = 5f;
    public void Activate(Vector3 direction, GameObject playerWhoCast)
    {
        StartCoroutine(FlyAndBurn(direction));
        playerImmune = playerWhoCast;
    }

    IEnumerator FlyAndBurn(Vector3 direction)
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
                pushDirection = other.transform.position - this.gameObject.transform.position;
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
                workedOnce = true;
                other.GetComponent<ArenaPlayerManager>().OnFire();
                other.GetComponent<ArenaPlayerManager>().TakeDamage(damage);
                Destroy(this.gameObject, 0.05f);
            }
        }

        if (other.tag == "Block")
        {
            Destroy(this.gameObject);
        }
    }
}
