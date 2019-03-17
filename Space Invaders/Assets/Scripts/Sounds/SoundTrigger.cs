using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{

    AudioSource SoundToPlay;
    [SerializeField] bool repeat;

    private void Start()
    {
        SoundToPlay = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (repeat)
            {
                if (!SoundToPlay.isPlaying) SoundToPlay.Play();
            } else
            {
                SoundToPlay.Play();
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
