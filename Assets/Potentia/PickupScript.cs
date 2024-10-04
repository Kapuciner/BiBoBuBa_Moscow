using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float pickupCooldown;
    [SerializeField] int pickupPower;
    SpriteRenderer renderer;
    AudioSource pickUpSound;
    [SerializeField] private CloudInterface cloudInterface;
    [SerializeField] private GameObject pointer;

    bool deactivated = false;
    float sleepTime = 0;
    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.CompareTag("Cloud") && !deactivated) {
            if(cloudInterface.TryAddEnergy()) {
                deactivated = true;
                sleepTime = 0;
                pickUpSound.Play();
            }
        }
    }

    void Start() {
        renderer = GetComponent<SpriteRenderer>();
        pickUpSound = GetComponent<AudioSource>();
    }

    void Update() {
        if (deactivated)
        {
            renderer.enabled = false;
            pointer.SetActive(false);
        }
        else
        {
            renderer.enabled = true;
            pointer.SetActive(true);
        }
            sleepTime += Time.deltaTime;
        if (sleepTime >= pickupCooldown) deactivated = false;
    }
}
