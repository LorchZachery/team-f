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
    public int targetScore;
    GameObject gridManager;
    GridManager grid;
    GameObject dashboard;
    DashBoardController dashboardController;
    public int coins;
    float x;
    float y;
    int ballSpeed = 7;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        coins = 0;
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
            if (this.score == script.points)
            {
                Destroy(collision.gameObject);
                SetScore(this.score + script.points);
            }
        }
        else if (collision.gameObject.CompareTag("upperBound"))
        {
            Debug.Log("HIT TOP");
            SceneManager.LoadScene("GameOver");
        }
        else if (collision.gameObject.CompareTag("lowerBound"))
        {
            Debug.Log("HIT BOTTOM");
        }
        if (collision.gameObject.CompareTag("coin"))
        {
            coins++;
            Destroy(collision.gameObject);
        }
    }



    public void SetScore(int score)
    {
        this.score = score;
        UpdateText(this.score.ToString());
        //Debug.Log("Score updated");
        if (this.score < 2)
        {
            SceneManager.LoadScene("GameOver");
        }
        else if (this.score == this.targetScore)
        {
            grid.AddWinBlock(this.targetScore);
            dashboardController.SetTime(20f);
        }
    }

    public void setTargetScore(int score)
    {
        this.targetScore = score;
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

    public void setGridManager(GameObject gm)
    {
        this.gridManager = gm;
        grid = this.gridManager.GetComponent<GridManager>();
    }

    public void setDashboardController(GameObject dc)
    {
        this.dashboard = dc;
        dashboardController = this.dashboard.GetComponent<DashBoardController>();
    }
}
