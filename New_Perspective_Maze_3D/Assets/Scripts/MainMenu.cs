using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Success!");
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void LoadMainScreen()
    {
        SceneManager.LoadScene(0);
    }
}
