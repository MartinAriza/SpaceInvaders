using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    
    public int startingHp = 5;
    /*[HideInInspector]*/ public int HP;

    private void Start()
    {
        HP = startingHp;
    }

    public void restore()
    {
        HP = startingHp;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        HP--;
        if (HP <= 0) destroyBarrier();
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "AlienLaser")
        {
            HP--;
            if(HP <= 0) destroyBarrier();
        }
    }

    void destroyBarrier()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
