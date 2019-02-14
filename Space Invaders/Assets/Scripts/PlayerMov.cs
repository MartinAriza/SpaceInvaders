using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{

    public float speed = 7.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ejeX = Input.GetAxis("Horizontal") * transform.right * Time.deltaTime * speed;

        transform.Translate(ejeX);

    }
}
