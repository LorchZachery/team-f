using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DashBoardController : MonoBehaviour
{

    GameObject player;
    float remainingTime;
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = 30f;
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
        if(remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            DisplayTime(remainingTime);
        }
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

}
