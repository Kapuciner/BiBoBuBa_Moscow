using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    [SerializeField] CloudTargetScript cloudTarget;
    [SerializeField] float cloudSpeed;
    public bool canMove = true;
    bool canCast = true;
    // [SerializeField] lightningScript lightningAbility;
    [SerializeField] AudioSource pushSound;
    [SerializeField] GameObject lightningPrefab;
    [SerializeField] GameObject lightningSmallPrefab;
    [SerializeField] GameObject wallPrefab;

    [SerializeField] GameObject icePrefab;

    SpriteRenderer cloudSprite;
    Animator _animator;
    bool canAnimationBeChanged = true;

    [SerializeField] float iceFieldLifetime;
    [SerializeField] float lightningDelay;

    [SerializeField] float lightningDelta;
    [SerializeField] float wallAnimTime;
    [SerializeField] float wallLiveTime;

    [SerializeField] float pushRadius;
    [SerializeField] int pushDamage;
    [SerializeField] float pushForce;

    [SerializeField] LayerMask heightMask;

    [SerializeField] GameManager gm;
    bool again = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        _animator = GetComponent<Animator>();
        cloudSprite = GetComponent<SpriteRenderer>();
        pushSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
            processInput();
            if (canMove)
            {
                Vector3 targetDelta = cloudTarget.transform.position - transform.position;
                if(targetDelta.sqrMagnitude > 1) {
                    //print(targetDelta.sqrMagnitude);
                    if (Mathf.Sign((Quaternion.Euler(0, 45, 0) * targetDelta).z) > 0)
                    {
                        cloudSprite.flipX = true;
                    }
                    else cloudSprite.flipX = false;

                    Vector3 deltaVec = new Vector3(
                        cloudTarget.transform.position.x - transform.position.x, 
                        0,
                        cloudTarget.transform.position.z - transform.position.z);
                    deltaVec.Normalize();
                    // deltaVec = Quaternion.Euler(0, 0, 90) * deltaVec * 10;
                    RaycastHit rayHitHorizontal;
                    Physics.Raycast(new Ray(transform.position, deltaVec), out rayHitHorizontal, heightMask);
                    Vector3 hitBoxVec = new Vector3(deltaVec.x, -0.4f, deltaVec.z);
                    Debug.DrawRay(transform.position, hitBoxVec * 10, Color.red);
                    RaycastHit rayHitAngle;
                    Physics.Raycast(new Ray(transform.position, hitBoxVec), out rayHitAngle, heightMask);
                    if ((rayHitAngle.distance < 2.5f && rayHitAngle.distance != 0) || 
                    (rayHitHorizontal.distance < 10f && rayHitHorizontal.distance != 0))
                    {
                        Vector3 horizontalMove = new Vector3(cloudTarget.transform.position.x, 
                        transform.position.y + 550/(rayHitAngle.distance - 0.6f), 
                        cloudTarget.transform.position.z);
                        transform.position = Vector3.MoveTowards(transform.position, horizontalMove, cloudSpeed);
                        //transform.position = new Vector3(transform.position.x, transform.position.y + (4.4f - rayHit.distance)* 10, transform.position.z);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, cloudTarget.transform.position, cloudSpeed);
                    }
                    /*float deltaY = cloudTarget.transform.position.y - transform.position.y;
                    if (deltaY > 2)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + deltaY / 10, transform.position.z);
                    }
                    else if (deltaY < -2)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y + deltaY / 10, transform.position.z);
                    }*/
                }
            }
    }
    /*Physics.Raycast(new Ray(transform.position, Vector3.down), out rayHit, heightMask);
            if (rayHit.distance < 4.4f && rayHit.distance != 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (4.4f - rayHit.distance)* 10, transform.position.z);
            }
            else if (rayHit.distance > 4.6f && rayHit.distance != 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (rayHit.distance - 4.6f), transform.position.z);
            }*/

    void processInput() {
        //Left button Input.GetMouseButton(0)
        //if (canCast && Input.GetKey(KeyCode.Alpha2)) { 
        //    StartCoroutine(LightningCoroutine(1));
        //    _animator.Play("cloud_cast");
        //} else if (canCast && Input.GetKey(KeyCode.Alpha1)) {
        //    StartCoroutine(WallCoroutine());
        //     _animator.Play("cloud_cast");
        //} else if (canCast && Input.GetKey(KeyCode.Alpha3)) {
        //    StartCoroutine(LightningCoroutine(19));
        //    StartCoroutine(LightningLongAnimation(10f));
        //} else if (canCast && Input.GetKey(KeyCode.Alpha4)) {
        //    StartCoroutine(PushCoroutine());
        //     _animator.Play("cloud_cast");
        //} else if (canCast && Input.GetKey(KeyCode.Alpha5)) {
        //    StartCoroutine(IceCoroutine());
        //    _animator.Play("cloud_cast");
        //}
    }

    public void gotPickup(int power) {

    }
    
    public void Ability1()
    {
        StartCoroutine(LightningCoroutine(1));
        _animator.Play("cloud_cast");
    }

    public void Ability2()
    {
        StartCoroutine(WallCoroutine());
        _animator.Play("cloud_cast");

    }

    public void Ability3()
    {
            StartCoroutine(LightningCoroutine(30));
        StartCoroutine(LightningLongAnimation(12f));
    }

    public void Ability4()
    { 
        StartCoroutine(PushCoroutine());
        _animator.Play("cloud_cast");
    }

    public void Ability5()
    {
            StartCoroutine(IceCoroutine());
        _animator.Play("cloud_cast");
    }
    IEnumerator LightningCoroutine(int lightningCount) {
        
        canCast = false;
        RaycastHit rayHit;
        Physics.Raycast(new Ray(transform.position, Vector3.down), out rayHit, heightMask);
        Vector3 toLand;
        if(rayHit.distance != 0) toLand = new Vector3(0, -rayHit.distance, 0);
        else toLand = new Vector3(0, -1.4f, 0);
        if (lightningCount == 1) {
            canMove = false;
            Instantiate(lightningPrefab, transform.position + toLand, Quaternion.identity);
            yield return new WaitForSeconds(lightningDelay);
        } else {
            for (int i = 0; i < lightningCount; i++) {
                
                Vector3 randomDeltaXZ = new Vector3(Random.Range(-lightningDelta, lightningDelta), 0, Random.Range(-lightningDelta, lightningDelta));
                Physics.Raycast(new Ray(transform.position+randomDeltaXZ, Vector3.down), out rayHit, heightMask);
                if(rayHit.distance != 0) toLand = new Vector3(0, -rayHit.distance, 0);
                else toLand = new Vector3(0, -1.4f, 0);
                Instantiate(lightningSmallPrefab, transform.position + toLand + randomDeltaXZ, Quaternion.identity);
                yield return new WaitForSeconds(0.2f);  
            }
        }
        _animator.SetBool("longCast", false);
        canMove = true;
        canCast = true;
    }

    IEnumerator LightningLongAnimation(float time) {
            _animator.SetBool("longCast", true);
            _animator.Play("cloud_cast");
            yield return new WaitForSeconds(time - 1.05f);
            _animator.SetBool("longCast", false);
    }

    IEnumerator WallCoroutine() {
        canMove = false;
        canCast = false;
        
        float timePassed = 0;
        Vector3 startScale = transform.localScale;
        Vector3 deltaScale = new Vector3(2, 1, 2) - startScale;
        while (timePassed < wallAnimTime / 2){
            transform.localScale = startScale + deltaScale * (timePassed / (wallAnimTime / 2));
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timePassed = 0;


        List<GameObject> rocks = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDeltaXZ = new Vector3(Random.Range(-lightningDelta, lightningDelta), 0, Random.Range(-lightningDelta, lightningDelta));
            rocks.Add(Instantiate(wallPrefab, transform.position + randomDeltaXZ, Quaternion.identity));
            yield return new WaitForSeconds(0.1f);
        }

        Vector3 midScale = transform.localScale;
        while (timePassed < wallAnimTime / 2){
            transform.localScale = midScale - deltaScale * (timePassed / (wallAnimTime / 2));
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        canMove = true;
        canCast = true;

        timePassed = 0;
        Color wallMaterial = rocks[0].GetComponent<MeshRenderer>().material.color;
        while (timePassed < wallLiveTime) {
            for (int i = 0; i < 10; i++)
            {
                rocks[i].GetComponent<MeshRenderer>().material.color = new Color(wallMaterial.r, wallMaterial.g, wallMaterial.b, 1 - timePassed / wallLiveTime);

            }
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 10; i++)
        {
            Destroy(rocks[i]);
        }
    }

    IEnumerator FireBallCoroutine()
    {
        SpriteRenderer _sr = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 6; i++)
        {
            canMove = false;
            canCast = false;
            _sr.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            _sr.color = Color.white;
            yield return new WaitForSeconds(0.3f);

        }
        canMove = true;
        canCast = true;
        again = false;
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "fireball" && again == false)
        {
            StartCoroutine(FireBallCoroutine());
            again = true;
        }
    }

    IEnumerator PushCoroutine() {
        canMove = false;
        canCast = false;
        yield return new WaitForSeconds(0.15f);
        pushSound.PlayOneShot(pushSound.clip);
        yield return new WaitForSeconds(0.4f);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, pushRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Mage"))
            {
                Vector3 cloudFixedY = new Vector3(transform.position.x, collider.transform.position.y, transform.position.z);
                collider.GetComponent<player>().TakeDamage(pushDamage);
                collider.GetComponent<PlayerManager>()._rb.AddForce(pushForce *
                (collider.transform.position - cloudFixedY).normalized, ForceMode.Impulse);
            }
        }
        canMove = true;
        canCast = true;
    }
    IEnumerator IceCoroutine() {
        canMove = false;
        canCast = false;
        yield return new WaitForSeconds(1);
        Instantiate(icePrefab, transform.position + Vector3.down * 2, Quaternion.Euler(90, 0, 0));;
        canMove = true;
        canCast = true;



    }
    

    
}
