    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField] TMP_Text nextMap;

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

    private void Start()
    {
        switch (rnd)
        {
            case 0:
                nextMap.text = "Cледующая карта: Парящий остров";
                break;
            case 1:
                nextMap.text = "Cледующая карта: Колизей";
                break;
            case 2:
                nextMap.text = "Cледующая карта: Заброшенная крепость";
                break;

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
