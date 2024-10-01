using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedup : Powerup
{
    public float time;
    public float speedMultiplier;
    
    public override void OnPicked(Player player)
    {
        GameObject.FindObjectOfType<MonoHandler>().StartCoroutine(SpeedUp(player));
    }
    private IEnumerator SpeedUp(Player player)
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            player.AddBuff(GetComponent<SpriteRenderer>().sprite, time);
        }
        float old = 1;
        player.controller.SetSpeedMultiplier(speedMultiplier);
        yield return new WaitForSeconds(time);
        player.controller.SetSpeedMultiplier(old);
    }
}
