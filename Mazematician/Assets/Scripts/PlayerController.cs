using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * This class deals with below logics:
 * 1. Player movement
 * 2. Updating text on player
 * 3. Collisioㄋn detections and handling accourding to game plan.
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
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        int isDiagonal = x * y != 0 ? 0 : 1;

        Vector2 dir1 = Camera.main.transform.right * x;
        Vector2 dir2 = Camera.main.transform.up * y;
        x = (dir1 + dir2).x;
        y = (dir1 + dir2).y;
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
        //Debug.Log("Score updated");
        
    }

    void UpdateText(string message)
    {
        scoreText.text = message;
        UpdateColor();
    }
    void UpdateColor()
    {
        if (score == 2)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255f / 255f, 153f / 255f, 153f / 255f);
        }
        if (score == 4)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255f / 255f, 204f / 255f, 153f / 255f);
        }
        if (score == 8)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255f / 255f, 255f / 255f, 153f / 255f);
        }
        if (score == 16)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(204f / 255f, 255f / 255f, 153f / 255f);
        }
        if (score == 32)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(153f / 255f, 255f / 255f, 255f / 255f);
        }
        if (score == 64)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(102f / 255f, 102f / 255f, 255f / 255f);
        }
        if (score == 128)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(204f / 255f, 153f / 255f, 255f / 255f);
        }
        if (score == 256)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(224f / 255f, 224f / 255f, 224f / 255f);
        }
    }

    public void NotifyPlayerWin()
    {
        this.score = 2;
        UpdateText("Player won");
    }
}
