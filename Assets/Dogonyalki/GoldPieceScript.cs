using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class GoldPieceScript : MonoBehaviour
{
    Coroutine turnOffInTime;

    bool available = false;
    SphereCollider goldCollider;
    void Start()
    {
        goldCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        
    }

    public void SetGoldAvailable(float availableTime = 20)
    {

        available = true;
        print(available);
        // turnOffInTime = StartCoroutine(OffColliderInTime(availableTime));
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("player_D") && available)
        {
            collision.GetComponent<PlayerScript_D>().AddGold();
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator OffColliderInTime(float offInTime){
        yield return new WaitForSeconds(offInTime);
        GetComponent<Collider>().enabled = false;
    }
}
