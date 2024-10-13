using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
public class ArenaPlayerManager : MonoBehaviour
{
    private bool ready = false;
    public int playerID;
    private GameManagerArena gm;
    public string nickname;

    public bool insideTheZone = true;
    [SerializeField] private float maxZoneDamageCooldown = 2;
    private float currentZoneDamageCooldown;
    [SerializeField] private int zoneDamage;

    private float initialSpeed;

    private Rigidbody _rb;
    private SpriteRenderer _sr;

    public List<string> Skill = new List<string> { "", "" };

    [SerializeField] private Sprite emptyAbilityImage;
    public List<Image> abilityImage; //первый или второй скилл
    public List<TMP_Text> skillName;
    public Slider hpBar;
    public TMP_Text hpTXT;
    public TMP_Text readyTXT;


    private float currentHP;
    private float maxHP = 20;

    private Rigidbody whoToHit;
    private Vector3 hitDirection;
    public static float hitPower = 15; //чем больше, тем сильнее отталкивает
    public static int hitDamage = 1;
    private bool canHit;
    float hitCurrentCooldown = 0;
    [SerializeField] float hitMaxCooldown = 1;

    private bool canDoStuff = true;
    public bool dead = false;

    [SerializeField] private int deadHeight;


    [Header("Animations")]
    [SerializeField] private Animator _animator;

    [Header("Sounds")]
    [SerializeField] private AudioSource audio; //for taking damage
    [SerializeField] private AudioSource audio2; // for fire
    [SerializeField] private AudioSource audio3; //for hits
    [SerializeField] private AudioClip painSound;
    [SerializeField] private AudioClip extinguishFire;
    [SerializeField] private AudioClip soundOnFire;
    [SerializeField] private AudioClip fireHitSound;
    [SerializeField] private AudioClip bubbleAppearSound;
    [SerializeField] private AudioClip bubbleHitSound;
    [SerializeField] private AudioClip earthHitSound;
    [SerializeField] private AudioClip waterHitSound;
    [SerializeField] private AudioClip fireAirHitSound;
    [SerializeField] private AudioClip streamOfWaterSound;
    [SerializeField] private AudioClip airHitSound;
    [SerializeField] private AudioClip waterEarthHitSound;
    [SerializeField] private AudioClip earthAirSound;
    [SerializeField] private AudioClip earthAirHitSound;
    [SerializeField] private AudioClip punchSound;
    [SerializeField] private AudioClip deathPuff;

    [Header("Projectiles")]
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject air;
    [SerializeField] private GameObject earth;
    [SerializeField] private GameObject fireWater;
    [SerializeField] private GameObject fireAir;
    [SerializeField] private GameObject fireEarth;
    [SerializeField] private GameObject fireFire;
    [SerializeField] private GameObject waterAir;
    [SerializeField] private GameObject waterEarth;
    [SerializeField] private GameObject airEarth;
    [SerializeField] private GameObject airAir;
    [SerializeField] private GameObject earthEarth;
    private Vector3 previousMove = Vector3.zero;

    [SerializeField] private GameObject fireIMG;
    [SerializeField] private LineRenderer _lr;
    private Coroutine burnCoroutine;
    private Coroutine stanCoroutine;
    private Coroutine slowCoroutine;

    [Header("EffectsCooldowns")]
    [SerializeField] private float waterRayCooldown = 0.25f;
    [SerializeField] private int rayDamage = 1;
    [SerializeField] private Transform waterAim;
    [SerializeField] private float waterRayDistance;
    public LayerMask waterIgnoreLayer;

    public bool steamDamageCooldownUp = true;
    [SerializeField] private float steamDamageCooldown = 0.5f;
    public bool lavaDamageCooldownUp = true;
    [SerializeField] private float lavaDamageCooldown = 0.5f;
    [SerializeField] private float lavaDamage = 0.5f;
    Vector3 pushDirection;
    [SerializeField] private float waterPushForce = 5f;

    [SerializeField] float castTime = 0.25f;
    private Vector3 aimDirection = Vector3.zero;

    //-- взято из LobbyDummy
    private Vector3 _lastDirection;
    private Vector3 _lastDirectionSmooth;
    private bool _canMove = true;
    private Vector3 _move;
    private float smoothSpeed = 0.2f;
    public int PlayerIndex;
    private bool _notMoving = false;
    public float SPEED;

