using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * This class deals with below logics:
 * 1. Player movement
 * 2. Updating text on player
 * 3. Collision detections and handling according to game plan.
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
    public int lives;

    float scale;
    float gridLength;
    List<Vector2Int> mazeWallGridList = new List<Vector2Int>();
    Vector2 playerCoordinates;

    AnalyticsManager analyticsManager;


    // Start is called before the first frame update
    void Start()
    {

        coins = 0;
        lives = 3;
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

        if (Input.GetKeyDown(KeyCode.M) && coins >= 3f && !dashboardController.isHelpButtonClicked())
        {
            analyticsManager.RegisterEvent(GameEvent.POWER_UP_USED, "shield");
            playerShield.SetActive(true);
            if (!isCoroutine)
            {
                isCoroutine = true;
                StartCoroutine(handleShield());
            }
        }

        UpdateIntagibleTimer();
    }

    private IEnumerator handleShield()
    {
        coins -= 3;
        yield return new WaitForSeconds(5.0f);
        playerShield.SetActive(false);
        isCoroutine = false;

        foreach (Collider2D col in collist)
        {
            Physics2D.IgnoreCollision(col, GetComponent<CircleCollider2D>(), false);
        }
        collist.Clear();
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
            if (playerShield.activeInHierarchy)
            {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<PolygonCollider2D>(), GetComponent<CircleCollider2D>());
                collist.Add(collision.gameObject.GetComponent<PolygonCollider2D>());
            }
            else
            {
                Debug.Log("HIT TOP");
                analyticsManager.RegisterEvent(GameEvent.COLLISION, "spike");
                lives--;
                if(lives == 0) {
                    PublishGameData(false, "obstacle");
                    SceneManager.LoadScene("GameOver");
                } else
                {
                    dashboardController.removeHealth(lives);
                }
            }
        }
        else if (collision.gameObject.CompareTag("SpikeBottom"))
        {
            Debug.Log("HIT BOTTOM");
            if (playerShield.activeInHierarchy)
            {
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
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<CircleCollider2D>());
            collist.Add(collision.gameObject.GetComponent<Collider2D>());

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
        // Once reach target, get running time and send score to GameOverWon scene
        if (collision.gameObject.CompareTag("target"))
        {
            if (targetScore == score)
            {
                int runningTime = Mathf.FloorToInt(dashboardController.GetRunningTime());
                GameOverWon.scoreTime = runningTime;
                PublishGameData(true, "won");
                SceneManager.LoadScene("GameOverWon");
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
            gameObject.GetComponent<Renderer>().material.color = new Color(251f / 255f, 140f / 255f, 69f / 255f);
        }
        if (score == 8)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255f / 255f, 255f / 255f, 153f / 255f);
        }
        if (score == 16)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(115f / 255f, 240f / 255f, 187f / 255f);
        }
        if (score == 32)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(196f / 255f, 245f / 255f, 243f / 255f);
        }
        if (score == 64)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(102f / 255f, 206f / 255f, 245f / 255f);
        }
        if (score == 128)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(219f / 255f, 196f / 255f, 245f / 255f);
        }
        if (score == 256)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(185f / 255f, 182f / 255f, 185f / 255f);
        }
        if (score == 512)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(245f / 255f, 224f / 255f, 46f / 255f);
        }
        if (score == 1024)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(245f / 255f, 120f / 255f, 219f / 255f);
        }
        if (score == 2048)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(250f / 255f, 206f / 255f, 133f / 255f);
        }
        if (score == 4096)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255f / 255f, 167f / 255f, 138f / 255f);
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
                // Walk Thru Wall Ends
                isIntangible = false;
                foreach (Collider2D col in collist)
                {
                    Physics2D.IgnoreCollision(col, GetComponent<CircleCollider2D>(), false);
                }
                collist.Clear();
                CancelInvoke("Flash");
                GetComponent<SpriteRenderer>().color = defaultColor;

                // get location, check location if is tile, re-spawn player if stuck
                Vector3 playerPos = transform.position;
                Vector2Int playerGrid = GetGridPosition(playerPos);
                int gridL = (int)gridLength;
                // correct playerGrid
                if (playerGrid[0] == 0) playerGrid[0] = 1;
                if (playerGrid[1] == 0) playerGrid[1] = 1;
                if (playerGrid[0] == gridL - 1) playerGrid[0] = gridL - 2;
                if (playerGrid[1] == gridL - 1) playerGrid[1] = gridL - 2;

                if (mazeWallGridList.Contains(playerGrid))
                {
                    bool free = freeThePlayer(playerGrid, 1);
                    if (!free) free = freeThePlayer(playerGrid, 2);
                    if(!free)
                    {
                        // reset
                        transform.position = GetCameraCoordinates((int)playerCoordinates[0], (int)playerCoordinates[1]);
                    }
                }
            }
        }
    }

    bool freeThePlayer(Vector2Int playerGrid, int i)
    {
        bool free = false;
        //check up and down, left and right first
        // up
        int x = playerGrid.x - i;
        int y = playerGrid.y;
        if (!free)
        {
            free = freeThePlayer(x, y);
        }
        // down
        x = playerGrid.x + i;
        y = playerGrid.y;
        if (!free)
        {
            free = freeThePlayer(x, y);
        }
        // left
        x = playerGrid.x;
        y = playerGrid.y - i;
        if (!free)
        {
            free = freeThePlayer(x, y);
        }
        // right
        x = playerGrid.x;
        y = playerGrid.y + i;
        if (!free)
        {
            free = freeThePlayer(x, y);
        }
        // top-left
        x = playerGrid.x - i;
        y = playerGrid.y - i;
        if (!free)
        {
            free = freeThePlayer(x, y);
        }
        // top-right
        x = playerGrid.x - i;
        y = playerGrid.y + i;
        if (!free)
        {
            free = freeThePlayer(x, y);
        }
        // btm-left
        x = playerGrid.x + i;
        y = playerGrid.y - i;
        if (!free)
        {
            free = freeThePlayer(x, y);
        }
        // btm-right
        x = playerGrid.x + i;
        y = playerGrid.y + i;
        if (!free)
        {
            free = freeThePlayer(x, y);
        }
        return free;
    }

    bool freeThePlayer(int x, int y)
    {
        if (x > 0 && y > 0 && x < gridLength-1&& y < gridLength-1)
        {
            if (!mazeWallGridList.Contains(new Vector2Int(x, y)))
            {
                transform.position = GetCameraCoordinates(x, y);
                return true;
            }
        }

        return false;
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
        if (won)
        {
            analyticsManager.RegisterEvent(GameEvent.PLAYER_WON, dashboardController.GetRemainingTime());
        }
        else
        {
            analyticsManager.RegisterEvent(GameEvent.PLAYER_LOST, (int)score);
        }
        analyticsManager.RegisterEvent(GameEvent.EXIT_REASON, reason);
        analyticsManager.RegisterEvent(GameEvent.TIME_SPENT, dashboardController.GetRemainingTime());
        analyticsManager.RegisterEvent(GameEvent.COINS_REMAINING, coins);
        analyticsManager.Publish();
    }


    Vector2Int GetGridPosition(Vector3 pos)
    {
        int y = (int)(pos[0] / scale + (gridLength + 1) / 2);
        int x = (int)(-pos[1] / scale + (gridLength + 1) / 2);

        return new Vector2Int(x, y);
    }

    Vector2 GetCameraCoordinates(int x, int y)
    {
        float cartesianX = ((y + 1) - (gridLength + 1) / 2) * scale;
        float cartesianY = (-(x + 1) + (gridLength + 1) / 2) * scale;
        return new Vector3(cartesianX, cartesianY);
    }

    public void setScale(float s) 
    {
        this.scale = s;
    }

    public void setGridLength(float g)
    {
        this.gridLength = g;
    }

    public void setMazeWallList(List<Vector2Int> list)
    {
        this.mazeWallGridList = list;
    }

    public void setPlayerCoordinates(Vector2 pc)
    {
        this.playerCoordinates = pc;
    }
}
