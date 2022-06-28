using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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

    public void LosePointsButton()
    {
        TutorialLevelNumber = 5;
        TutorialLevelName = tutorialLevels[TutorialLevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }
}
