using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{

    [SerializeField] float speed = 200;
    [Tooltip("Numero de golpes que aguanta la nave")]public int HP = 3;
    [SerializeField][Tooltip("Numero maximo de disparos del jugador que puede haber a la vez en pantalla")] int maxShots = 2;
    [SerializeField] float shotSpeed = 15;
    
    [SerializeField][Tooltip("Cuanta distancia puede moverse la nave desde el centro de la pantalla")] float allowedMovement = 10.0f;

    [SerializeField] ParticleSystem gun;
    [SerializeField] ParticleSystem deathFX;

    void Start()
    {
        //Son deprecate pero la otra opcion es solo de lectura
        gun.maxParticles = maxShots;
        gun.startSpeed = shotSpeed;
    }

    void FixedUpdate()
    {
        if (HP > 0)
        {
            movement();
            fire();
        }
    }

    void movement()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        Vector3 ejeX = Input.GetAxisRaw("Horizontal") * Vector3.right * Time.deltaTime * speed;
        rb.velocity = ejeX;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -allowedMovement, allowedMovement), transform.position.y, transform.position.z);
    }

    void fire()
    {
        if (Input.GetButton("Fire1")) gun.Play();
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "AlienLaser")
        {
            HP--;
            if(HP <= 0)
            {
                deathFX.Play();
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            
        }
    }
}