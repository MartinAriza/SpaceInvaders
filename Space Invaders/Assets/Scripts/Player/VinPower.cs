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
    [SerializeField]SphereCollider shieldCollider;
    VinCamera1 mainCam;

    [SerializeField] float maxSpeed = 100.0f;
    [SerializeField] float aceleration = 20.0f;
    Vector2 direction = new Vector2(0f, 0f);
    Vector2 input = new Vector2(0f, 0f);
    bool shoot;

    void Start()
    {
        layerMask = LayerMask.GetMask("controllable");
        mov = GetComponent<VinMov>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VinCamera1>();
    }

    void Update()
    {
        shield = Input.GetKey("e") && !move && !control;
        cast = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown("e");
        move = Input.GetMouseButton(0) && !shield && !control;
        control = Input.GetMouseButton(1) && !move && !shield;
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        shoot = Input.GetButtonDown("Fire1");


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
                mainCam.setDefaultTarget();
            }
            else
            {
                mov.usingPower = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
            shieldCollider.enabled = false;
            if (shield)
            {
                shieldCollider.enabled = true;
            } else if (control && shoot)
            {
                print("shoot");
            }
        }
    }

    void castObject()
    {

        if (!shield)
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMask))
            {
                print(hitInfo.transform.name);
                target = hitInfo.transform.gameObject;
                rb = target.GetComponent<Rigidbody>();
                if ((target.transform.position - transform.position).magnitude < powerRange)
                    mov.usePower();
                if(control && target.tag == "Alien")
                    mainCam.changeTarget(target.transform);
            }

        }
        else if (shield)
        {
            target = gameObject;
            mov.usePower();
        }
        
    }

    private void FixedUpdate()
    {
        if (target)
        {
            if (move)
            {
                moveObject();
            }
            else if (control && target.tag == "Alien")
            {
                controlObject();
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
    private void controlObject()
    {
        //Operations
        if (maxSpeed > 0.0f)
        {
            direction += (input.normalized - direction / maxSpeed) * aceleration;
            rb.velocity = new Vector3(direction.x * Time.deltaTime, rb.velocity.y, direction.y * Time.deltaTime);
            Vector3 look = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
            if (look.magnitude > 0.01)
                target.transform.LookAt(target.transform.position + look);
        }
    }
}
