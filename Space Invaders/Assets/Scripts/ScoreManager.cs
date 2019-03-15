using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text scoreTextUI;

    public float playerScore = 0.0f;
    public string playerName;
    public InputField inp;
    //public Button backToMenu;
    [SerializeField]GameObject ranking;
    bool setName;

    Horde horde;
    [SerializeField] Text scoreText;
    [SerializeField] Text setNameText;
    [SerializeField] Text nameText;
    

    void start()
    {
        //Se recogen los componentes necesarios
        horde = FindObjectOfType<Horde>();
        setName = false;
    }

    private void Update()
    {
        if(inp.gameObject.activeSelf == true && setName)
        {
            if(inp.text.Length == 3)
            {
                inp.interactable = false;
                //backToMenu.gameObject.SetActive(true);
                ranking.gameObject.SetActive(true);
                playerName = inp.text;
                playerName.ToUpper();
                rankingController.insertPlayer((int)playerScore, playerName);
                rankingController.insertPlayer(1000, "PCM");
                setName = false;
                inp.gameObject.SetActive(false);
                setNameText.gameObject.SetActive(false);
                scoreText.gameObject.SetActive(false);
            }
        }

        scoreTextUI.text = "score   " + playerScore;
    }

    //Se detiene la horda, deja de disparar y se muestre la puntuación por pantalla con un botón para volver al menú
    public void scoreAnimation()
    {
        horde = FindObjectOfType<Horde>();
        horde.speed = 0.0f;
        horde.stopFiring();

        inp.gameObject.SetActive(true);
        setNameText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        scoreText.text = "Puntos:  " + playerScore.ToString();
        setName = true;

        if (horde.speed <= 0.0f) horde.speed = 0.0f;
    }
}
