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


    public void OnPowerMoveEnter()
    {
        onPowerMove.Invoke();
    }

    public void OnPowerControlEnter()
    {
        onPowerControl.Invoke();
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
}
