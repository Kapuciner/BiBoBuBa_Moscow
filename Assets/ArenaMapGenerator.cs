    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaMapGenerator : MonoBehaviour
{
    static private int previousID;
    [SerializeField] private GameObject[] maps;
    private int rnd;

    void Start()
    {
        GetRnd();
        Instantiate(maps[rnd]);
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
