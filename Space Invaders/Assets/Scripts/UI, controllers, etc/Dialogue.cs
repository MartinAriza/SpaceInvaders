using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue: MonoBehaviour
{
    //Las strings de diálogo están codificadas de la siguiente manera: [0]Mayúscula inicial del personaje (indica quien habla) [1, ...]Frase a decir

    [SerializeField] Text dialogueTextUI;   //Texto de la UI donde irá el diálogo
    [SerializeField] Text speakerNameUI;    //Texto de la UI donde irá el nombre del interlocutor

    [SerializeField] Image speakerImageUI;  //Imagen de la UI que muestra un sprite del interlocutor
    [SerializeField] Image dialogueOverlayUI;   //Imagen de fondo del diálogo

    [SerializeField] Button continueButtonUI;   //Botón de continuar el diálogo
    [SerializeField] Text continueButtonTextUI; //Texto del botón de continuar

    [SerializeField][Tooltip("Dialogo codificado con la inicial del personaje en mayúscula en el caracter 0")] string [] dialogue;
    [SerializeField][Tooltip("Sprites posibles para los personajes que hablan en el diálogo")] Sprite[] speakerImages;

    [SerializeField][Tooltip("Cuantos segundos tarda cada letra en escribirse")] float timeToWrite = 0.2f;

    int dialogueIndex = 0;
    bool finishedWriting = false;

    public void startDialogue()
    {
        Time.timeScale = 0.0f;                         //Se pausa el juego (menos la interfaz)

        //Se activan los elementos de la interfaz que muestran cosas del diálogo
        dialogueOverlayUI.gameObject.SetActive(true);
        continueButtonUI.gameObject.SetActive(true);
        dialogueTextUI.gameObject.SetActive(true);

        speakerImageUI.enabled = true;
        speakerNameUI.enabled = true;

        continueButtonUI.onClick.AddListener(delegate { nextString(); }); //Se asigna el evento de pasar de línea de diálogo al botón de la interfaz
        continueButtonTextUI.text = "Continuar";

        StartCoroutine(write(dialogue[dialogueIndex])); //Se escribe la primera línea
    }

    public void nextString()
    {
        //Si se ha acabadoo
        if(finishedWriting)
        {
            checkEndOfDialogue();
            dialogueIndex++;
            dialogueIndex = Mathf.Clamp(dialogueIndex, 0, dialogue.Length - 1);
            StartCoroutine(write(dialogue[dialogueIndex]));
        }

        else
        {
            finishedWriting = true;
            dialogueTextUI.text = dialogue[dialogueIndex].Substring(1);

            if(dialogueIndex >= dialogue.Length - 1) continueButtonTextUI.text = "Cerrar";
        }
    }
    
    void checkEndOfDialogue()
    {
        if(finishedWriting && dialogueIndex >= dialogue.Length - 1)
        {
            StopAllCoroutines();

            continueButtonUI.gameObject.SetActive(false);

            dialogueOverlayUI.gameObject.SetActive(false);
            dialogueTextUI.gameObject.SetActive(false);

            speakerImageUI.enabled = false;
            speakerNameUI.enabled = false;

            speakerImageUI.gameObject.SetActive(false);
            speakerNameUI.gameObject.SetActive(false);

            Time.timeScale = 1.0f;

            Destroy(gameObject);
        }
    }

    IEnumerator write(string st)
    {
        finishedWriting = false;

        chooseSpeakerImageAndName(st);
        dialogueTextUI.text = "";

        st = st.Substring(1);

        foreach (char leter in st.ToCharArray())
        {
            if(!finishedWriting)
            {
                dialogueTextUI.text += leter;
                yield return new WaitForSecondsRealtime(timeToWrite);
            }
        }

        if (dialogueIndex >= dialogue.Length - 1) continueButtonTextUI.text = "Cerrar";

        finishedWriting = true;
    }

    void chooseSpeakerImageAndName(string st)
    {
        switch(st[0])
        {
            case 'V':
                speakerImageUI.sprite = speakerImages[0];
                speakerNameUI.text = "Vin";
                break;

            case 'L':
                speakerImageUI.sprite = speakerImages[1];
                speakerNameUI.text = "Lord Trumpet";
                break;

            case 'D':
                speakerImageUI.sprite = speakerImages[2];
                speakerNameUI.text = "Doctor Ali";
                break;

            case 'S':
                speakerImageUI.sprite = speakerImages[3];
                speakerNameUI.text = "Soldado";
                break;

            case 'I':
                speakerImageUI.sprite = speakerImages[4];
                speakerNameUI.text = "???";
                break;

            case 'N':
                speakerImageUI.sprite = speakerImages[5];
                speakerNameUI.text = "Neyrblat";
                break;
        }

        speakerImageUI.gameObject.SetActive(true);
        speakerNameUI.gameObject.SetActive(true);
    }
}