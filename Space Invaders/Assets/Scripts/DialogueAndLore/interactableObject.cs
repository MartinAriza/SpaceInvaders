using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class interactableObject : MonoBehaviour
{
    public UnityEvent onClick;
    Outline outline;
    int layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Interactable");
        outline = GetComponent<Outline>();
    }

    private void LateUpdate()
    {
        checkMouseOver();
        if(Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMask))
            {
                if(hitInfo.transform.gameObject.tag == "Interactable")
                {
                    onClick.Invoke();
                }
            }
        }
    }

    private void checkMouseOver()
    {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMask))
        {
            if (hitInfo.transform.gameObject.tag == "Interactable") outline.enabled = true;
        }
        else outline.enabled = false;
    }
}