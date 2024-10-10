using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPlatform_D : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ArrowTrapScript_D connectedTrap;

    float cooldown = 0;

    void Update() {
        cooldown -= Time.deltaTime;
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("player_D") && cooldown < 0)
        {
            connectedTrap.PlayerStepped();
            cooldown = 5;
        }
    }
}
