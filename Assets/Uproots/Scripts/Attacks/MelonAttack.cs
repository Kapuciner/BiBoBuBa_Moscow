using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MelonAttack : Attack
{
    private Player _attacker;
    private Vector3 _dir;
    private float _speed;
    GameObject projectile =
        Resources.Load("Projectile") as GameObject;
    public MelonAttack(Player attacker, float speed)
    {
        _attacker = attacker;
        
        _speed = speed;
    }

    public override void Execute()
    {
        GameObject.FindObjectOfType<MonoHandler>().StartCoroutine(Delay(0.5f));
    }
    private IEnumerator Delay(float t)
    {
        _dir = _attacker.GetDirection();
        yield return new WaitForSeconds(t);
        Projectile p = GameObject.Instantiate(projectile,
            _attacker.transform.position, Quaternion.identity).GetComponent<Projectile>();
        p.GetComponentInChildren<SpriteRenderer>().color = Color.black;
        p.Launch(_dir, _speed, _attacker);
    }
}
