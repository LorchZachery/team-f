using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float time;
    public GameObject windowText;
    // Start is called before the first frame update
    //void Start()
    //{
        //GameObject timerObject = Instantiate(windowText, new Vector3(379f, 17f, -10f), Quaternion.identity);
    //}

    // Update is called once per frame
    void Update()
    {
        if (time > 0){
            updateTimer(time + 1);
            time -= Time.deltaTime;
        }
        else {
            //windowText.text = "Times Up!";
            updateAnalytics();
            SceneManager.LoadSceneAsync(2);
            return;
        }
    }
    void updateTimer(float currentTime)
    {
        // if(currentTime < 0){
        //     currentTime = 0;
        // }
        //currentTime += 1;
        
        //GameObject go = GameObject.FindGameObjectWithTag("timer");
        TextMeshProUGUI timerText = windowText.GetComponent<TextMeshProUGUI>();
        
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //Debug.Log("Timer Text: " + timerText.text);
        //Debug.Log("Timer Text: " + windowText.GetComponent<TextMeshProUGUI>().text);
    }

    void updateAnalytics()
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");
        int playerScore = player.GetComponent<PlayerController>().score;
        Debug.Log("Player Score: " + playerScore);

        AnalyticsResult analyticsResult = Analytics.CustomEvent("LevelLoss", new Dictionary<string, object>
        {
            {
                "Level", "Level_0"
            },
            {
                "Player Score during end of game", playerScore
            }
        });
        Debug.Log("analyticsResult for LevelLoss: " + analyticsResult);
    }

}
