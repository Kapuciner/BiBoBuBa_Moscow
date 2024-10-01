using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
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
        camera = Camera.main;
    }

    private void Update()
    {
        if (Player1.activeSelf == false && Player2.activeSelf == false)
        {
            center = new Vector3(10, 0, -4.5f);
            StartCoroutine(CameraZoomRoutine());
        }
        else if (Player1.activeSelf == false)
        {
            center = Player2.transform.position;
        }
        else if (Player2.activeSelf == false)
        {
            center = Player1.transform.position;
        }
        else
        {
            center = (Player1.transform.position + Player2.transform.position) / 2;
            _distance = Vector3.Distance(Player1.transform.position, Player2.transform.position);
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
