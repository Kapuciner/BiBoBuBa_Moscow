using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Falliant : MonoBehaviour
{
    [SerializeField] private int groupID;

    [SerializeField] private GameManager gm;

    private int rand;


    [SerializeField] public Transform player;
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] private Transform goal;
    [SerializeField] private float followSpeed;
    public bool taken = false;
    public bool carried = false;
    private Vector3 targetFollow;

    [SerializeField] private Sprite mageEnergyFull;
    [SerializeField] private Image[] mageEmptyEnergy;

    private Vector3 startPosition;

    private bool returnBack = false;

    public AudioClip vitalityA;
    public AudioSource audioSource;


    private void Start()
    {
        startPosition = this.transform.position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mage" && carried == false && collision.gameObject.GetComponent<PlayerManager>().alreadyCarrying == false)
        {
            taken = true;
            playerManager = collision.gameObject.GetComponent<PlayerManager>();
            playerManager.alreadyCarrying = true;
            player = collision.gameObject.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            this.GetComponent<Animator>().Play("faliant_open");
            other.GetComponent<AudioSource>().Play();
            playerManager.alreadyCarrying = false;
            taken = false;
            carried = true;
            GameManager.faliantsCarriedAmount++;
            playerManager.faliantCounter++;
            playerManager.faliantCounterTXT.text = $"{playerManager.faliantCounter}";
            gm.UpdateFalliantCounter();

            if (playerManager.faliantCounter % 2 != 0 && playerManager.faliantCounter < 8)
            {
                mageEmptyEnergy[playerManager.abilitiesGotten].sprite = mageEnergyFull;
                playerManager.abilitiesGotten++;

                rand = Random.Range(0, playerManager.abilitiesID.Count);

                if (playerManager.abilitiesID[rand] == 0)
                {
                    player.gameObject.GetComponent<player>().hpSkill();
                    audioSource.clip = vitalityA;
                    audioSource.Play();
                    playerManager.GotAbilityHP();

                }
                if (playerManager.abilitiesID[rand] == 1)
                {
                    playerManager.GotAbilityDash();
                }
                if (playerManager.abilitiesID[rand] == 2)
                {
                    playerManager.GotAbilityShield();
                }
                if (playerManager.abilitiesID[rand] == 3)
                {
                    playerManager.GotAbilityAttack();
                }

                playerManager.abilitiesID.RemoveAt(rand);
            }
        }
    }

    private void FixedUpdate()
    {
        if (taken)
        {
            StartCoroutine(Follow());
        }

        if (carried)
        {
            StartCoroutine(PutTheBook());
        }
    }

    IEnumerator Follow()
    {
        while (Vector3.Distance(transform.position, player.position) > 2f && carried == false && returnBack == false)
        {
            targetFollow = new Vector3(player.position.x, player.position.y + 1, player.position.z); //����� ������� �����
            transform.position = Vector3.Lerp(transform.position, targetFollow, followSpeed * Time.deltaTime);
            yield return null; 
        }

        taken = false;

        if (returnBack == false)
            Invoke("QuickPause", 0.25f);
        else
        {
            Invoke("ReturnBackInverse", 3f);
        }
    }

    void ReturnBackInverse()
    {
        returnBack = false;
    }

    IEnumerator PutTheBook()
    {
        while (Vector3.Distance(transform.position, goal.position) > 2f)
        {
            transform.position = Vector3.Lerp(transform.position, goal.position, followSpeed * Time.deltaTime);
            yield return null;
        }
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        this.enabled = false;


    }

    void QuickPause()
    {
        taken = true;
    }

    public void ReturnToTheStart()
    {
        if (taken && !carried)
        {
            playerManager.alreadyCarrying = false;
            returnBack = true;
            this.transform.position = startPosition;
            taken = false;
        }
    }

    public void limitFalliantsAmount(int maxFall)
    {
        if (maxFall < 24 && groupID == 3)
        {
            this.gameObject.SetActive(false);
        }
        if (maxFall < 16 && groupID == 2)
        {
            this.gameObject.SetActive(false);
        }
    }
}
