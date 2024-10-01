using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropGenerator : MonoBehaviour
{
    public List<Transform> tiles;
    public GameObject tilesContainer;
    public List<GameObject> tilePrefabs;
    public List<GameObject> props;
    
    public void PlaceProp(GameObject prop, Transform tile)
    {
        Vector3 displace = GetRandomDirection() * Random.Range(0f, 1f);
        Quaternion rot = Quaternion.AngleAxis(-90, new Vector3(1,0,0));
        Instantiate(prop, tile.transform.position + displace, rot);
    }

    private void Start()
    {
        tiles = tilesContainer.GetComponentsInChildren<Transform>().ToList();
        foreach (var tile in tiles)
        {
            if (tile == tilesContainer.transform)
            {
                continue;
            }
            tile.gameObject.SetActive(false);
            Quaternion rot = Quaternion.AngleAxis(-90, new Vector3(1,0,0));
            Instantiate(tilePrefabs[0], tile.transform.position, rot);
        }
        
        foreach (var tile in tiles)
        {
            PlaceProp(RandomProp(), tile);
        }
    }

    public Vector3 GetRandomDirection()
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float z = UnityEngine.Random.Range(-1f, 1f);
        return new Vector3(x, 0, z).normalized;
    }

    private GameObject RandomProp()
    {
        int i = Random.Range(0, props.Count);
        return props[i];
    }
}
