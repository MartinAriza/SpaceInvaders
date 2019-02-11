using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    float speed;
    float downSpeed;
    Horde horde;
    Rigidbody rb;

    public void setSpeed(float s) { speed = s; }
    public void setDownSpeed(float s) { downSpeed = s; }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        horde = FindObjectOfType<Horde>();
    }

    void Update()
    {
        rb.velocity = Vector3.right * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        horde.goDownAndChangeDirection();
    }

    public void changeDirection() { speed = -speed; }

    public void moveDown()
    {
        rb.MovePosition(new Vector3(transform.position.x, 0, transform.position.z - downSpeed));
    }
}
