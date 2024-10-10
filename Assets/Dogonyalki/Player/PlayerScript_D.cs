using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript_D : MonoBehaviour
{
    GameObject pickaxe;

    [SerializeField] public int index;
    [SerializeField] float speed;

    [SerializeField] Animator playerAnimator;

    bool canMove = true;
    bool canPunch = true;

    GameObject pickaxeZone;

    Coroutine pickaxeActionCoroutine = null;
    [SerializeField] GameObject pickaxePrefab;

    [SerializeField] GameManager_D gameManager;
    [SerializeField] ActionTimerCircle actionTimer;

    public delegate void GoldCollected();
    public GoldCollected goldCollected;

    [SerializeField] public Rigidbody _rb;
    bool hasPickaxe = false;
    [SerializeField] Slider playerSlider;
    [SerializeField] TextMeshProUGUI playerRespawnTimer;

    [SerializeField] float attackCooldown;
    [SerializeField] float attackRange;
    [SerializeField] float attackArc;
    [SerializeField] float attackForce;

    float currAttackCooldown = 0f;

    private SpriteRenderer playerSprite;
    private Vector3 lastMoveVector = new Vector3(0, 0, 1);

    Vector3 moveDir;
    void Awake()
    {
        pickaxe = gameObject.transform.GetChild(0).gameObject;
        _rb = GetComponent<Rigidbody>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    public void SetPickaxeState(bool newPickaxeState) {
        if(newPickaxeState && !hasPickaxe) {
            pickaxe.SetActive(true);
        } else if (!newPickaxeState && hasPickaxe) {
            pickaxe.SetActive(false);
        }
        hasPickaxe = newPickaxeState;
    }

    public void Move(Vector2 moveVector)
    {
        moveDir = Vector3.zero;
        moveDir += new Vector3(moveVector.x, 0, moveVector.y);
        moveDir.Normalize();
        moveDir = Quaternion.Euler(0, 45, 0) * moveDir;
    }

    public void Attack()
    {
        if(currAttackCooldown < 0) {
            currAttackCooldown = attackCooldown;
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("player_D") && Vector3.Angle(collider.transform.position-transform.position, lastMoveVector) < attackArc
                && collider.gameObject != this.gameObject)
                {
                    PlayerScript_D targetPlayer = collider.GetComponent<PlayerScript_D>();
                    targetPlayer._rb.AddForce(attackForce *
                    (collider.transform.position - transform.position).normalized, ForceMode.Impulse);
                    targetPlayer.DropPickaxe();
                }
            }
        }
    }

    public void PickaxeAction()
    {
        if(hasPickaxe && pickaxeZone != null && pickaxeActionCoroutine == null) {
            switch (pickaxeZone.tag)
            {
                case "pickaxeZone_D":
                    if (pickaxeZone.GetComponent<GoldSpotScript_D>().GetWorkLeft() > 0) {
                        pickaxeActionCoroutine = StartCoroutine(MineCoroutine());
                        transform.position = pickaxeZone.transform.position;
                    }
                    break;
                case "doorZone_D":
                    if (pickaxeZone.GetComponent<CaveZone_D>().GetWorkLeft() > 0) {
                        pickaxeActionCoroutine = StartCoroutine(DoorCoroutine());
                        transform.position = pickaxeZone.transform.position;
                    }
                    break;
                default:
                    break;
            }
            
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("pickaxeZone_D") || collision.gameObject.CompareTag("doorZone_D"))
        {
            pickaxeZone = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("droppedPickaxe_D"))
        {
            hasPickaxe = true;
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("pickaxeZone_D") || collision.gameObject.CompareTag("doorZone_D"))
        {
            pickaxeZone = null;
        }
    }

    void Update()
    {
        ProcessMove();
        currAttackCooldown -= Time.deltaTime;
    }

    void ProcessMove() {
        if(canMove){
        if(moveDir != Vector3.zero) {
            if (Mathf.Sign((Quaternion.Euler(0, 45, 0) * moveDir).z) > 0) playerSprite.flipX = true;
            else playerSprite.flipX = false;
            playerAnimator.SetBool("isMoving", true);
            lastMoveVector = moveDir;
        }
        else {
            if (Mathf.Sign((Quaternion.Euler(0, 45, 0) * lastMoveVector).z) > 0) playerSprite.flipX = true;
            else playerSprite.flipX = false;
            playerAnimator.SetBool("isMoving", false);
        }
        _rb.MovePosition(_rb.position + moveDir * speed * Time.deltaTime);     
        } 
    }

    public void AddGold() {
        //goldCollected?.Invoke();
        gameManager.AddGoldToPlayer(index);
    }

    public void DoDeath() {
        if(hasPickaxe) Instantiate(pickaxePrefab, transform.position, Quaternion.Euler(45, 45, 0));
        pickaxeZone = null;
        hasPickaxe = false;
        gameManager.playerRespawn(index);
    }

    public void DropPickaxe() {
        if(hasPickaxe) Instantiate(pickaxePrefab, transform.position, Quaternion.Euler(45, 45, 0));
        hasPickaxe = false;
    }
    IEnumerator MineCoroutine()
    {
        canMove = false;
        canPunch = false;
        playerSprite.flipX = false;
        GoldSpotScript_D zoneScript = pickaxeZone.GetComponent<GoldSpotScript_D>();
        
        actionTimer.transform.gameObject.SetActive(true);
        actionTimer.StartTimer(1, zoneScript.GetWorkLeft());
        yield return new WaitForSeconds(1);
        zoneScript.DoWork();
        pickaxeActionCoroutine = null;
        canMove = true;
        canPunch = true;
    }

    IEnumerator DoorCoroutine()
    {
        canMove = false;
        canPunch = false;
        playerSprite.flipX = false;
        CaveZone_D zoneScript = pickaxeZone.GetComponent<CaveZone_D>();
        
        actionTimer.transform.gameObject.SetActive(true);
        actionTimer.StartTimer(1, zoneScript.GetWorkLeft());
        yield return new WaitForSeconds(1);
        zoneScript.DoWork();
        pickaxeActionCoroutine = null;
        canMove = true;
        canPunch = true;
    }
}
