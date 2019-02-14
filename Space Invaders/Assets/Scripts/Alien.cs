using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float minTimeBetweenShots = 1.0f;
    public float maxTimeBetweenShots = 3.0f;

    [SerializeField] ParticleSystem deathFX;

    bool stopFiring = false;

    void Start()
    {
        gameObject.GetComponent<CubeEditor>().enabled = false; //Se desactiva el snap para que se puedan mover en el play
        StartCoroutine(fire());
    }

    IEnumerator fire()
    {
        while(!stopFiring)
        {
            yield return new WaitForSeconds( Random.Range(minTimeBetweenShots, maxTimeBetweenShots) );
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "PlayerLaser")
        {
            Debug.Log("muerto");
            Instantiate(deathFX, transform.position, Quaternion.identity);
            gameObject.SendMessageUpwards("calculateBoxCollider");
            gameObject.SetActive(false);
        }
    }
}