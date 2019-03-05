using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinMov : MonoBehaviour
{
    [SerializeField] [Range(1, 500)] float maxSpeed = 100;
    [SerializeField] [Range(1, 100)] float aceleration = 20;
    [SerializeField] float runMultiplier = 1.5f;
    [SerializeField] float sneakyMultiplier = 0.5f;
    [SerializeField] [Tooltip("Activa para levitar pulsando e")]bool levitate = false;
    [SerializeField] float levitateHeight = 0.5f;
    [SerializeField] Transform feetPos;
    Vector2 direction = new Vector3(0, 0);
    Rigidbody rb;
    [SerializeField] [Tooltip("Activa para mantener el input actual, pulsa f para activarlo")] bool freezeInput = false;
    Vector2 input = new Vector2(0f, 0f);
    float levitateTargetHeight;
    Animator anim;

    //Bool names
    private static string Anim_idle = "idle";
    private static string Anim_levitate = "levitate";
    private static string Anim_talk = "talk";
    private static string Anim_walk = "walk";
    private static string Anim_sneakyIdle = "sneakyIdle";
    private static string Anim_sneakyWalk = "sneakyWalk";
    private static string Anim_run = "run";
    private static string Anim_power = "power";

    //Layer names
    private static string solidLayer = "staticSolid";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        Move();
        if (Input.GetKeyDown("f")) freezeInput = !freezeInput;
        print("the speed is " + rb.velocity.magnitude);
    }

    private void Move()
    {
        //DEFAULT NIMATION SPEED
        anim.speed = 1.0f;
        //Inputs
        //LEVITATE
        int layerMask = LayerMask.GetMask(solidLayer);
        Collider[] onFloor = Physics.OverlapSphere(feetPos.position, 0.1f, layerMask);
        if (Input.GetKeyDown("space"))
        {
            levitate = !levitate;
            if (levitate)
            {
                if (onFloor.Length == 0) levitate = false;
                else levitateTargetHeight = transform.position.y + levitateHeight;
            }
            rb.useGravity = !levitate;
            anim.SetBool(Anim_levitate, levitate);
        }
        //WALK
        if (!freezeInput) input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool walk = input.magnitude > 0f;
        if (walk)
            anim.speed = Mathf.Clamp(direction.magnitude / maxSpeed, 0.35f, 1);
        anim.SetBool(Anim_walk, walk);

        

        //SNEAKY IDLE
        bool sneaky = Input.GetKey(KeyCode.LeftControl)/* && !levitate*/;
        bool run = Input.GetKey(KeyCode.LeftShift)/* && !levitate*/;
        bool sneakyIdle = (run || sneaky) && !walk && !levitate;

        //MIENTRAS ESTE ANDANDO DEBERÍA PODER EMPEZAR A CORRER Y ANDAR CON SIGILO, NO DEBERÍAS TENER QUE PARARTE

        anim.SetBool(Anim_sneakyIdle, sneakyIdle);

        //SNEAKY WALK
        anim.SetBool(Anim_sneakyWalk, sneaky && walk);

        //RUN
        anim.SetBool(Anim_run, run && walk);

        //IDLE
        bool notInput = input.magnitude == 0 && !levitate && !sneakyIdle/*...*/;
        if (notInput)
            anim.SetBool(Anim_idle, true);
        else
            anim.SetBool(Anim_idle, false);

        //Operations
        float actualMaxSpeed = maxSpeed;
        if (sneaky && walk) actualMaxSpeed *= sneakyMultiplier;
        else if (run && walk) actualMaxSpeed *= runMultiplier;

        if(actualMaxSpeed > 0.0f)
        {
            direction += (input.normalized - direction / actualMaxSpeed) * aceleration;
            float Yvel = 0f;
            if (levitate)
                Yvel = (levitateTargetHeight - transform.position.y) * aceleration / 5f;
            else
                Yvel = rb.velocity.y;
            rb.velocity = new Vector3(direction.x * Time.deltaTime, Yvel, direction.y * Time.deltaTime);
            transform.LookAt(transform.position + Vector3.ProjectOnPlane(rb.velocity, Vector3.up));
        }
    }
}
