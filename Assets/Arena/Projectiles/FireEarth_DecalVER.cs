using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Äëÿ URP

public class FireEarth_DecalVER : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private float timePassed = 0;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage = 1;
    private bool workedOnce = false;

    [SerializeField] private GameObject lavaDecalPrefab;
    [SerializeField] private GameObject lavaDecalPrefab2;
    [SerializeField] private float expansionRate = 1f;

    Vector3 pushDirection;
    [SerializeField] float pushForce = 2f;
    public void Activate(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(45, 45, angle - 45));

        StartCoroutine(FlyAndBurn(direction));
    }

    IEnumerator FlyAndBurn(Vector3 direction)
    {


        while (timePassed < lifetime)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position += direction * projectileSpeed;
            lavaDecalPrefab.transform.position += direction * projectileSpeed / 2;
            lavaDecalPrefab2.transform.position += direction * projectileSpeed / 2;

            float angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            lavaDecalPrefab.transform.rotation = Quaternion.Euler(90, angleY - 90, 0);
            lavaDecalPrefab2.transform.rotation = Quaternion.Euler(90, angleY - 90, 0);

            ExpandLavaDecal();
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
            Debug.Log("33");
            Destroy(this.gameObject);
        }
    }

    private void ExpandLavaDecal()
    {
        DecalProjector decalProjector = lavaDecalPrefab.GetComponent<DecalProjector>();
        DecalProjector decalProjector2 = lavaDecalPrefab2.GetComponent<DecalProjector>();

        if (decalProjector != null)
        {
            float newWidth = decalProjector.size.x + expansionRate * Time.deltaTime;
            decalProjector.size = new Vector3(newWidth, decalProjector.size.y, 0.2f);
            decalProjector2.size = new Vector3(newWidth, decalProjector2.size.y, 0.5f);

            BoxCollider boxCollider = lavaDecalPrefab.GetComponent<BoxCollider>();
            BoxCollider boxCollider2 = lavaDecalPrefab.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.size = new Vector3(newWidth, boxCollider.size.y, 1);
                boxCollider.center = new Vector3(0, boxCollider.center.y, 0.5f);

                boxCollider2.size = new Vector3(newWidth, boxCollider2.size.y, 1);
                boxCollider2.center = new Vector3(0, boxCollider2.center.y, 0.5f);
            }

            Material decalMaterial = decalProjector.material;
            Material decalMaterial2 = decalProjector2.material;

            if (!decalMaterial.name.EndsWith("(Instance)"))
            {
                decalMaterial = new Material(decalMaterial);
                decalMaterial2 = new Material(decalMaterial2);
                decalProjector.material = decalMaterial;
                decalProjector2.material = decalMaterial2;
            }

            Vector2 currentTiling = decalMaterial.GetVector("_Tiling");
            Vector2 currentTiling2 = decalMaterial2.GetVector("_Tiling");
            currentTiling += new Vector2(0.02f, 0);
            currentTiling2 += new Vector2(0.02f, 0);
            decalMaterial.SetVector("_Tiling", currentTiling);
            decalMaterial2.SetVector("_Tiling", currentTiling);
        }
    }
}
