using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : Powerup
{
    public float power;
    public float time;
    public override void OnPicked(R_Player player)
    {
        GameObject.FindObjectOfType<MonoHandler>().StartCoroutine(IncreasePushForce(player));
    }

    private IEnumerator IncreasePushForce(R_Player player)
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            player.AddBuff(GetComponent<SpriteRenderer>().sprite, time);
        }

        
        float old = player.controller.pushForce;
        player.controller.SetPushForce(power);
        yield return new WaitForSeconds(time);
        player.controller.SetPushForce(old);
    }
}
