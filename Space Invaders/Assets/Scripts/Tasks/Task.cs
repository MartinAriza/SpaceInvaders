using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    [Tooltip("This will be the name showed in the pause menu")] public string name = "";
    /*[HideInInspector] */
    public bool completed = false;
    [TextArea]public string description;
    [SerializeField] UnityEvent taskCompletedEvent = new UnityEvent();
    private TasksLayer layer;

    private void Start()
    {
        layer = GetComponentInParent<TasksLayer>();
    }

    /*public Task(string n, string d)
    {
        name = n;
        description = d;
    }*/
    public void complete() {
        if (!completed)
        {
            completed = true;
            layer.checkComplete();
            taskCompletedEvent.Invoke();
            //gameObject.SetActive(false);
        }
    }
    //public bool isComplete() { return completed; }
    /*public string getName() { return name; }
    public string getDescription() { return description; }*/
}
