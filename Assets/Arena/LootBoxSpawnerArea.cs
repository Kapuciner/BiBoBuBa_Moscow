using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootBoxSpawnerArea : MonoBehaviour
{
    bool goodPlaceToSpawn = false;
    [SerializeField] private GameObject SpawnCheckObject;
    [SerializeField] private MeshCollider targetCollider; 
    [SerializeField] private float spawnInterval = 5f; 
    [SerializeField] private GameObject lootBox;

    [SerializeField] private TMP_Text timerText;
    private float timer; 

    private void Start()
    {

        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {

            goodPlaceToSpawn = false;

            while (!goodPlaceToSpawn)
            {
                FindPlaceToSpawn();
                timer = spawnInterval;
                yield return null; 
            }
            yield return StartCoroutine(LootBoxTimer()); 
        }
    }

    private void FindPlaceToSpawn()
    {
        Bounds bounds = targetCollider.bounds;
        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );

        SpawnCheckObject.transform.position = randomPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("spawner"))
        {

            Instantiate(lootBox, SpawnCheckObject.transform.position, Quaternion.identity);
            goodPlaceToSpawn = true; 
            SpawnCheckObject.transform.position += Vector3.up * 50;
        }
    }

    private IEnumerator LootBoxTimer()
    {
        timer = spawnInterval;
           
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = $"{Mathf.Ceil(timer)}"; 
            yield return null;
        }

        yield return null;
    }

}