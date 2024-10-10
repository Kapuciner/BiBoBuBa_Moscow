using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_D : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] private Transform targetCamera;

    float timer = 0;

    void Awake(){
        this.transform.localPosition = new Vector3(1, 0, 0);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    void Update()
    {
        timer += Time.deltaTime;
        transform.localPosition = transform.localPosition + Vector3.right * Time.deltaTime * speed;
        if(timer > 10) Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("player_D"))
        {
            collision.GetComponent<PlayerScript_D>().DoDeath();
        }
        Destroy(this.gameObject);
    }
}
