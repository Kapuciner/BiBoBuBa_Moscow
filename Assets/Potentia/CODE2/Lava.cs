using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private float cooldown = 0.75f;
    private bool inCooldown = false;

    bool showFire = false;

    [SerializeField] bool isWater;
    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Mage" && inCooldown == false)
        {
            if (!isWater)
                collision.gameObject.GetComponent<PlayerManager>().showFire = true;
            if (inCooldown == false)
            {
                if (!isWater)
                    collision.gameObject.GetComponent<PlayerManager>().fire.SetActive(true);
                collision.gameObject.GetComponent<player>().TakeDamage(1);
                inCooldown = true;
                Invoke("Reset", cooldown);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Mage" && !isWater)
        {
            collision.gameObject.GetComponent<PlayerManager>().showFire = false;
            StartCoroutine(NoFire(collision.gameObject.GetComponent<PlayerManager>()));
        }
    }

    IEnumerator NoFire(PlayerManager pm)
    {
        yield return new WaitForSeconds(2);
        if (pm.showFire == false)
            pm.fire.SetActive(false);
    }


    private void Reset()
    {
        inCooldown = false;
    }
}
