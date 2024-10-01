using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DisappearWithChance : MonoBehaviour
{
    int randNum;

    private void Start()
    {
        randNum = UnityEngine.Random.Range(1, 101);
        if (randNum < 31)
            this.gameObject.SetActive(false);
    }
}
