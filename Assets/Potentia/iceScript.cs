using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class iceScript : MonoBehaviour
{
    [SerializeField] float iceLifespan;
    Collider player = null;
    void Start() {
        StartCoroutine(DestroySelf());
    }
    void OnTriggerEnter(Collider collision) {
        print(collision);
        if (collision.gameObject.tag == "Mage") {
            player = collision;
            collision.gameObject.GetComponent<PlayerManager>().SwitchIceMode(true);
        }
    }
    void OnTriggerExit(Collider collision) {
        if (collision.gameObject.tag == "Mage") {
            collision.gameObject.GetComponent<PlayerManager>().SwitchIceMode(false);
            player = null;
        }
    }

    void OnTriggerStay(Collider collision) {
        if (collision.gameObject.tag == "Mage") {
            player = collision;
            collision.gameObject.GetComponent<PlayerManager>().SwitchIceMode(true);
        }
    }

    IEnumerator DestroySelf() {
        yield return new WaitForSeconds(iceLifespan);
        if (player != null) player.gameObject.GetComponent<PlayerManager>().SwitchIceMode(false);
        Destroy(gameObject);
    }
}
