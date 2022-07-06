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
        DisplayBestScoreText();
        DisplayStars();
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

    void DisplayStars()
    {
        GameObject oneStar = GameObject.FindGameObjectWithTag("oneStar");
        GameObject twoStars = GameObject.FindGameObjectWithTag("twoStar");
        GameObject threeStars = GameObject.FindGameObjectWithTag("threeStar");
        oneStar.SetActive(false);
        twoStars.SetActive(false);
        threeStars.SetActive(false);
        Debug.Log("ScoreTime: " + scoreTime);
        if (scoreTime < 50)
        {
            threeStars.SetActive(true);
            twoStars.SetActive(false);
            oneStar.SetActive(false);
        }
        else if(scoreTime >= 50 && scoreTime < 75)
        {
            twoStars.SetActive(true);
            oneStar.SetActive(false);
            threeStars.SetActive(false);
        }
        else
        {
            oneStar.SetActive(true);
            twoStars.SetActive(false);
            threeStars.SetActive(false);
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

    void DisplayBestScoreText()
    {
        // GameObject bestScoreObject = gameObject.transform.GetChild(2).gameObject;
        // TextMeshProUGUI bestScoreText = bestScoreObject.GetComponent<TextMeshProUGUI>();
        // int currLevelBestScore = SaveGame.savedData[LevelsController.LevelNumber - 1].timeBestScore;
        // if (scoreTime < currLevelBestScore)
        // {
        //     bestScoreText.text = "New best score!";
        //     SaveGame.savedData[LevelsController.LevelNumber - 1].timeBestScore = scoreTime;
        //     SaveGame.SaveData();
        // }
        // else
        // {
        //     bestScoreText.text = "";
        // }

        GameObject bestScoreObject = gameObject.transform.GetChild(2).gameObject;
        TextMeshProUGUI bestScoreText = bestScoreObject.GetComponent<TextMeshProUGUI>();
        int levelNum = LevelsController.LevelNumber;
        string levelKey = "BestScoreLevel" + levelNum.ToString();
        int currBestScore = PlayerPrefs.GetInt(levelKey, int.MaxValue);
        if (scoreTime < currBestScore)
        {
            bestScoreText.text = "New best score!";
            PlayerPrefs.SetInt(levelKey, scoreTime);
        }
        else
        {
            bestScoreText.text = "";
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
        LevelsController.LevelNumber += 1;
        LevelsController.LevelName = LevelsController.levelNumberToName[LevelsController.LevelNumber];
        SceneManager.LoadScene("SampleGrid");
    }
}
