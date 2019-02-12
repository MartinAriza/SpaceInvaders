using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    [SerializeField] float downSpeed = 1.0f;

    Rigidbody rb;
    BoxCollider bc;
    Alien [] aliens;

    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
        aliens = FindObjectsOfType<Alien>();

        calculateBoxCollider();
    }

    //Es public ya que los aliens lo llaman cuando son destruidos (NOTA: al destruit aliens debemos ponerlos en disable no destruir el gameObject) 
    public void calculateBoxCollider() //El box collider de la horda ajusta su tamaño para el cálculo de colisiones con los limites
    {
        //Punto más arriba, abajo, a la izquierda y a la derecha de los aliens de la horda
        float top = 0;
        float bottom = 0;
        float left = 0;
        float right = 0;

        foreach (Alien alien in aliens) //Obtenemos el valor de los puntos
        {
            if(alien.isActiveAndEnabled)
            {
                if (alien.transform.localPosition.z > top) { top = alien.transform.localPosition.z; }
                if (alien.transform.localPosition.z < bottom) { bottom = alien.transform.localPosition.z; }

                if (alien.transform.localPosition.x < right) { right = alien.transform.localPosition.x; }
                if (alien.transform.localPosition.x > left) { left = alien.transform.localPosition.x; }
            }
        }

        //Se recalcula el box collider para el valor de esos puntos
        bc.center = new Vector3((left + right) / 2, 0, (top + bottom) / 2);
        bc.size = new Vector3(Mathf.Abs(right - left), 1, Mathf.Abs(top - bottom));
    }

    void Update()
    {
        rb.velocity = Vector3.right * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Limit") //Para evitar que interactua con los colliders de los aliens, ver Compound Colliders en el manual
        {
            speed = -speed;
            rb.MovePosition(new Vector3(transform.position.x, transform.position.y, transform.position.z - downSpeed)); //¿Mas suave?
        }
    }
}
