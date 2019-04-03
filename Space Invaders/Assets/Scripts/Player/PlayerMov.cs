using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{

    [SerializeField] [Tooltip("Velocidad máxima de la nave")] Vector3 speed = new Vector3(0f,0f,0f);
    [SerializeField] [Tooltip("Aceleración de la nave")] Vector3 aceleration = new Vector3(0f,0f,0f);
    [SerializeField] [Tooltip("multiplicador de la aceleración para frenar al no pulsar nada")] [Range(0,3)]float brakeMultiplier = 2.0f;

    [Tooltip("Numero de golpes que aguanta la nave")] public int HP = 3;
    [SerializeField] [Tooltip("Numero maximo de disparos del jugador que puede haber a la vez en pantalla")] int maxShots = 2;
    [SerializeField] float shotSpeed = 15;

    [SerializeField][Tooltip("Cuanto rota la nave al moverse")] float rotationAmount = 4.0f;
    [SerializeField][Tooltip("Cuanta distancia puede moverse la nave desde el centro de la pantalla en el eje X")] float allowedMovementX = 10.0f;
    [SerializeField][Tooltip("Cuanta distancia puede moverse la nave desde el centro de la pantalla en el eje Y")] float allowedMovementY = 5.0f;

    ParticleSystem gun = null;
    [SerializeField] ParticleSystem gunWithBounce;
    [SerializeField] ParticleSystem gunWithoutBounce;
    [SerializeField] ParticleSystem deathFX;
    [SerializeField] ParticleSystem [] engineParticles;
    [SerializeField] GameObject aimCylinder;

    ScoreManager scoreManager;

    [HideInInspector] public bool adult;
    public bool bounce;
    AudioSource laserSound;
    Rigidbody rb;

    Vector3 velocity = new Vector3(0f, 0f, 0f);
    Vector3 locRotation = new Vector3(0f, 0f, 0f);

    void Start()
    {
        if (!bounce)
        {
            gun = gunWithoutBounce;
            gunWithBounce.gameObject.SetActive(false);
        }
        else
        {
            gun = gunWithBounce;
            gunWithoutBounce.gameObject.SetActive(false);
        }

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
        //X Movement
        float inputX = Input.GetAxisRaw("Horizontal");
        float absX = Mathf.Abs(inputX);
        velocity.x += (inputX - velocity.x / speed.x) * aceleration.x * ((1-absX) * brakeMultiplier + 1 * absX); //si no pulso nada me freno más lento de lo que acelero

        //Y Movement
        float inputY = Input.GetAxisRaw("Vertical");
        float absY = Mathf.Abs(inputY);
        velocity.y += (inputY - velocity.y / speed.y) * aceleration.y * ((1-absY) * brakeMultiplier + 1 * absY);

        //Z Movement

        //Clamping
        velocity = new Vector3(
            Mathf.Clamp(velocity.x, -speed.x, speed.x),
            Mathf.Clamp(velocity.y, -speed.y, speed.y),
            Mathf.Clamp(velocity.z, -speed.z, speed.z)
            );
        rb.velocity = velocity * Time.deltaTime;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -allowedMovementX, allowedMovementX),
            Mathf.Clamp(transform.position.y, -allowedMovementY, allowedMovementY),
            transform.position.z
            );
    }

    //La nave rota un poco hacia el lado que el jugador se está moviendo
    void rotation()
    {
        //float roll = Input.GetAxis("Horizontal") * rotationAmount;
        //transform.localRotation = Quaternion.Euler(0, 0, -roll);
        locRotation.z = (velocity.x / speed.x) * -rotationAmount;
        locRotation.x = (velocity.y / speed.y) * -rotationAmount;
        transform.localRotation = Quaternion.Euler(locRotation.x, 0, locRotation.z);
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
            deathFX.Play();
            if (HP <= 0) destroyPlayer();
        }
    }

    //Si un láser de los aliens toca al jugaodor le quita una vida
    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "AlienLaser")
        {
            HP--;
            deathFX.Play();
            if (HP <= 0) destroyPlayer(); 
        }
    }

    public void destroyPlayer()
    {                                                  //Suena el sonido de explosión
        scoreManager.scoreAnimation();                                      //Aparece la puntuación en pantalla
        //Se desactivan colisiones y el renderizado
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        aimCylinder.gameObject.SetActive(false);

        //Se destruyen las partículas de los motores
        foreach(ParticleSystem engine in engineParticles) engine.gameObject.SetActive(false);
    }
}