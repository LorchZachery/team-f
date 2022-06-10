using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * This class deals with below logics:
 * 1. Player movement
 * 2. Updating text on player
 * 3. Collision detections and handling accourding to game plan.
 */

public class PlayerController : MonoBehaviour
{
    public int score;
    float x;
    float y;
    int ballSpeed = 7;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateText(this.score.ToString());
    }

    // Update is called once per frame

    void Update()
    {
        Vector3 v = Camera.main.transform.eulerAngles;

        /*
         * 0 dont change
         * 
         */

       
       
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        int isDiagonal = x * y != 0 ? 0 : 1;

        if(v.z == 270)
        {
            /* Convert coordinates accoordint to camera
             * -1  0 =>  0  1
             *  1  0 =>  0 -1
             *  0  1 =>  1  0
             *  = -1 => -1  0
             */ 
            if(x != 0f || y != 0f)
            {
                //float tempX = x;
                //float tempY = y;
                //x = tempY;
                //y = -tempX;

                Vector2 dir1 = Camera.main.transform.right * x;
                Vector2 dir2 = Camera.main.transform.up * y;
                x = (dir1 + dir2).x;
                y = (dir1 + dir2).y;
            }
        }
        else if(v.z == 180)
        {
            if (x != 0f || y != 0f)
            {
                Debug.Log("before x = " + x + " y = " + y);
                //x = -1 * x;
                //y = -1 * y;
                Vector2 dir1 = Camera.main.transform.right * x;
                Vector2 dir2 = Camera.main.transform.up * y;
                x = (dir1 + dir2).x;
                y = (dir1 + dir2).y;
            }
            
        }
        else if(v.z == 90)
        {
            if (x != 0f || y != 0f)
            {
                Debug.Log("before x = " + x + " y = " + y);
            }
            /* Convert coordinates accoordint to camera
             * -1  0 =>  0 -1
             *  1  0 =>  0  1
             *  0  1 => -1  0
             *  = -1 =>  1  0
             */
            if (x != 0f || y != 0f)
            {
                //float tempX = x;
                //float tempY = y;
                //x = -tempY;
                //y = tempX;
                Vector2 dir1 = Camera.main.transform.right * x;
                Vector2 dir2 = Camera.main.transform.up * y;
                x = (dir1 + dir2).x;
                y = (dir1 + dir2).y;
            }
            if (x != 0f || y != 0f)
            {
                Debug.Log("after x = " + x + " y = " + y);
            }
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(x * ballSpeed * isDiagonal, y * ballSpeed * isDiagonal);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         * If Colliding object is block, and has same points as player,
         * then add the points to player's score, destroy the block, and update the text.
         */
        if (collision.gameObject.CompareTag("block"))
        {
            var script = collision.gameObject.GetComponent<BlockController>();
            if (score == script.points)
            {
                score += script.points;
                Destroy(collision.gameObject);
                UpdateText(this.score.ToString());
            }
        }
        else if (collision.gameObject.CompareTag("spikeObstacle"))
        {
            SceneManager.LoadScene("GameOver");
        }
    }



    public void SetScore(int score)
    {
        this.score = score;
        UpdateText(this.score.ToString());
        Debug.Log("Score updated");
    }

    void UpdateText(string message)
    {
        scoreText.text = message;
    }

    public void NotifyPlayerWin()
    {
        this.score = 2;
        UpdateText("Player won");
    }
}
