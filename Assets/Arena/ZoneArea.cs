using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneArea : MonoBehaviour
{
    [SerializeField] private Renderer zoneRenderer;
    private void Start()
    {
        zoneRenderer.sortingLayerName = "TransparentFX";
        zoneRenderer.sortingOrder = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Renderer>().sortingOrder = 3;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Renderer>().sortingOrder = 1;
        }
    }
}
