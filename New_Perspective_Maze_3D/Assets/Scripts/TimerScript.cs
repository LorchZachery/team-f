using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    public float timeRemaining = 10;
    public float endPointTime = 60;
    public bool timerIsRunning = false;
    public TextMeshPro timeText;
    [SerializeField]
    public GameObject player;
    public GameObject go1, go2;
    public TextMeshPro tmpro1, tmpro2;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }
    void Update()
    {
        go1 = player.transform.GetChild(0).gameObject;
        tmpro1 = go1.GetComponent<TextMeshPro>();
        go2 = player.transform.GetChild(1).gameObject;
        tmpro2 = go2.GetComponent<TextMeshPro>();
        Debug.Log("Calling from timer: value of player: " + tmpro1.text);
        
        if (timerIsRunning)
        {
            
            if (timeRemaining > 0)
            {
                if (timeRemaining >= 0 && tmpro1.text == "16")
                {
                    Debug.Log("Calling from Timer!!!");
                    DisplayTime(timeRemaining);
                    GameObject textGameObject = GameObject.FindGameObjectWithTag("GuideText");
                    TextMeshPro textField = textGameObject.GetComponent<TextMeshPro>();
                    textField.text = "Great! Now, reach the exit :)";
                    //timeRemaining -= Time.deltaTime;

                    Debug.Log(player.transform.position);
                    Vector3 lhs = player.transform.position;
                    Vector3 rhs = new Vector3(-5.0f, 0.0f, 4.0f);
                    Vector3 rhs2 = new Vector3(5.0f, 0.0f, -4.0f);
                    Vector3 rhs3 = new Vector3(-5.0f, 0.0f, -4.0f);
                    Vector3 rhs4 = new Vector3(5.0f, 0.0f, 4.0f);
                    Vector3 rhs5 = new Vector3(-4.0f, 0.0f, -5.0f);
                    Vector3 rhs6 = new Vector3(4.0f, 0.0f, 5.0f);
                    Vector3 rhs7 = new Vector3(4.0f, 0.0f, -5.0f);
                    Vector3 rhs8 = new Vector3(-4.0f, 0.0f, 5.0f);
                    //Vector3 rhs5 = new Vector3(1.0f, 0.0f, 0.0f);
                    //Vector3 rhs6 = new Vector3(-1.0f, 0.0f, 0.0f);
                    //Vector3 rhs7 = new Vector3(0.0f, 0.0f, 1.0f);
                    //Vector3 rhs8 = new Vector3(0.0f, 0.0f, -1.0f);
                    //Vector3 rhs9 = new Vector3(1.0f, 0.0f, 1.0f);
                    //Vector3 rhs10 = new Vector3(1.0f, 0.0f, -1.0f);
                    //Vector3 rhs11 = new Vector3(-1.0f, 0.0f, 1.0f);
                    //Vector3 rhs12 = new Vector3(-1.0f, 0.0f, -1.0f);
                    //Vector3 rhs13 = new Vector3(0.0f, 0.0f, 0.0f);



                    Debug.Log("1: " + Vector3.SqrMagnitude(lhs - rhs));
                    Debug.Log("2: " + Vector3.SqrMagnitude(lhs - rhs2));
                    Debug.Log("3: " + Vector3.SqrMagnitude(lhs - rhs3));
                    Debug.Log("4: " + Vector3.SqrMagnitude(lhs - rhs4));
                    if (Vector3.SqrMagnitude(lhs - rhs) < 1.4f || Vector3.SqrMagnitude(lhs - rhs2) < 1.4f
                        || Vector3.SqrMagnitude(lhs - rhs3) < 1.4f || Vector3.SqrMagnitude(lhs - rhs4) < 1.4f
                        || Vector3.SqrMagnitude(lhs - rhs5) < 1.4f || Vector3.SqrMagnitude(lhs - rhs6) < 1.4f
                        || Vector3.SqrMagnitude(lhs - rhs7) < 1.4f || Vector3.SqrMagnitude(lhs - rhs8) < 1.4f)
                        //|| Vector3.SqrMagnitude(lhs - rhs5) < 1.4f || Vector3.SqrMagnitude(lhs - rhs6) < 1.4f
                        //|| Vector3.SqrMagnitude(lhs - rhs7) < 1.4f || Vector3.SqrMagnitude(lhs - rhs8) < 1.4f
                        //|| Vector3.SqrMagnitude(lhs - rhs9) < 1.4f || Vector3.SqrMagnitude(lhs - rhs10) < 1.4f
                        //|| Vector3.SqrMagnitude(lhs - rhs11) < 1.4f || Vector3.SqrMagnitude(lhs - rhs12) < 1.4f
                        //|| Vector3.SqrMagnitude(lhs - rhs13) < 1.4f )
                    {
                        Debug.Log("Reached Destination!");
                        SceneManager.LoadSceneAsync(2);

                    }
                }
                else
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                }
                
            }
            
            else 
            {
                Debug.Log("Time has run out!");
                Debug.Log("Active Scene : " + SceneManager.GetActiveScene().name);
                SceneManager.LoadSceneAsync(3);
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
