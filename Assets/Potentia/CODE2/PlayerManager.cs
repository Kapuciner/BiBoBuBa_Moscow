using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Rigidbody _rb;
    private SpriteRenderer _sr;
    [SerializeField] private player playerMage;

    [SerializeField] private KeyCode up;
    [SerializeField] private KeyCode down;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;

    
    [SerializeField] private float speed;
    private Vector3 move = Vector3.zero;
    [SerializeField] float icedMass;
    [SerializeField] LayerMask iceLayers;
    public bool iceMove = false;

    [Header("Dash Settings")]
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private float dashSpeed; 
    [SerializeField] private float dashTime; 
    [SerializeField] private int dashCooldown;
    [SerializeField] private ParticleSystem dashParticle;
    [SerializeField] private GameObject dashImage;

    [Header("Abilities")]
    public bool gotAbilityDash = false;
    public bool gotAbilityHeal = false;
    public bool gotAbilityAttack = false;
    public bool gotAbilityShield = false;
    [SerializeField] private KeyCode fireballKey;
    [SerializeField] private GameObject fireball;
    [SerializeField] private float fireballDuration;
    [SerializeField] private KeyCode shieldKey;
    [SerializeField] private GameObject shield;
    [SerializeField] private float shieldDuration;
    [SerializeField] private TMP_Text dashCooldownTimer;
    [SerializeField] private TMP_Text attackCooldownTimer;
    [SerializeField] private TMP_Text shieldCooldownTimer;
    private bool canAttack = true;
    private bool canDefend = true;
    private bool canDash = true;
    [SerializeField] private int shieldCooldown;
    [SerializeField] private int attackCooldown;
    [SerializeField] private GameObject shieldImage;
    [SerializeField] private GameObject attackImage;

    public AudioClip dashA;
    public AudioClip shieldA;
    public AudioClip attackA;
    public AudioSource audioSource;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        if(!iceMove) {
            _rb.useGravity = true;
            _rb.mass = 1;
            _rb.drag = 1;
            _rb.MovePosition(_rb.position + move * speed * Time.deltaTime);
        }
        else {
            
            _rb.drag = 0.3f;
            _rb.mass = icedMass;
            if(Physics.Raycast(transform.position, Vector3.down, 2f, iceLayers)) {
                _rb.useGravity = false;
                _rb.AddForce(move * speed * Time.deltaTime * 90);
            } else {
                _rb.useGravity = true;
                _rb.MovePosition(_rb.position + move * speed * Time.deltaTime);
            }
        }
        
    }

    public float GetVelocity()
    {
        float vel = (move * speed * Time.deltaTime).magnitude;
        if (vel <= 0.005f)
        {
            vel = 0;
        }
        return vel;
    }

    public void MoveMage(Vector2 moveVector) {
        move = Vector3.zero;
        move += new Vector3(moveVector.x, 0, moveVector.y);
        move.Normalize();
        move = Quaternion.Euler(0, 45, 0) * move;
    }

    public void TryDash() {
        if (canDash && GetVelocity() != 0 && gotAbilityDash)
        {
            audioSource.clip = dashA;
            audioSource.Play();
            ParticleSystem dashPart = Instantiate(dashParticle);
            dashPart.transform.position = this.transform.position;
            Destroy(dashPart.gameObject, 2f);

            speed *= dashSpeed;
            Invoke("StopDash", dashTime);
            canDash = false;
            dashImage.SetActive(false);
            StartCoroutine(CooldownRoutine(dashCooldown, dashCooldownTimer, dashImage, "canDash"));
        }
    }

    public void TryFireball() {
        if (gotAbilityAttack && canAttack)
        {
            audioSource.clip = attackA;
            audioSource.Play();
            fireball.SetActive(true);
            canAttack = false;
            Invoke("FireballCooldown", fireballDuration);
            attackImage.SetActive(false);
            StartCoroutine(CooldownRoutine(attackCooldown, attackCooldownTimer, attackImage, "canAttack"));
        }
    }

    public void TryShield() {
         if (gotAbilityShield && canDefend)
        {
            audioSource.clip = shieldA;
            audioSource.Play();
            shield.SetActive(true);
            playerMage.shieldOn = true;
            canDefend = false;
            Invoke("ShieldCooldown", shieldDuration);
            shieldImage.SetActive(false);
            StartCoroutine(CooldownRoutine(shieldCooldown, shieldCooldownTimer, shieldImage, "canDefend"));
        }
    }
    private void HandleInput()
    {
        
        move = Vector3.zero;
        if (Input.GetKey(up))
        {
            move += new Vector3(1f, 0f, 1f);
        }
        if (Input.GetKey(down))
        {
            move -= new Vector3(1f, 0f, 1f);
        }
        if (Input.GetKey(right))
        {
            move += new Vector3(1f, 0f, -1f);
            _sr.flipX = false;
        }
        if (Input.GetKey(left))
        {
            move -= new Vector3(1f, 0f, -1f);
            _sr.flipX = true;
        }

        move.Normalize(); 

        if (Input.GetKeyDown(dashKey) && canDash && GetVelocity() != 0 && gotAbilityDash)
        {
            audioSource.clip = dashA;
            audioSource.Play();
            ParticleSystem dashPart = Instantiate(dashParticle);
            dashPart.transform.position = this.transform.position;
            Destroy(dashPart.gameObject, 2f);

            speed *= dashSpeed;
            Invoke("StopDash", dashTime);
            canDash = false;
            dashImage.SetActive(false);
            StartCoroutine(CooldownRoutine(dashCooldown, dashCooldownTimer, dashImage, "canDash"));
        }

        if (Input.GetKeyDown(fireballKey) && gotAbilityAttack && canAttack)
        {
            audioSource.clip = attackA;
            audioSource.Play();
            fireball.SetActive(true);
            canAttack = false;
            Invoke("FireballCooldown", fireballDuration);
            attackImage.SetActive(false);
            StartCoroutine(CooldownRoutine(attackCooldown, attackCooldownTimer, attackImage, "canAttack"));
        }

        if (Input.GetKeyDown(shieldKey) && gotAbilityShield && canDefend)
        {
            audioSource.clip = shieldA;
            audioSource.Play();
            shield.SetActive(true);
            playerMage.shieldOn = true;
            canDefend = false;
            Invoke("ShieldCooldown", shieldDuration);
            shieldImage.SetActive(false);
            StartCoroutine(CooldownRoutine(shieldCooldown, shieldCooldownTimer, shieldImage, "canDefend"));
        }
    }

    public void SwitchIceMode(bool mode) {
        if(mode) {
            iceMove = true;
        } else {
            iceMove = false;
        }
    }
    private IEnumerator CooldownRoutine(int cooldownDuration, TMP_Text cooldownTimerTXT, GameObject image, string type)
    {
        int remainingCooldown = cooldownDuration;
        while (remainingCooldown > 0)
        {

            cooldownTimerTXT.text = $"{remainingCooldown}";
            yield return new WaitForSeconds(1f); 
            remainingCooldown--;
        }

        if (type == "canDefend")
            canDefend = true;
        else if (type == "canAttack")
            canAttack = true;
        else if (type == "canDash")
            canDash = true;

        image.SetActive(true);
        cooldownTimerTXT.text = $"";
    }

    //------------------------
    void FireballCooldown()
    {
        fireball.SetActive(false);
    }

    void ShieldCooldown()
    {
        shield.SetActive(false);
        playerMage.shieldOn = false;
    }

    void StopDash()
    {
        speed /= dashSpeed;
    }

    void DashCooldown()
    {
        canDash = true;
    }


}
