using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinCamera : MonoBehaviour
{
    [SerializeField] Vector3 rotation = new Vector3(45f, 0f, 0f);
    [SerializeField] Vector3 postDisplacement = new Vector3(0f, 0f, -1f);
    [SerializeField] [Range(0, 10)] float maxOffset = 2f;
    [SerializeField] float distance = 3.0f;
    Transform target;
    Rigidbody rb;
    //Vector3 lastTargetPos = new Vector3(0f, 0f, 0f);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Vin").GetComponent<Transform>();
        toVarsPosition(target.position);
        //lastTargetPos = target.position;
    }

    void LateUpdate()
    {
        Move();
        toVarsPosition(getLastTargetPosition());
        //lastTargetPos = target.position;
    }

    void toVarsPosition(Vector3 targetPos)
    {
        transform.position = targetPos;
        transform.localRotation = Quaternion.Euler(rotation);
        transform.position = transform.position - transform.forward * distance;
        transform.position += postDisplacement;
    }

    private Vector3 getLastTargetPosition()
    {
        return transform.position - postDisplacement + transform.forward * distance;
    }

    private void Move()
    {
        Vector3 offset = target.position - getLastTargetPosition();
        Vector3 spd = new Vector3(0f, 0f, 0f);
        if (maxOffset > 0.0f && offset.magnitude < maxOffset) spd = offset * offset.magnitude / maxOffset;
        else if (maxOffset > 0.0f) spd = offset;
        else toVarsPosition(target.position);
        rb.velocity = spd * Time.deltaTime * 150f;
    }
}
