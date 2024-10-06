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

    public void Activate(Vector3 direction)
    {
        StartCoroutine(AirHit(direction));
    }

    IEnumerator AirHit(Vector3 direction)
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
        if (collision.gameObject.tag == "Player" && !workedOnce)
        {
            workedOnce = true;
            direction = collision.gameObject.transform.position - this.gameObject.transform.position;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * PushForce, ForceMode.Impulse);
            collision.gameObject.GetComponent<ArenaPlayerManager>().TakeDamage(damage);
            Destroy(this.gameObject, 0.05f);
        }
    }
}
