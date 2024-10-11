using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Äëÿ URP
public class FireEarth : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private float timePassed = 0;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage = 1;
    private bool workedOnce = false;

    [SerializeField] private GameObject lavaPlanePrefab;
    [SerializeField] private GameObject lavaPlanePrefab2;
    [SerializeField] private float expansionRate = 1f;

    Vector3 pushDirection;
    [SerializeField] float pushForce = 2f;

    private Material lavaMaterial;
    private Vector2 initialTiling;

    public bool stopLava = false;
    public void Activate(Vector3 direction)
    {
        lavaMaterial = lavaPlanePrefab.GetComponent<Renderer>().material;

        StartCoroutine(FlyAndBurn(direction));
    }

    IEnumerator FlyAndBurn(Vector3 direction) 
    {


        while (timePassed < lifetime)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position += direction * projectileSpeed;
            if (!stopLava)
            {
                lavaPlanePrefab.transform.position += direction * projectileSpeed / 2;
                lavaPlanePrefab2.transform.position += direction * projectileSpeed / 2;
                float angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                lavaPlanePrefab.transform.rotation = Quaternion.Euler(0, angleY, 0);
                lavaPlanePrefab2.transform.rotation = Quaternion.Euler(0, angleY, 0);
                ExpandLavaDecal();
            }
            yield return new WaitForFixedUpdate();

        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !workedOnce)
        {
            pushDirection = other.transform.position - this.gameObject.transform.position;
            other.GetComponent<Rigidbody>().AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
            workedOnce = true;
            other.GetComponent<ArenaPlayerManager>().OnFire();
            other.GetComponent<ArenaPlayerManager>().TakeDamage(damage);
            Destroy(this.gameObject, 0.05f);
        }
        if (other.tag == "Block")
        {
            Destroy(this.gameObject);
        }
    }

    private void ExpandLavaDecal()
    {
        if (!stopLava)
        {
            Vector3 newScale = lavaPlanePrefab.transform.localScale + new Vector3(0, 0, expansionRate) * Time.deltaTime;
            Vector3 newScale2 = lavaPlanePrefab2.transform.localScale + new Vector3(0, 0, expansionRate) * Time.deltaTime;
            lavaPlanePrefab.transform.localScale = newScale;
            lavaPlanePrefab2.transform.localScale = newScale2;
        }

    }
}
