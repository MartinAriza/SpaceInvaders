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

    public Sprite sprite_0;
    public Sprite sprite_ex;

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        print("I was clicked");
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("I'm being dragged!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = sprite_ex;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = sprite_0;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
