using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
        {4, "lorch_3"},
        {5, "iven_2"},
        {6, "sodhi_1"},
        {7, "nic_lvl_4"},
        {8, "level4_ashley"},
        {9, "sodhi_2"},
        {10, "lorch_2"}

    };

    void Start()
    {
        // // Looping through each level to see if there is a best score and displaying such score
        // for (int i = 0; i < 10; i++)
        // {
        //     int levelScoreTime = SaveGame.savedData[i].timeBestScore;
        //     if (levelScoreTime != int.MaxValue)
        //     {
        //         int sceneIndex = i + 11; // Index of game object in unity scene
        //         GameObject levelScoreObject = gameObject.transform.GetChild(sceneIndex).gameObject;
        //         TextMeshProUGUI levelScoreText = levelScoreObject.GetComponent<TextMeshProUGUI>();
        //         int minutes = levelScoreTime / 60;
        //         int seconds = levelScoreTime % 60;
        //         levelScoreText.text = "Best Score: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        //     }
        // }

        // Looping through each level to see if there is a best score and displaying such score
        int starIndex = 21;
        for (int i = 0; i < 10; i++)
        {
            int levelNum = i + 1;
            string levelKey = "BestScoreLevel" + levelNum.ToString();
            int currBestScore = PlayerPrefs.GetInt(levelKey, int.MaxValue);
            int firstStarIndex = starIndex;
            int secondStarIndex = firstStarIndex + 1;
            int thirdStarIndex = secondStarIndex + 1;
            GameObject firstStar = gameObject.transform.GetChild(firstStarIndex).gameObject;
            GameObject secondStar = gameObject.transform.GetChild(secondStarIndex).gameObject;
            GameObject thirdStar = gameObject.transform.GetChild(thirdStarIndex).gameObject;
            if (currBestScore != int.MaxValue)
            {
                int sceneIndex = i + 11; // Index of game object in unity scene
                GameObject levelScoreObject = gameObject.transform.GetChild(sceneIndex).gameObject;
                TextMeshProUGUI levelScoreText = levelScoreObject.GetComponent<TextMeshProUGUI>();
                int minutes = currBestScore / 60;
                int seconds = currBestScore % 60;
                levelScoreText.text = "Best Score: " + string.Format("{0:00}:{1:00}", minutes, seconds);
                if (currBestScore < 50)
                {
                    firstStar.SetActive(true);
                    secondStar.SetActive(true);
                    thirdStar.SetActive(true);
                }
                else if (currBestScore >= 50 && currBestScore < 75)
                {
                    firstStar.SetActive(true);
                    secondStar.SetActive(true);
                    thirdStar.SetActive(false);
                }
                else
                {
                    firstStar.SetActive(true);
                    secondStar.SetActive(false);
                    thirdStar.SetActive(false);
                }
            }
            else
            {
                firstStar.SetActive(false);
                secondStar.SetActive(false);
                thirdStar.SetActive(false);
            }
            starIndex = thirdStarIndex + 1;
        }
    }

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
