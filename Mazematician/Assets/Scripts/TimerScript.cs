using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float time = 0;
    public Text windowText;
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        if (time < 62){
            time += Time.deltaTime;
            updateTimer(time);

        }
        else {
            windowText.text = "Times Up!";
        }
    }
    void updateTimer(float currentTime)
    {
        // if(currentTime < 0){
        //     currentTime = 0;
        // }
        //currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        windowText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
