using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{

    [SerializeField] float speed = 200;
    Rigidbody rb;
    [SerializeField] ParticleSystem gun;
    [SerializeField] float allowedmovement = 10.0f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        movement();
        fire();
    }

    void movement()
    {
        Vector3 ejeX = Input.GetAxisRaw("Horizontal") * Vector3.right * Time.deltaTime * speed;
        rb.velocity = ejeX;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -allowedmovement, allowedmovement), transform.position.y, transform.position.z);
    }

    void fire()
    {
        if (Input.GetButton("Fire1"))
        {
            gun.Play();
        }
    }
}