using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Interaction : MonoBehaviour {
    #region Events
    public UnityEvent OnGetClose;
    public UnityEvent OnSelected;
    public UnityEvent OnLeave;
    public UnityEvent OnInteract;
    public UnityEvent OnSuccessInteract;
    #endregion

    #region References
    public PlayerIntaractor playerInteractor;
    #endregion

    #region Internal
    public Color hightightedColor;
    public bool oneTime;
    public bool used;
    public List<int> prerequisites;
    #endregion
    [SerializeField]TotalLightsChange lightsChangeObj;
    [SerializeField] float transitionTime;

    protected virtual void Start() {
        playerInteractor = FindObjectOfType<PlayerIntaractor>();

        //OnGetClose.AddListener(GetClose);
        //OnLeave.AddListener(GetFar);
        //OnSelected.AddListener(Selected);
        OnInteract.AddListener(Interact);
        lightsChangeObj = GetComponentInChildren<TotalLightsChange>();
    }

    void Update() {
        
    }

    public void GetClose() {
        GetComponent<MeshRenderer>().material.color = hightightedColor - (Color.grey * 2);
    }

    public void Selected() {
        GetComponent<MeshRenderer>().material.color = hightightedColor;

    }

    public void GetFar() {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public virtual void Interact() {
        if (used && oneTime) { 
            playerInteractor.PlayWrongSound();
            Debug.Log("<Color=red>Already used " + name + "</Color>");
            return;
        }
        foreach (var item in prerequisites) {
            if (!playerInteractor.itemGot.Contains(item)) {
                playerInteractor.PlayWrongSound();
                Debug.Log("<Color=red>" + name + " not meeting condition " + item + "</Color>");
                return;
            }
        }
        playerInteractor.PlayCorrectSound();
        used = true;
        Debug.Log("<Color=blue>Used " + name + "</Color>");
        OnSuccessInteract.Invoke();
        if (oneTime) {
            OnLeave.Invoke();
            playerInteractor.closestInteraction = null;
            playerInteractor.closeInteractions.Remove(this);
        }

        lightsChangeObj.FadeAllLights(0.0f, transitionTime);
    }
}