    private Vector3 direction;

    private void Awake()
    {
        gm = FindObjectOfType<GameManagerArena>();
        _rb = GetComponent<Rigidbody>();
        _sr = GetComponent<SpriteRenderer>(); //должно быть в awake, иначе игра ломается 
    }
    void Start()
    {

        currentZoneDamageCooldown = -1;
        currentHP = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = currentHP;
        initialSpeed = SPEED;
    }

    void Update()
    {
        if (transform.position.y < deadHeight && dead == false)
        {
            dead = true;
            hpTXT.text = $"{currentHP}/20";
            currentHP = 0;
            hpBar.value = 0;
            Die();
        }

        if (!insideTheZone && currentZoneDamageCooldown < 0)
        {
            currentZoneDamageCooldown = maxZoneDamageCooldown;
            TakeDamage(zoneDamage);
        }

        if (_notMoving)
        {
            Move(Vector2.zero);
        }

        if (GetVelocity() == 0 || canDoStuff == false)
        {
            _animator.SetFloat("velocity", 0);
        }
        else
        {
            _animator.SetFloat("velocity", 1);
        }
    }

    void FixedUpdate()
    {
        hitCurrentCooldown -= Time.fixedDeltaTime;
        currentZoneDamageCooldown -= Time.fixedDeltaTime;
        if (canDoStuff)
            _rb.MovePosition(_rb.position + _move * SPEED * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = Vector2.zero;
        var _targetPlayer = this;
        value = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            _notMoving = true;
            return;
        }
        _notMoving = false;

        _targetPlayer.Move(value);
    }

