using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
public class ZoneShrinking : MonoBehaviour
{
    public DecalProjector decalProjector;
    public Transform zoneCylinder; 
    public float shrinkDuration = 95f; 
    public float minWidth = 25f;
    public float minHeight = 25f;

    public float minScale = 25;

    private void Start()
    {
        StartCoroutine(ShrinkZoneAfterDelay(5f));
    }

    private IEnumerator ShrinkZoneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        decalProjector.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Vector2 initialSize = new Vector2(decalProjector.size.x, decalProjector.size.y);
        Vector3 initialScale = zoneCylinder.localScale;

        while (elapsedTime < shrinkDuration)
        {
            float t = elapsedTime / shrinkDuration;

            float newWidth = Mathf.Lerp(initialSize.x, minWidth, t);
            float newHeight = Mathf.Lerp(initialSize.y, minHeight, t);
            decalProjector.size = new Vector3(newWidth, newHeight, 0.689f);

            float newScale = Mathf.Lerp(initialScale.x, minScale, t);
            zoneCylinder.localScale = new Vector3(newScale, newScale, newScale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Завершите сужение
        decalProjector.size = new Vector3(minWidth, minHeight, 0.689f);
        zoneCylinder.localScale = new Vector3(minScale, minScale, minScale);
    }
}
