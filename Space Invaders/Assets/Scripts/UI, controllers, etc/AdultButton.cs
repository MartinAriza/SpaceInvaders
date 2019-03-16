using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Funcionamiento del botón para elegir si eres adulto
public class AdultButton : MonoBehaviour
{
    public void setAdult(bool b)
    {
        AdultManager.adult = b; //Se cambia la variable static adult al valor b indicado
    }
}