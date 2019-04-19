using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggererScript : MonoBehaviour
{
    [Header("OnCollisionOrTriggerEvent")]
    [SerializeField] UnityEvent onCollision = new UnityEvent();
    [SerializeField] string objectCollidedTag = "";
    [SerializeField] string objectCollidedLayer = "";

    [Header("OnDisableEvent")]
    [SerializeField] UnityEvent onDisable = new UnityEvent();

    [Header("OnPowerMoveEvent")]
    [SerializeField] UnityEvent onPowerMove = new UnityEvent();

    [Header("OnPowerControlEvent")]
    [SerializeField] UnityEvent onPowerControl = new UnityEvent();
    [SerializeField] float controlTimer = 5f;

    [SerializeField] UnityEvent onParticleCollision = new UnityEvent();


    public void OnPowerMoveEnter()
    {
        onPowerMove.Invoke();
    }

    public void OnPowerControlEnter()
    {
        StartCoroutine(powerControl());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == objectCollidedTag ||
            LayerMask.LayerToName(other.gameObject.layer) == objectCollidedLayer ||
            (objectCollidedTag == "" && objectCollidedLayer == ""))
        {
            onCollision.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == objectCollidedTag || 
            LayerMask.LayerToName(collision.gameObject.layer) == objectCollidedLayer ||
            (objectCollidedTag == "" && objectCollidedLayer == ""))
        {
            onCollision.Invoke();
        }
    }

    public void OnDisable()
    {
        onDisable.Invoke();
    }

    private void OnParticleCollision(GameObject other)
    {
        VinPower vinPower = gameObject.GetComponent<VinPower>();
        if(vinPower != null && vinPower.isShielding() && other.tag == "AlienLaser") onParticleCollision.Invoke();
    }

    IEnumerator powerControl()
    {
        yield return new WaitForSecondsRealtime(controlTimer);
        onPowerControl.Invoke();
    }
}
