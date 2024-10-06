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

    [SerializeField] private GameObject lavaDecalPrefab;
    [SerializeField] private float expansionRate = 1f; 


    public void Activate(Vector3 direction)
    {
        StartCoroutine(FlyAndBurn(direction));
    }

    IEnumerator FlyAndBurn(Vector3 direction) 
    {


        while (timePassed < lifetime)
        {
            timePassed += Time.fixedDeltaTime;
            transform.position += direction * projectileSpeed;
            lavaDecalPrefab.transform.position += direction * projectileSpeed / 2;
                    
            float angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            lavaDecalPrefab.transform.rotation = Quaternion.Euler(90, angleY - 90, 0); 

            ExpandLavaDecal();
            yield return new WaitForFixedUpdate();

        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !workedOnce)
        {
            workedOnce = true;
            other.GetComponent<ArenaPlayerManager>().OnFire();
            other.GetComponent<ArenaPlayerManager>().TakeDamage(damage);
            Destroy(this.gameObject, 0.05f);
        }
    }

    private void ExpandLavaDecal()
    {
        DecalProjector decalProjector = lavaDecalPrefab.GetComponent<DecalProjector>();

        if (decalProjector != null)
        {
            float newWidth = decalProjector.size.x + expansionRate * Time.deltaTime;
            decalProjector.size = new Vector3(newWidth, decalProjector.size.y, newWidth);

            BoxCollider boxCollider = lavaDecalPrefab.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.size = new Vector3(newWidth, boxCollider.size.y, 1); 
                boxCollider.center = new Vector3(0, boxCollider.center.y, 0.5f); 
            }

            Material decalMaterial = decalProjector.material;

            if (!decalMaterial.name.EndsWith("(Instance)"))
            {
                decalMaterial = new Material(decalMaterial);
                decalProjector.material = decalMaterial;
            }

            Vector2 currentTiling = decalMaterial.GetVector("_Tiling"); 
            currentTiling += new Vector2(0.015f, 0);
            decalMaterial.SetVector("_Tiling", currentTiling); 
        }
    }
}
