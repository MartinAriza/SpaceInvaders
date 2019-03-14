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
    int layerMask;
    private float actualMaxSpeed;
    private float actualAceleration;
    bool falling = false;
    bool wasLevitating = false;
    [HideInInspector]public bool usingPower = false;

    //Bool names
    private static string Anim_idle = "idle";
    private static string Anim_levitate = "levitate";
    private static string Anim_talk = "talk";
    private static string Anim_walk = "walk";
    private static string Anim_sneakyIdle = "sneakyIdle";
    private static string Anim_sneakyWalk = "sneakyWalk";
    private static string Anim_run = "run";
    private static string Anim_power = "power";
    private static string Anim_falling = "falling";
    private static string Anim_usingPower = "usingPower";

    //Trigger names
    private static string Anim_Fall = "fall";
    private static string Anim_Power = "power";

    //Layer names
    private static string solidLayer = "staticSolid";


    void Awake()
    {
        layerMask = LayerMask.GetMask(solidLayer);
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        InputController();
        if (Input.GetKeyDown("f")) freezeInput = !freezeInput;
    }

    void FixedUpdate()
    {
        Move();
    }

    private void InputController()
    {
        //DEFAULT NIMATION SPEED
        anim.speed = 1.0f;
        //Inputs
        //LEVITATE
        Collider[] onFloor = Physics.OverlapSphere(feetPos.position, 0.2f, layerMask);
        RaycastHit hit;
        if (Input.GetKeyDown("space") && !usingPower)
        {
            levitate = !levitate;
            if (levitate)
            {
                if (onFloor.Length == 0) levitate = false;
                else levitateTargetHeight = transform.position.y + levitateHeight;
            }
            else
            {
                if (Physics.Raycast(transform.position, -Vector3.up, out hit, levitateHeight + (feetPos.position - transform.position).magnitude + 0.5f, layerMask))
                {
                    wasLevitating = true;
                }
            }
            rb.useGravity = !levitate;
            anim.SetBool(Anim_levitate, levitate);
        }

        //POWER
        anim.SetBool(Anim_usingPower, usingPower);

        //FALL
        if(onFloor.Length == 0 && !falling && !levitate && !wasLevitating)
        {
            falling = true;
            anim.SetTrigger(Anim_Fall);
        } else if(onFloor.Length > 0 && falling)
        {
            falling = false;
        }
        anim.SetBool(Anim_falling, falling);

        if (onFloor.Length > 0) wasLevitating = false;

        //WALK
        if (!freezeInput) input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (usingPower) input = new Vector2(0f, 0f);
        bool walk = input.magnitude > 0f;
        if (walk)
            anim.speed = Mathf.Clamp(direction.magnitude / maxSpeed, 0.35f, 1);
        anim.SetBool(Anim_walk, walk);

        //SNEAKY IDLE
        bool sneaky = Input.GetKey(KeyCode.LeftControl);
        bool run = Input.GetKey(KeyCode.LeftShift) && !sneaky;
        bool sneakyIdle = (run || sneaky) && !walk && !levitate;

        //MIENTRAS ESTE ANDANDO DEBERÍA PODER EMPEZAR A CORRER Y ANDAR CON SIGILO, NO DEBERÍAS TENER QUE PARARTE

        anim.SetBool(Anim_sneakyIdle, sneakyIdle);

        //SNEAKY WALK
        anim.SetBool(Anim_sneakyWalk, sneaky && walk);

        //RUN
        anim.SetBool(Anim_run, run && walk);

        //IDLE
        bool notInput = input.magnitude == 0 && !levitate && !sneakyIdle && !falling;
        if (notInput)
            anim.SetBool(Anim_idle, true);
        else
            anim.SetBool(Anim_idle, false);

        actualMaxSpeed = maxSpeed;
        actualAceleration = aceleration;
        if (sneaky && walk)
        {
            actualMaxSpeed *= sneakyMultiplier;
            actualAceleration *= sneakyMultiplier;
        }
        else if (run && walk)
        {
            actualMaxSpeed *= runMultiplier;
            actualAceleration *= runMultiplier;
        }
    }

    private void Move()
    {
        //Operations
        if (actualMaxSpeed > 0.0f)
        {
            direction += (input.normalized - direction / actualMaxSpeed) * actualAceleration;
            float Yvel = 0f;
            if (levitate)
                Yvel = (levitateTargetHeight - transform.position.y) * actualAceleration / 5f;
            else
                Yvel = rb.velocity.y;
            rb.velocity = new Vector3(direction.x * Time.deltaTime, Yvel, direction.y * Time.deltaTime);
            Vector3 look = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
            if (look.magnitude > 0.01)
                transform.LookAt(transform.position + look);
        }
    }
    public float getMaxPossibleSpeed()
    {
        return maxSpeed * runMultiplier;
    }
    public void usePower()
    {
        anim.SetTrigger(Anim_Power);

        levitate = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, levitateHeight + (feetPos.position - transform.position).magnitude + 0.5f, layerMask))
        {
            wasLevitating = true;
        }
        rb.useGravity = true;
        anim.SetBool(Anim_levitate, false);
    }
}
