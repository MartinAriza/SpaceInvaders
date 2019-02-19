using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float playerScore = 0.0f;    

    Horde horde;
    [SerializeField] Text scoreText;

    void start()
    {
        //Se recogen los componentes necesarios
        horde = FindObjectOfType<Horde>();
    }

    //Se detiene la horda, deja de disparar y se muestre la puntuación por pantalla con un botón para volver al menú
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
