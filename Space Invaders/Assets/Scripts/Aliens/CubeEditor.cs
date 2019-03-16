using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

//Este script hará un snap a la posicion del objeto al que se lo coloques, usado para colocar los aliens en la escena

public class CubeEditor : MonoBehaviour
{
    [SerializeField] int gridSize = 10; //Como de grande es la rejilla a la que ajustarse

    void Update()
    {
        SnapPositionToGRid();
    }

    private void SnapPositionToGRid()
    {
        //Se limita la posición del objeto para que siempre sea múltiplo de gridSize
        transform.position = new Vector3(

                Mathf.RoundToInt(transform.position.x / gridSize) * gridSize,
                0,
                Mathf.RoundToInt(transform.position.z / gridSize) * gridSize
            );
    }
}