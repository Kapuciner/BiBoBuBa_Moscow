using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;

    public List<GameObject> Players;

    private Camera camera;

    private float _distance;
    private Vector3 center;

    private float dx;
    private float dy;
    private float dz;

    public float minSize;
    public float maxSize;

    public List<float> ZoomValues;

    private void Start()
    {
        Players = new List<GameObject>();
        camera = Camera.main;
        if (Player1 != null)
        {
            Players.Add(Player1);
        }
        if (Player2 != null)
        {
            Players.Add(Player2);
        }
        if (Player3 != null)
        {
            Players.Add(Player3);
        }
        if (Player4 != null)
        {
            Players.Add(Player4);
        }
    }

    public void SetPlayers(R_Player P1, R_Player P2, R_Player P3, R_Player P4)
    {
        Players = new List<GameObject>();
        if (P1 != null)
        {
            Player1 = P1.gameObject;
            Players.Add(Player1);
        }
        if (P2 != null)
        {
            Player2 = P2.gameObject;
            Players.Add(Player2);
        }
        if (P3 != null)
        {
            Player3 = P3.gameObject;
            Players.Add(Player3);
        }
        if (P4 != null)
        {
            Player4 = P4.gameObject;
            Players.Add(Player4);
        }
    }

    private List<GameObject> AlivePlayers()
    {
        List<GameObject> result = new List<GameObject>();
        foreach (var player in Players)
        {
            if (player.activeSelf)
            {
                result.Add(player);
            }
        }

        return result;
    }
    private bool AllDead()
    {
        foreach (var player in Players)
        {
            if (player.activeSelf == true)
            {
                return false;
            }
        }
        return true;
    }

    private Vector3 LeftBottomCorner()
    {
        Vector3 result;
        float minZ = Mathf.Infinity;
        float minX = Mathf.Infinity;
        foreach (var player in AlivePlayers())
        {
            if (player.transform.position.z < minZ)
            {
                minZ = player.transform.position.z;
            }
            if (player.transform.position.x < minX)
            {
                minX = player.transform.position.x;
            }
        }

        result = new Vector3(minX, 0, minZ);
        return result;
    }
    private Vector3 RightTopCorner()
    {
        Vector3 result;
        float maxZ = Mathf.NegativeInfinity;
        float maxX = Mathf.NegativeInfinity;
        foreach (var player in AlivePlayers())
        {
            if (player.transform.position.z > maxZ)
            {
                maxZ = player.transform.position.z;
            }
            if (player.transform.position.x > maxX)
            {
                maxX = player.transform.position.x;
            }
        }
        result = new Vector3(maxX, 0, maxZ);
        return result;
    }
    private void Update()
    {
        if (AllDead())
        {
            center = new Vector3(10, 0, -4.5f);
            StartCoroutine(CameraZoomRoutine());
        }
        else
        {
            center = (LeftBottomCorner() + RightTopCorner()) / 2;
            _distance = Vector3.Distance(RightTopCorner(),LeftBottomCorner());
            StartCoroutine(CameraZoomRoutine());
        }
        dx = -20 / 1.4f;
        dy = 20;
        dz = -20 / 1.4f;

        StartCoroutine(CameraSmoothMove(new Vector3(center.x + dx, 20, center.z + dz)));
    }

    private float GetZoomValue()
    {
        if (Player1.activeSelf == false && Player2.activeSelf == false)
        {
            return 15;
        }
        float desired = Mathf.Clamp(_distance, minSize, maxSize);
        float closestMin = 0;
        for (int i = 0; i < ZoomValues.Count; i++)
        {
            if (ZoomValues[i] >= closestMin && ZoomValues[i] <= desired)
            {
                closestMin = ZoomValues[i];
            }   
        }
        return closestMin;
    }
    private IEnumerator CameraZoomRoutine()
    {
        if (Math.Abs(GetZoomValue() - camera.orthographicSize) < 0.1f)
        {
            yield break;
        }

        float elapsed = 0;
        float time = 2f;
        float start = camera.orthographicSize;
        float end = GetZoomValue();
        while (elapsed < time)
        {
            camera.orthographicSize = Mathf.Lerp(start, end, elapsed / time);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CameraSmoothMove(Vector3 desired)
    {
        float elapsed = 0;
        float time = 1f;
        Vector3 start = camera.transform.position;
        while (elapsed < time)
        {
            camera.transform.position = Vector3.Lerp(start, desired, elapsed / time);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
