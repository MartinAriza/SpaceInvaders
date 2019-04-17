using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsSound : MonoBehaviour
{
    [SerializeField] AudioSource leftFoot;
    [SerializeField] AudioSource rightFoot;

    [SerializeField] [Range(0f, 1f)] float sneakVolume = 0.05f;
    [SerializeField] [Range(0f, 1f)] float walkVolume = 0.1f;
    [SerializeField] [Range(0f, 1f)] float runVolume = 0.3f;
    

    void right(string state)
    {
        switch(state)
        {
            case "Sneaking":
                rightFoot.volume = sneakVolume;
                break;

            case "Walking":
                rightFoot.volume = walkVolume;
                break;

            case "Running":
                rightFoot.volume = runVolume;
                break;
        }

        rightFoot.Play();
    }

    void left(string state)
    {
        switch (state)
        {
            case "Sneaking":
                leftFoot.volume = sneakVolume;
                break;

            case "Walking":
                leftFoot.volume = walkVolume;
                break;

            case "Running":
                leftFoot.volume = runVolume;
                break;
        }

        leftFoot.Play();
    }
}
