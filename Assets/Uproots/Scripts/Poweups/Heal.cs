using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Powerup
{
    public override void OnPicked(Player player)
    {
        player.RestoreHealth();
    }
}
