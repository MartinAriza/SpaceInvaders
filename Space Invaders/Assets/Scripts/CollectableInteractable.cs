using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableInteractable : Interaction {
    public int objectCode;

    protected override void Start() {
        base.Start();
    }


    public override void Interact() {
        base.Interact();
        if (!playerInteractor.itemGot.Contains(objectCode))
            playerInteractor.itemGot.Add(objectCode);
        gameObject.SetActive(false);
    }
}
