using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Loads levels screen
    public void PlayButton()
    {
        SceneManager.LoadScene("Levels");
    }

    public void TutorialButton()
    {
        LevelsController.LevelName = "ag_tutorial";
        SceneManager.LoadScene("SampleGrid");
    }

    public void StoreButton()
    {
        // Put store scene here
    }

}
