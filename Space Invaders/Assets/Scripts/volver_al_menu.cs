using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class volver_al_menu : MonoBehaviour, IPointerClickHandler
{
   
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        EyesController.first = true;
        SceneManager.LoadScene("menu");
    }

}
