using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Loads levels screen
    public void PlayButton()
    {
        SceneManager.LoadScene("Levels");
    }

}
