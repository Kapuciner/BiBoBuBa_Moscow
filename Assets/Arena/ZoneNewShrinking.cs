using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneNewShrinking : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float minimumScale;
    [SerializeField] private float maximumScale;

    bool decrease = true;
    bool increase = false;

    [SerializeField] bool repetitiveMode = false;

    [SerializeField] private ZoneProjector zoneProjector;

    private void Start()
    {
        this.transform.position = new Vector3(0, 0, 0);
        this.transform.position += new Vector3(Random.Range(-23, 23), 0, Random.Range(-23, 23));
    }
    public void StartShrinking()
    {
        StartCoroutine(ShrinkAndExpand());
        StartCoroutine(zoneProjector.StartMarking(minimumScale, this.transform.position, (maximumScale - minimumScale) / shrinkSpeed));
    }

    IEnumerator ShrinkAndExpand()
    {
        while (true)
        {
            if (transform.localScale.x > minimumScale && decrease)
                transform.localScale = new Vector3(transform.localScale.x - shrinkSpeed * Time.deltaTime, transform.localScale.y,
                    transform.localScale.z - shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= minimumScale && decrease && repetitiveMode)
            {
                Invoke("StartIncrease", 5);
                decrease = false;
            }
            if (transform.localScale.x < maximumScale && increase)
                transform.localScale = new Vector3(transform.localScale.x + shrinkSpeed * Time.deltaTime, transform.localScale.y,
                    transform.localScale.z + shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x >= maximumScale && increase)
            {
                Invoke("StartDecrease", 0);
                increase = false;
            }
            yield return null;
        }
    }


    void StartIncrease()
    {
        StartCoroutine(zoneProjector.SetDefaultSize());
        increase = true;
    }

    void StartDecrease()
    {
        if (minimumScale != 8)
            minimumScale -= 3;
        if (minimumScale < 8)
            minimumScale = 8;

        shrinkSpeed += 5f;
        decrease = true;
        this.transform.position = new Vector3(0, 0, 0);
        this.transform.position += new Vector3(Random.Range(-25, 25), 0, Random.Range(-25, 25));

        StartCoroutine(zoneProjector.StartMarking(minimumScale, this.transform.position, (maximumScale - minimumScale)/shrinkSpeed));
    }
}
