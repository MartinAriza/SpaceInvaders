using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Parte del objeto game manager, este script cambia el comportamiento del juego
 * según si el jugador es mayor de 13 años (puede disparar) o no.
 */

public class AdultManager : MonoBehaviour
{
    public static bool adult; //Variable static que cambia el botón de adulto, se mantiene entre escenas
    public static bool bounce;
    public bool adultReference;

    void Awake()
    { 
        adultReference = adult;
        //Transmite si eres mayor de 13 o no a la horda de aliens y al jugador para cambiar su comportamiento
        PlayerMov player = FindObjectOfType<PlayerMov>();
        player.adult = adult;
        player.bounce = bounce;

        FindObjectOfType<Horde>().adult = adult;
    }
}
