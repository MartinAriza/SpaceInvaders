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

    Horde horde;
    PlayerMov player;

    [HideInInspector] public int waveNumber = 1;

    void Start()
    {
        player = FindObjectOfType<PlayerMov>();
    }

    public void spawnNewHorde()
    {
        horde = Instantiate(hordePrefab, initialPosition, Quaternion.identity);

        horde.speed += speedIncrement * waveNumber;
        horde.downSpeed += downSpeedIncrement * waveNumber;

        horde.alienHP += hpIncrement * waveNumber;
        horde.alienShotSpeed += shotSpeedIncrement * waveNumber;
        horde.alienScoreValue += alienScoreValueIncrement * waveNumber;

        player.HP += extraPlayerLives;

        waveNumber++;
    }
}
