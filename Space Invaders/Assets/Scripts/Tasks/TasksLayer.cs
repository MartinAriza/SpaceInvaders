using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TasksLayer : MonoBehaviour
{
    
    public string name = "";
    //public TasksLayer nextLayer;
    [Tooltip("Esta descripción puede utilizarse para contextualizar el momento de la historia en que se deben cumplir estas tareas")]
    [TextArea]public string description = "";
    public Task[] tasksList;
    private TasksManager taskMg;

    

    private void Start()
    {
        tasksList = GetComponentsInChildren<Task>();
        taskMg = GetComponentInParent<TasksManager>();
    }

    public string[] getInfo()
    {
        tasksList = GetComponentsInChildren<Task>();
        string[] info = new string[tasksList.Length];
        for(int i = 0; i < tasksList.Length; i++)
        {
            info[i] = tasksList[i].name + ": " + tasksList[i].description;
        }
        return info;
    }


    public bool checkComplete()
    {
        bool complete = true;
        foreach (Task t in tasksList)
        {
            if (!t.completed) complete = false;
        }
        if (complete) taskMg.changeLayer();
        return complete;
    }
}