using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootZone : MonoBehaviour
{
    public bool HasRootedPlayer = false;
    public float RootTime = 5;
    public Player RootedPlayer;
    
    
    public Player CurrentPlayer;

    public Roots Roots;

    public void RootPlayer(Player player)
    {
        HasRootedPlayer = true;
        RootedPlayer = player;
        
    }

    public void ClearZone()
    {
        HasRootedPlayer = false;
        RootedPlayer = null;
        CurrentPlayer = null;
        Roots.Clear();
    }

    public void StartRooting()
    {
        GetComponent<SoundPlayer>().Play(GetComponent<AudioSource>().clip);
        StartCoroutine(Rooting());
    }

    private IEnumerator Rooting()
    {
        Roots.ClearInTime(1f);
        yield return new WaitForSeconds(1f);
  
        Roots.Setup(CurrentPlayer);
        Player p = CurrentPlayer;
        while (CurrentPlayer != null && CurrentPlayer == p && RootedPlayer == null)
        {
            Roots.MakeStep();
            yield return new WaitForSeconds((RootTime - 1) / Roots.MaxSteps);
        }

    }

    private void Update()
    {
        if (RootedPlayer != null)
        {
            Roots.Pulse();
        }
    }
}
