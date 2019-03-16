using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class exit_game : MonoBehaviour, IPointerClickHandler
{

    // Use this for initialization
    public void OnPointerClick(PointerEventData eventData) // 3
    {
        Application.Quit();
    }
}
