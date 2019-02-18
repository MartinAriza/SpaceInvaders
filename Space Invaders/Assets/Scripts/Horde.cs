using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde : MonoBehaviour
{
    public float speed = 1.0f;
    public float downSpeed = 1.0f;

    public int alienHP = 1;
    public float alienShotSpeed = 15;
    public float alienScoreValue = 10;
    public float alienMinTimeBetweenShots = 3.0f;
    public float alienMaxTimeBetweenShots = 6.0f;

    public bool adult = true;

    Rigidbody rb;
    BoxCollider bc;
    Alien [] aliens;
    HordeManager hordeManager;
    ScoreManager scoreManager;

    public int numberOfAliveAliens;

    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();

        aliens = gameObject.GetComponentsInChildren<Alien>();

        hordeManager = FindObjectOfType<HordeManager>();
        scoreManager = FindObjectOfType<ScoreManager>();

        numberOfAliveAliens = aliens.Length + 1;
        //El +1 se debe a que el metodo calculateBoxCollider es el que reduce el numero de aliens vivos
        //se llama cada vez que uno muere pero también se llama la primera vez para ajustar el bc 

        initialiceAlienProperties();

        calculateBoxCollider();
    }

    void initialiceAlienProperties()
    {
        foreach(Alien alien in aliens)
        {
            alien.setHp(alienHP);
            alien.setShotSpeed(alienShotSpeed);
            alien.setScoreValue(alienScoreValue);
            alien.setAdult(adult);
            alien.setMinTimeBetweenShots(alienMinTimeBetweenShots);
            alien.setMaxTimeBetweenShots(alienMaxTimeBetweenShots);
        }
    }

    void calculateBoxCollider() //El box collider de la horda ajusta su tamaño para el cálculo de colisiones con los limites
    {
        numberOfAliveAliens--;

        //Punto más arriba, abajo, a la izquierda y a la derecha de los aliens de la horda
        float top = 0;
        float bottom = 0;
        float left = -float.MaxValue;
        float right = float.MaxValue;

        foreach (Alien alien in aliens) //Obtenemos el valor de los puntos
        {
            if(alien.isAlive())
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

    void increaseScore(float sv)
    {
        scoreManager.playerScore += sv;
    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.right * speed * Time.deltaTime;
        if (numberOfAliveAliens <= 0) { hordeManager.spawnNewHorde(); Destroy(gameObject); }
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
