using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinCamera1 : MonoBehaviour
{
    [SerializeField] Vector3 rotation = new Vector3(45f, 0f, 0f);
    [SerializeField] Vector3 postDisplacement = new Vector3(0f, 0f, -1f);
    [SerializeField] [Range(0, 4)] float maxOffset = 2f;
    [SerializeField] [Range(3, 6)] float minDistance = 1.0f;
    [SerializeField] [Range(6, 20)] float maxDistance = 15.0f;
    [SerializeField] float distance = 5.0f;
    [SerializeField] float offsetAceleration = 1.0f;
    Transform target;
    Transform defaultTarget;
    VinMov targetMoveScript;
    Rigidbody targetRB;
    float targetMaxPossibleSpeed;
    Vector3 currentOffset = new Vector3(0f, 0f, 0f);

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Vin").GetComponent<Transform>();
        defaultTarget = target;
        targetMoveScript = target.GetComponent<VinMov>();
        targetRB = target.GetComponent<Rigidbody>();
        toVarsPosition(target.position);
        targetMaxPossibleSpeed = targetMoveScript.getMaxPossibleSpeed();
    }

    private void Update()
    {
        distance -= Input.GetAxis("Mouse ScrollWheel") * (distance);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void FixedUpdate()
    {
        Move();
    }

    void toVarsPosition(Vector3 targetPos)
    {
        transform.position = targetPos;
        transform.localRotation = Quaternion.Euler(rotation);
        transform.position = transform.position - transform.forward * distance;
        transform.position += postDisplacement;
    }

    private void Move()
    {
        float currentMaxOffset = maxOffset + (distance - minDistance)/2.0f + 0.5f;
        float currentOffsetAceleration = offsetAceleration * (distance - minDistance) + offsetAceleration;
        Vector3 targetedPosition = targetRB.velocity / (targetMaxPossibleSpeed * Time.deltaTime);
        currentOffset += (targetedPosition - currentOffset/ currentMaxOffset) * offsetAceleration;
        currentOffset.y = 0;
        if (currentOffset.magnitude > currentMaxOffset) currentOffset = currentOffset.normalized * currentMaxOffset;
        toVarsPosition(target.position + currentOffset);
    }
    public void changeTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void setDefaultTarget()
    {
        target = defaultTarget;
    }
}
