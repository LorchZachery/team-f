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
    public static bool PlayButtonClicked;
    public static bool TutorialButtonClicked;

    void Start()
    {
        PlayButtonClicked = false;
        TutorialButtonClicked = false;
    }

    public void PlayButton()
    {
        PlayButtonClicked = true;
        SceneManager.LoadScene("Levels");
    }

    public void TutorialButton()
    {
        TutorialButtonClicked = true;
        SceneManager.LoadScene("Tutorial");
        // LevelsController.LevelNumber = 0;
        // LevelsController.LevelName = "ag_tutorial";
        // // LevelsController.LevelName = "breakable_tile_tutorial";
        // SceneManager.LoadScene("SampleGrid");
    }
}
