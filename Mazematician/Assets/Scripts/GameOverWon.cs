using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/*
 * This class deals with navigation from game over won screen
*/
public class GameOverWon : MonoBehaviour
{
    public static int scoreTime;
    void Start()
    {
        DisplayLevelCompleteText();
        DisplayScoreTimeText();
        GameObject nextLevelButtonObject = gameObject.transform.GetChild(5).gameObject;
        // Checking whether at last level in order to dispplay next button or not
        if (!LevelsController.levelNumberToName.ContainsKey(LevelsController.LevelNumber + 1))
        {
            nextLevelButtonObject.SetActive(false);
        }
        else
        {
            nextLevelButtonObject.SetActive(true);
        }
    }

    void DisplayLevelCompleteText()
    {
        GameObject levelCompleteObject = gameObject.transform.GetChild(0).gameObject;
        TextMeshProUGUI levelCompleteText = levelCompleteObject.GetComponent<TextMeshProUGUI>();
        levelCompleteText.text = "Level " + LevelsController.LevelNumber + " Complete!";
    }

    void DisplayScoreTimeText()
    {
        GameObject scoreObject = gameObject.transform.GetChild(1).gameObject;
        TextMeshProUGUI scoreText = scoreObject.GetComponent<TextMeshProUGUI>();
        int minutes = scoreTime / 60;
        int seconds = scoreTime % 60;
        scoreText.text = "Score: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("SampleGrid");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NextButton()
    {
        LevelsController.LevelNumber += 1;
        LevelsController.LevelName = LevelsController.levelNumberToName[LevelsController.LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }
}
