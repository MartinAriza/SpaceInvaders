using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soliderAnims : MonoBehaviour
{

    Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void idle()
    {
        anim.SetTrigger("idle");
    }
    public void raiseHand()
    {
        anim.SetTrigger("raiseHand");
    }
    public void lowerHand()
    {
        anim.SetTrigger("lowerHand");
    }
}
