using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverWon : MonoBehaviour
{

    void Start()
    {
        DisplayLevelCompleteText();
        GameObject nextLevelButtonObject = gameObject.transform.GetChild(3).gameObject;
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

    public void NextButton()
    {
        LevelsController.LevelNumber += 1;
        LevelsController.LevelName = LevelsController.levelNumberToName[LevelsController.LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }
}
