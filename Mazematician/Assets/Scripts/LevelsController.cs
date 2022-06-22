using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsController : MonoBehaviour
{
    public static string LevelName;

    public void BackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Level1Button()
    {
        LevelName = "null";
        SceneManager.LoadScene("SampleGrid");
    }

    public void Level2Button()
    {
        // LevelName = "tutorial_lorch";
        // SceneManager.LoadScene("SampleGrid");
    }

    public void Level3Button()
    {
        // LevelName = "level1_lorch";
        // SceneManager.LoadScene("SampleGrid");
    }

    public void Level4Button()
    {
        // LevelName = "level2_lorch";
        // SceneManager.LoadScene("SampleGrid");
    }

    public void Level5Button()
    {
        // LevelName = "level3_lorch";
        // SceneManager.LoadScene("SampleGrid");
    }
}
