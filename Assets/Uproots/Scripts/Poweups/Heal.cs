using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Powerup
{
    public override void OnPicked(R_Player player)
    {
        player.RestoreHealth();
    }
}
