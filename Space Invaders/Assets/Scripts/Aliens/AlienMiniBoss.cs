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

    Animator anim;                          //Objeto animator para poder hacer transiciones entre animaciones 

    bool stopFiring = false;                //Si el alien debería dejar de disparar
    bool alive = true;                      //Si el alien está vivo vale true
    bool adult = true;                      //Si eres mayor de 13 vale true y el alien puede disparar

    Transform parent;                       //La explosion del alien se asigna como hijo a este objeto (será el game manager que las irá destruyendo periodicamente para ahorrar recursos)
    [SerializeField] ParticleSystem deathFX;//Efecto de explosion que los aliens harán al morir
    [SerializeField] ParticleSystem gun;    //Láser que dispara el alien

    bool firstTime = true;                  //Evita que el juego comience con una oleada de disparos al ejecutarse al subrutina

    //Cuando muere se desactiva el renderizado de estos dos elementos
    [SerializeField] GameObject alienBody;  //Enlace a los ojos del alien
    [SerializeField] GameObject alienEyes;  //Enlace al cuerpo del alien

    ScoreManager scoreManager;
    HordeManager hordeManager;

    Rigidbody rb;

    //Setters de las variables del alien para poder modificar su dificultad
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

    public void setTimeBetweenShots(float t)
    { timeBetweenShots = t; }

    void Start()
    {
        adult = FindObjectOfType<AdultManager>().adultReference;
        scoreManager = FindObjectOfType<ScoreManager>();
        hordeManager = FindObjectOfType<HordeManager>();

        rb = gameObject.GetComponent<Rigidbody>();
        //Se buscan los objetos y componentes necesarios
        parent = FindObjectOfType<ExplosionDestroyer>().transform;

        anim = GetComponentsInChildren<Animator>()[0];

        gameObject.GetComponent<CubeEditor>().enabled = false; //Se desactiva el script de snap para que el alien pueda moverse

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
        Instantiate(deathFX, transform.position, Quaternion.identity).gameObject.transform.parent = parent;

        stopFiring = true;  //El alien deja de disparar
        gun.Stop();
        alive = false;      //Indicamos a la horda que no está vivo

            //Se desactiva la colisión y el render de la mesh del alien
        gameObject.GetComponent<BoxCollider>().enabled = false;

        alienBody.SetActive(false);
        alienEyes.SetActive(false);

        scoreManager.playerScore += scoreValue;

        hordeManager.spawnNewAlienMiniBoss();

        Destroy(gameObject);
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
}
