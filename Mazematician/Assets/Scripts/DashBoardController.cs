using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DashBoardController : MonoBehaviour
{

    GameObject player;
    GameObject helpMenu;
    float remainingTime;
    bool timerRunning;
    int target;
    float flashTimer;
    float flashDuration = 1f;
    bool bonusTime = false;
    bool shrinkTime = false;
    int bonusCount = 0;
    AnalyticsManager analyticsManager;

    public TextMeshProUGUI rewardsText;
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        remainingTime = 60 * 2f;
        if(MainMenu.TutorialButtonClicked) {
            if(TutorialController.TutorialLevelName == "Tutorial_2") {
                remainingTime = 25 * 1f;
            }
        } else {
            remainingTime = 60 * 2f;
        }
        timerRunning = true;
        UpdateScore(0);
        DisplayTargetText();
        DisplayLevelText();
        helpMenu = GameObject.FindGameObjectWithTag("help");
        helpMenu.SetActive(false);
        analyticsManager = AnalyticsManager.GetAnalyticsManager();
        analyticsManager.RegisterEvent(GameEvent.TOTAL_TIME, remainingTime);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        DisplayPowerupIcons();
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
                //Debug.Log("Time remaining: " + remainingTime);
                remainingTime -= Time.deltaTime;

                if (player != null)
                {
                    if (player.GetComponent<PlayerController>().coins >= 3)
                    {
                        //The Bonus Time icon.
                        //Adds 10 seconds to the timer.
                        //Can be activated by pressing the "B" key
                        if (Input.GetKeyDown(KeyCode.B))
                        {
                            bonusTime = false;
                            remainingTime += 11;
                            analyticsManager.RegisterEvent(GameEvent.TOTAL_TIME, 11f);
                            analyticsManager.RegisterEvent(GameEvent.POWER_UP_USED, "bonusTime");
                            StartCoroutine("BonusTime");
                            if (!bonusTime)
                            {
                                player.GetComponent<PlayerController>().coins -= 3;
                                bonusTime = true;
                            }
                        }
                        //The Shrink Player icon.
                        //Shrinks the size of the player for 5 seconds.
                        //Can be activated by pressing the "B" key
                        else if (Input.GetKeyDown(KeyCode.N))
                        {
                            shrinkTime = false;
                            StartCoroutine("Shrink");
                            analyticsManager.RegisterEvent(GameEvent.POWER_UP_USED, "shrink");
                            if (!shrinkTime)
                            {
                                player.GetComponent<PlayerController>().coins -= 3;
                                shrinkTime = true;
                            }
                        }
                    }

                }
                DisplayTime(remainingTime);
                if (remainingTime < 3)
                {
                    Flash();
                }
                //if (player.GetComponent<PlayerController>().coins == 3)
                //{
                //    freezeCount = 1;
                //    StartCoroutine("Freeze");
                //    DisplayFreezeCount();
                //    remainingTime += 10 * Time.deltaTime;
                //}
                //else
                //{
                //    remainingTime -= Time.deltaTime;
                //    DisplayTime(remainingTime);
                //    if (remainingTime < 5)
                //    {
                //        Flash();
                //    }
                //}  
            }
            else
            {
                timerText.enabled = true;
                var script = player.GetComponent<PlayerController>();
                script.PublishGameData(false, "Out of Time");
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

    void DisplayBonusCount()
    {
        GameObject targetObject = gameObject.transform.GetChild(5).gameObject;
        TextMeshProUGUI targetText = targetObject.GetComponent<TextMeshProUGUI>();
        targetText.text = bonusCount.ToString();
    }

    void DisplayPowerupIcons()
    {
        GameObject bonusIconObject = gameObject.transform.GetChild(4).gameObject;
        GameObject shrinkIconObject = gameObject.transform.GetChild(5).gameObject;
        GameObject shieldObject = gameObject.transform.GetChild(6).gameObject;
        GameObject indicator1 = gameObject.transform.GetChild(7).gameObject;
        GameObject indicator2 = gameObject.transform.GetChild(8).gameObject;
        GameObject indicator3 = gameObject.transform.GetChild(9).gameObject;
        if (player != null)
        {
            if (player.GetComponent<PlayerController>().coins < 3)
            {
                bonusIconObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("no bonus time");
                shrinkIconObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("no resize");
                shieldObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("no shield");
                indicator1.SetActive(false);
                indicator2.SetActive(false);
                indicator3.SetActive(false);
            }
            else if (player.GetComponent<PlayerController>().coins >= 3)
            {
                bonusIconObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("bonus time");
                shrinkIconObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("shrink");
                shieldObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("shield 1");
                indicator1.SetActive(true);
                indicator2.SetActive(true);
                indicator3.SetActive(true);
            }
        }
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


    IEnumerator Shrink()
    {
        float myScale = 0.16f;
        Vector3 originalScale = player.transform.localScale;
        player.transform.localScale = new Vector3(myScale, myScale, 1.0f);
        Debug.Log("Local Scale after shrinking: " + player.transform.localScale);
        yield return new WaitForSeconds(5f);
        player.transform.localScale = originalScale;
        Debug.Log("Local Scale before shrinking: " + player.transform.localScale);
    }

    IEnumerator BonusTime()
    {
        timerText.color = Color.green;
        yield return new WaitForSeconds(10f);
        timerText.color = Color.white;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }
    //IEnumerator Freeze()
    //{
    //    {
    //        freezeTime = false;
    //        timerText.color = Color.cyan;
    //        yield return new WaitForSecondsRealtime(5);
    //        if (!freezeTime)
    //        {
    //            player.GetComponent<PlayerController>().coins -= 3;
    //            freezeCount -= 1;
    //            DisplayFreezeCount();
    //            freezeTime = true;
    //        }
    //        timerText.color = Color.black;
    //    }


    //}

    void DisplayLevelText()
    {
        GameObject[] levelObjectArray = GameObject.FindGameObjectsWithTag("levelText");
        GameObject levelObject = levelObjectArray[0];
        TextMeshProUGUI levelText = levelObject.GetComponent<TextMeshProUGUI>();
        if (MainMenu.PlayButtonClicked)
        {
            levelText.text = "Level: " + LevelsController.LevelNumber;
        }
        else
        {
            levelText.text = "Tutorial: " + TutorialController.TutorialLevelNumber;
        }
    }

    public void QuitButton()
    {
        UpdateAnalytics("quit");
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartButton()
    {
        UpdateAnalytics("restart");
        SceneManager.LoadScene("SampleGrid");
    }

    public void HelpButton()
    {
        helpMenu.SetActive(true);
    }

    public void QuitHelpButton()
    {
        helpMenu.SetActive(false);
    }


    private void UpdateAnalytics(string reason)
    {
        if (player != null)
        {
            var script = player.GetComponent<PlayerController>();
            script.PublishGameData(false, reason);
        }
        else
        {
            Debug.LogError("Some thing is wrong, player not found");
        }
    }
}
