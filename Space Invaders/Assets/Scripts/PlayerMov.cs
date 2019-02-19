using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{

    [SerializeField] [Tooltip("Velocidad horizontal de la nave")]float speed = 200;
    [Tooltip("Numero de golpes que aguanta la nave")] public int HP = 3;
    [SerializeField] [Tooltip("Numero maximo de disparos del jugador que puede haber a la vez en pantalla")] int maxShots = 2;
    [SerializeField] float shotSpeed = 15;

    [SerializeField][Tooltip("Cuanto rota la nave al moverse")] float rotationAmount = 4.0f;
    [SerializeField][Tooltip("Cuanta distancia puede moverse la nave desde el centro de la pantalla")] float allowedMovement = 10.0f;

    [SerializeField] ParticleSystem gun;
    [SerializeField] ParticleSystem deathFX;
    [SerializeField] ParticleSystem [] engineParticles;

    ScoreManager scoreManager;

    public bool adult = true;
    AudioSource laserSound;
    Rigidbody rb;

    void Start()
    {
        //Se modifican las propiedades del láser de la nave
        gun.maxParticles = maxShots;
        gun.startSpeed = shotSpeed;

        //Se recogen componentes necesarios
        scoreManager = FindObjectOfType<ScoreManager>();
        laserSound = GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (HP > 0)
        {
            movement();
            rotation();
        }
    }
   
    //cuando se han procesado todas las físicas se procesa el disparo y si el jugador quiere salir del juego
    private void LateUpdate()
    {
        if (HP > 0)
        {
            if (adult) fire();
        }

        checkExit();
    }

    void movement()
    {
        Vector3 ejeX = Input.GetAxisRaw("Horizontal") * Vector3.right * Time.deltaTime * speed; //Se mueve horizontalmente al jugador
        rb.velocity = ejeX;

        //Se limita la posición del jugador a los límites establecidos
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -allowedMovement, allowedMovement), 
            transform.position.y, 
            transform.position.z
            );
    }

    //La nave rota un poco hacia el lado que el jugador se está moviendo
    void rotation()
    {
        float roll = Input.GetAxisRaw("Horizontal") * rotationAmount;
        transform.localRotation = Quaternion.Euler(0, 0, -roll);
    }

    //Si se pulsa espacio o mouse Izq se dispara un láser y suena su efecto de sonido
    void fire()
    {
        if (Input.GetButton("Fire1"))
        {
            gun.Play();
            laserSound.Play();
        }
    }

    //Si se pulsa escape se cierra el juego
    void checkExit()
    {
        if( Input.GetKey("escape") ) { Application.Quit(); }
    }

    //Si un alien toca al jugador le quita una vida
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Alien")
        {
            HP--;
            if (HP <= 0) destroyPlayer();
        }
    }

    //Si un láser de los aliens toca al jugaodor le quita una vida
    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "AlienLaser")
        {
            HP--;
            if(HP <= 0) destroyPlayer(); 
        }
    }

    void destroyPlayer()
    {
        deathFX.Play();                                                     //Suena el sonido de explosión
        scoreManager.scoreAnimation();                                      //Aparece la puntuación en pantalla
        //Se desactivan colisiones y el renderizado
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;

        //Se destruyen las partículas de los motores
        foreach(ParticleSystem engine in engineParticles) engine.gameObject.SetActive(false);
    }
}