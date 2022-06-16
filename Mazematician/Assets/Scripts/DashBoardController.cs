using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DashBoardController : MonoBehaviour
{

    GameObject player;
    float remainingTime;
    bool timerRunning;
    int target;
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = 300f;
        timerRunning = true;
        UpdateScore(0);
        DisplayTargetText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        if (player != null)
        {
            var playerScript = player.GetComponent<PlayerController>();
            UpdateScore(playerScript.coins);
        }
    }

    public void UpdateScore(int score)
    {
        GameObject rewards = gameObject.transform.GetChild(1).gameObject;
        TextMeshProUGUI rewardsText = rewards.GetComponent<TextMeshProUGUI>();
        rewardsText.text = "" + score;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    void UpdateTime()
    {
        if (timerRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                DisplayTime(remainingTime);
            }
            else
            {
                Debug.Log("Out of time");
                //TODO End Game ? or Use rewards?
                SceneManager.LoadScene("GameOver");
                timerRunning = false;
            }
        }

    }

    public void SetTime(float time)
    {
        remainingTime = time;
    }

    void DisplayTime(float time)
    {
        time++;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        GameObject timer = gameObject.transform.GetChild(2).gameObject;
        TextMeshProUGUI timerText = timer.GetComponent<TextMeshProUGUI>();
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetTarget(int target)
    {
        this.target = target;
    }

    void DisplayTargetText()
    {
        GameObject targetObject = gameObject.transform.GetChild(3).gameObject;
        TextMeshProUGUI targetText = targetObject.GetComponent<TextMeshProUGUI>();
        targetText.text = "Target: " + this.target;
    }

}
