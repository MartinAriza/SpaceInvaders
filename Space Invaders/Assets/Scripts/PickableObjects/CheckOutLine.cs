using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutLine : MonoBehaviour
{

    [SerializeField]Material OutLineMat;
    [SerializeField]Material[] mats;
    [SerializeField] bool active;
    float widthOriginal;

    void Start()
    {
        mats = GetComponent<Renderer>().materials;
        foreach(Material m in mats)
        {
            if (m.name.Equals("Outline (Instance)"))
            {
                OutLineMat = m;
                widthOriginal = m.GetFloat("_Outline");
            }
        }
        if (active) OutLineMat.SetFloat("_Outline", widthOriginal);
        else OutLineMat.SetFloat("_Outline", 0.0f);
    }

    private void Update()
    {
        //if (Input.GetKeyDown("w")) EnableOrDisableOutLine(); //Quitable
    }

    public void EnableOrDisableOutLine()
    {
        active = !active;
        if (active) OutLineMat.SetFloat("_Outline", widthOriginal);
        else OutLineMat.SetFloat("_Outline", 0.0f);
    }

}
