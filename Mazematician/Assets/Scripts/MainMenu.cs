using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * This class deals with navigation from main menu
*/
public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Levels");
    }

    public void TutorialButton()
    {
        LevelsController.LevelNumber = 0;
        LevelsController.LevelName = "ag_tutorial";
        SceneManager.LoadScene("SampleGrid");
    }
}