    public void Move(Vector2 dir2D)
    {
        Vector3 direction = new Vector3(dir2D.x, 0, dir2D.y);

        if (direction.x < 0)
        {
            _sr.flipX = true; 
        }
        else if (direction.x > 0)
        {
            _sr.flipX = false;
        }
        direction = Quaternion.AngleAxis(45, new Vector3(0, 1, 0)) * direction;

        if (direction.magnitude != 0)
        {
            previousMove = direction;
        }

        if (_canMove == false)
        {
            direction = Vector3.zero;
        }

        if (!gameObject.activeSelf)
        {
            return;
        }
        StartCoroutine(LerpMove(direction));
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

    public void OnCast(InputAction.CallbackContext context)
    {
        if (context.started && canDoStuff)
        {
            _animator.SetTrigger("cast");
            UseAbility();
        }
    }

    public void OnHit(InputAction.CallbackContext context)
    {
        if (context.started && canDoStuff)
        {
            if (hitCurrentCooldown < 0)
            {
                _animator.SetTrigger("punch");
                hitCurrentCooldown = hitMaxCooldown;
                audio3.clip = punchSound;
                audio3.Play();
                if (canHit)
                {
                    Invoke("DelayedHit", 0.15f);
                }
            }
        }
    }

    void DelayedHit()
    {
        audio3.clip = punchSound;
        audio3.Play();
        whoToHit.AddForce(hitDirection.normalized * hitPower, ForceMode.Impulse);
        whoToHit.gameObject.GetComponent<ArenaPlayerManager>().TakeDamage(hitDamage);
    }


    public void GotNewAbility(string name, Sprite abilSprite)
    {
        int skillNum = 0;
        if (Skill[0] == "")
            skillNum = 0;
        else if (Skill[1] == "")
            skillNum = 1;
        abilityImage[skillNum].color = new Color(1, 1, 1, 1);
        switch (name)
        {
            case "Water":
                abilityImage[skillNum].sprite = abilSprite;
                skillName[skillNum].text = "Вода";
                Skill[skillNum] = "Water";
                break;
            case "Fire":
                abilityImage[skillNum].sprite = abilSprite;
                skillName[skillNum].text = "Огонь";
                Skill[skillNum] = "Fire";
                break;
            case "Air":
                abilityImage[skillNum].sprite = abilSprite;
                skillName[skillNum].text = "Воздух";
                Skill[skillNum] = "Air";
                break;
            case "Earth":
                abilityImage[skillNum].sprite = abilSprite;
                skillName[skillNum].text = "Земля";
                Skill[skillNum] = "Earth";
                break;
        }
    }

    void UseAbility()
    {
        aimDirection = previousMove;
        switch ((Skill[0], Skill[1]))
        {
            case ("", ""):
                break;
            case ("Fire", ""):
                StartCasting(() => {
                    Instantiate(fire, transform.position + aimDirection * 1, fire.transform.rotation)
                        .GetComponent<FireProjectile>().Activate(aimDirection, this.gameObject);
                }); break;
            case ("Water", ""):
                StartCasting(() =>
                {
                    Instantiate(water, transform.position + aimDirection * 1, water.transform.rotation)
                        .GetComponent<WaterProjectile>().Activate(aimDirection, this.gameObject);
                });
                break;
            case ("Air", ""):
                StartCasting(() =>
                {
                    Instantiate(air, transform.position + aimDirection * 1, air.transform.rotation).
                        GetComponent<AireProjectile>().Activate(aimDirection, this.gameObject);
                });
            break;
            case ("Earth", ""):
                StartCasting(() =>
                {
                    Instantiate(earth, transform.position + aimDirection * 1, earth.transform.rotation)
                        .GetComponent<EarthProjectiles>().Activate(aimDirection, this.gameObject);
                });
                break;
            case ("Fire", "Fire"):
                StartCasting(() =>
                {
                    Instantiate(fireFire, transform.position + aimDirection * 3 + Vector3.up * 0.5f, fireFire.transform.rotation)
                        .GetComponent<FireProjectile>().Activate(aimDirection, this.gameObject);

                });
               break;
            case ("Water", "Water"):
                StartCoroutine(WaterWater());
                break;
            case ("Air", "Air"):
                StartCasting(() =>
                {
                    Instantiate(airAir, transform.position + aimDirection * 3 + Vector3.up * 0.5f, airAir.transform.rotation).
                        GetComponent<AireProjectile>().Activate(aimDirection, this.gameObject);
                });
                break;
            case ("Earth", "Earth"):
                StartCasting(() =>
                {
                    Instantiate(earthEarth, transform.position + aimDirection * 3 + Vector3.up * 0.5f, earthEarth.transform.rotation)
                        .GetComponent<EarthProjectiles>().Activate(aimDirection, this.gameObject);
                });
               break;
            case ("Fire", "Water"):
            case ("Water", "Fire"):
                StartCasting(() =>
                {
                    Instantiate(fireWater, transform.position + aimDirection * 6, fireWater.transform.rotation)
                        .GetComponent<FireWaterProjectile>().Activate(aimDirection);
                });
                break;
            case ("Fire", "Air"):
            case ("Air", "Fire"):
                StartCasting(() =>
                {
                    FireAir();
                });
                break;
            case ("Fire", "Earth"):
            case ("Earth", "Fire"):
                StartCasting(() =>
                {
                    Instantiate(fireEarth, transform.position + aimDirection * 4 + Vector3.up * 0.22f, fireEarth.transform.rotation)
                        .GetComponent<FireEarth2>().Activate(aimDirection);
                });
               break;
            case ("Water", "Air"):
            case ("Air", "Water"):
                StartCoroutine(WaterAir());
                break;
            case ("Water", "Earth"):
            case ("Earth", "Water"):
                StartCasting(() =>
                {
                    Instantiate(waterEarth, transform.position + aimDirection * 2 + Vector3.up * 0.5f, waterEarth.transform.rotation).GetComponent<WaterEarthProjectile>()
                        .Activate(aimDirection, this.gameObject);
                });
                break;
            case ("Air", "Earth"):
            case ("Earth", "Air"):
                StartCasting(() =>
                {
                    EarthAir();
                });
                break;
            default:
                Debug.Log($"Combination {Skill[0]}, {Skill[1]} does not work?");
                break;
        }

        abilityImage[0].color = new Color(0, 0, 0, 0);
        skillName[0].text = "";
        Skill[0] = "";
        abilityImage[1].color = new Color(0, 0, 0, 0);
        skillName[1].text = "";
        Skill[1] = "";
    }

    void StartCasting(System.Action action)
    {
        StartCoroutine(WaitBeforeCast(action));
    }

    IEnumerator WaitBeforeCast(System.Action action)
    {
        float timePassed = 0;
        canDoStuff = false;
        yield return new WaitForSeconds(castTime);
        action.Invoke();
        canDoStuff = true;
    }
    IEnumerator WaterAir()
    {
        canDoStuff = false;
        int countTimes = 0;
        while (countTimes < 10)
        {
            _animator.SetTrigger("cast");
            audio3.clip = bubbleAppearSound;
            audio3.Play();
            Instantiate(waterAir, transform.position + previousMove * 1, waterAir.transform.rotation)
                .GetComponent<WaterProjectile>().Activate(previousMove, this.gameObject);
            countTimes++;
            yield return new WaitForSeconds(0.2f);
        }
        canDoStuff = true;
    }

    void EarthAir()
    {
        audio3.clip = earthAirSound;
        audio3.Play();

        Instantiate(airEarth, transform.position + aimDirection * 2, transform.rotation)
            .GetComponent<EarthAirProjectile>().Activate(aimDirection, this.gameObject);

        float coneAngle = 40f;

        for (int i = -1; i <= 1; i += 2)
        {
            float currentAngle = i * coneAngle / 4;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * aimDirection;
            Vector3 spawnPosition = transform.position + direction * 4f;
            Instantiate(airEarth, spawnPosition, transform.rotation).GetComponent<EarthAirProjectile>()
                .Activate(aimDirection, this.gameObject);
        }

        coneAngle = 30f; 
        for (int i = -1; i <= 1; i++) 
        {
            float currentAngle = i * coneAngle / 2; 
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * aimDirection;
            Vector3 spawnPosition = transform.position + direction * 6;
            Instantiate(airEarth, spawnPosition, transform.rotation).GetComponent<EarthAirProjectile>()
                .Activate(aimDirection, this.gameObject);
        }
    }

    void FireAir()
    {

        Instantiate(fireAir, transform.position + aimDirection * 2, transform.rotation)
            .GetComponent<WaterProjectile>().Activate(aimDirection, this.gameObject);

        float coneAngle = 120f;
        for (int i = -1; i <= 1; i += 2) 
        {
            float currentAngle = i * coneAngle / 4; 

            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * aimDirection;

            Vector3 spawnPosition = transform.position + direction * 2;

            Instantiate(fireAir, spawnPosition, transform.rotation).GetComponent<WaterProjectile>()
                .Activate(direction, this.gameObject);
        }
    }

    IEnumerator WaterWater()
    {
        canDoStuff = false;
        _animator.SetFloat("waterWater", 1);
        _lr.enabled = true;
        RaycastHit hit;
        float passedTimeWater = 0;
        float timeSinceLastDamage = 0;
        audio3.clip = streamOfWaterSound;
        audio3.Play();
        while (passedTimeWater < 4)
        {
            _lr.SetPosition(0, waterAim.position + previousMove * 0.5f);

            if (Physics.Raycast(waterAim.position + previousMove * 0.5f, previousMove * 3, out hit, waterRayDistance, ~waterIgnoreLayer))
            {
                if (hit.collider.tag == "Player")
                {
                    ArenaPlayerManager playerManager = hit.collider.gameObject.GetComponent<ArenaPlayerManager>();

                    timeSinceLastDamage += Time.fixedDeltaTime;
                    if (timeSinceLastDamage >= waterRayCooldown)
                    {
                        pushDirection = hit.transform.position - transform.position;
                        hit.collider.gameObject.GetComponent<Rigidbody>()
                            .AddForce(pushDirection.normalized * waterPushForce, ForceMode.Impulse);

                        playerManager.TakeDamage(rayDamage);
                        timeSinceLastDamage = 0;
                        playerManager.ExtinguishFire();
                    }
                }

                Vector3 extendedPoint = hit.point + (hit.point - transform.position).normalized * 0.5f;
                _lr.SetPosition(1, extendedPoint);
            }
            else
            {
                _lr.SetPosition(1, waterAim.position + previousMove * waterRayDistance);
            }
            passedTimeWater += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        canDoStuff = true;
        _lr.enabled = false;
        _animator.SetFloat("waterWater", 0);
    }

    public void TakeDamage(int value)
    {

        if (currentHP <= 1)
        {
            currentHP = 0;
            Die();
        }
        else
        {
            audio.clip = painSound;
            audio.Play();
            currentHP -= value;
            hpBar.value = currentHP;
            hpTXT.text = $"{currentHP}/20";

            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("ColorBack", 0.2f);
        }
    }

    public void Die()
    {
        fireIMG.SetActive(false);
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _animator.SetTrigger("Death");
        audio.clip = deathPuff;
        audio.Play();
        dead = true;
        hpTXT.text = $"{currentHP}/20";
        gm.CheckIfRoundEnd(); // did all players die?
    }

    void ColorBack()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnFire()
    {
        if (burnCoroutine == null)
            burnCoroutine = StartCoroutine(Burn());
    }

    IEnumerator Burn()
    {
        audio2.clip = soundOnFire;
        audio2.Play();
        float passed = 0;
        float burnLasts = 15;
        fireIMG.SetActive(true);
        while (passed < burnLasts)
        {
            yield return new WaitForSeconds(2.5f);
            passed += 2.5f;
            TakeDamage(1);
        }
        fireIMG.SetActive(false);

        burnCoroutine = null;
    }

    public void ExtinguishFire()
    {
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
            burnCoroutine = null;
            fireIMG.SetActive(false);
            audio2.clip = extinguishFire;
            audio2.Play();
        }
    }

    public void Stan(float time)
    {
        if (stanCoroutine == null)
            stanCoroutine = StartCoroutine(Stanned(time));
    }

    IEnumerator Stanned(float timeLasts)
    {
        canDoStuff = false;
        float passedTimeStanned = 0;
        while (passedTimeStanned < timeLasts)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
            passedTimeStanned += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        canDoStuff = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        stanCoroutine = null;
    }

    public void Slow(int time, float speedReductionTimes)
    {
        SPEED = initialSpeed;
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);
        slowCoroutine = StartCoroutine(SlowDown(time, speedReductionTimes));
    }

