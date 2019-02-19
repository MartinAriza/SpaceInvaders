using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroyer : MonoBehaviour
{
    //Este objeto se coloca en la escena y todas las explosiones de los aliens se almacenaran como sus hijos
    //cada destructionPeriod segundos se destruiran todos sus hijos para que no se acumulen explosiones en la escena al ir pasando oleadas

    [SerializeField] [Tooltip("Cada cuantos segundos se destruyen las explosiones guardadas bajo este gameObject")] float destructionPeriod = 10.0f;

    bool stopDestroying = false;
    ParticleSystem[] explosions;

    void Start()
    {
        StartCoroutine(killThemAllNotJustTheMenButTheWomenAndChildrenToo());
    }

    IEnumerator killThemAllNotJustTheMenButTheWomenAndChildrenToo()
    {
        while (!stopDestroying)
        {
            explosions = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem explosion in explosions)
            {
                Destroy(explosion.gameObject);
            }
            yield return new WaitForSeconds(destructionPeriod);
        }
    }
}