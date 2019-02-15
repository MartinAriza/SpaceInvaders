using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float minTimeBetweenShots = 1.0f;
    public float maxTimeBetweenShots = 3.0f;
    [SerializeField] bool checkLineOfSight = false;
    [SerializeField] ParticleSystem deathFX;
    ParticleSystem gun;

    bool stopFiring = false;

    void Start()
    {
        gameObject.GetComponent<CubeEditor>().enabled = false; //Se desactiva el snap para que se puedan mover en el play
        StartCoroutine(fire());
        gun = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    IEnumerator fire()
    {
        while(!stopFiring)
        {
            gun.Play();
            yield return new WaitForSeconds( Random.Range(minTimeBetweenShots, maxTimeBetweenShots) );
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "PlayerLaser")
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
            gameObject.SendMessageUpwards("calculateBoxCollider");
            gameObject.SetActive(false);
        }
    }
}