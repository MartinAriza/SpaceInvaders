using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    
    public int startingHp = 5;
    [HideInInspector] public int HP;

    float alpha;
    Vector4 color;
    Vector4 aux;
    [SerializeField] [Range(0, 100)]float colorChangeRate = 25.0f;

    private void Start()
    {
        HP = startingHp;
        alpha = gameObject.GetComponent<MeshRenderer>().material.color.a;
        color = gameObject.GetComponent<MeshRenderer>().material.color;
        aux = color;
    }

    public void restore()
    {
        color = aux;
        HP = startingHp;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HP--;
        if (HP <= 0) destroyBarrier();
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "AlienLaser" || other.tag == "PlayerLaser")
        {
            HP--;

            color.x += colorChangeRate;
            color.z -= colorChangeRate;

            //color.x = Mathf.Clamp(0, 1, color.x);
            color.z = Mathf.Clamp(0, 1, color.z);

            gameObject.GetComponent<MeshRenderer>().material.color = color;
            //gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color/2);

            if (HP <= 0) destroyBarrier();
        }
    }

    void destroyBarrier()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
