using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    
    public int startingHp = 5; //La vida inicial de la barrera
    [HideInInspector] public int HP; //Vida actual de la barrera

    Color color;    //Color de la barrera (cambia según la vida)

    [SerializeField] [Range(0, 100)]float colorChangeRate = 25.0f; //Cuanto cambia el color según la vida

    private void Start()
    {
        HP = startingHp;
        color = gameObject.GetComponent<MeshRenderer>().material.color;
    }

    //Vuelve a crear la barrera (al completar una horda lo llama el game manager)
    public void restore()
    {
        HP = startingHp; //Se resetea la vida de la barrera

        //Se reactivan colisiones y el renderizado
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Si choca con un Alien la barrera se destruye automáticamente
        if(collision.gameObject.tag == "Alien")
        {
            HP = 0;
            destroyBarrier();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        //Si un laser le da a una barrera se reduce en 1 su vida y se cambia su color
        if(other.tag == "AlienLaser" || other.tag == "PlayerLaser")
        {
            HP--;

            color.r += colorChangeRate;
            color.b -= colorChangeRate;

            //color.x = Mathf.Clamp(0, 1, color.x);
            color.b = Mathf.Clamp(0, 1, color.b);

            gameObject.GetComponent<MeshRenderer>().material.color = color;
            //gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color/2);

            if (HP <= 0) destroyBarrier();
        }
    }

    void destroyBarrier()
    {
        //Se desactivan las colisiones y el renderizado
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
