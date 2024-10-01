using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class TomatoMeleeAttack : Attack
{
    private float radius;
    private float pushForce;
    private PlayerController attacker;
    public TomatoMeleeAttack(float radius, float pushForce, PlayerController attacker)
    {
        this.radius = radius;
        this.pushForce = pushForce;
        this.attacker = attacker;
    }
    
    
    public override void Execute()
    {
        GameObject.FindObjectOfType<MonoHandler>().StartCoroutine(Delay(0.5f));
    }

    private IEnumerator Delay(float t)
    {
        yield return new WaitForSeconds(t);
        Collider[] colliders = Physics.OverlapSphere(attacker.transform.position, radius);
        
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player") && collider.gameObject != attacker.gameObject)
            {
                collider.GetComponent<Player>().TakeDamage(attacker.GetDamage());
                collider.GetComponent<PlayerController>().Push(pushForce,
                    GetDirection(attacker.transform.position, collider.transform.position));
                
            }
        }
    }

    private Vector3 GetDirection(Vector3 attacker, Vector3 target)
    {
        return (target - attacker).normalized;
    }
}
