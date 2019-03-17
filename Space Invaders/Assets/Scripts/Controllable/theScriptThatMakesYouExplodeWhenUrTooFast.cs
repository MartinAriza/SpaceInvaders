using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theScriptThatMakesYouExplodeWhenUrTooFast : MonoBehaviour
{
    int layerControllable;
    [SerializeField] float speedToExplode = 10.0f;
    Rigidbody rb;
    Vector3 lastSpeed = new Vector3(0f,0f,0f);
    Vector3 actualSpeed = new Vector3(0f, 0f, 0f);

    public ParticleSystem powerWave;
    [SerializeField] ParticleSystem dustParticles;

    // Start is called before the first frame update
    void Start()
    {
        //layerMask = LayerMask.GetMask("staticSolid");
        layerControllable = LayerMask.NameToLayer("staticSolid");
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lastSpeed = actualSpeed;
        actualSpeed = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == layerControllable && collision.gameObject.tag == "Wall")
        {
            
            if (lastSpeed.magnitude > speedToExplode)
            {
                Instantiate(dustParticles, gameObject.transform.position, Quaternion.Euler(-lastSpeed));
                powerWave.Stop();
                print("toy muerto");
                gameObject.SetActive(false);
            }
        }
    }
}
