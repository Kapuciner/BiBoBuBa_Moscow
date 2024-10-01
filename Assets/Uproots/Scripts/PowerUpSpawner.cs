using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpSpawner : MonoBehaviour
{
    public List<GameObject> Powerups;

    public GameObject current;

    public float time;
    private float timer;
    private bool timerStarted = false;
    
    public void Spawn(GameObject powerup)
    {
        timerStarted = false;
        timer = 0;
        Vector3 pos = new Vector3(0, powerup.GetComponent<Powerup>().offset.y - 1.28f, 0);
        GameObject a = Instantiate(powerup, transform.position + pos, transform.rotation, transform);
    }

    private void Update()
    {
        if (timerStarted)
        {
            timer += Time.deltaTime;
        }
        if (gameObject.GetComponentInChildren<Powerup>() == null)
        {
            if (timerStarted == false)
            {
                timer = 0;
                timerStarted = true;
                if (Time.timeSinceLevelLoad >= time)
                {
                    GetComponent<SoundPlayer>().Play(GetComponent<AudioSource>().clip);
                }

            }
        }

        if (timer >= time)
        {
            Spawn(RandomPowerup());
        }
    }

    private GameObject RandomPowerup()
    {
        return Powerups[Random.Range(0, Powerups.Count)];
    }
}
