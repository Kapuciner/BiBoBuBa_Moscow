using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerChoose : MonoBehaviour
{
    public Player P1;
    public Player P2;

    public Choose choose1;
    public Choose choose2;
    private int id1;
    private int id2;

    public AudioClip change;
    public AudioClip select;
    public SoundPlayer SoundPlayer;
    
    
    public List<VegetableType> VegetableTypes;

    private void Start()
    {
        choose1.SetVegetable(VegetableTypes[0]);
        choose2.SetVegetable(VegetableTypes[0]);
        id1 = 0;
        id2 = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(P1.controller.left))
        {
            if (choose1.SelectedType == null)
            {
                id1--;
                SoundPlayer.Play(change);
                if (id1 < 0)
                {
                    SoundPlayer.Play(change);
                    id1 = VegetableTypes.Count - 1;
                }
            }

        }
        if (Input.GetKeyDown(P1.controller.right))
        {
            if (choose1.SelectedType == null)
            {
                id1++;
                SoundPlayer.Play(change);
                if (id1 > VegetableTypes.Count - 1)
                {
                    SoundPlayer.Play(change);
                    id1 = 0;
                }
            }

        }
        if (Input.GetKeyDown(P2.controller.left))
        {
            if (choose2.SelectedType == null)
            {
                id2++;
                SoundPlayer.Play(change);
                if (id2 > VegetableTypes.Count - 1)
                {
                    SoundPlayer.Play(change);
                    id2 = 0;
                }
            }

        }
        if (Input.GetKeyDown(P2.controller.right))
        {
            if (choose2.SelectedType == null)
            {
                id2--;
                SoundPlayer.Play(change);
                if (id2 < 0)
                {
                    SoundPlayer.Play(change);
                    id2 = VegetableTypes.Count - 1;
                }
            }
        }
        
        choose1.SetVegetable(VegetableTypes[id1]);
        choose2.SetVegetable(VegetableTypes[id2]);

        if (Input.GetKeyDown(P1.controller.attack))
        {
            if (choose1.SelectedType == null)
            {
                choose1.Select(choose1.VegetableType);
                SoundPlayer.Play(select);
            }
            else choose1.Unselect();

        }
        if (Input.GetKeyDown(P2.controller.attack))
        {
            if (choose2.SelectedType == null)
            {
                choose2.Select(choose2.VegetableType);
                SoundPlayer.Play(select);
            }
            else choose2.Unselect();
        }

        if (choose1.SelectedType != null && choose2.SelectedType != null)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        StartCoroutine(StartG());
    }

    IEnumerator StartG()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<SessionLogic>().StartGame(choose1.SelectedType, choose2.SelectedType);
        gameObject.SetActive(false);
    }
}
