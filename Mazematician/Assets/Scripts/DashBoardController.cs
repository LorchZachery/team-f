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
    float flashTimer;
    float flashDuration = 1f;
    bool freezeTime = false;
    int freezeCount = 0;

    public TextMeshProUGUI rewardsText;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = 60 * 2f;
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
                Debug.Log("Time remaining: " + remainingTime);
                if (player.GetComponent<PlayerController>().coins == 3)
                {
                    freezeCount = 1;
                    StartCoroutine("Freeze");
                    DisplayFreezeCount();
                }
                else
                {
                    remainingTime -= Time.deltaTime;
                    DisplayTime(remainingTime);
                    if (remainingTime < 5)
                    {
                        Flash();
                    }
                }  
            }
            else
            {
                timerText.enabled = true;
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

    void DisplayFreezeCount()
    {
        GameObject targetObject = gameObject.transform.GetChild(5).gameObject;
        TextMeshProUGUI targetText = targetObject.GetComponent<TextMeshProUGUI>();
        targetText.text = freezeCount.ToString();
    }

    private void Flash()
    {
        timerText.color = Color.red;
        if (flashTimer <= 0)
        {
            flashTimer = flashDuration;
        }
        else if (flashTimer <= flashDuration / 2)
        {
            flashTimer -= Time.deltaTime;
            timerText.enabled = false;

        }
        else
        {
            flashTimer -= Time.deltaTime;
            timerText.enabled = true;
        }
    }

    IEnumerator Freeze()
    {
        {
            freezeTime = false;
            timerText.color = Color.cyan;
            yield return new WaitForSecondsRealtime(5);
            if (!freezeTime)
            {
                player.GetComponent<PlayerController>().coins -= 3;
                freezeCount -= 1;
                DisplayFreezeCount();
                freezeTime = true;
            }
            timerText.color = Color.black;
        }
        
    }
}
