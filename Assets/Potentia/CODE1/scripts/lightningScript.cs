using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lightningScript : MonoBehaviour
{
    Transform c_contour;
    Transform c_filled;

    Animator animator;
    [SerializeField] float delay;
    [SerializeField] int lightningDamage;
    // Start is called before the first frame update
    void Start()
    {
        c_contour = transform.Find("circle_contour");
        c_filled = transform.Find("circle_filled");
        animator = transform.Find("animation").GetComponent<Animator>();
        animator.gameObject.SetActive(false);
        StartCoroutine(filling(delay));
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayDelayed(0.5f );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator filling(float fillTime) {
        float timePassed = 0;
        Vector3 startScale = c_filled.transform.localScale;
        Vector3 deltaScale = c_contour.transform.localScale - startScale;
        while (timePassed < fillTime){
            c_filled.transform.localScale = startScale + deltaScale * (timePassed / fillTime);
            timePassed += Time.deltaTime;
            if(fillTime - timePassed < 0.3f) animator.gameObject.SetActive(true);
            yield return new WaitForEndOfFrame();
        }
        float radius = c_filled.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Mage"))
            {
                print(collider);
                print(collider.gameObject.GetComponent<player>());
                collider.gameObject.GetComponent<player>().TakeDamage(lightningDamage);
            }
        }
        Destroy(c_filled.gameObject);
        Destroy(c_contour.gameObject);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
