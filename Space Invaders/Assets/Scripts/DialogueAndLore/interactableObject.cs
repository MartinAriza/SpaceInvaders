using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class interactableObject : MonoBehaviour
{
    public UnityEvent onClick;
    int layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Interactable");
    }

    private void LateUpdate()
    {
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
}