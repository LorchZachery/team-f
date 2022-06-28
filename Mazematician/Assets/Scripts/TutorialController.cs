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
        {2, "null"},
        {3, "breakable_tile_tutorial"},
        {4, "null"},
        {5, "null"},
        {6, "null"},
        {7, "null"},
        {8, "null"},
        {9, "null"},
        {10, "null"},
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

    public void CoinsButton()
    {
        TutorialLevelNumber = 2;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void BreakTileButton()
    {
        TutorialLevelNumber = 3;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void SpikeButton()
    {
        TutorialLevelNumber = 4;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void LosePointsButton()
    {
        TutorialLevelNumber = 5;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void OneWayDoorButton()
    {
        TutorialLevelNumber = 6;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void WalkThroughWallButton()
    {
        TutorialLevelNumber = 7;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void IncreaseTimeButton()
    {
        TutorialLevelNumber = 8;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void ShieldButton()
    {
        TutorialLevelNumber = 9;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void SmallBallButton()
    {
        TutorialLevelNumber = 10;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }
}
