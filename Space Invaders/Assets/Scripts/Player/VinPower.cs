using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinPower : MonoBehaviour
{

    [SerializeField]GameObject target;
    [SerializeField] float forceImp = 2.0f;
    [SerializeField] float powerRange = 3.0f;
    bool move = false;
    bool control = false;
    bool shield = false;
    bool cast = false;
    int layerMask;
    Rigidbody rb;
    VinMov mov;

    void Start()
    {
        layerMask = LayerMask.GetMask("controllable");
        mov = GetComponent<VinMov>();
        //Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        cast = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
        shield = Input.GetKey("e");
        move = Input.GetMouseButton(0) && !shield;
        control = Input.GetMouseButton(1) && !move;
        if (cast) castObject();
        if (target)
        {
            float distanceTarget = (target.transform.position - transform.position).magnitude;
            if ((!shield && !move && !control) ||  distanceTarget > powerRange)
            {
                mov.usingPower = false;
                target = null;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                mov.usingPower = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }

    void castObject()
    {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMask))
        {
            print(hitInfo.transform.name);
            target = hitInfo.transform.gameObject;
            rb = target.GetComponent<Rigidbody>();
            if((target.transform.position - transform.position).magnitude < powerRange)
                mov.usePower();
        }
    }

    private void FixedUpdate()
    {
        if (target)
        {
            if (shield)
            {

            }
            else if (move)
            {
                moveObject();
            }
            else if (control && target.tag == "alien")
            {

            }
        }
    }

    private void moveObject()
    {
        float multiplier = powerRange - Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up).magnitude;
        Vector3 force = new Vector3(Input.GetAxis("Mouse X") * forceImp * multiplier,
            0f,
            Input.GetAxis("Mouse Y")*forceImp * multiplier) * Time.deltaTime;
        rb.velocity = new Vector3(force.x,
            transform.position.y - target.transform.position.y,
            force.z);
    }
}
