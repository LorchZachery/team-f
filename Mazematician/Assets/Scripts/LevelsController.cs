using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelsController : MonoBehaviour
{
    public static string LevelName;
    public static int LevelNumber;

    // Initializing dictionary storing level numbers and level names
    public static Dictionary<int, string> levelNumberToName = new Dictionary<int, string>
    {
        {1, "lorch_1"},
        {2, "lorch_2"},
        {3, "iven_1"},
        {4, "iven_2"},
        {5, "level4_ashley"}
    };


    public void BackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
}
