using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylindre_Roll : MonoBehaviour
{

    public float speed; //Variable de velocidad

    // Start is called before the first frame update
    void Start()
    {
        speed = 10f; //Se le da un valor de velocidad
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, Time.deltaTime * speed); //Se aplica una transformación de rotación con la velocidad establecida
    }
}
