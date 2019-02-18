using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public float playerScore = 0.0f;
    //y aqui es donde vamos a.... esta clase es totalmente innecesaria

    Horde horde;

    void start()
    {
        horde = FindObjectOfType<Horde>();
    }

    public void scoreAnimation()
    {
        horde = FindObjectOfType<Horde>();
        horde.speed = 0.0f;

        if (horde.speed <= 0.0f) horde.speed = 0.0f;
    }
}
