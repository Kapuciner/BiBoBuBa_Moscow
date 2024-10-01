using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageup : Powerup
{
    public float time;
    
    public override void OnPicked(Player player)
    {
        GameObject.FindObjectOfType<MonoHandler>().StartCoroutine(DamageUp(player));
    }
    private IEnumerator DamageUp(Player player)
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            player.AddBuff(GetComponent<SpriteRenderer>().sprite, time);
        }
        int old = 1;
        player.SetDamage(2);
        yield return new WaitForSeconds(time);
        player.SetDamage(old);
    }
}
