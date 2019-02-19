using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    [SerializeField] AudioClip soundEnter;
    [SerializeField] AudioClip soundClick;

    [SerializeField] AudioSource audioSource;

    public void PlayClick()
    {
        audioSource.clip = soundClick;
        /*if (!audioSource.isPlaying)*/ audioSource.Play();
    }

    public void PlayEnter()
    {
        audioSource.clip = soundEnter;
        /*if (!audioSource.isPlaying)*/ audioSource.Play();
    }
}
