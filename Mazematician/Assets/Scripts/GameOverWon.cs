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
    public static int tutorialLevel = 1;
    void Start()
    {
        DisplayLevelCompleteText();
        GameObject nextLevelButtonObject = gameObject.transform.GetChild(3).gameObject;
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

    public void RestartButton()
    {
        SceneManager.LoadScene("SampleGrid");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void IncrementLevel()
    {
        tutorialLevel += 1;
    }

    public void Update()
    {
        GameObject nextLevelButtonObject = gameObject.transform.GetChild(3).gameObject;
        if (tutorialLevel >= LevelsController.tutorialLevels.Count)
        {
            nextLevelButtonObject.SetActive(false);
        }
    }

    public void NextButton()
    {        
        if (LevelsController.tutorialLevels.ContainsValue(LevelsController.LevelName))
        {
            IncrementLevel();
            LevelsController.LevelName = LevelsController.tutorialLevels[tutorialLevel];
            SceneManager.LoadScene("SampleGrid");
        }
        else
        {
            LevelsController.LevelNumber += 1;
            LevelsController.LevelName = LevelsController.levelNumberToName[LevelsController.LevelNumber];
            SceneManager.LoadScene("SampleGrid");

        }
        
    }
}
