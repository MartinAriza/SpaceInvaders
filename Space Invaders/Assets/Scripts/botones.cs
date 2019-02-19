using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class botones : MonoBehaviour,
        IPointerClickHandler // 2
      , IDragHandler
      , IPointerEnterHandler
      , IPointerExitHandler
{
    [SerializeField] Sprite sprite_0;
    [SerializeField] Sprite sprite_ex;

    [SerializeField] MenuAudio menuAudio;

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        menuAudio.PlayClick();
        print("I was clicked");
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("I'm being dragged!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuAudio.PlayEnter();
        GetComponent<Image>().sprite = sprite_ex;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = sprite_0;
    }
}
