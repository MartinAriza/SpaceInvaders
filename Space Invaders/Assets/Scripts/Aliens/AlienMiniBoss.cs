using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMiniBoss : MonoBehaviour
{
    //Segundos que el alien tendrá que esperar para volver a disparar (numero aleatorio entre este intervalo)
    [SerializeField] float timeBetweenShots = 1.0f;

    [SerializeField] [Tooltip("Como de rápido se desplazan los láseres de los aliens")] float shotSpeed = 15.0f;
    [SerializeField] [Tooltip("Numero de disparos necesarios para destruir el alien")] int HP = 1;
    [SerializeField] [Tooltip("Cuantos puntos vale el alien")] float scoreValue = 10.0f;

    [SerializeField] [Tooltip("Variable velocidad del alien")] Vector3 speed = new Vector3(0f,0f,0f);
    [SerializeField] [Tooltip("Tiempo entre cada alien miniboss")] int alienMiniBossSpawnRate = 10;
    [SerializeField] [Tooltip("Posición de spawn del alien miniboss")] Vector3 alienMiniBossSpawnPosition;

    static Animator anim;                          //Objeto animator para poder hacer transiciones entre animaciones 
    bool adult = true;                      //Si eres mayor de 13 vale true y el alien puede disparar

    [SerializeField] ParticleSystem deathFX;//Efecto de explosion que los aliens harán al morir
    [SerializeField] ParticleSystem gun;    //Láser que dispara el alien

    bool firstTime = true;                  //Evita que el juego comience con una oleada de disparos al ejecutarse al subrutina

    //Cuando muere se desactiva el renderizado de estos dos elementos
    [SerializeField] GameObject alienBody;  //Enlace a los ojos del alien
    [SerializeField] GameObject alienEyes;  //Enlace al cuerpo del alien

    static ScoreManager scoreManager;

    Rigidbody rb;

    void Start()
    {
        adult = FindObjectOfType<AdultManager>().adultReference;
        scoreManager = FindObjectOfType<ScoreManager>();

        rb = gameObject.GetComponent<Rigidbody>();
        //Se buscan los objetos y componentes necesarios

        anim = GetComponentsInChildren<Animator>()[0];

        gun = gameObject.GetComponentInChildren<ParticleSystem>();
        gun.startSpeed = shotSpeed;
        if(adult)
        {
            gun.Play();
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        rb.velocity = speed*Time.deltaTime;
        transform.position = new Vector3(transform.position.x, speed.y*Mathf.Sin(transform.position.x/2), transform.position.z);
    }

    //Manejo de colisiones con partículas (láseres)
    private void OnParticleCollision(GameObject other)
    {
        //Colision con el láser del jugador
        if (other.tag == "PlayerLaser")
        {
            HP--;   //Se reduce la vida del alien una unidad
            if (HP <= 0)
            {
                Death();
            }
        }
    }

    private void Death()
    {
        //Se crea al efecto de explosion en la posición del alien y se asigna su padre
        deathFX.Play();
        gun.Stop();

            //Se desactiva la colisión y el render de la mesh del alien
        gameObject.GetComponent<BoxCollider>().enabled = false;

        alienBody.SetActive(false);
        alienEyes.SetActive(false);

        scoreManager.playerScore += scoreValue;
        StartCoroutine(reset());
    }

    //Manejo de colisiones con la horda, los límites de la horda y el límite del minijefe
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Limit" || collision.gameObject.tag == "Alien")
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<BoxCollider>());
        }
        else if (collision.gameObject.tag == "AlienMiniBossLimit")
        {
            scoreManager.playerScore -= scoreValue;
            Death();
        }
    }

    IEnumerator reset()
    {
        yield return new WaitForSecondsRealtime(alienMiniBossSpawnRate);
        transform.position = alienMiniBossSpawnPosition;

        gun.Play();

        //Se reactiva la colisión y el render de la mesh del alien
        gameObject.GetComponent<BoxCollider>().enabled = true;

        alienBody.SetActive(true);
        alienEyes.SetActive(true);
    }
}
