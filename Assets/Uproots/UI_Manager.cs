using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [Header("По порядку по индексам и игрокам!!")] 
    [SerializeField] private List<Image> portraits;

    [SerializeField] private List<PointsBar> scores;

    [SerializeField] private List<Sprite> victoryScreens;

    private void Start()
    {
        foreach (var portrait in portraits)
        {
            portrait.GetComponentInChildren<RespawnTimer>().gameObject.SetActive(false);
        }
    }

    public HealthBar GetHealthBar(int id)
    {
        return portraits[id].GetComponentInChildren<HealthBar>();
    }
    public BuffBar GetBuffBar(int id)
    {
        return portraits[id].GetComponentInChildren<BuffBar>();
    }
    public PointsBar GetPointsBar(int id)
    {
        return scores[id];
    }

    public Image GetPortrait(int id)
    {
        return portraits[id].GetComponent<Image>();
    }

    public GameObject GetRespawnTimer(int id)
    {
        return portraits[id].GetComponentInChildren<RespawnTimer>().gameObject;
    }

    public Sprite GetVictoryScreen(int id)
    {
        return victoryScreens[id];
    }

    public CooldownSlider GetCooldownSlider(int id)
    {
        return portraits[id].GetComponentInChildren<CooldownSlider>();
    }
}
