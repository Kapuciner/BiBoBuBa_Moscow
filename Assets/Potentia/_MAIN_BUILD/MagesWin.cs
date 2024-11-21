using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class MagesWin : MonoBehaviour
{
    [SerializeField] float rotationDuration = 5;
    [SerializeField] Vector3 initialScale = new Vector3(1,1,1);
    [SerializeField] Vector3 targetScale = new Vector3(2, 2, 2);
    [SerializeField] Vector3 initialPosition = new Vector3(0, 0, 0);
    [SerializeField] Vector3 targetPosition = new Vector3(0,2,0);

    [SerializeField] float initialSpeed = 0.1f;
    [SerializeField] float targetSpeed = 1f;

    private float rotationSpeed;
    private List<Transform> childObjects = new List<Transform>();

    [SerializeField] Vector3 initialRayScale = new Vector3(1, 1, 1);
    [SerializeField] Vector3 targetRayScale = new Vector3(20, 20, 20);
    [SerializeField] float rayDuration = 2;

    [SerializeField] private GameObject goodRay;
    [SerializeField] private GameObject badRay;
    private GameObject mainRay;

    public Volume volume;
    [SerializeField] Vignette vignette;
    [SerializeField] float initialSmoothness = 0.2f;
    [SerializeField] float initialIntensity;
    [SerializeField] float targetIntensity;

    [SerializeField] GameObject rsp;

    [SerializeField] Material darkMaterial;
    [SerializeField] GameManager gm;
    void Start()
    {
        volume.profile.TryGet<Vignette>(out vignette);
        goodRay.GetComponent<Renderer>().sortingOrder = 2;
        badRay.GetComponent<Renderer>().sortingOrder = 2;
        childObjects = GetChildren(this.gameObject.transform);
        //StartAnimationSequence("mages");
    }

    public void StartAnimationSequence(string winner)
    {
        if (winner == "clouds")
        {
            mainRay = badRay;
        }
        else if (winner == "mages")
        {
            mainRay = goodRay;
        }

        StartCoroutine(RotateBooks());
        StartCoroutine(StartWinSequence());
    }
    IEnumerator StartWinSequence()
    {
        float elapsed = 0f;
        
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;

            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            rotationSpeed = Mathf.Lerp(initialSpeed, targetSpeed, t);

            yield return null;
        }
        mainRay.SetActive(true);

        elapsed = 0f;

        while (elapsed < rayDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rayDuration;

            mainRay.transform.localScale = Vector3.Lerp(initialRayScale, targetRayScale, t);
            vignette.intensity.value = Mathf.Lerp(initialIntensity, targetIntensity, t);
            vignette.smoothness.value = Mathf.Lerp(initialSmoothness, 1, t);

            yield return null;
        }


        elapsed = 0f;

        if (mainRay == badRay)
        {
            initialRayScale = mainRay.transform.localScale;
            targetRayScale = new Vector3(100, 100, 100);

            while (elapsed < rayDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / rayDuration;

                mainRay.transform.localScale = Vector3.Lerp(initialRayScale, targetRayScale, t);

                yield return null;
            }
        }

        rsp.SetActive(true);
    }

    IEnumerator RotateBooks()
    {
        if (gm.cloudWon)
        {
            foreach (Transform child in childObjects)
            {
                if (child.gameObject.activeSelf)
                    child.gameObject.GetComponent<TrailRenderer>().material = darkMaterial;
            }
        }
        else
        {
            foreach (Transform child in childObjects)
            {
                child.gameObject.GetComponent<TrailRenderer>().enabled = false;
            }
        }
        while (true)
        {
            this.transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
            for (int i = 0; i < childObjects.Count; i++)
            {
                childObjects[i].rotation = Quaternion.Euler(45,45,0);

            }
            yield return null;
        }
    }

    List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            children.Add(child);
        }

        return children;
    }

}
