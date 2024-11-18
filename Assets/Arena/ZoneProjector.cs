using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class ZoneProjector : MonoBehaviour
{
    [SerializeField] private DecalProjector decalProjector;
    [SerializeField] private float maxFadeValue;

    private void Start()
    {
        decalProjector.fadeFactor = maxFadeValue;
    }
    public IEnumerator SetDefaultSize()
    {
        float duration = 1;
        float elapsedTime = 0f;

        float initialFade = decalProjector.fadeFactor;
        float targetFade = 0;


        while (decalProjector.fadeFactor > 0)
        {
            elapsedTime += Time.deltaTime;

            decalProjector.fadeFactor = Mathf.Lerp(initialFade, targetFade, elapsedTime / duration);

            yield return null;
        }

        decalProjector.size = new Vector3(0, 0, 0.689f);
    }

    public IEnumerator StartMarking(float radius, Vector3 newPosition, float duration)
    {
        decalProjector.fadeFactor = maxFadeValue;
        this.transform.position = newPosition;

        float elapsedTime = 0f;

        Vector3 initialSize = new Vector3(0, 0, 0.689f); 
        Vector3 targetSize = new Vector3(radius * 2, radius * 2, 0.689f);

        while (decalProjector.size.x < radius * 2)
        {
            elapsedTime += Time.deltaTime;

            decalProjector.size = Vector3.Lerp(initialSize, targetSize, elapsedTime / duration);

            yield return null; 
        }

    }
}
