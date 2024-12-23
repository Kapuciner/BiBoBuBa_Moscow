using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloudTargetScript : MonoBehaviour
{
    public GameManager gm;
    // Start is called before the first frame update
    [SerializeField] public float mouseSensitivity;
    [SerializeField] LayerMask heightMask;

    Vector3 rawMoveDelta = Vector3.zero;

    public void SetDirection(Vector2 moveVector) {
        rawMoveDelta = new Vector3(moveVector.x, 0, moveVector.y) / 20;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
    }

    void Update()
    {
            //Vector3 rawMoveDelta = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));.
            Vector3 processedDelta = Quaternion.Euler(0, 45, 0) * rawMoveDelta;
            transform.position = transform.position + processedDelta * mouseSensitivity;
            RaycastHit rayHit;
            Physics.Raycast(new Ray(transform.position, Vector3.down), out rayHit, heightMask);;
            if (rayHit.distance < 4.4f && rayHit.distance != 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (4.4f - rayHit.distance)* 10, transform.position.z);
            }
            else if (rayHit.distance > 4.6f && rayHit.distance != 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (rayHit.distance - 4.6f), transform.position.z);
            }
            Physics.Raycast(new Ray(transform.position, Vector3.up), out rayHit, heightMask);;
            if (rayHit.distance != 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            }
    }
}
