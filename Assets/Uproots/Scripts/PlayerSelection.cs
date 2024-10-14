using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public SelectionInterface LeftSelection;
    private class Selection 
    {
        public R_Player assignedPlayer;
        public SelectionInterface selectionInterface;
        public int selectionIndex;

        public Selection(int playerIndex, VegetableType vegetableType, int _selectionIndex)
        {   
            var interfaces = FindObjectsOfType<SelectionInterface>();
            foreach (var i in interfaces)
            {
                if (i.CorrespondingPlayerIndex == playerIndex)
                {
                    this.selectionInterface = i;
                }
            }
            selectionInterface.SetVegetable(vegetableType);
            selectionIndex = _selectionIndex;
            var players = FindObjectsOfType<R_Player>();
            foreach (var player in players)
            {
                if (player.GetIndex() == playerIndex)
                {
                    this.assignedPlayer = player;
                    return;
                }
            }
            throw new IndexOutOfRangeException("Failed to find player with index " + playerIndex.ToString());
        }
    }
    
    public R_Player P1;
    public R_Player P2;

    //public SelectionInterface selection1;
    //public SelectionInterface selection2;
    private Selection _selectionLeft;
    private Selection _selectionRight;
    private int id1;
    private int id2;

    public AudioClip change;
    public AudioClip select;
    public SoundPlayer SoundPlayer;

    private bool _gameStarted = false;
    
    public List<VegetableType> VegetableTypes;

    private void Start()
    {
        _selectionLeft = new Selection(0, VegetableTypes[0], 0);
        _selectionRight = new Selection(1, VegetableTypes[0], 0);

        //selection1.SetVegetable(VegetableTypes[0]);
        //selection2.SetVegetable(VegetableTypes[0]);
        //id1 = 0;
        //id2 = 0;
        //choose1.Select(choose1.VegetableType);
        //choose2.Select(choose2.VegetableType);
    }

    public void ShiftSelection(int playerIndex, int increment)
    {
        Selection s;
        if (playerIndex == 0)
        {
            s = _selectionLeft;
        }
        else
        {
            s = _selectionRight;
        }
        if (s.selectionInterface.SelectedType == null)
        {
            s.selectionIndex += increment;
            SoundPlayer.Play(change);
            if (s.selectionIndex < 0)
            {
                s.selectionIndex = VegetableTypes.Count - 1;
            }
            else if (s.selectionIndex > VegetableTypes.Count - 1)
            {
                s.selectionIndex = 0;
            }
            s.selectionInterface.SetVegetable(VegetableTypes[s.selectionIndex]);
        }
    }
    private void Update()
    {
        
        if (_selectionLeft.selectionInterface.SelectedType != null 
            && _selectionRight.selectionInterface.SelectedType != null)
        {
            StartGame();
        }
    }

    public void SelectPlayer(int playerIndex)
    {
        if (_gameStarted)
        {
            return;
        }
        Selection s;
        if (playerIndex == 0)
        {
            s = _selectionLeft;
        }
        else
        {
            s = _selectionRight;
        }

        if (s.selectionInterface.SelectedType == null)
        {
            s.selectionInterface.Select(s.selectionInterface.VegetableType);
            SoundPlayer.Play(select);
        }
        else s.selectionInterface.Unselect();
    }
    private void StartGame()
    {
        _gameStarted = true;
        StartCoroutine(StartG());
    }

    IEnumerator StartG()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<SessionLogic>().StartGame(_selectionLeft.selectionInterface.SelectedType,
            _selectionRight.selectionInterface.SelectedType);
        gameObject.SetActive(false);
        if (FindObjectOfType<pauseManager>() != null)
            FindObjectOfType<pauseManager>().canPause = true;
        
    }
}
