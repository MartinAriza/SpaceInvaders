using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class martin_GameManager : MonoBehaviour
{
    [SerializeField] string gameSceneName;
    public GameObject controlsImage;
    public GameObject backButton;

    public void showControls()
    {
        controlsImage.SetActive(true);
        backButton.SetActive(true);
    }

    public void goBackToMenu()
    {
        controlsImage.SetActive(false);
        backButton.SetActive(false);
    }

    public void quitGame() { Application.Quit(); }

    public void loadScene()
    {
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }
}
