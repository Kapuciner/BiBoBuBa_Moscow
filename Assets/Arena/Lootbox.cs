using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lootbox : MonoBehaviour
{
    [SerializeField] private GameObject brokenBox;
    [SerializeField] private GameObject pickUp;

    [SerializeField] private float explosionForce = 1500f;  // Сила взрыва
    [SerializeField] private float explosionRadius = 15f;   // Радиус взрыва
    [SerializeField] private float upwardModifier = 1f;    // Модификатор подъема

    [SerializeField] private float fragmentsLiveTime = 3;

    bool alreadyWorked = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (alreadyWorked) return;

        alreadyWorked = true;
        Vector3 spawnBrokenBoxPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        GameObject box2 = Instantiate(brokenBox, spawnBrokenBoxPosition, Quaternion.identity);
        Instantiate(pickUp, spawnBrokenBoxPosition, pickUp.transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(collision.contacts[0].point, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.tag != "Player")
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null && !rb.isKinematic)
                {
                    rb.AddExplosionForce(explosionForce, collision.contacts[0].point, explosionRadius, upwardModifier, ForceMode.Impulse);
                }
            }
        }

        StartCoroutine((DieOut(box2)));

        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;

    }

    IEnumerator DieOut(GameObject brokenBox)
    {
        Transform[] fragmentsTransform = brokenBox.GetComponentsInChildren<Transform>();

        Color fragmentMaterial = fragmentsTransform[1].gameObject.GetComponent<MeshRenderer>().materials[0].color;
        Color fragmentMaterial2 = fragmentsTransform[1].gameObject.GetComponent<MeshRenderer>().materials[1].color;
        float passedTime = 0;

        while (passedTime < fragmentsLiveTime)
        {
            for (int i = 1; i < fragmentsTransform.Length; i++)
            {
                fragmentsTransform[i].GetComponent<MeshRenderer>().materials[0].color = new Color(fragmentMaterial.r, fragmentMaterial.g, fragmentMaterial.b, 1 - passedTime / fragmentsLiveTime);
                fragmentsTransform[i].GetComponent<MeshRenderer>().materials[1].color = new Color(fragmentMaterial2.r, fragmentMaterial2.g, fragmentMaterial2.b, 1 - passedTime / fragmentsLiveTime);
             }
            passedTime += Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(brokenBox);
        Destroy(this.gameObject);
    }



}
