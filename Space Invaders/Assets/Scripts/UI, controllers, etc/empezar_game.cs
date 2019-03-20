using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class empezar_game : MonoBehaviour, IPointerClickHandler
{
   
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        SceneManager.LoadScene("GAME 2");
    }

}
