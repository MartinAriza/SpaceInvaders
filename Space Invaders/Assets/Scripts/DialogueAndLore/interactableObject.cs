using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class interactableObject : MonoBehaviour
{
    [SerializeField] UnityEvent onClick;
    [SerializeField] UnityEvent onMouseOver;
    [SerializeField] UnityEvent onMouseExit;
    int layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Interactable");
    }

    private void LateUpdate()
    {
        checkMouseOver();
        if(Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMask))
            {
                if(hitInfo.transform.gameObject == gameObject && Time.timeScale == 1.0f)
                {
                    this.onClick.Invoke();
                }
            }
        }
    }

    private void checkMouseOver()
    {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMask))
        {
            if (hitInfo.transform.gameObject == gameObject && Time.timeScale == 1.0f) this.onMouseOver.Invoke();
        }
        else this.onMouseExit.Invoke();
    }

    public void pauseGame(bool pause)
    {
        if (pause) Time.timeScale = 0.0f;
        else Time.timeScale = 1.0f;
    }
}