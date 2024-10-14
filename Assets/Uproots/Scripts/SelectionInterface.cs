using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionInterface : MonoBehaviour
{
    public VegetableType VegetableType;
    public Image image;

    public VegetableType SelectedType;
    public Image ready;
    public Image P;

    public int CorrespondingPlayerIndex;

    public AspectRatioFitter AspectRatioFitter;
    
    
    public void SetVegetable(VegetableType vegetableType)
    {
        VegetableType = vegetableType;
        AspectRatioFitter.aspectRatio = vegetableType.spriteAspectRatio;
        if (CorrespondingPlayerIndex == 0)
        {
            image.sprite = vegetableType.Sprite; 
        }
        else image.sprite = vegetableType.Red_Sprite;
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
