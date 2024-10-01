using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool CanReloadAbility = true;
    
    private Rigidbody _rb;
    private VegetableType _type;
    private float _speedMultiplier = 1;
    public Vector3 _move;
    private Vector3 _moveRaw;
    private Vector3 _lastDirection;
    private Vector3 _lastDirectionSmooth;
    [Header("Settings")]
    [SerializeField] private float smoothSpeed;
    [Header("Controls")]
    [SerializeField] public KeyCode up;
    [SerializeField] public KeyCode down;
    [SerializeField] public KeyCode left;
    [SerializeField] public KeyCode right;
    [SerializeField] public KeyCode attack;
    [SerializeField] public KeyCode skill;
    [SerializeField] public float pushForce;
    [Header("")] [SerializeField] private GameObject projectile;
    public delegate void PlayerAction();

    public CooldownSlider CooldownSlider;
    
    public float _abilityTimer = 0;
    public float _attackTimer = 0;

    public bool CanAttack = true;
    public bool CanCast = true;

    public Attack PlayerAttack;
    public Ability PlayerAbility;
    
    
    public PlayerAction PlayerAttacks;
    public PlayerAction PlayerCasts;

    private bool _canMove = true;
    private void Start()
    {

        
        
        _move = Vector3.zero;

        Reload();

    }

    public void Reload()
    {
        _rb = GetComponent<Rigidbody>();
        _type = GetComponent<Player>().VegetableType;
        _rb.mass = _type.mass;
        GetAttacks();
        
        CooldownSlider.SetMaxValue(_type.abilityCooldown);
    }

    public void AddReloadProgress(float value)
    {
        value = Mathf.Clamp(value, 0, 1);
        _abilityTimer -= value * _type.abilityCooldown;

        if (_abilityTimer < 0)
        {
            _abilityTimer = 0;
        }
    }
    public void SetReloadProgress(float value)
    {
        value = Mathf.Clamp(value, 0, 1);
        _abilityTimer = value * _type.abilityCooldown;
        if (_abilityTimer < 0)
        {
            _abilityTimer = 0;
        }
    }
    public void SetMass(float value)
    {
        _rb.mass = value;
    }
    private void GetAttacks()
    {
        switch (_type.Skills)
        {
            case VegetableType.SkillSet.tomato:
                PlayerAttack = new TomatoMeleeAttack(2f, pushForce * 2, this);
                PlayerAbility = new Pomidor(12, 10,
                    10, 10, GetComponent<Player>());
                break;
            case VegetableType.SkillSet.watermelon:
                PlayerAttack = new MelonAttack(GetComponent<Player>(), 10);
                PlayerAbility = new MelonAbility(GetComponent<Player>(), 10f);
                break;
            case VegetableType.SkillSet.garlic:
                PlayerAttack = new GarlicAttack(GetComponent<Player>());
                PlayerAbility = new GarlicAbility(GetComponent<Player>());
                break;
        }
    }

    public Vector3 GetDirection()
    {
        if (_lastDirection.magnitude == 0)
        {
            return Vector3.right;
        }
        return _lastDirection.normalized;
    }
    public Vector3 GetDirectionSmooth()
    {
        if (_lastDirectionSmooth.magnitude == 0)
        {
            return Vector3.right;
        }
        return _lastDirectionSmooth.normalized;
    }
    private void Update()
    {
        HandleInput();
        if (Input.GetKeyDown(attack) && _attackTimer == 0 && CanAttack)
        {
            Attack();
            PlayerAttacks?.Invoke();
        }
        if (Input.GetKeyDown(skill) && _abilityTimer == 0 && CanCast)
        {
            Cast();
            PlayerCasts?.Invoke();
        }

        if (_abilityTimer > 0)
        {
            if (CanReloadAbility)
            {
                _abilityTimer -= Time.deltaTime;
            }
            if (_abilityTimer < 0)
            {
                _abilityTimer = 0;
            }
        }
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer < 0)
            {
                _attackTimer = 0;
            }
        }
        CooldownSlider.SetValue(_type.abilityCooldown - _abilityTimer);
    }

    public void SetCanMove(bool value)
    {
        _canMove = value;
    }

    public void StopImmediate()
    {
        _move = Vector3.zero;
    }
    private void Attack()
    {
        // Projectile p = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        // p.Launch(_move.normalized, 10f, GetComponent<Player>());
        
        GetComponent<Player>().SoundPlayer.Play(_type.ShootSound);
        
        PlayerAttack.Execute();
        _attackTimer = _type.attackCooldown;
    }

    public int GetDamage()
    {
        return GetComponent<Player>().damage;
    }
    private void FixedUpdate()
    {
        //_rb.AddForce(_move * 100 * _speedMultiplier, ForceMode.Force);
        _rb.MovePosition(_rb.position + _move * _speedMultiplier * _type.speed);
    }

    private void Cast()
    {
        PlayerAbility.Cast();

        if (_type.Skills != VegetableType.SkillSet.watermelon)
        {
            GetComponent<Player>().SoundPlayer.Play(_type.AbilitySound);
        }

        _abilityTimer = _type.abilityCooldown;
    }

    private void HandleInput()
    {
        Vector3 newMove;
        newMove = Vector3.zero;
        if (Input.GetKey(up))
        {
            newMove += new Vector3(1f, 0f, 1f);
        }
        if (Input.GetKey(down))
        {
            newMove -= new Vector3(1f, 0f, 1f);
        }
        if (Input.GetKey(right))
        {
            newMove += new Vector3(1f, 0f, -1f);
        }
        if (Input.GetKey(left))
        {
            newMove -= new Vector3(1f, 0f, -1f);
        }
        newMove.Normalize();
        _moveRaw = newMove;
        if (_moveRaw.magnitude != 0)
        {
            _lastDirection = _moveRaw;
        }

        if (_canMove == false)
        {
            newMove = Vector3.zero;
        }
        StartCoroutine(LerpMove(newMove));
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player._type.mass <= _type.mass)
            {
                Vector3 dir = -Vector3.Normalize(transform.position - player.transform.position);
                dir.y = 0;
                player._rb.AddForce(dir * pushForce, ForceMode.Impulse);
            }
        }
        
    }

    public void Push(float power, Vector3 dir)
    {
        _rb.AddForce(dir * power, ForceMode.Impulse);
    }

    public void Drag(float power, Vector3 dir)
    {
        _rb.MovePosition(_rb.position + dir * power);
    }
    public float GetVelocity()
    {
        float v = (_move * _speedMultiplier * _type.speed).magnitude;
        if (v <= 0.005f)
        {
            v = 0;
        }
        return v;
    }
    public void SetSpeedMultiplier(float value)
    {
        _speedMultiplier = value;
    }

    public void SetPushForce(float value)
    {
        pushForce = value;
    }
}
