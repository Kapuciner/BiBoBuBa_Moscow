using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private int amount;
    public List<Image> containers;
    private GameObject containerPrefab;
    private void Awake()
    {
        containerPrefab = Resources.Load("Container") as GameObject;
    }


    public void CreateContainers(int n)
    {
        amount = n;
        if (containers.Count != 0)
        {
            foreach (var container in containers)
            {
                Destroy(container.gameObject);
            }
            containers.Clear();
            containers = new List<Image>();

        }
        for (int i = 0; i < n; i++)
        {
            Image c = Instantiate(containerPrefab, transform).GetComponent<Image>();
            containers.Add(c);
        }
    }
    public void UpdateContainers(int value)
    {
        if (value < 0)
        {
            value = 0;
        }
        for (int i = 0; i < amount; i++)
        {
            containers[i].GetComponent<HeartContainer>().SetFull(false);
        }
        for (int i = 0; i < value; i++)
        {
            containers[i].GetComponent<HeartContainer>().SetFull(true);
        }


    }
}
