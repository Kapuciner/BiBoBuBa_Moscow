using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyDummy : MonoBehaviour
{
    private Rigidbody _rb;
    public readonly ConnectionData.PlayerControlData ControlData;

    private Vector3 _lastDirection;
    private Vector3 _lastDirectionSmooth;
    private bool _canMove;
    private Vector3 _move;
    private float smoothSpeed = 0.2f;
    public int PlayerIndex;

    public float SPEED;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _canMove = true;
    }
    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _move * SPEED);
    }
    public void Move(Vector2 dir2D)
    {
        Vector3 direction = new Vector3(dir2D.x, 0, dir2D.y);
        direction = Quaternion.AngleAxis(45, new Vector3(0, 1, 0)) * direction;
        if (direction.magnitude != 0)
        {
            _lastDirection = direction;
        }

        if (_canMove == false)
        {
            direction = Vector3.zero;
        }

        if (!gameObject.activeSelf)
        {
            return;
        }
        StartCoroutine(LerpMove(direction));
        StartCoroutine(SmoothDirection());
    }
    IEnumerator LerpMove(Vector3 newMove)
    {
        float elapsed = 0;
        Vector3 start = _move;
        while (elapsed < smoothSpeed)
        {
            if (_canMove == false)
            {
                _move = Vector3.zero;
                yield break;
            }
            _move = Vector3.Lerp(start, newMove, elapsed / smoothSpeed);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
    }
    
    IEnumerator SmoothDirection()
    {
        float elapsed = 0;
        
        Vector3 start = _lastDirectionSmooth;
        if (_lastDirectionSmooth.magnitude == 0)
        {
            start = Vector3.right;
        }
        Vector3 end;
        if (_move.magnitude == 0)
        {
            if (_lastDirection.magnitude == 0)
            {
                end = Vector3.right;
            }
            else end = _lastDirection;
        }
        else end = _move;
        while (elapsed < smoothSpeed)
        {
            _lastDirectionSmooth = Vector3.Lerp(start, end, elapsed / smoothSpeed);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    
}
