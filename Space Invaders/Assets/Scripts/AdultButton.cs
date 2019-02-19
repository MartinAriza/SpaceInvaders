using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdultButton : MonoBehaviour
{

    public void setAdult(bool b)
    {
        AdultManager.adult = b;
    }
}