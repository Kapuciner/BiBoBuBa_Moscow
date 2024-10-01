using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    public AudioSource Audio;

    private AudioListener listener;
    private void Start()
    {
        listener = Camera.main.GetComponent<AudioListener>();
    }

    public void Play(AudioClip audioClip)
    {
        Audio.clip = audioClip;
        StartCoroutine(Play());


    }

    IEnumerator Play()
    {
        Audio.Stop();
        yield return new WaitForEndOfFrame();
        Audio.Play(0);
    }
}
