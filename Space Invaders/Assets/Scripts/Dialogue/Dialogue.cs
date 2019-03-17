using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Dialogue: MonoBehaviour
{
    
    //Las strings de diálogo están codificadas de la siguiente manera: [0]Mayúscula inicial del personaje [1]G o N indica si está glitcheada o no [2, ...]Frase a decir

    #region Events
    [Tooltip("Este evento se ejecuta al comenzar el diálogo")] public UnityEvent onDialogueStart;
    [Tooltip("Este evento se ejecuta al terminar el diálogo")] public UnityEvent onDialogueFinish;
    #endregion

    #region inspector parameters
    [Header("Contenido de los diálogos")]
    [SerializeField] [TextArea] [Tooltip("Dialogo codificado con la inicial del personaje en mayúscula en el caracter 0")] string[] dialogue;
    [SerializeField] [Tooltip("Sprites posibles para los personajes que hablan en el diálogo")] Sprite[] speakerImages;

    [Header("Parámetros de los diálogos")]
    [SerializeField] Font normalFont;
    [SerializeField] Font glitchedFont;
    [SerializeField][Range(30, 60)] int fontSizeName = 30;
    [SerializeField][Range(24, 50)] int fontSizeDialogue = 24;
    [SerializeField][ColorUsage(true, false)] Color fontColor;

    [SerializeField] [Tooltip("Cuantos segundos tarda cada letra en escribirse")] float timeToWrite = 0.2f;
    [Space(2)]
    #endregion

    #region references
    [Header("Referencias a la UI")]
    [SerializeField] Text dialogueTextUI;   //Texto de la UI donde irá el diálogo
    [SerializeField] Text speakerNameUI;    //Texto de la UI donde irá el nombre del interlocutor

    [SerializeField] Image speakerImageUI;  //Imagen de la UI que muestra un sprite del interlocutor
    [SerializeField] Image dialogueOverlayUI;   //Imagen de fondo del diálogo

    [SerializeField] Button continueButtonUI;   //Botón de continuar el diálogo
    [SerializeField] Text continueButtonTextUI; //Texto del botón de continuar
    #endregion

    #region class variables
    int dialogueIndex = 0;
    bool finishedWriting = false;
    #endregion

    public void startDialogue()
    {
        onDialogueStart.Invoke();
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Time.timeScale = 0.0f;                         //Se pausa el juego (menos la interfaz)

        //Se activan los elementos de la interfaz que muestran cosas del diálogo
        dialogueOverlayUI.gameObject.SetActive(true);
        continueButtonUI.gameObject.SetActive(true);
        dialogueTextUI.gameObject.SetActive(true);

        speakerImageUI.enabled = true;
        speakerNameUI.enabled = true;

        continueButtonUI.onClick.AddListener(delegate { nextString(); }); //Se asigna el evento de pasar de línea de diálogo al botón de la interfaz
        continueButtonTextUI.text = "Continuar";

        speakerNameUI.font = normalFont;
        speakerNameUI.fontSize = fontSizeName;
        speakerNameUI.color = fontColor;

        dialogueTextUI.font = normalFont;
        dialogueTextUI.fontSize = fontSizeDialogue;
        dialogueTextUI.color = fontColor;

        StartCoroutine(write(dialogue[dialogueIndex])); //Se escribe la primera línea
    }

    IEnumerator write(string st)
    {
        finishedWriting = false;

        chooseSpeakerImageAndName(st);
        dialogueTextUI.text = "";

        st = st.Substring(2);

        foreach (char leter in st.ToCharArray())
        {
            if (!finishedWriting)
            {
                dialogueTextUI.text += leter;
                yield return new WaitForSecondsRealtime(timeToWrite);
            }
        }

        if (dialogueIndex >= dialogue.Length - 1) continueButtonTextUI.text = "Cerrar";

        finishedWriting = true;
    }

    public void nextString()
    {
        //Si se ha acabado de escribir la líne de diálogo
        if(finishedWriting)
        {
            checkEndOfDialogue();   //Se comprueba si es la última
            dialogueIndex++;        //Se pasa a la siguiente línea
            dialogueIndex = Mathf.Clamp(dialogueIndex, 0, dialogue.Length - 1);
            checkGlitched(dialogue[dialogueIndex]);
            StartCoroutine(write(dialogue[dialogueIndex])); //Se escribe la siguiente línea
        }

        //Si no se ha acabado y se pulsa el botón se escribe toda la línea de una vez
        else
        {
            finishedWriting = true;
            dialogueTextUI.text = dialogue[dialogueIndex].Substring(2);

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

            onDialogueFinish.Invoke();

            Destroy(gameObject);
        }
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

    void checkGlitched(string st)
    {
        if(st[1] == 'G')
        {
            speakerNameUI.font = glitchedFont;
            dialogueTextUI.font = glitchedFont;
        }
        else
        {
            speakerNameUI.font = normalFont;
            dialogueTextUI.font = normalFont;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Vin")
        {
            startDialogue();
        }
    }
}