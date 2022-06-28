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
        if (MainMenu.PlayButtonClicked)
        {
            if (!LevelsController.levelNumberToName.ContainsKey(LevelsController.LevelNumber + 1))
            {
                nextLevelButtonObject.SetActive(false);
            }
            else
            {
                nextLevelButtonObject.SetActive(true);
            }
        }
        else if (MainMenu.TutorialButtonClicked)
        {
            if (!TutorialController.tutorialLevels.ContainsKey(TutorialController.TutorialLevelNumber + 1))
            {
                nextLevelButtonObject.SetActive(false);
            }
            else
            {
                nextLevelButtonObject.SetActive(true);
            }
        }
    }

    void DisplayLevelCompleteText()
    {
        GameObject levelCompleteObject = gameObject.transform.GetChild(0).gameObject;
        TextMeshProUGUI levelCompleteText = levelCompleteObject.GetComponent<TextMeshProUGUI>();
        if (MainMenu.PlayButtonClicked)
        {
            levelCompleteText.text = "Level " + LevelsController.LevelNumber + " Complete!";
        }
        else if (MainMenu.TutorialButtonClicked)
        {
            levelCompleteText.text = "Tutorial " + TutorialController.TutorialLevelNumber + " Complete!";
        }
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
        if (MainMenu.PlayButtonClicked)
        {
            LevelsController.LevelNumber += 1;
            LevelsController.LevelName = LevelsController.levelNumberToName[LevelsController.LevelNumber];
            SceneManager.LoadScene("SampleGrid");
        }
        else if (MainMenu.TutorialButtonClicked)
        {
            TutorialController.TutorialLevelNumber += 1;
            TutorialController.TutorialLevelName = TutorialController.tutorialLevels[TutorialController.TutorialLevelNumber];
            SceneManager.LoadScene("SampleGrid");
        }
    }
}
