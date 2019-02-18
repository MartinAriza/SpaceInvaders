using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float playerScore = 0.0f;
    //y aqui es donde vamos a.... esta clase es totalmente innecesaria

    Horde horde;
    [SerializeField] Text scoreText;

    void start()
    {
        horde = FindObjectOfType<Horde>();
    }

    public void scoreAnimation()
    {
        horde = FindObjectOfType<Horde>();
        horde.speed = 0.0f;
        horde.stopFiring();

        scoreText.gameObject.SetActive(true);
        scoreText.text = "Puntos:  " + playerScore.ToString();

        if (horde.speed <= 0.0f) horde.speed = 0.0f;
    }
}
