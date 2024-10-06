using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaPlayerManager : MonoBehaviour
{
    [SerializeField] private GameManagerArena gm;

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
    [SerializeField] private GameObject waterWater;
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

    public bool steamDamageCooldownUp = true;
    [SerializeField] private float steamDamageCooldown = 0.5f;
    public bool lavaDamageCooldownUp = true;
    [SerializeField] private float lavaDamageCooldown = 0.5f;
    [SerializeField] private float lavaDamage = 0.5f;

    void Start()
    {
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

        move = Vector3.zero;
        if (canDoStuff)
            Handle_Input();
    }

    void FixedUpdate()
    {
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
        if (Input.GetKeyDown(Attack) && canHit)
        {
            whoToHit.AddForce(hitDirection.normalized * hitPower, ForceMode.Impulse);
            whoToHit.gameObject.GetComponent<ArenaPlayerManager>().TakeDamage(hitDamage);
        }
        if (Input.GetKeyDown(SkillButton))
            UseAbility();

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
        switch((Skill[0], Skill[1]))
        {
            case ("", ""):
                //make sound!~!!!
                    break;
            case ("Fire", ""):
                Instantiate(fire, transform.position + previousMove * 3, fire.transform.rotation).GetComponent<FireProjectile>().Activate(previousMove);
                break;
            case ("Water", ""):
                Instantiate(water, transform.position + previousMove * 3, water.transform.rotation).GetComponent<WaterProjectile>().Activate(previousMove);
                break;
            case ("Air", ""):
                Instantiate(air, transform.position + previousMove * 3, air.transform.rotation).GetComponent<AireProjectile>().Activate(previousMove);
                break;
            case ("Earth", ""):
                Instantiate(earth, transform.position + previousMove * 3, earth.transform.rotation).GetComponent<EarthProjectiles>().Activate(previousMove);
                break;
            case ("Fire", "Fire"):
                Instantiate(fireFire, transform.position + previousMove * 5, fireFire.transform.rotation).GetComponent<FireProjectile>().Activate(previousMove);
                break;
            case ("Water", "Water"):
                StartCoroutine(WaterWater());
                break;
            case ("Air", "Air"):
                Instantiate(airAir, transform.position + previousMove * 5, airAir.transform.rotation).GetComponent<AireProjectile>().Activate(previousMove);
                break;
            case ("Earth", "Earth"):
                Instantiate(earthEarth, transform.position + previousMove * 5, earthEarth.transform.rotation).GetComponent<EarthProjectiles>().Activate(previousMove);
                break;
            case ("Fire", "Water"):
            case ("Water", "Fire"):
                Instantiate(fireWater, transform.position + previousMove * 6, fireWater.transform.rotation).GetComponent<FireWaterProjectile>().Activate(previousMove);
                break;
            case ("Fire", "Air"):
            case ("Air", "Fire"):
                FireAir();
                break;
            case ("Fire", "Earth"):
            case ("Earth", "Fire"):
                Instantiate(fireEarth, transform.position + previousMove * 3, fireEarth.transform.rotation).GetComponent<FireEarth2>().Activate(previousMove);
                break;
            case ("Water", "Air"):
            case ("Air", "Water"):
                StartCoroutine(WaterAir());
                break;
            case ("Water", "Earth"):
            case ("Earth", "Water"):
                Instantiate(waterEarth, transform.position + previousMove * 5, waterEarth.transform.rotation).GetComponent<WaterEarthProjectile>().Activate(previousMove);
                break;
            case ("Air", "Earth"):
            case ("Earth", "Air"):
                StartCoroutine(EarthAir());
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
    
    IEnumerator WaterAir()
    {
        int countTimes = 0;
        while (countTimes < 6)
        {
            Instantiate(waterAir, transform.position + previousMove * 3, waterAir.transform.rotation).GetComponent<WaterProjectile>().Activate(previousMove);
            countTimes++;
            yield return new WaitForSeconds(0.4f);
        }
    }

    IEnumerator EarthAir()
    {
        canDoStuff = false;
        float passedTimeStanned = 0;
        while (passedTimeStanned < 0.5)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
            passedTimeStanned += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        canDoStuff = true;
        GetComponent<SpriteRenderer>().color = Color.white;

        Instantiate(airEarth, transform.position + previousMove * 3, transform.rotation).GetComponent<EarthAirProjectile>().Activate(previousMove);

        float coneAngle = 20f;

        for (int i = -1; i <= 1; i += 2)
        {
            float currentAngle = i * coneAngle / 4;
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * previousMove;
            Vector3 spawnPosition = transform.position + direction * 3.5f;
            Instantiate(airEarth, spawnPosition, transform.rotation).GetComponent<EarthAirProjectile>().Activate(direction);
        }

        coneAngle = 20f; 
        for (int i = -1; i <= 1; i++) 
        {
            float currentAngle = i * coneAngle / 2; 
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * previousMove;
            Vector3 spawnPosition = transform.position + direction * 4;
            Instantiate(airEarth, spawnPosition, transform.rotation).GetComponent<EarthAirProjectile>().Activate(direction);
        }
    }

    void FireAir()
    {
        Instantiate(fireAir, transform.position + previousMove * 4, transform.rotation).GetComponent<WaterProjectile>().Activate(previousMove);

        float coneAngle = 120f;
        for (int i = -1; i <= 1; i += 2) 
        {
            float currentAngle = i * coneAngle / 4; 

            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * previousMove;

            Vector3 spawnPosition = transform.position + direction * 4;

            Instantiate(fireAir, spawnPosition, transform.rotation).GetComponent<WaterProjectile>().Activate(direction);
        }
    }

    IEnumerator WaterWater()
    {
        _lr.enabled = true;
        RaycastHit hit;
        float passedTimeWater = 0;
        float timeSinceLastDamage = waterRayCooldown;
        while (passedTimeWater < 4)
        {
            _lr.SetPosition(0, transform.position + previousMove * 0.5f);

            if (Physics.Raycast(transform.position + previousMove * 2.5f, previousMove * 3, out hit, 12))
            {
                if (hit.collider.tag == "Player")
                {
                    ArenaPlayerManager playerManager = hit.collider.gameObject.GetComponent<ArenaPlayerManager>();

                    timeSinceLastDamage += Time.fixedDeltaTime;
                    if (timeSinceLastDamage >= waterRayCooldown)
                    {
                        playerManager.TakeDamage(rayDamage);
                        timeSinceLastDamage = 0;
                        playerManager.ExtinguishFire();
                    }
                }

                _lr.SetPosition(1, hit.point);
            }
            else
            {
                _lr.SetPosition(1, transform.position + previousMove * 12);
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
            hitDirection = other.transform.position - this.transform.gameObject.transform.position;
            whoToHit = other.GetComponent<Rigidbody>();
            canHit = true;
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