    IEnumerator SlowDown(int timeLasts, float speedReductionTimes)
    {
        int passedTimeSlowed = 0;
        SPEED /= speedReductionTimes;

        while (passedTimeSlowed < timeLasts)
        {
            passedTimeSlowed++;
            yield return new WaitForSeconds(1);
        }

        SPEED *= speedReductionTimes;
        slowCoroutine = null;
    }

    public void cooldownPass(string ability)
    {
        if (ability == "steam")
        {
            StartCoroutine(steamTimer(steamDamageCooldown));
        }
        else if (ability == "lava")
        {
            StartCoroutine(lavaTimer(lavaDamageCooldown));
        }
    }

    IEnumerator steamTimer(float time)
    {
        float steamTimePassed = 0;

        while (steamTimePassed < time)
        {
            steamTimePassed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        steamDamageCooldownUp = true;
    }

    IEnumerator lavaTimer(float time)
    {
        float lavaTimePassed = 0;

        while (lavaTimePassed < time)
        {
            lavaTimePassed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        lavaDamageCooldownUp = true;
    }

    private void OnTriggerStay(Collider other) //для удара
    {
        if (other.tag == "Player")
        {
            if (hitCurrentCooldown < 0)
            {
                hitDirection = other.transform.position - this.transform.gameObject.transform.position;
                whoToHit = other.GetComponent<Rigidbody>();
                canHit = true;
            }
            else
            {
                canHit = false;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canHit = false;
        }
    }

    public void PlayOnHit(string WhatHit)
    {
        switch (WhatHit)
        {
            case "fire":
                audio3.clip = fireHitSound;
                audio3.Play();
                break;
            case "waterAir":
                audio3.clip = bubbleHitSound;
                audio3.Play();
                break;
            case "earth":
                audio3.clip = earthHitSound;
                audio3.Play();
                break;
            case "water":
                audio3.clip = waterHitSound;
                audio3.Play();
                break;
            case "fireAir":
                audio3.clip = fireAirHitSound;
                audio3.Play();
                break;
            case "air":
                audio3.clip = airHitSound;
                audio3.Play();
                break;
            case "waterEarth":
                audio3.clip = waterEarthHitSound;
                audio3.Play();
                break;            
            case "earthAir":
                audio3.clip = earthAirHitSound;
                audio3.Play();
                break;
        }
    }

    public void SetAnimation(string animation)
    {
        _animator.Play(animation);
    }

    public float GetVelocity()
    {
        float vel = (_move * SPEED * Time.deltaTime).magnitude;
        if (vel <= 0.005f)
        {
            vel = 0;
        }
        return vel;
    }

    public void OnReady(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            ready = !ready;
            if (ready)
            {
                GameManagerArena.playersReady += 1;
                readyTXT.text = "Готов";
                readyTXT.color = Color.green;
            }
            else
            {
                GameManagerArena.playersReady -= 1;
                readyTXT.text = "Не готов";
                readyTXT.color = Color.red;
            }
            return;
        }
    }



}