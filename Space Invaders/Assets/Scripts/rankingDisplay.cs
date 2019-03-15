using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rankingDisplay : MonoBehaviour
{
    [SerializeField] GameObject textsParent;
    private void OnEnable()
    {
        if (!textsParent) textsParent = GameObject.Find("textsParent");
        if (textsParent)
        {
            string[] lineas = rankingController.getRanking();
            //GameObject texts = textsParent.getChi
        }
        

    }
}
