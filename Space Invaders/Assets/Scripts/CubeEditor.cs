using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

//Este script hará un snap a la posicion del objeto al que se lo coloques, usado para colocar los aliens en la escena

public class CubeEditor : MonoBehaviour
{
    [SerializeField] int gridSize = 10;

    void Update()
    {
        SnapPositionToGRid();
    }

    private void SnapPositionToGRid()
    {
        transform.position = new Vector3(

                Mathf.RoundToInt(transform.position.x / gridSize) * gridSize,
                0,
                Mathf.RoundToInt(transform.position.z / gridSize) * gridSize
            );
    }
}