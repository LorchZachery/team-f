using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float time = 120;
    public GameObject windowText;
    // Start is called before the first frame update
    //void Start()
    //{
    //    GameObject timerObject = Instantiate(windowText, new Vector3(379f, 17f, -10f), Quaternion.identity);
    //}

    // Update is called once per frame
    void Update()
    {
        if (time > 0){
            time -= Time.deltaTime;
            updateTimer(time);

        }
        else {
            //windowText.text = "Times Up!";
            SceneManager.LoadSceneAsync(2);
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
        Debug.Log("Timer Text: " + timerText.text);
        //Debug.Log("Timer Text: " + windowText.GetComponent<TextMeshProUGUI>().text);
    }
}
