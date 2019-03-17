using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private Horde horda;
    float clearTime = 1.0f;
    int nHits = 0;
    public int startingHp = 5; //La vida inicial de la barrera
    [HideInInspector] public int HP; //Vida actual de la barrera

    Color color;    //Color de la barrera (cambia según la vida)

    [SerializeField]
    [Tooltip("Colores posibles con los que se tintan los aliens al disparar a las barreras")]
    [ColorUsageAttribute(true, true)]
    Color[] randomColor = { new Color(1, 0, 0, 1), new Color(0, 1, 0, 1), new Color(0, 0, 1, 1), new Color(1, 1, 0, 1), new Color(0, 0, 0, 1), new Color(1, 0, 1, 1) };

    [SerializeField] [Range(0, 100)]float colorChangeRate = 25.0f; //Cuanto cambia el color según la vida

    private void Start()
    {
        StartCoroutine(clearTimeBetweenShots());
        HP = startingHp;
        color = gameObject.GetComponent<MeshRenderer>().material.color;
        this.findHorde();
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
    private void AlienRainbow()
    {
        if (nHits == 1)
        {
            horda.changeAlienColor(randomColor, Random.Range(0, randomColor.Length-1));
        }else if (nHits > 1)
        {
            horda.changeAlienColor(randomColor);
        }
    }

    public void findHorde()
    {
        horda = FindObjectOfType<Horde>();
    }

    private void OnParticleCollision(GameObject other)
    {
        //Si un laser le da a una barrera se reduce en 1 su vida y se cambia su color
        if(other.tag == "AlienLaser" || other.tag == "PlayerLaser")
        {
            HP--;
            nHits++;
            color.r += colorChangeRate;
            color.b -= colorChangeRate;
            
            //color.x = Mathf.Clamp(0, 1, color.x);
            color.b = Mathf.Clamp(0, 1, color.b);

            gameObject.GetComponent<MeshRenderer>().material.color = color;
            //gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color/2);

            this.AlienRainbow();

            if (HP <= 0) destroyBarrier();
        }
    }

    void destroyBarrier()
    {
        //Se desactivan las colisiones y el renderizado
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    IEnumerator clearTimeBetweenShots()
    {
        nHits = 0;
        yield return new WaitForSecondsRealtime(clearTime);
        StartCoroutine(clearTimeBetweenShots());
    }
}
