using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicAttack : Attack
{
    private Player _attacker;
    public GarlicAttack(Player attacker)
    {
        _attacker = attacker;
    }
    GameObject effect =
        Resources.Load("GarlicAttackEffect") as GameObject;
    public override void Execute()
    {
        GameObject.FindObjectOfType<MonoHandler>().StartCoroutine(Delay(0f));

    }
            
    private IEnumerator Delay(float t)
    {
        yield return new WaitForSeconds(t);
        Vector3 dir = _attacker.GetDirectionSmooth();
        if (dir.magnitude == 0)
        {
            dir = Vector3.right;
        }
        
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, dir);
        
        
        
        GameObject.Instantiate(effect, _attacker.transform.position, rot, _attacker.transform);

        float elapsed = 0;
        while (elapsed <= 0.5f)
        {
            Vector3 pos = _attacker.transform.position + _attacker.GetDirectionSmooth();
            Collider[] res = Physics.OverlapSphere(pos, 3f);
            foreach (var c in res)
            {
                if (c.CompareTag("Player") && c.GetComponent<Player>() != _attacker)
                {
                    c.GetComponent<Player>().TakeDamage(_attacker.controller.GetDamage());
                    yield break;
                }
            }
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
            

    }
}
