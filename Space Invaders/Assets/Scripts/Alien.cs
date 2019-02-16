using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] float minTimeBetweenShots = 1.0f;
    [SerializeField] float maxTimeBetweenShots = 3.0f;
    [SerializeField] float shotSpeed = 15.0f;
    [SerializeField][Tooltip("Numero de disparos necesarios para destruir el alien")] int HP = 1;
    [SerializeField] float scoreValue = 10.0f;

    bool alive = true;
    bool adult = true;

    Transform parent;
    [SerializeField] ParticleSystem deathFX;
    [SerializeField] ParticleSystem gun;

    bool stopFiring = false; //Permite que la subrutina de disparo se ejecute periodicamente hasta que se ponga a true
    bool firstTime = true; //Evita que el juego comience con una oleada de disparos al ejecutarse al subrutina

    public bool isAlive()
    { return alive; }

    public void setHp(int h)
    { HP = h; }

    public void setShotSpeed(float s)
    { shotSpeed = s; }

    public void setScoreValue(float sv)
    { scoreValue = sv; }

    public void setAdult(bool a)
    { adult = a; }

    void Start()
    {
        parent = FindObjectOfType<ExplosionDestroyer>().transform;

        gameObject.GetComponent<CubeEditor>().enabled = false; //Se desactiva el snap para que se puedan mover en el play
        gun = gameObject.GetComponentInChildren<ParticleSystem>();
        gun.startSpeed = shotSpeed; //Es deprecate pero la otra opcion es solo de lectura

        StartCoroutine(fire());
    }

    IEnumerator fire()
    {
        //No es necesario comprobar si el alien tiene visión del jugador, si tiene otro delante su disparo colisionará con este y se destruirá
        while(adult && !stopFiring)
        {
            if (!firstTime) gun.Play();

            firstTime = false;
            yield return new WaitForSeconds( Random.Range(minTimeBetweenShots, maxTimeBetweenShots) );
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "PlayerLaser")
        {
            HP--;
            if (HP <= 0)
            {
                Instantiate(deathFX, transform.position, Quaternion.identity).gameObject.transform.parent = parent;

                alive = false;
                stopFiring = true;

                gameObject.SendMessageUpwards("increaseScore", scoreValue);
                gameObject.SendMessageUpwards("calculateBoxCollider");

                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}