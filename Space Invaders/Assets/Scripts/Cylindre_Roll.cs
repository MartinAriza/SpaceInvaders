using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylindre_Roll : MonoBehaviour
{

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, Time.deltaTime * speed);
    }
}
