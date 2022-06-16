using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DashBoardController : MonoBehaviour
{

    GameObject player;
    float remainingTime;
    bool timerRunning;
    float flashTimer;
    float flashDuration = 1f;

    public TextMeshProUGUI rewardsText;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = 10f;
        timerRunning = true;
        UpdateScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        if(player != null)
        {
            var playerScript = player.GetComponent<PlayerController>();
            UpdateScore(playerScript.coins);
        }
    }

    public void UpdateScore(int score)
    {
        rewardsText.text = "" + score;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    void UpdateTime()
    {
        if(timerRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                DisplayTime(remainingTime);
                if(remainingTime < 5)
                {
                    Flash();
                }
            } else
            {
                timerText.enabled = true;
                Debug.Log("Out of time");
                //TODO End Game ? or Use rewards?
                timerRunning = false;
            }
        }
        
    }

    void DisplayTime(float time)
    {
        time++;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void Flash()
    {
        timerText.color = Color.red;
        if(flashTimer <= 0)
        {
            flashTimer = flashDuration;
        } else if(flashTimer <= flashDuration / 2)
        {
            flashTimer -= Time.deltaTime;
            timerText.enabled = false;

        } else
        {
            flashTimer -= Time.deltaTime;
            timerText.enabled = true;
        }
    }
}
