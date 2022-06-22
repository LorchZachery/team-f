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
    private bool isIntangible = false;
    private float intangibleTime = 5;
    private float intangibleTimer = 0;
    private List<Collider2D> collist;
    Color defaultColor;
    public GameObject playerShield;
    private bool isCoroutine = false;

    AnalyticsManager analyticsManager;


    // Start is called before the first frame update
    void Start()
    {
        coins = 0;
        UpdateText(this.score.ToString());
        collist = new List<Collider2D>();
        defaultColor = GetComponent<SpriteRenderer>().color;
        analyticsManager = AnalyticsManager.GetAnalyticsManager();
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

        if (Input.GetKeyDown(KeyCode.P) && coins > 0f) {
            playerShield.SetActive(true);
            if (!isCoroutine) {
                isCoroutine = true;
                StartCoroutine(handleShield());
            }
        }

        UpdateIntagibleTimer();
    }

    private IEnumerator handleShield()
    {
        coins--;
        yield return new WaitForSeconds(5.0f);
        playerShield.SetActive(false);
        isCoroutine = false;
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
        else if (collision.gameObject.CompareTag("SpikeTop"))
        {

            if (playerShield.activeInHierarchy) {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<PolygonCollider2D>(), GetComponent<CircleCollider2D>());
                collist.Add(collision.gameObject.GetComponent<PolygonCollider2D>());
            }
            else {
                Debug.Log("HIT TOP");
                PublishGameData(false, "spike");
                SceneManager.LoadScene("GameOver");
            }
        }
        else if (collision.gameObject.CompareTag("SpikeBottom"))
        {
            Debug.Log("HIT BOTTOM");
            if (playerShield.activeInHierarchy) {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<PolygonCollider2D>(), GetComponent<CircleCollider2D>());
                collist.Add(collision.gameObject.GetComponent<PolygonCollider2D>());
            }
        }
        if (collision.gameObject.CompareTag("coin"))
        {
            coins++;
            analyticsManager.RegisterEvent(GameEvent.COINS_COLLECTED, null);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("tile") && isIntangible)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
            collist.Add(collision.gameObject.GetComponent<BoxCollider2D>());

        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("powerUpWalkThru"))
        {
            analyticsManager.RegisterEvent(GameEvent.POWER_UP_USED, collision.gameObject.tag);
            Destroy(collision.gameObject);
            isIntangible = true;
            intangibleTimer = intangibleTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            InvokeRepeating("Flash", intangibleTime - 2, 0.2f);
        }
        
        if (collision.gameObject.CompareTag("target"))
        {
            if (targetScore == score)
            {
                PublishGameData(true, "won");
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    public void SetScore(int score)
    {
        this.score = score;
        UpdateText(this.score.ToString());
        //Debug.Log("Score updated");


        if (this.score < 2)
        {
            PublishGameData(false, "obstacle");
            SceneManager.LoadScene("GameOver");
        }
        else if (this.score == this.targetScore)
        {
            //grid.AddWinBlock(this.targetScore);
            //dashboardController.SetTime(20f);
        }
    }

    public void setTargetScore(int score)
    {
        this.targetScore = score;
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
    void UpdateIntagibleTimer()
    {
        if (intangibleTimer > 0)
        {
            intangibleTimer -= Time.deltaTime;
            if (intangibleTimer <= 0)
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
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
    }

    public void PublishGameData(bool won, string reason)
    {
        if(won)
        {
            analyticsManager.RegisterEvent(GameEvent.PLAYER_WON, dashboardController.GetRemainingTime());
        } else
        {
            analyticsManager.RegisterEvent(GameEvent.PLAYER_LOST, (int) score);
        }
        analyticsManager.RegisterEvent(GameEvent.EXIT_REASON, reason);
        analyticsManager.RegisterEvent(GameEvent.TIME_SPENT, dashboardController.GetRemainingTime());
        analyticsManager.RegisterEvent(GameEvent.COINS_SPENT, coins);
        analyticsManager.Publish();
    }

}
