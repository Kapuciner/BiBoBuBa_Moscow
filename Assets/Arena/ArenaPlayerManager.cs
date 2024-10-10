using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaPlayerManager : MonoBehaviour
{
    [SerializeField] private GameManagerArena gm;
    public string nickname;

    public bool insideTheZone = true;
    [SerializeField] private float maxZoneDamageCooldown = 2;
    private float currentZoneDamageCooldown;
    [SerializeField] private int zoneDamage;

    private Vector3 move;
    [SerializeField] private KeyCode up;
    [SerializeField] private KeyCode down;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode Attack;
    [SerializeField] private KeyCode SkillButton;
    [SerializeField] private float speed;
    private float initialSpeed;

    private Rigidbody _rb;
    private SpriteRenderer _sr;

    public List<string> Skill = new List<string> { "", "" };

    [SerializeField] private Image[] abilityImage; //первый или второй скилл
    [SerializeField] private Sprite emptyAbilityImage;
    [SerializeField] private TMP_Text[] skillName;

    private float currentHP;
    private float maxHP = 20;
    [SerializeField] private Slider hpBar;
    [SerializeField] private TMP_Text hpTXT;

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

    [SerializeField] private AudioSource audio;

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


    public bool steamDamageCooldownUp = true;
    [SerializeField] private float steamDamageCooldown = 0.5f;
    public bool lavaDamageCooldownUp = true;
    [SerializeField] private float lavaDamageCooldown = 0.5f;
    [SerializeField] private float lavaDamage = 0.5f;
    Vector3 pushDirection;
    [SerializeField] private float waterPushForce = 5f;

    [SerializeField] float castTime = 0.25f;
    private Vector3 aimDirection = Vector3.zero;
    void Start()
    {
        currentZoneDamageCooldown = -1;
        currentHP = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = currentHP;

        _rb = GetComponent<Rigidbody>();
        _sr = GetComponent<SpriteRenderer>();
        initialSpeed = speed;
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
        move = Vector3.zero;

        Handle_Input();
    }

    void FixedUpdate()
    {
        hitCurrentCooldown -= Time.fixedDeltaTime;
        currentZoneDamageCooldown -= Time.fixedDeltaTime;
        if (canDoStuff)
            _rb.MovePosition(_rb.position + move * speed * Time.deltaTime);
    }

    void Handle_Input()
    {
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
        if (canDoStuff)
        {
            if (Input.GetKeyDown(Attack) && canHit)
            {
                whoToHit.AddForce(hitDirection.normalized * hitPower, ForceMode.Impulse);
                whoToHit.gameObject.GetComponent<ArenaPlayerManager>().TakeDamage(hitDamage);
                hitCurrentCooldown = hitMaxCooldown;

            }
            if (Input.GetKeyDown(SkillButton))
                UseAbility();
        }

        move.Normalize();
        if (move != Vector3.zero)
            previousMove = move.normalized;
    }

    public void GotNewAbility(string name, Sprite abilSprite)
    {
        int skillNum = 0;
        if (Skill[0] == "")
            skillNum = 0;
        else if (Skill[1] == "")
            skillNum = 1;
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
                    Instantiate(fireFire, transform.position + aimDirection * 3, fireFire.transform.rotation)
                        .GetComponent<FireProjectile>().Activate(aimDirection, this.gameObject);

                });
               break;
            case ("Water", "Water"):
                StartCasting(() =>
                {
                    StartCoroutine(WaterWater());
                });
                break;
            case ("Air", "Air"):
                StartCasting(() =>
                {
                    Instantiate(airAir, transform.position + aimDirection * 3, airAir.transform.rotation).
                        GetComponent<AireProjectile>().Activate(aimDirection, this.gameObject);
                });
                break;
            case ("Earth", "Earth"):
                StartCasting(() =>
                {
                    Instantiate(earthEarth, transform.position + aimDirection * 3, earthEarth.transform.rotation)
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
                    Instantiate(fireEarth, transform.position + aimDirection * 4, fireEarth.transform.rotation)
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
                    Instantiate(waterEarth, transform.position + aimDirection * 2, waterEarth.transform.rotation).GetComponent<WaterEarthProjectile>()
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

        abilityImage[0].sprite = emptyAbilityImage;
        skillName[0].text = "Empty";
        Skill[0] = "";
        abilityImage[1].sprite = emptyAbilityImage;
        skillName[1].text = "Empty";
        Skill[1] = "";
    }

    void StartCasting(System.Action action)
    {
        StartCoroutine(WaitBeforeCast(action));
    }

    IEnumerator WaitBeforeCast(System.Action action)
    {
        float timePassed = 0;
        GetComponent<SpriteRenderer>().color = Color.black;
        canDoStuff = false;
        yield return new WaitForSeconds(castTime);
        action.Invoke();
        canDoStuff = true;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    IEnumerator WaterAir()
    {
        canDoStuff = false;
        int countTimes = 0;
        while (countTimes < 6)
        {
            Instantiate(waterAir, transform.position + previousMove * 1, waterAir.transform.rotation)
                .GetComponent<WaterProjectile>().Activate(previousMove, this.gameObject);
            countTimes++;
            yield return new WaitForSeconds(0.2f);
        }
        canDoStuff = true;
    }

    void EarthAir()
    {
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
        _lr.enabled = true;
        RaycastHit hit;
        float passedTimeWater = 0;
        float timeSinceLastDamage = 0;
        while (passedTimeWater < 4)
        {
            _lr.SetPosition(0, waterAim.position + previousMove * 0.5f);

            if (Physics.Raycast(waterAim.position + previousMove * 0.5f, previousMove * 3, out hit, 15))
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
                _lr.SetPosition(1, waterAim.position + previousMove * 15);
            }

            passedTimeWater += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        _lr.enabled = false;
    }

    public void TakeDamage(int value)
    {
        audio.Play();
        currentHP -= value;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }

        hpBar.value = currentHP;
        hpTXT.text = $"{currentHP}/20";

        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ColorBack", 0.2f);
    }

    public void Die()
    {
        dead = true;
        hpTXT.text = $"{currentHP}/20";
        gm.CheckIfRoundEnd(); // did all players die?
;
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
            StopCoroutine(burnCoroutine);
        burnCoroutine = null;
        fireIMG.SetActive(false);
        //!!!!!!!!!!!!!!!! добавить звук тушения (срочно)
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
        speed = initialSpeed;
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);
        slowCoroutine = StartCoroutine(SlowDown(time, speedReductionTimes));
    }

    IEnumerator SlowDown(int timeLasts, float speedReductionTimes)
    {
        int passedTimeSlowed = 0;
        speed /= speedReductionTimes;

        while (passedTimeSlowed < timeLasts)
        {
            passedTimeSlowed++;
            yield return new WaitForSeconds(1);
        }

        speed *= speedReductionTimes;
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
}