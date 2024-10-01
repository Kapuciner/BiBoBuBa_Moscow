using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Roots : MonoBehaviour
{
    public int N;
    public LineRenderer Prefab;
    public LineRenderer[] LineRenderer;
    public Player player;
    private Vector3[] lastDir;
    private Vector3[] lastPos;
    private float stepLength = 0.1f;
    public float PulseSpeed;
    public float PulsePower;
    public int MaxSteps;
    private int steps = 0;


    private Coroutine growRoutine;
    private void Awake()
    {
        LineRenderer = new LineRenderer[N];
        for (int i = 0; i < N; i++)
        {
            LineRenderer[i] = Instantiate(Prefab, transform);
        }
    }

    public void Setup(Player _player)
    {
        if (_player == null)
        {
            return;
        }
        foreach (var lineRenderer in LineRenderer)
        {
            lineRenderer.positionCount = 0;
        }

        player = _player;
        
        lastDir = new Vector3[N];
        lastPos = new Vector3[N];
        for (int i = 0; i < N; i++)
        {
            float x = UnityEngine.Random.Range(-1f, 1f);
            float z = UnityEngine.Random.Range(-1f, 1f);
            lastDir[i] = new Vector3(x, 0, z);
            lastPos[i] = player.transform.position;
        }
    }

    private void Update()
    {
        // foreach (var lineRenderer in LineRenderer)
        // {
        //     if (lineRenderer.positionCount > 0)
        //     {
        //         lineRenderer.SetPosition(0, player.transform.position);
        //     }
        // }
    }

    public void Clear()
    {
        foreach (var lineRenderer in LineRenderer)
        {
            if (this != null)
            {
                StartCoroutine(Clear(lineRenderer));
            }
        } 
       
    }

    public void ClearInTime(float time)
    {
        foreach (var lineRenderer in LineRenderer)
        {
            StartCoroutine(ClearInTime(lineRenderer, time));
        } 
    }

    public void ClearImmediate()
    {
        foreach (var lineRenderer in LineRenderer)
        {
            lineRenderer.positionCount = 0;
        } 
    }

    public void Pulse()
    {
        float startThickness = 0.1f;
        foreach (var line in LineRenderer)
        {
            float w = startThickness +
                      Mathf.Sin(Time.timeSinceLevelLoad * PulseSpeed) * PulsePower;
            line.startWidth = w;
            line.endWidth = w;
        }
    }

    IEnumerator Clear(LineRenderer lineRenderer)
    {
        while (lineRenderer.positionCount > 0)
        {
            lineRenderer.positionCount--;
            yield return new WaitForSeconds(0.02f);
        }
    }
    IEnumerator ClearInTime(LineRenderer lineRenderer, float time)
    {
        float step = time / lineRenderer.positionCount;
        while (lineRenderer.positionCount > 0)
        {
            lineRenderer.positionCount--;
            yield return new WaitForSeconds(step);
        }
    }

    public void Grow()
    {
        growRoutine = StartCoroutine(Growing());
    }

    public void StopGrowing()
    {
        StopCoroutine(growRoutine);
    }
    private IEnumerator Growing()
    {
        while (true)
        {
            MakeStep();
            yield return new WaitForSeconds(0.03f);
        }
    }
    public void MakeStep()
    {
        for (int i = 0; i < N; i++)
        {
            LineRenderer[i].positionCount++;
            Vector3 dir = Rotate(lastDir[i], UnityEngine.Random.Range(-45f, 45f));
            Vector3 pos = lastPos[i] + dir * stepLength;
            LineRenderer[i].SetPosition(LineRenderer[i].positionCount - 1, pos);
            lastPos[i] = pos;
            lastDir[i] = dir; 
        }
    }
    public Vector3 Rotate(Vector3 v, float degrees)
    {
        float DegToRad = Mathf.PI/180;
        return RotateRadians(v, degrees * DegToRad);
    }

    public Vector3 RotateRadians(Vector3 v, float radians)
    {
        var ca = Mathf.Cos(radians);
        var sa = Mathf.Sin(radians);
        return new Vector3(ca*v.x - sa*v.z,v.y, sa*v.x + ca*v.z);
    }
}
