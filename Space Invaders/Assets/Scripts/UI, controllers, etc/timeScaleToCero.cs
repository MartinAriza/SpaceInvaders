using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeScaleToCero : MonoBehaviour
{
    public void changeTimeScale(bool paused)
    {
        if (paused) Time.timeScale = 0.0f;
        else Time.timeScale = 1.0f;
    }
}
