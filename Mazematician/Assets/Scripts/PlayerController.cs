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
    private bool isIntangible = false;
    private int intangibleTime = 5;
    private int intangibleTimer = 0;
    private List<Collider2D> collist;
    Color defaultColor;


    // Start is called before the first frame update
    void Start()
    {
        UpdateText(this.score.ToString());
        collist = new List<Collider2D>();
        defaultColor = GetComponent<SpriteRenderer>().color;
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

        UpdateIntagibleTimer();
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
        else if (collision.gameObject.CompareTag("tile") && isIntangible)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
            collist.Add(collision.gameObject.GetComponent<BoxCollider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("powerUpWalkThru")) 
        {
            Destroy(collision.gameObject);
            isIntangible = true;
            intangibleTimer = intangibleTime * 60;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            InvokeRepeating("Flash", intangibleTime - 2, 0.2f);
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
    }

    public void NotifyPlayerWin()
    {
        this.score = 2;
        UpdateText("Player won");
    }

    void UpdateIntagibleTimer()
    {
        if (intangibleTimer > 0)
        {
            intangibleTimer--;
            if (intangibleTimer == 0)
            {
                isIntangible = false;
                foreach (Collider2D col in collist)
                {
                    Physics2D.IgnoreCollision(col, GetComponent<CircleCollider2D>(), false);
                }
                collist.Clear();
                CancelInvoke("Flash");
                GetComponent<SpriteRenderer>().color = defaultColor;
            }
        }
    }

    void Flash()
    {
        if (GetComponent<SpriteRenderer>().color.Compare(new Color(1, 1, 1)))
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        } else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
    }
}
