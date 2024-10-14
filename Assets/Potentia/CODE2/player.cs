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

    [SerializeField] public GameObject gameOverScreen;
    [SerializeField] public Animator gameOverTXT;

    [SerializeField] public TMP_Text livesCounter;

    [SerializeField] public GameObject[] hearts;
    private float currentHearts;
    [SerializeField] private float maxHearts;
    private int currentLives;
    [SerializeField] private int maxLives;

    [SerializeField] private PlayerAnimation playerAnimation;

    [SerializeField] public Transform start;
    [SerializeField] private int minHeight; //for falling

    public int faliantsCollected = 0;
    public bool inProccessOfCarrying = false;

    public bool shieldOn = false;

    private GameObject[] faliants;

    public AudioSource audioSour;
    public AudioClip damage;
    public AudioClip death;

    private bool finishedDying = true;


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

        if (currentLives > 1)
        {
            //playerAnimation.SetAnimation("player_die");
            StartCoroutine(Respawn());
        }
        else
        {
            GameOver();
        }

        currentLives--;
        if (currentLives >= 0)
            livesCounter.text = $"x{currentLives}";
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        this.gameObject.SetActive(false);
        gm.gameOver = true;
        gameOverTXT.Play("TxTappear");
    }

    IEnumerator Respawn()
    {
        audioSour.clip = death;
        audioSour.Play();
        playerAnimation.blockAnimation = true;
        playerAnimation.SetAnimation("magDeath");
        yield return new WaitForSeconds(1);
        playerAnimation.blockAnimation = false;
        currentHearts = maxHearts+1;
        UpdateHP();
        this.transform.position = start.transform.position;
        finishedDying = true;
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
        this.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 1.2f);
    }
}
