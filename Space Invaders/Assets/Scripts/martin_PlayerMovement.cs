using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class martin_PlayerMovement : MonoBehaviour
{
    Rigidbody rigidBody;
    public Camera camera;

    public float movementSpeed = 10.0f;
    float xThrow, yThrow;

    float mouseX, mouseY;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        
        //Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        movePlayer();
        rotatePlayer();
    }

    void movePlayer()
    {
        xThrow = Input.GetAxisRaw("Horizontal");
        yThrow = Input.GetAxisRaw("Vertical");

        Vector3 forwardMovement = calculateForwardMovement();

        rigidBody.velocity = ((xThrow * transform.right).normalized + forwardMovement.normalized) * movementSpeed;
    }

    Vector3 calculateForwardMovement()
    {
        RaycastHit hit;
        Vector3 planeNormal = Vector3.zero;

        int mask = LayerMask.GetMask("Default");

        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1, mask))
        {
            planeNormal = hit.normal;
        }

        Vector3 forwardMovement = Vector3.ProjectOnPlane((yThrow * transform.forward), planeNormal);
        return forwardMovement;
    }

    void rotatePlayer()
    {
        mouseX = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
        mouseY = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;

        float angleX = Mathf.Lerp(-360, 360, mouseX);
        float angleY = Mathf.Lerp(-75, 75, mouseY);

        rigidBody.rotation = Quaternion.Euler(0, angleX, 0);

        camera.transform.rotation = Quaternion.Euler(-angleY, angleX, 0);
    }
}