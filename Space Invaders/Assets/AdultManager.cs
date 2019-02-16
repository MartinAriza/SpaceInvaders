using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdultManager : MonoBehaviour
{
    public bool adult = true;

    void Start()
    {
        FindObjectOfType<Horde>().adult = adult;
        FindObjectOfType<PlayerMov>().adult = adult;
    }
}
