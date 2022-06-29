using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * This class deals with navigation from levels screen
*/
public class LevelsController : MonoBehaviour
{

    // Variables to store level inforamation
    public static int LevelNumber;
    public static string LevelName;

    // Initializing dictionary storing level numbers and level names
    public static Dictionary<int, string> levelNumberToName = new Dictionary<int, string>
    {
        {1, "ag_tutorial"},
        {2, "breakable_tile_tutorial"},
        {3, "Tutorial_2"},
        {4, "lorch_1"},
        {5, "sodhi_1"},
        {6, "iven_2"},
        {7, "level4_ashley"},
        {8, "lorch_4"},
        {9, "level1_ashley"},
        {10, "lorch_2"}
    };

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Level1Button()
    {
        LevelNumber = 1;
        LevelName = levelNumberToName[LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void Level2Button()
    {
        LevelNumber = 2;
        LevelName = levelNumberToName[LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void Level3Button()
    {
        LevelNumber = 3;
        LevelName = levelNumberToName[LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void Level4Button()
    {
        LevelNumber = 4;
        LevelName = levelNumberToName[LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void Level5Button()
    {
        LevelNumber = 5;
        LevelName = levelNumberToName[LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void Level6Button()
    {
        LevelNumber = 6;
        LevelName = levelNumberToName[LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void Level7Button()
    {
        LevelNumber = 7;
        LevelName = levelNumberToName[LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void Level8Button()
    {
        LevelNumber = 8;
        LevelName = levelNumberToName[LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void Level9Button()
    {
        LevelNumber = 9;
        LevelName = levelNumberToName[LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }

    public void Level10Button()
    {
        LevelNumber = 10;
        LevelName = levelNumberToName[LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }
}
