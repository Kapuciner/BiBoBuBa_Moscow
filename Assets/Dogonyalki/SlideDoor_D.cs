using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor_D : MonoBehaviour
{
    // Start is called before the first frame update
    public float deltaY;
    float startY;

    public int workNeeded;
    void Start()
    {
        startY = transform.position.y;
    }

    public void SlideDoorOneStep() {
        transform.position = transform.position + new Vector3(0, -deltaY / workNeeded, 0);
        workNeeded -= 1;
    }

}
