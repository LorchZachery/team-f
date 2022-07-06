using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

/*
 * This class deals with navigation from main menu
*/
public class MainMenu : MonoBehaviour
{
    void Start()
    {
        // // Filling array with best scores from JSON if file exists
        // SaveGame.savedData = new GameData[10];
        // if (File.Exists(SaveGame.path))
        // {
        //     GameData[] importedGameData = SaveGame.ReadData();
        //     for (int i = 0; i < importedGameData.Length; i++)
        //     {
        //         SaveGame.savedData[i] = importedGameData[i];
        //     }
        // }
        // // Filling array with default scores of max int value if JSON file doesn't exist
        // else
        // {
        //     for (int i = 0; i < SaveGame.savedData.Length; i++)
        //     {
        //         SaveGame.savedData[i] = new GameData(i + 1, int.MaxValue);
        //     }
        // }
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Levels");
    }
}
