using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue: MonoBehaviour
{
    //Las strings de diálogo están codificadas de la siguiente manera: [0]Mayúscula inicial del personaje (indica quien habla) [1, ...]Frase a decir

    [SerializeField] Text dialogueTextUI;
    [SerializeField] Text speakerNameUI;

    [SerializeField] Image speakerImageUI;
    [SerializeField] Image dialogueOverlayUI;

    [SerializeField][Tooltip("Dialogo codificado con la inicial del personaje en mayúscula en el caracter 0")]string [] dialogue;
    [SerializeField][Tooltip("Sprites posibles para los personajes que hablan en el diálogo")] Sprite[] speakerImages;

    Sprite currentSpeakerImage;
    string currentSpeakerName;

    void startDialogue()
    {
        dialogueOverlayUI.enabled = true;
        
    }

    void chooseSpeakerImage(string st)
    {
        switch(st[0])
        {
            case 'V':
                break;
        }
    }
}
