using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] Button continueButtonUI;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Vin")
        {
            dialogue.startDialogue();
            Destroy(gameObject);
        }  
    }
}
