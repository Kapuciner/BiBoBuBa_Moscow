using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : Powerup
{
    public override void OnPicked(Player player)
    {
        player.controller.AddReloadProgress(0.5f);
    }
}
