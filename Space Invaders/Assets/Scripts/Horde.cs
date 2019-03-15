using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Conjunto de los alienigenas
public class Horde : MonoBehaviour
{
    [Header("Propiedades de la horda")]
    [Tooltip("Velocidad horizontal de los aliens")]public float speed = 1.0f;
    [Tooltip("Cuantas unidades se desplaza la horda hacia abajo al tocar una pared")]public float downSpeed = 1.0f;
    [Space]

    [Header("Propiedades de los aliens de la horda")]
    [Tooltip("Vida inicial de los aliens de la horda")]public int alienHP = 1;
    [Tooltip("Velocidad inicial de los disparos de los aliens")]public float alienShotSpeed = 15;
    [Tooltip("Cuantos puntos vale inicialmente cada alien")]public float alienScoreValue = 10;
    [Tooltip("Tiempo minimo que esperará cada alien para disparar")]public float alienMinTimeBetweenShots = 3.0f;
    [Tooltip("Tiempo máximo que esperará cada alien para disparar")]public float alienMaxTimeBetweenShots = 6.0f;

    [Tooltip("Si la horda es para adultos los aliens podrán disparar")]public bool adult = true;

    Rigidbody rb;
    BoxCollider bc;
    Alien [] aliens;
    HordeManager hordeManager;
    ScoreManager scoreManager;
    AudioSource collision;
    PlayerMov player;

    public int numberOfAliveAliens;

    void Start()
    {
        //Se recogen los componentes necesarios del gameObject
        player = FindObjectOfType<PlayerMov>();
        bc = gameObject.GetComponent<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();

        aliens = gameObject.GetComponentsInChildren<Alien>();
        collision = GetComponent<AudioSource>();

        hordeManager = FindObjectOfType<HordeManager>();
        scoreManager = FindObjectOfType<ScoreManager>();

        numberOfAliveAliens = aliens.Length;    //Numero de aliens que están vivos

        initialiceAlienProperties();

        calculateBoxCollider();
    }

    //Se inician las propiedades de los aliens a las variables 
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

    //El box collider de la horda ajusta su tamaño para el cálculo de colisiones con los limites
    public void calculateBoxCollider() 
    {
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

    public void increaseScore(float sv)
    {
        scoreManager.playerScore += sv;
    }

    //todos los aliens de la horda dejan de disparar
    public void stopFiring()
    {
        foreach (Alien alien in aliens)
        {
            alien.setStopFiring(true);
        }
    }

    public void playDeathSound()
    {
        collision.Play();
    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.right * speed * Time.deltaTime; //Se mueve la horda en el eje horizontal
        if (numberOfAliveAliens <= 0) { hordeManager.spawnNewHorde(); Destroy(gameObject); } //Si la horda se queda sin aliens se crea otra más fuerte y se destruye esta
    }

    private void OnTriggerEnter(Collider other)
    {
        //Si choca con los limites de la escena
        if(other.tag == "Limit")
        {
            speed = -speed; //Su dirección cambia
            rb.MovePosition(new Vector3(transform.position.x, transform.position.y, transform.position.z - downSpeed)); //Se mueve hacia abajo
        }
        else if (other.tag == "KillBarrier")
        {
            player.destroyPlayer();
        }
    }

    public void changeAlienColor(Color[] colors, int index)
    {
        foreach (Alien alien in aliens)
        {
            alien.changeColor (colors[index]);
        }
    }

    public void changeAlienColor(Color[] colors)
    {
        foreach (Alien alien in aliens)
        {
            alien.changeColor (colors[UnityEngine.Random.Range(0,colors.Length - 1)]);
        }
    }
}
