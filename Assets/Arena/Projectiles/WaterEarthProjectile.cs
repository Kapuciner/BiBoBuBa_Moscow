using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEarthProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private float timePassed = 0;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage = 2;
    [SerializeField] private int slowTime = 2;
    [SerializeField] private float slowReductionTimes = 2;  //���� 2, �� speed = speed / 2;
    bool workedOnce = false;

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
                other.GetComponent<ArenaPlayerManager>().PlayOnHit("waterEarth");
                pushDirection = other.transform.position - this.gameObject.transform.position;
                other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
                workedOnce = true;
                other.GetComponent<ArenaPlayerManager>().TakeDamage(damage);
                other.GetComponent<ArenaPlayerManager>().Slow(slowTime, slowReductionTimes);
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
