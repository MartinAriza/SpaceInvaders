using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksManager : MonoBehaviour
{

    [SerializeField] TasksLayer[] TasksLayersList;
    [SerializeField] TasksLayer currentLayer;
    int index = 0;

    private void Start()
    {
        TasksLayersList = GetComponentsInChildren<TasksLayer>();
        if (TasksLayersList.Length > 0)
        {
            currentLayer = TasksLayersList[0];
        }
    }
    
    public void changeLayer()
    {
        index++;
        if(index < TasksLayersList.Length)
        {
            currentLayer = TasksLayersList[index];
        } else
        {
            print("finished all tasks");
        }
    }
}
