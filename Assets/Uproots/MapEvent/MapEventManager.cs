using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapEventManager : MonoBehaviour
{
    public float baseChance;
    private float chance;
    public float increasePerSecond;

    private bool isEventInProcess = false;

    private List<Player> _players;

    private void Start()
    {
        _players = GameObject.FindObjectsOfType<Player>().ToList();
        chance = baseChance;
    }

    public void StartEvent()
    {
        isEventInProcess = true;
        int i = Random.Range(0, 3);
        if (i == 0)
        {
            StartCoroutine(StormEvent());
        }
        if (i == 1)
        {
            StartCoroutine(RainEvent());
        }
        if (i == 2)
        {
            StartCoroutine(DryEvent());
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(StormEvent());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(RainEvent());
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(DryEvent());
        }
        if (!isEventInProcess && 
            Random.Range(0f, 100f) <= chance * Time.deltaTime &&
            Time.timeSinceLevelLoad >= 60)
        {
            StartEvent();
            chance = baseChance;
        }

        if (!isEventInProcess)
        {
            chance += Time.deltaTime * increasePerSecond;
        }
    }

    private IEnumerator StormEvent()
    {
        float elapsed = 0;
        float step = 0;
        float power = 0.01f;
        float duration = 20f;
        Vector3 direction = GetRandomDirection();
        while (elapsed < duration)
        {
            if (step >= 5f)
            {
                step = 0;
                direction = GetRandomDirection();
            }
            foreach (var player in _players)
            {
                player.controller.Drag(power, direction);
            }

            elapsed += Time.deltaTime;
            step += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator RainEvent()
    {
        float elapsed = 0;
        float duration = 20f;
        while (elapsed < duration)
        {
            foreach (var player in _players)
            {
                player.controller.AddReloadProgress(0.1f * Time.deltaTime);
            }
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator DryEvent()
    {
        float elapsed = 0;
        float duration = 20f;
        while (elapsed < duration)
        {
            foreach (var player in _players)
            {
                player.controller.SetReloadProgress(1f);
            }
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private Vector3 GetRandomDirection()
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float z = UnityEngine.Random.Range(-1f, 1f);
        return new Vector3(x, 0, z);
    }
}
