
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Este script hace que als hordas sean cada vez más difíciles
public class HordeManager : MonoBehaviour
{
    [SerializeField] [Tooltip("Cuanto aumenta la velocida horizontal de la horda siguiente al matar a la anterior")] float speedIncrement = 10.0f;
    [SerializeField] [Tooltip("Cuanto aumenta la velocida vertical de la horda siguiente al matar a la anterior")] float downSpeedIncrement = 0.25f;

    [SerializeField] [Tooltip("Cuanto aumenta la vida de los aliens al matar a una horda completa")] int hpIncrement = 1;
    [SerializeField] [Tooltip("Cuanto aumenta la velocidad de disparo de los aliens ")]float shotSpeedIncrement = 0.5f;
    [SerializeField] [Tooltip("Cuanto aumenta el valor en puntos de los aliens")]float alienScoreValueIncrement = 10.0f;

    [SerializeField] [Tooltip("Cuantas vidas extras se le dan al jugador entre horda y horda")]int extraPlayerLives = 1;
    [SerializeField] [Tooltip("Cuantas vidas sele dan a las barreras entre horda y horda")] int extraBarriersHP = 2;

    [SerializeField] AlienMiniBoss alienMiniBoss;

    static Horde horde;
    PlayerMov player;
    [SerializeField] Text waveNumberUI;
    [SerializeField] Text playerLivesUI;
    [SerializeField] Dialogue[] dialogues;

    [HideInInspector] public int waveNumber = 1;

    Barrier [] barriers;

    void Start()
    {
        horde = FindObjectOfType<Horde>();
        player = FindObjectOfType<PlayerMov>();
        barriers = FindObjectsOfType<Barrier>();
        checkDialogue();
    }

    public void increaseHordeStats()
    {
        waveNumber++;
        waveNumberUI.text = "Oleada    " + waveNumber;

        //Se ajustan las propiedades de la nueva horda según en que oleada esté el jugador
        horde.speed = Mathf.Abs(horde.speed) + speedIncrement * waveNumber;
        horde.downSpeed += downSpeedIncrement * waveNumber;

        horde.alienShotSpeed += shotSpeedIncrement * waveNumber;
        horde.alienScoreValue += alienScoreValueIncrement * waveNumber;

        //A partir de la oleada 3 ni el jugador ni los aliens reciben puntos de vida extra
        if (waveNumber <= 3)
        {
            player.HP += extraPlayerLives;
            //horde.alienHP += hpIncrement * waveNumber;
        }

        if (waveNumber >= 3) alienMiniBoss.gameObject.SetActive(true);

        //Se restauran las barreras y se les modifican los puntos de vida según el número de oleada
        foreach(Barrier barrier in barriers)
        {
            barrier.restore();
            barrier.findHorde();
            barrier.HP += extraBarriersHP * waveNumber;
        }

        checkDialogue();
    }

    private void Update()
    {
        playerLivesUI.text =  player.HP + "    Vidas";
    }

    private void checkDialogue()
    {
        switch(waveNumber)
        {
            case 1:
                dialogues[0].startDialogue();
                break;
            case 2:
                dialogues[1].startDialogue();
                break;
            case 3:
                dialogues[2].startDialogue();
                break;
            case 5:
                dialogues[3].startDialogue();
                break;
        }
    }
}