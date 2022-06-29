using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    // Variables to store level inforamation
    public static int TutorialLevelNumber;
    public static string TutorialLevelName;

    // Initializing dictionary storing level numbers and level names
    public static Dictionary<int, string> tutorialLevels = new Dictionary<int, string>
    {
        {1, "ag_tutorial"},
        {2, "breakable_tile_tutorial"},
        {3, "null"},
        {4, "Tutorial_2"}
    };

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PointsButton()
    {
        TutorialLevelNumber = 1;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void BreakTileButton()
    {
        TutorialLevelNumber = 2;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void Obstaclesbutton()
    {
        TutorialLevelNumber = 3;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void PowerUpButton()
    {
        TutorialLevelNumber = 4;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }
}
