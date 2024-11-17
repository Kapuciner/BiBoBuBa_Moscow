using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    public class Selection 
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
    private Selection _selection1;
    private Selection _selection2;
    private Selection _selection3;
    private Selection _selection4;
    private int id1;
    private int id2;

    public AudioClip change;
    public AudioClip select;
    public SoundPlayer SoundPlayer;

    private bool _gameStarted = false;
    
    public List<VegetableType> VegetableTypes;
    private List<Selection> _selections;
    private void Start()
    {
        _selections = new List<Selection>();
        _selection1 = new Selection(0, VegetableTypes[0], 0);
        _selection2 = new Selection(1, VegetableTypes[0], 0);
        _selections.Add(_selection1);
        _selections.Add(_selection2);
        if (GameObject.FindObjectOfType<PlayerSpawner>().players.Count > 2)
        {
            if (GameObject.FindObjectOfType<PlayerSpawner>().players.Count == 3)
            {
                _selection3 = new Selection(2, VegetableTypes[0], 0);
                _selections.Add(_selection3);
            }
            else if (GameObject.FindObjectOfType<PlayerSpawner>().players.Count == 4)
            {
                _selection3 = new Selection(2, VegetableTypes[0], 0);
                _selection4 = new Selection(3, VegetableTypes[0], 0);
                _selections.Add(_selection3);
                _selections.Add(_selection4);
            }
        }
        var interfaces = FindObjectsOfType<SelectionInterface>();
        foreach (var i in interfaces)
        {
            if (i.CorrespondingPlayerIndex >= GameObject.FindObjectOfType<PlayerSpawner>().players.Count)
            {
                i.gameObject.SetActive(false);
            }
        }
        //selection1.SetVegetable(VegetableTypes[0]);
        //selection2.SetVegetable(VegetableTypes[0]);
        //id1 = 0;
        //id2 = 0;
        //choose1.Select(choose1.VegetableType);
        //choose2.Select(choose2.VegetableType);
    }

    public void ShiftSelection(int playerIndex, int increment)
    {
        Selection s = GetSelectionFromIndex(playerIndex);

        
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
        bool allSelected = true;
        foreach (var selection in _selections)
        {
            if (selection.selectionInterface.SelectedType == null)
            {
                allSelected = false;
            }
        }
        if (allSelected)
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
        Selection s = GetSelectionFromIndex(playerIndex);

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
        FindObjectOfType<SessionLogic>().StartGame(_selections);
        gameObject.SetActive(false);
        if (FindObjectOfType<pauseManager>() != null)
            FindObjectOfType<pauseManager>().canPause = true;
        
    }

    private Selection GetSelectionFromIndex(int playerIndex)
    {
        Selection s;
        if (playerIndex == 0)
        {
            s = _selection1;
        }
        else if (playerIndex == 1)
        {
            s = _selection2;
        }
        else if (playerIndex == 2)
        {
            s = _selection3;
        }
        else 
        {
            s = _selection4;
        }

        return s;
    }
}
