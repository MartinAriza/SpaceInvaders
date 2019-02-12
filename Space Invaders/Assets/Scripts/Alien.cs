using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<CubeEditor>().enabled = false; //Se desactiva el snap para que se puedan mover en el play
    }

    void Update()
    {

    }
}