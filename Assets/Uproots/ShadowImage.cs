using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowImage : MonoBehaviour
{
    public Player p;
    public Sprite small;
    public Sprite big;

    void Update()
    {
        if (p.VegetableType.Skills == VegetableType.SkillSet.garlic)
        {
            GetComponent<Image>().sprite = small;
        }        
        else GetComponent<Image>().sprite = big;
    }
}
