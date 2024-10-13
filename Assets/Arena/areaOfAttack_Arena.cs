using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaOfAttack_Arena : MonoBehaviour
{

    [SerializeField] private ArenaPlayerManager player;
    private void OnTriggerStay(Collider other) //��� �����
    {
        if (other != player.gameObject && other.tag == "Player")
        {
            if (player.hitCurrentCooldown < 0)
            {
                player.hitDirection = other.transform.position - player.gameObject.transform.gameObject.transform.position;
                player.whoToHit = other.GetComponent<Rigidbody>();
                player.canHit = true;
            }
            else
            {
                player.canHit = false;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player.canHit = false;
        }
    }

    void Update()
    {
        if (!this.gameObject.activeSelf)
            player.canHit = false;
    }
}
