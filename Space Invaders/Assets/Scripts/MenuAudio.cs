using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    [SerializeField] AudioClip soundEnter;  //Audio que suena al poner el ratón encima de un botón
    [SerializeField] AudioClip soundClick;  //Audio que suena al hacer click en un botón

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
