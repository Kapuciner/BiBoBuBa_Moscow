using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pomidor : Ability
{
    
    
    GameObject projectile =
        Resources.Load("Projectile") as GameObject;

    public Pomidor(int _num, float _speed, float _damage, float _pushForce, Player caster)
    {
        numDirections = _num;
        speed = _speed;
        damage = _damage;
        pushForce = _pushForce;
        this.caster = caster;

    }

    private Player caster;
    private int numDirections;
    private float speed;
    private float damage;
    private float pushForce;
    
    public override void Cast()
    {
        GameObject.FindObjectOfType<MonoHandler>().StartCoroutine(Delay(0.6f));
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

    IEnumerator Delay(float t)
    {
        List<Vector3> directions = new List<Vector3>();
        for (int i = 0; i < numDirections; i++)
        {
            directions.Add(Rotate(new Vector3(1,0,0),
                360f * i / numDirections));
        }
        
        yield return new WaitForSeconds(t);

        foreach (var direction in directions)
        {
            Projectile p = GameObject.Instantiate(projectile,
                caster.transform.position, Quaternion.identity).GetComponent<Projectile>();
            p.Launch(direction, speed, caster);
        }
    }
}
