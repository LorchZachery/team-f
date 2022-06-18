using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class GameEndController : MonoBehaviour
{
    public int targetScore;
    public int totalTime = 120;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setTargetScore(int value)
    {
        this.targetScore = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            var script = collision.gameObject.GetComponent<PlayerController>();
            if (script.score == targetScore)
            {
                // script.NotifyPlayerWin();
                getTimeInfo();
                SceneManager.LoadScene("GameOverWon");
            }
        }
    }

    private void getTimeInfo()
    {
        GameObject timer = GameObject.FindGameObjectWithTag("timer");
        string timeText = timer.GetComponent<TextMeshProUGUI>().text;
        string[] timeFormat = timeText.Split(':');
        int totalTimeRemaining = Mathf.FloorToInt(Int32.Parse(timeFormat[0]) * 60 + Int32.Parse(timeFormat[1]));
        int timeTakenToCompleteLevel = totalTime - Mathf.FloorToInt(totalTimeRemaining);

        Debug.Log("Time Remaining: " + totalTimeRemaining + " seconds");
        Debug.Log("Time Taken to Complete Level: " + timeTakenToCompleteLevel + " seconds");

        int score = totalTimeRemaining * 30;
        Debug.Log("Score: " + score);

        AnalyticsResult analyticsResult = Analytics.CustomEvent("LevelWin", new Dictionary<string, object>
        {
            {
                "Level", "Level_0"
            },
            {
                "Time To Complete Level", timeTakenToCompleteLevel
            },
            {
                "Score", score
            }
        }) ;
        Debug.Log("analyticsResult for Level Win: " + analyticsResult);
        
        
    }

}
