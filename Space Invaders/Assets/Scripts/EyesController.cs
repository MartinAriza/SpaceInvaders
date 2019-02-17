using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesController : MonoBehaviour
{

    private Vector3 originalLook;
    [SerializeField] string targetName;
    [SerializeField] PlayerMov target;
    float maxAngle = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        originalLook = transform.forward;

        target = FindObjectOfType<PlayerMov>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 actualLook = transform.forward;
        transform.LookAt(target.transform);
        float actualAngle = Vector3.Angle(originalLook, transform.forward);
        //print(actualAngle);
        if (actualAngle > maxAngle)
        {
            transform.LookAt(transform.position + actualLook);
            //print("blocking");
        }
    }
}
