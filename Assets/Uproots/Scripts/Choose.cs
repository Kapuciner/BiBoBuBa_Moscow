using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choose : MonoBehaviour
{
    public VegetableType VegetableType;
    public Image image;

    public VegetableType SelectedType;
    public Image ready;
    public Image P;
    
    public void SetVegetable(VegetableType vegetableType)
    {
        VegetableType = vegetableType;
        image.sprite = vegetableType.Sprite;
    }

    public void Select(VegetableType vegetableType)
    {
        SelectedType = vegetableType;
        
        ready.gameObject.SetActive(true);
        P.gameObject.SetActive(false);
    }

    public void Unselect()
    {
        SelectedType = null;
        
        ready.gameObject.SetActive(false);
        P.gameObject.SetActive(true);
    }

    private void Update()
    {
        
    }
}
