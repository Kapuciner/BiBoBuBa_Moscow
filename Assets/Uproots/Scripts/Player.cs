using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public VegetableType VegetableType;
    
    [SerializeField] public PlayerController controller;

    public SoundPlayer SoundPlayer;
    
    public HealthBar HealthBar;
    public BuffBar BuffBar;
    public PointsBar PointsBar;

    public Image Portrait;
        
    public float Points = 0;
    public GameObject SpawnPoint;
    public GameObject RespawnTimer;

    private float _invincibilityTime = 1f;
    private bool _isInvincible = true;
    private float _invincibilityTimer = 0;

    public Sprite VictoryScreen;
    
    public AnimationCurve slowCurve;    


    public int health { get; private set; }
    public int damage { get; private set; }

    private float _rootProgress;
    private RootZone _zone;

    public PlayerController.PlayerAction PlayerDies;
    private void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.clear;
        Reset(VegetableType);
        
        controller.SetCanMove(false);
        controller.CanAttack = false;
    }
    
    public void Reset(VegetableType vegetableType)
    {
        damage = 1;
        _zone = GameObject.FindObjectOfType<RootZone>();
        _rootProgress = 0;
        controller.SetCanMove(true);
        controller.CanAttack = true;
        VegetableType = vegetableType;
        health = vegetableType.health;
        HealthBar.CreateContainers(health);
        controller.SetSpeedMultiplier(1);
        GetComponent<SpriteRenderer>().color = Color.white;

        Portrait.sprite = vegetableType.Portrait;
        GetComponent<PlayerAnimator>().SetAnimator(vegetableType.Animator);
        GetComponent<PlayerAnimator>().SetAnimation(PlayerAnimator.PLAYER_IDLE);
        transform.position = SpawnPoint.transform.position;
        controller.SetCanMove(true);
        controller._move = Vector3.zero;
        controller.CanAttack = true;

        controller._abilityTimer = vegetableType.abilityCooldown;
        controller._attackTimer = vegetableType.attackCooldown;
        controller.Reload();
        
        
    }

    private void Update()
    {
        HealthBar.UpdateContainers(health);
        if (_rootProgress >= _zone.RootTime)
        {
            _rootProgress = _zone.RootTime;
            _zone.RootPlayer(this);
            controller.SetMass(100f);
        }

        if (_isInvincible)
        {
            _invincibilityTimer += Time.deltaTime;
        }

        if (_invincibilityTimer >= _invincibilityTime)
        {
            _isInvincible = false;
            _invincibilityTimer = 0;
        }

        if (transform.position.y <= -3f)
        {
            
            PlayerDies?.Invoke();
        }
    }


    public void SetDamage(int d)
    {
        damage = d;
    }
    public void SetVegetable(VegetableType vegetableType)
    {
        VegetableType = vegetableType;
    }

    public void AddBuff(Sprite sprite, float time)
    {
        BuffBar.Add(sprite, time);
    }
    public void RestoreHealth()
    {
        health = VegetableType.health;
    }
    public void TakeDamage(int amount)
    {
        if (_isInvincible)
        {
            return;
        }
        health -= amount;
        StartCoroutine(Blink(_invincibilityTime));
        if (health <= 0)
        {
            health = 0;
            SoundPlayer.Play(VegetableType.DeathSound); 
            PlayerDies?.Invoke();
        }
        _isInvincible = true;
        _invincibilityTimer = 0;
    }

    private IEnumerator Blink(float duration)
    {
        float elapsed = 0;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color color = sprite.color;
        Color newColor = color;
        while (elapsed < duration)
        {
            float a = Mathf.Sin(Time.timeSinceLevelLoad * 20) * 0.7f + 1;
            sprite.color = new Color(color.r, color.g, color.b, a);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        sprite.color = color;
    }

    public Vector3 GetDirection()
    {
        return controller.GetDirection();
    }
    public Vector3 GetDirectionSmooth()
    {
        return controller.GetDirectionSmooth();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            if (_zone.CurrentPlayer == null)
            {
                _zone.CurrentPlayer = this;
                _zone.StartRooting();
            }
            
            if (_zone.CurrentPlayer == this)
            {
                _rootProgress += Time.deltaTime;
                controller.SetSpeedMultiplier(slowCurve.Evaluate(_rootProgress / _zone.RootTime));
            }
        }
    }

    public float GetRootingProgress()
    {
        return _rootProgress / _zone.RootTime;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zone") && _zone.CurrentPlayer == this)
        {
            _zone.ClearZone();
            _rootProgress = 0;
            controller.SetMass(VegetableType.mass);
            controller.SetSpeedMultiplier(1);
        }
    }

    private void OnDisable()
    {
        if (_zone.CurrentPlayer == this)
        {
            _zone.ClearZone();
            _rootProgress = 0;
            controller.SetMass(VegetableType.mass);
            controller.SetSpeedMultiplier(1);
        }

        foreach (var system in GetComponentsInChildren<ParticleSystem>())
        {
            GameObject.Destroy(system);
        }
    }
}
