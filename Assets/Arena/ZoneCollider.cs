using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class ZoneCollider : MonoBehaviour
{
    public Volume volume;
    int numOfPlayersInZone = 0;
    [SerializeField] Vignette vignette;
    [SerializeField] float initialSmoothness;
    [SerializeField] float targetSmoothness;
    [SerializeField] GameManagerArena gma;
    private int playerCount;
    public float duration = 2f;
    public float durationBack = 1f;
    [SerializeField] Color initialColor = Color.black;      
    [SerializeField] Color targetColor = Color.red;
    Coroutine makeDark;
    Coroutine makeBright;

    [SerializeField] AudioSource audio;
    [SerializeField] float initialAudio;
    [SerializeField] float targetAudio;
    private void Start()
    {
        

        volume.profile.TryGet<Vignette>(out vignette);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            numOfPlayersInZone++;
            other.GetComponent<ArenaPlayerManager>().insideTheZone = true;

            print($"in zone Count {numOfPlayersInZone}");
            print($"player Count {gma.getPlayerCount()}");

            if (numOfPlayersInZone == gma.getPlayerCount() && makeDark != null)
            {
                StopCoroutine(makeDark);
                makeBright = StartCoroutine(ScreenLightens());
                makeDark = null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            numOfPlayersInZone--;
            if (makeBright != null)
                StopCoroutine(makeBright);
            if (makeDark == null)
                makeDark = StartCoroutine(ScreenDarkens());
            other.GetComponent<ArenaPlayerManager>().insideTheZone = false;
        }
    }

    IEnumerator ScreenDarkens()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            audio.pitch = Mathf.Lerp(initialAudio, targetAudio, t);
            vignette.smoothness.value = Mathf.Lerp(initialSmoothness, targetSmoothness, t);
            vignette.color.value = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }
        vignette.smoothness.value = targetSmoothness;
    }

    IEnumerator ScreenLightens()
    {
        float elapsed = 0f;
        float newInitialSmoothness = vignette.smoothness.value;
        Color newInitialColor = vignette.color.value;
        float newInitialAudio = audio.pitch;
        while (elapsed < durationBack)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / durationBack;
            audio.pitch = Mathf.Lerp(newInitialAudio, initialAudio, t);
            vignette.smoothness.value = Mathf.Lerp(newInitialSmoothness, initialSmoothness, t);
            vignette.color.value = Color.Lerp(newInitialColor, initialColor, t);
            yield return null;
        }
        vignette.smoothness.value = initialSmoothness;
        vignette.color.value = initialColor;
        makeBright = null;
    }
}
