using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasksManager : MonoBehaviour
{
    [Header("TASK MANAGEMENT")]
    [SerializeField] TasksLayer[] TasksLayersList;
    [SerializeField] TasksLayer currentLayer;
    int index = 0;
    bool paused = false;

    [Header("PAUSE MENU")]
    [SerializeField]GameObject pauseMenu;
    [SerializeField] Text stateText;
    [SerializeField] Text tasksText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            checkPausedMenu();
    }

    private void Start()
    {
        //TASKS
        TasksLayersList = GetComponentsInChildren<TasksLayer>();
        if (TasksLayersList.Length > 0)
        {
            currentLayer = TasksLayersList[0];
        }

        //PAUSE MENU
        //checkPausedMenu();
    }

    private void checkPausedMenu()
    {
        paused = !paused;
        pauseMenu.SetActive(paused);
        if (paused)
        {
            writeTasksInUI();
            Time.timeScale = 0f;
        }
        else Time.timeScale = 1f;
        
    }

    private void writeTasksInUI()
    {
        stateText.text = currentLayer.name;
        string[] info = currentLayer.getInfo();
        tasksText.text = "";
        for(int i = 0; i < info.Length; i++)
        {
            tasksText.text += info[i] + "\n \n";
        }
    }


    public void changeLayer()
    {
        index++;
        if(index < TasksLayersList.Length)
        {
            currentLayer = TasksLayersList[index];
        }
    }
}
