using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesController : MonoBehaviour
{

    private Vector3 originalLook;       //Almacenará el vector de dirección original del ojo
    [SerializeField] PlayerMov target;  //Guarda una referencia al objeto al que mira el ojo
    float maxAngle = 50.0f;             //Ángulo de giro máximo del ojo respecto a originalLook


    void Start()
    {
        originalLook = transform.forward;       //Al inicio guardamos el vector forward (z) de la base geométrica del objeto
        target = FindObjectOfType<PlayerMov>(); //Al inicio encuentra el jugador y guarda una referencia a él
    }

    void Update()
    {
        Vector3 actualLook = transform.forward;                             //Guarda el vector de dirección de los ojos al inicio del frame
        transform.LookAt(target.transform);                                 //Apunto al jugador (ahora el ojo mira hacia el jugador target)
        float actualAngle = Vector3.Angle(originalLook, transform.forward); //Guardo el ángulo entre el vector de dirección original y el actual
        if (actualAngle > maxAngle)                                         //Si es mayor que el ángulo máximo permitido:
        {
            transform.LookAt(transform.position + actualLook);              //Mantengo la dirección al inicio del frame
        }
    }
}