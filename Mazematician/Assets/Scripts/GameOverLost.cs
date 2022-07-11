using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This class deals with navigation from game over lost screen
*/
public class GameOverLost : MonoBehaviour
{
    [SerializeField] private AudioSource gameLostSound;

    public void RestartButton()
    {
        SceneManager.LoadScene("SampleGrid");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
