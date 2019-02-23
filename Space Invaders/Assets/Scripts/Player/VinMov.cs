using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinMov : MonoBehaviour
{
    [SerializeField] [Range(0, 500)] float maxSpeed = 100;
    [SerializeField] [Range(0, 100)] float aceleration = 20;
    Vector2 direction = new Vector3(0, 0);
    Rigidbody rb;
    [SerializeField] [Tooltip("Activa para mantener el input actual, pulsa f para activarlo")] bool freezeInput = false;
    Vector2 input = new Vector2(0f, 0f);
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Move();
        if (Input.GetKeyDown("f")) freezeInput = !freezeInput;
    }

    private void Move()
    {
        if(!freezeInput) input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction += (input.normalized-direction/maxSpeed) * aceleration;
        if (direction.magnitude > maxSpeed) direction = direction.normalized * maxSpeed;
        rb.velocity = new Vector3(direction.x * Time.deltaTime,rb.velocity.y, direction.y * Time.deltaTime);
        transform.LookAt(transform.position + Vector3.ProjectOnPlane(rb.velocity, Vector3.up));
    }
}
