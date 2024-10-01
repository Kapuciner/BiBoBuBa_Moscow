using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelonAbility : Ability
{
    private LineRenderer lineRenderer;
    public MelonAbility(Player caster, float speed)
    {
        _caster = caster;
        _speed = speed;
        lineRenderer = _caster.GetComponentInChildren <LineRenderer>();
    }
    
    GameObject projectile =
        Resources.Load("Projectile") as GameObject;
    
    public override void Cast()
    {
        GameObject.FindObjectOfType<MonoHandler>().StartCoroutine(Delay(0.6f));
    }
    
    private Player _caster;
    private float _speed;
    IEnumerator Delay(float t)
    {
        lineRenderer.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        _caster.controller.SetCanMove(false);
        _caster.controller.StopImmediate();
        
        float duration = 6f;
        float elapsed = 0;
        float cd = 0.65f;
        float timer = cd;
        while (elapsed < duration)
        {
            DrawDirection();
            _caster.controller.SetCanMove(false);
            if (timer >= cd)
            {
                Projectile p = GameObject.Instantiate(projectile,
                    _caster.transform.position, Quaternion.identity).GetComponent<Projectile>();
                p.Launch(_caster.GetDirectionSmooth(), _speed, _caster);
                
                _caster.SoundPlayer.Play(_caster.VegetableType.ShootSound);
                p.GetComponentInChildren<SpriteRenderer>().color = Color.black;
                timer = 0;
            }


            elapsed += Time.deltaTime;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _caster.controller.SetCanMove(true);
        lineRenderer.gameObject.SetActive(false);

    }

    public void DrawDirection()
    {
        Vector3 pos1 = _caster.transform.position;
        Vector3 pos2 = _caster.transform.position + _caster.GetDirectionSmooth() * 3f;
        
        LineRenderer lineRenderer = _caster.GetComponentInChildren <LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, pos1);
        lineRenderer.SetPosition(1, pos2);
    }
}
