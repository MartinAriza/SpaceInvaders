using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{
    [SerializeField] Horde hordePrefab;
    [SerializeField] Vector3 initialPosition;
    
    [SerializeField] float speedIncrement = 10.0f;
    [SerializeField] float downSpeedIncrement = 0.25f;

    [SerializeField] int hpIncrement = 1;
    [SerializeField] float shotSpeedIncrement = 0.5f;
    [SerializeField] float alienScoreValueIncrement = 10.0f;

    [SerializeField] int extraPlayerLives = 1;
    [SerializeField] int extraBarriersHP = 2;

    Horde horde;
    PlayerMov player;

    [HideInInspector] public int waveNumber = 1;

    Barrier [] barriers;

    void Start()
    {
        player = FindObjectOfType<PlayerMov>();
        barriers = FindObjectsOfType<Barrier>();
    }

    public void spawnNewHorde()
    {
        horde = Instantiate(hordePrefab, initialPosition, Quaternion.identity);

        horde.speed += speedIncrement * waveNumber;
        horde.downSpeed += downSpeedIncrement * waveNumber;

        horde.alienShotSpeed += shotSpeedIncrement * waveNumber;
        horde.alienScoreValue += alienScoreValueIncrement * waveNumber;
        if (waveNumber <= 3)
        {
            player.HP += extraPlayerLives;
            horde.alienHP += hpIncrement * waveNumber;
        }

        foreach(Barrier barrier in barriers)
        {
            barrier.restore();
            barrier.HP += extraBarriersHP * waveNumber;
        }

        waveNumber++;
    }
}
