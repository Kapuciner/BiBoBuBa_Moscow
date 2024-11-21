using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class player : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField] public GameManager gm;
    [SerializeField] public TMP_Text livesCounter;

    [SerializeField] public GameObject[] hearts;
    private float currentHearts;
    [SerializeField] private float maxHearts;
    private int currentLives;
    [SerializeField] private int maxLives;

    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private PlayerManager playerManager;

    [SerializeField] public Transform start;
    [SerializeField] private int minHeight; //for falling

    public int faliantsCollected = 0;
    public bool inProccessOfCarrying = false;

    public bool shieldOn = false;

    private GameObject[] faliants;

    public AudioSource audioSour;
    public AudioClip damage;
    public AudioClip death;
    public bool dead;

    private bool finishedDying = true;

    [SerializeField] public GameObject deathCross;

    Coroutine respawn = null;


    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        currentHearts = maxHearts;
        currentLives = maxLives;
        UpdateHP();
    }

    private void Update()
    {
        if (this.transform.position.y < minHeight && finishedDying)
        {
            finishedDying = false;
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        audioSour.clip = damage;
        audioSour.Play();
        if (!shieldOn)
        {
            _sr.color = Color.red;
            Invoke("ColorBack", 0.2f);

            currentHearts -= amount;
            currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
            UpdateHP();
            
            if (currentHearts == 0)
            {
                Die();
            }
        }
        else
        {
            _sr.color = Color.blue;
            Invoke("ColorBack", 0.2f);
        }
    }

    // Call this method to heal
    public void Heal(float amount)
    {
        currentHearts += amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);
        UpdateHP();
    }

    public void Die()
    {
        faliants = GameObject.FindGameObjectsWithTag("faliant");
        for (int i = 0; i < faliants.Length; i++)
        {
            faliants[i].GetComponent<Falliant>().ReturnToTheStart();
        }

        if (currentLives >= 1)
        {
            //playerAnimation.SetAnimation("player_die");
            if (respawn is null)
            {
                respawn = StartCoroutine(Respawn());
                currentLives--;
            }
        }
        else
        {
            playerManager.canMove = false;
            playerAnimation.DeathAnimation(true);
            dead = true;
            gm.ChangeCameraTarget(this.gameObject);
            deathCross.SetActive(true);
            Destroy(this.gameObject, 1);
            gm.isGameOver();
        }

        if (currentLives >= 0)
            livesCounter.text = $"x{currentLives}";
    }

    IEnumerator Respawn()
    {
        audioSour.clip = death;
        audioSour.Play();
        playerAnimation.blockAnimation = true;

        playerManager.canMove = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        playerAnimation.DeathAnimation(true);

        yield return new WaitForSeconds(1);

        playerAnimation.blockAnimation = false;
        playerAnimation.DeathAnimation(false);
        currentHearts = maxHearts;
        UpdateHP();
        this.transform.position = start.transform.position;

        playerManager.canMove = true;
        finishedDying = true;
        respawn = null;
    }

    void ColorBack()
    {
        _sr.color = Color.white;
    }

    void UpdateHP()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i + 1 <= currentHearts)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }

    public void hpSkill()
    {
        currentLives++;
        livesCounter.text = $"x{currentLives}";
        this.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1.4f);
    }
}
