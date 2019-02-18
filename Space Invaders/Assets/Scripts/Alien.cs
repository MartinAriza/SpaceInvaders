using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    //Segundos que el alien tendrá que esperar para volver a disparar (numero aleatorio entre este intervalo)
    [SerializeField] float minTimeBetweenShots = 3.0f;
    [SerializeField] float maxTimeBetweenShots = 6.0f;

    [SerializeField] [Tooltip("Como de rápido se desplazan los láseres de los aliens")]float shotSpeed = 15.0f;
    [SerializeField] [Tooltip("Numero de disparos necesarios para destruir el alien")] int HP = 1;
    [SerializeField] [Tooltip("Cuantos puntos vale el alien")]float scoreValue = 10.0f;

    Animator anim;                          //Objeto animator para poder hacer transiciones entre animaciones 

    Horde horde;

    bool stopFiring = false;
    bool alive = true;                      //Si el alien está vivo vale true
    bool adult = true;                      //Si eres mayor de 13 vale true y el alien puede disparar

    Transform parent;                       //La explosion del alien se asigna como hijo a este objeto (será el game manager que las irá destruyendo periodicamente para ahorrar recursos)
    [SerializeField] ParticleSystem deathFX;//Efecto de explosion que los aliens harán al morir
    [SerializeField] ParticleSystem gun;    //Láser que dispara el alien

    bool firstTime = true;                  //Evita que el juego comience con una oleada de disparos al ejecutarse al subrutina

    //Cuando muere se desactiva el renderizado de estos dos elementos
    [SerializeField] GameObject alienBody;  //Enlace a los ojos del alien
    [SerializeField] GameObject alienEyes;  //Enlace al cuerpo del alien

    public void setStopFiring(bool b)
    { stopFiring = b; }

    public bool isAlive()
    { return alive; }

    public void setHp(int h)
    { HP = h; }

    public void setShotSpeed(float s)
    { shotSpeed = s; }

    public void setScoreValue(float sv)
    { scoreValue = sv; }

    public void setAdult(bool a)
    { adult = a; }

    public void setMinTimeBetweenShots(float t)
    { minTimeBetweenShots = t; }

    public void setMaxTimeBetweenShots(float t)
    { maxTimeBetweenShots = t; }

    void Start()
    {
        //Se buscan los objetos y componentes necesarios
        parent = FindObjectOfType<ExplosionDestroyer>().transform;

        anim = GetComponentsInChildren<Animator>()[0];

        horde = FindObjectOfType<Horde>();

        gameObject.GetComponent<CubeEditor>().enabled = false; //Se desactiva el snap para que se puedan mover bien al darle a play

        gun = gameObject.GetComponentInChildren<ParticleSystem>();
        gun.startSpeed = shotSpeed;

        StartCoroutine(fire());
    }

    IEnumerator fire()
    {
        while (!stopFiring)
        {
            //Solo puedes disparar si eres mayor de 13
            if (adult && !firstTime)
            {
                gun.Play();               //El láser se dispara
                anim.SetBool("atk", true); //Se reproduce la animación de ataque
            }

            firstTime = false;
            yield return new WaitForSeconds(Random.Range(minTimeBetweenShots, maxTimeBetweenShots)); //La subrutina espera un tiempo para seguir ejecutándose
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        //Colision con el láser del jugador
        if(other.tag == "PlayerLaser")
        {
            HP--;
            if (HP <= 0)
            {
                //Se crea al efecto de explosion en la posición del alien y se asigna su padre
                Instantiate(deathFX, transform.position, Quaternion.identity).gameObject.transform.parent = parent;
                horde.playDeathSound();

                stopFiring = true;
                alive = false;

                gameObject.SendMessageUpwards("increaseScore", scoreValue); //El game manager aumenta el score del jugador
                horde.calculateBoxCollider();      //Se recalculan las colisiones de la horda para que los rebotes con las paredes funcionen correctamente

                //Se desactiva la colisión y el render de la mesh del alien
                gameObject.GetComponent<BoxCollider>().enabled = false;

                alienBody.SetActive(false);
                alienEyes.SetActive(false);
            }
        }
    }
}