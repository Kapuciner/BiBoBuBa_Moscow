    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaMapGenerator : MonoBehaviour
{
    static private int previousID;
    [SerializeField] private GameObject[] maps;
    private int rnd;

    [SerializeField] private Transform[] spawners;
    [SerializeField] private Transform[] spawnPointsMap1;
    [SerializeField] private Transform[] spawnPointsMap2;
    [SerializeField] private Transform[] spawnPointsMap3;
    List<Transform[]> allSpawnPoints;

    void Awake()
    {
        allSpawnPoints = new List<Transform[]> { spawnPointsMap1, spawnPointsMap2, spawnPointsMap3 };
        GetRnd();
        Instantiate(maps[rnd]);

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].position = allSpawnPoints[rnd][i].position;
            print(allSpawnPoints[rnd][i]);
        }
    }

    void GetRnd()
    {
        rnd = Random.Range(0, maps.Length);

        if (rnd == previousID && maps.Length > 1)
            GetRnd();
        else
            previousID = rnd;
    }

}
