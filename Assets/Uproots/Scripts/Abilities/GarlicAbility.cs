using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicAbility : Ability
{
    private Player _caster;

    GameObject effect =
        Resources.Load("GarlicAbilityEffect") as GameObject;

    public GarlicAbility(Player caster)
    {
        _caster = caster;
    }

    public override void Cast()
    {
        GameObject.FindObjectOfType<MonoHandler>().StartCoroutine(Delay(0f));

    }

    IEnumerator Delay(float t)
    {
        List<GameObject> points = new List<GameObject>();
        yield return new WaitForSeconds(t);
        float elapsed = 0;
        while (elapsed <= 1.5f)
        {
            if (elapsed < 3f)
            {
                Vector3 pos = _caster.transform.position;
                ParticleSystem ps = GameObject.Instantiate(effect, pos,
                    Quaternion.identity).GetComponent<ParticleSystem>();
                var main = ps.main;
                main.startLifetime = 6f - elapsed;
                points.Add(ps.gameObject);
            }

            foreach (var point in points)
            {
                Collider[] res = Physics.OverlapSphere(point.transform.position, 1f);
                foreach (var c in res)
                {
                    if (c.CompareTag("Player") && c.GetComponent<Player>() != _caster)
                    {
                        c.GetComponent<Player>().TakeDamage(_caster.controller.GetDamage());
                    }
                }
            }

            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        foreach (var point in points)
        {
            GameObject.Destroy(point.gameObject);
        }
    }
}

