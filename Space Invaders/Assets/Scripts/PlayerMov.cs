using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{

    [SerializeField] float speed = 200;
    [Tooltip("Numero de golpes que aguanta la nave")]public int HP = 3;
    [SerializeField][Tooltip("Numero maximo de disparos del jugador que puede haber a la vez en pantalla")] int maxShots = 2;
    [SerializeField] float shotSpeed = 15;

    [SerializeField][Tooltip("Cuanto rota la nave al moverse")] float rotationAmount = 4.0f;
    [SerializeField][Tooltip("Cuanta distancia puede moverse la nave desde el centro de la pantalla")] float allowedMovement = 10.0f;

    [SerializeField] ParticleSystem gun;
    [SerializeField] ParticleSystem deathFX;
    [SerializeField] ParticleSystem [] engineParticles;

    ScoreManager scoreManager;

    public bool adult = true;

    void Start()
    {
        //Son deprecate pero la otra opcion es solo de lectura
        gun.maxParticles = maxShots;
        gun.startSpeed = shotSpeed;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void FixedUpdate()
    {
        if (HP > 0)
        {
            movement();
            rotation();
        }
    }

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
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        Vector3 ejeX = Input.GetAxisRaw("Horizontal") * Vector3.right * Time.deltaTime * speed;
        rb.velocity = ejeX;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -allowedMovement, allowedMovement), transform.position.y, transform.position.z);
    }

    void rotation()
    {
        float xThrow = Input.GetAxisRaw("Horizontal");
        float roll = xThrow * rotationAmount;

        transform.localRotation = Quaternion.Euler(0, 0, -roll);
    }

    void fire()
    {
        if (Input.GetButton("Fire1")) gun.Play();
    }

    void checkExit()
    {
        if( Input.GetKey("escape") ) { Application.Quit(); }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Alien")
        {
            HP--;
            if (HP <= 0) destroyPlayer();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "AlienLaser")
        {
            HP--;
            if(HP <= 0)
            {
                deathFX.Play();
                destroyPlayer();
            }
            
        }
    }

    void destroyPlayer()
    {
        scoreManager.scoreAnimation();
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        foreach(ParticleSystem engine in engineParticles)
        {
            engine.gameObject.SetActive(false);
        }
    }
}