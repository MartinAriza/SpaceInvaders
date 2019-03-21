using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Funcionamiento del botón para elegir si eres adulto
public class AdultButton : MonoBehaviour
{
    [SerializeField] Text rebotesButton;

    public void setAdult(bool b)
    {
        AdultManager.adult = b; //Se cambia la variable static adult al valor b indicado
        if(rebotesButton != null)
        {
            rebotesButton.enabled = true;
        }
    }
}