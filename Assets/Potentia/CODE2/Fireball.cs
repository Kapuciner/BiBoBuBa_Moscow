using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    float currentAngle = 0.0f;

    [SerializeField] PlayerManager centerBody;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform targetCamera;

    private void Start()
    {
        targetCamera = FindObjectOfType<Camera>().transform;
    }
    void Update()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        currentAngle %= 360;
        float radians = currentAngle * Mathf.Deg2Rad; 
        transform.position = new Vector3(centerBody.transform.position.x + Mathf.Sin(radians) * 6, centerBody.transform.position.y, centerBody.transform.position.z + Mathf.Cos(radians) * 6);
        Quaternion rot = Quaternion.LookRotation( transform.position - targetCamera.position, Vector3.forward );
		transform.rotation = rot;
        Transform ballTrans = transform.Find("ball");
        ballTrans.LookAt(targetCamera);
        //ballTrans.rotation = Quaternion.LookRotation( ballTrans.position - centerBody.transform.position, new Vector3(-1,0,1) );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cloud")
        {
            // ???
        }
    }
}
