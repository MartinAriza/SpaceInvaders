using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntaractor : MonoBehaviour {
    #region InternalAndDebug
    public List<Interaction> closeInteractions;
    public Interaction closestInteraction;
    public Interaction oldClosestInteraction;
    public float minDistance;
    public string interactableTagName = "Interactable";
    public KeyCode interactionKey1 = KeyCode.E;
    public KeyCode interactionKey2 = KeyCode.Mouse0;
    public List<int> itemGot;

    public float interactionDistance = 1;
    public LayerMask interactableMask;
    #endregion

    #region References
    public AudioClip wrongSound;
    public AudioClip correctSound;
    public AudioSource audioSource;
    #endregion

    void Start() {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update() {
        FindFirst();
        CheckInteractionKey();
    }

    void FindFirst() {
        if (closeInteractions.Count > 0) {
            Interaction tempClosestInteraction = closeInteractions[0];
            minDistance = Vector3.Distance(transform.position, closeInteractions[0].transform.position);
            foreach (var item in closeInteractions) {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if (distance < minDistance && !(item.used && item.oneTime)) {
                    tempClosestInteraction = item;
                    minDistance = distance;
                }
            }
            if (closestInteraction != tempClosestInteraction) {
                if (closestInteraction != null) {
                    closestInteraction.OnGetClose.Invoke();
                }
                closestInteraction = tempClosestInteraction;
                closestInteraction.OnSelected.Invoke();
            }
        } else {
            closestInteraction = null;
        }
    }

    void CheckInteractionKey() {
        if (Input.GetKeyDown(interactionKey1) || Input.GetKeyDown(interactionKey2)) {
            if (closestInteraction != null) {
                closestInteraction.OnInteract.Invoke();
            }
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag(interactableTagName)) { //tag de interactuable
            if (Physics.Raycast(transform.position, (other.transform.position - transform.position), interactableMask)) {
                Interaction interaction = other.GetComponent<Interaction>();
                if (!closeInteractions.Contains(interaction) && !(interaction.used && interaction.oneTime)) { //no conocidoy no usado
                    closeInteractions.Add(interaction);
                    interaction.OnGetClose.Invoke();
                }
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.CompareTag(interactableTagName)) {
            Interaction interaction = other.GetComponent<Interaction>();
            if (closeInteractions.Contains(interaction)) {
                closeInteractions.Remove(interaction);
                interaction.OnLeave.Invoke();
            }
        }
    }

    public void PlayCorrectSound() {
        if (audioSource.isPlaying) {
            return;
        }
        if (correctSound != null) audioSource.clip = correctSound;
        audioSource.Play();
    }

    public void PlayWrongSound() {
        if (audioSource.isPlaying) {
            return;
        }
        if (wrongSound != null) audioSource.clip = wrongSound;
        audioSource.Play();
    }
}


