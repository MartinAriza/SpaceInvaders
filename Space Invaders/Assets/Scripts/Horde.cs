using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider bc;
    public Alien [] aliens;
    [SerializeField] float distance = 10.0f;
    [SerializeField] uint columns = 10;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float downSpeed = 1.0f;
   
    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
        aliens = FindObjectsOfType<Alien>();
        
        arrangeAliens();
    }

    void arrangeAliens()
    {
        int i = 0;
        int j = 0;

        foreach (Alien alien in aliens)
        {
            if(alien.isActiveAndEnabled)
            {
                alien.setSpeed(speed);
                alien.setDownSpeed(downSpeed);
                alien.transform.position = new Vector3(i * distance, 0, j * distance);

                i++;
                if (i >= columns) { i = 0; j++; }
            }
        }
    }
    
    void Update()
    {
    }

    public void goDownAndChangeDirection()
    {
        foreach (Alien alien in aliens)
        {
            if (alien.isActiveAndEnabled)
            {
                alien.changeDirection();
                alien.moveDown();
            }
        }
    }
}
