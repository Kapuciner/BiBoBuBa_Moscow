using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public Rigidbody rb;

    public float lifeTime = 3f;
    private float _time = 0;

    public R_Player Owner;
    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector3 dir, float _speed, R_Player owner)
    {
        rb.velocity = dir * _speed;
        Owner = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<R_Player>().Equals(Owner) == false)
        {
            other.GetComponent<R_Player>().TakeDamage(Owner.controller.GetDamage());
        }
    }
}
