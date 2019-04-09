using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theScriptThatMakesYouExplodeWhenUrTooFast : MonoBehaviour
{
    #region particleDependency
    public ParticleSystem alienLaser;
    #endregion

    int layerControllable;
    [SerializeField] float speedToExplode = 10.0f;
    
    Rigidbody rb;
    Vector3 lastSpeed = new Vector3(0f,0f,0f);
    Vector3 actualSpeed = new Vector3(0f, 0f, 0f);

    public ParticleSystem powerWave;
    [SerializeField] ParticleSystem deathParticles;
    Outline ol;
    [SerializeField] float timeToDeactivate = 0.3f;
    [SerializeField] MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        ol = GetComponent<Outline>();
        layerControllable = LayerMask.NameToLayer("staticSolid");
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ol.enabled = false;
        lastSpeed = actualSpeed;
        actualSpeed = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == layerControllable && collision.gameObject.tag == "Wall")
        {
            
            if (lastSpeed.magnitude > speedToExplode)
            {
                //Instantiate(dustParticles, gameObject.transform.position, Quaternion.Euler(-lastSpeed));
                deathParticles.Play();
                powerWave.Stop();
                print("toy muerto");
                meshRenderer.enabled = false;
                StartCoroutine(deactivateObject());
            }
        }
    }

    IEnumerator deactivateObject()
    {
        yield return new WaitForSecondsRealtime(timeToDeactivate);
        gameObject.SetActive(false);
    }
}
