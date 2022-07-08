using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEditor;
using System.IO;



static class OConst
{
    public const int normObj = 0;
    public const int wallkThru = 1;
    public const int spike = 2;
    public const int coin = 3;
    public const int oneway = 4;
    public const int breakableTile = 5;
    public const int spikeTwo = 6;


    public const int TRIANGLE_NW = 7;
    public const int TRIANGLE_NE = 8;
    public const int TRIANGLE_SW = 9;
    public const int TRIANGLE_SE = 10;

}

/**
 * This class deals with the logic of generating grid from prefabs
 * */


public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update
    float screenWidth;
    float screenHeight;

    public float gridLength;
    float scale;

    public List<MazeWall> mazeWallsList = new List<MazeWall>();

    public System.Random random = new System.Random();

    public GameObject tile;
    public GameObject player;
    public GameObject block;
    public GameObject obstacle;
    public GameObject winBlock;
    public GameObject myCamera;
    public GameObject spikeObstacle;
    public GameObject spikeObstacleTwoWide;
    public GameObject coin;
    public GameObject powerUpWalkThru;
    public GameObject breakableWall;
    public GameObject oneWayDoorSet;
    public GameObject breakableWallHint;
    public GameObject shieldHint;
    public GameObject shrinkHint;
    public GameObject walkThroughWallHint;
    public GameObject timeHint;
    public GameObject gravityHint;

    public GameObject triangle_nw;
    public GameObject triangle_ne;
    public GameObject triangle_sw;
    public GameObject triangle_se;



    public Coroutine rountine;

    public int target = 32;
    public Generator generator;

    public Vector2 playerCoordinates;
    public Vector2 winBlockCoor;

    //TODO work for reset of map (blocklist, mazeWallList, winblockcorr)
    public List<Vector3> blockList = new List<Vector3>();
    public List<Vector4> objectList = new List<Vector4>();

    public List<Vector2Int> mazeWallGridList = new List<Vector2Int>();

    private FileClass fileObject = new FileClass();


    private float rotation;


    public GameObject warningPrefab;
    private GameObject warning;
    public GameObject gravityText;
    private GameObject p_gravityText;


    public string LevelName;

    AnalyticsManager analyticsManager;

    //adds win block script to winblock
    //calculates to see if the player is at the target
    void Awake()
    {
        LevelName = LevelsController.LevelName;
        analyticsManager = AnalyticsManager.GetAnalyticsManager();
        analyticsManager.Reset(LevelsController.LevelNumber.ToString());
    }

    //Maze Generation, player, blocks and obsticle placement
    void Start()

    {


        // Instantiate warning red flash creation to alert user to gravity switch
        warning = Instantiate(warningPrefab, new Vector2(Screen.width, Screen.height), Quaternion.identity);
        warning.gameObject.SetActive(false);
        p_gravityText = Instantiate(gravityText);
        p_gravityText.gameObject.SetActive(false);

        TextAsset levelFile = Resources.Load<TextAsset>("Levels/" + LevelName);



        string fileData = levelFile.text;
        fileData = fileData.Replace("\r", "");
        string[] levelData = fileData.Split("\n");
        fileObject.ReadTextAsset(levelData);
        setFileClassVars(fileObject);



        screenHeight = Camera.main.orthographicSize * 2;
        scale = Mathf.Min(screenWidth, screenHeight) / gridLength;

        GenerateWalls();
        foreach (var wall in mazeWallsList)
        {

            
            if (wall.isWall())
            {
                mazeWallGridList.Add(new Vector2Int(wall.x, wall.y));
                GenerateTile(wall.x, wall.y);
            }


        }

        DrawGridLines();



        //creating player
        GeneratePlayer(playerCoordinates);

        // Commenting this code because win block gets generated only when target score is reached
        if (winBlockCoor != new Vector2(0, 0))
        {
            PlaceWinBlock((int)winBlockCoor[0], (int)winBlockCoor[1], target);
        }




        if (blockList.Count != 0)
        {
            foreach (Vector3 block in blockList)
            {
                GenerateBlock((int)block[0], (int)block[1], (int)block[2]);
            }
        }


        //placing object (powerups spikes...)
        if (objectList.Count != 0)
        {
            foreach (Vector4 obj in objectList)
            {
                if (obj[3] == OConst.normObj)
                {
                    PlaceObstacle((int)obj[0], (int)obj[1], obj[2]);
                }
                if (obj[3] == OConst.spike)
                {
                    PlaceSpikeObstacle((int)obj[0], (int)obj[1]);
                }
                if (obj[3] == OConst.wallkThru)
                {
                    PlacePowerUpWalkThru((int)obj[0], (int)obj[1]);
                }
                if (obj[3] == OConst.coin)
                {
                    GenerateCoin((int)obj[0], (int)obj[1]);
                }
                if (obj[3] == OConst.oneway)
                {
                    PlaceOneWayDoor((int)obj[0], (int)obj[1], (int)obj[2]);
                }

                if (obj[3] == OConst.spikeTwo)
                {
                    PlaceSpikeObstacleTwoWide((int)obj[0], (int)obj[1]);

                }

                // Corners
                if (obj[3] == OConst.TRIANGLE_NW)
                {
                    PlaceCornerNW((int)obj[0], (int)obj[1]);

                }
                if (obj[3] == OConst.TRIANGLE_NE)
                {
                    PlaceCornerNE((int)obj[0], (int)obj[1]);

                }
                if (obj[3] == OConst.TRIANGLE_SW)
                {
                    PlaceCornerSW((int)obj[0], (int)obj[1]);

                }
                if (obj[3] == OConst.TRIANGLE_SE)
                {
                    PlaceCornerSE((int)obj[0], (int)obj[1]);

                }
                if (obj[3] == OConst.breakableTile)
                {
                    PlaceBreakableWall((int)obj[0], (int)obj[1]);
                }

            }
        }

        //giving gavity to objects
        ApplyGravity(GameObject.FindGameObjectsWithTag("block"));

        //invoking gravity to switch every 7 seconds, with a red screen flash before
        if (LevelName != "ag_tutorial" && LevelName != "Tutorial_2")
        {
            InvokeRepeating("rotateGameRoutine", 7.0f, 7.0f);
        }
        if (LevelName == "breakable_tile_tutorial")
        {

            PlaceBreakableWallHint(14, 16);
            //PlaceGravityHint(15, 10);

        }
        if (LevelName == "Tutorial_2")
        {

            PlaceShieldHint(15, 12);
            PlaceShrinkHint(10, 2);
            PlaceWalkThroughHint(11, 7);
            PlaceTimeHint(14, 8);
        }

        InitAnalyticsData();
    }


    void InitAnalyticsData()
    {
        int totalCoins = 0;
        foreach (var obj in objectList)
        {
            if (obj[3] == OConst.coin)
            {
                totalCoins++;
            }
        }
        analyticsManager.RegisterEvent(GameEvent.COINS_TOTAL, totalCoins);
    }
    // Update is called once per frame
    //on update there is a create to rotate the screen slowly
    void Update()
    {
        if (rotation > 0)
        {
            float currentRotation = Time.deltaTime * 90 / 1.5f;
            currentRotation = Math.Min(currentRotation, rotation);

            rotation -= currentRotation;
            RotateGame(currentRotation);

        }

    }



    void GenerateWalls()
    {
        for (int i = 0; i < gridLength; i++)
        {
            //top : x = 0, y = i
            GenerateOuterTile(0, i);

            //bottom: x = 9, y = i
            GenerateOuterTile((int)gridLength - 1, i);
        }

        for (int i = 1; i < gridLength - 1; i++)
        {
            //left x = i, y = 0
            GenerateOuterTile(i, 0);
            //right x = i, y = 9
            GenerateOuterTile(i, (int)gridLength - 1);
        }
    }

    /*
    * Generate grid based on coordinates
    */
    void GenerateTile(int x, int y)
    {
        GameObject t = Instantiate(tile, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale, scale, 1);

    }

    void GeneratePlayer(Vector2 coordinates)
    {
        GameObject t = Instantiate(player, GetCameraCoordinates((int)coordinates[0], (int)coordinates[1]), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.9f, scale * 0.9f, 1);
        var script = t.GetComponent<PlayerController>();
        script.setTargetScore(target);
        script.SetScore(2);
        script.setGridManager(gameObject);

        // to free player if stuck
        script.setScale(scale);
        script.setGridLength(gridLength);
        script.setMazeWallList(mazeWallGridList);
        script.setPlayerCoordinates(playerCoordinates);

        var cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.SetPlayer(t);

        GameObject dashboard = GameObject.Find("Dashboard");
        var dashBoardController = dashboard.GetComponent<DashBoardController>();
        dashBoardController.SetPlayer(t);
        dashBoardController.SetTarget(this.target);
        script.setDashboardController(dashboard);
    }

    void GenerateOuterTile(int x, int y)
    {
        GameObject t = Instantiate(tile, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale, scale, 1);
        t.tag = "outerTile";

    }

    Vector2Int GetGridPosition(Vector3 pos)
    {
        int y = (int)(pos[0] / scale + (gridLength + 1) / 2 );
        int x = (int)(-pos[1] / scale + (gridLength + 1) / 2 );

        return new Vector2Int(x, y);
    }

    Vector2 GetCameraCoordinates(int x, int y)
    {
        float cartesianX = ((y + 1) - (gridLength + 1) / 2) * scale;
        float cartesianY = (-(x + 1) + (gridLength + 1) / 2) * scale;
        return new Vector3(cartesianX, cartesianY);
    }

    Vector2 GetCameraCoordinates(float x, float y)
    {
        float cartesianX = ((y + 1) - (gridLength + 1) / 2) * scale;
        float cartesianY = (-(x + 1) + (gridLength + 1) / 2) * scale;
        return new Vector3(cartesianX, cartesianY);
    }

    Vector2 GetCameraCoordinates(int x, int y, int z)
    {
        float cartesianX = ((y + 1) - (gridLength + 1) / 2) * scale;
        float cartesianY = (-(x + 1) + (gridLength + 1) / 2) * scale;
        return new Vector3(cartesianX + (0.5f * scale), cartesianY - (0.5f * scale), z);
    }

    void PlaceWinBlock(int x, int y, int value)
    {
        GameObject t = Instantiate(winBlock, GetCameraCoordinates(x, y), Quaternion.identity);
        GameObject textChild = t.transform.GetChild(0).gameObject;
        TMP_Text textOf = textChild.GetComponent<TextMeshPro>();
        textOf.text = value.ToString();
        t.transform.localScale = new Vector3(scale, scale, 1);
    }



    void GenerateBlock(int x, int y, int points)
    {
        GameObject t = Instantiate(block, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.9f, scale * 0.9f, 1);

        var script = t.GetComponent<BlockController>();
        script.SetPoints(points);
    }

    void GenerateCoin(int x, int y)
    {
        GameObject t = Instantiate(coin, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.7f, scale * 0.7f, 1);
    }

    void RotateGame(float angle)
    {
        Camera.main.transform.Rotate(0, 0, angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("block"), angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("obstacle"), angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("player"), angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("target"), angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("coin"), angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("powerUpWalkThru"), angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("breakableWallHint"), angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("gravityHint"), angle);
        ApplyGravity(GameObject.FindGameObjectsWithTag("block"));
    }

    void TransformGameObjects(GameObject[] gameObjects, float z)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            Vector3 currentTransform = gameObject.transform.eulerAngles;
            //gameObject.transform.Rotate(myCamera.transform.up, 0, Space.World);
            Vector3 rotationVector = new Vector3(0, 0, currentTransform.z + z);
            gameObject.transform.rotation = Quaternion.Euler(rotationVector); ;
        }
    }

    void ApplyGravity(GameObject[] gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            ConstantForce2D constantForce = gameObject.GetComponent<ConstantForce2D>();
            Vector2 direction = Camera.main.transform.up * -1;
            constantForce.force = direction * 50f;
        }
        //Debug.Log(gameObjects[0].transform.x);
    }

    //function that is called every 7 seconds that then starts a screen flash co routine
    void rotateGameRoutine()
    {

        rountine = StartCoroutine(flash());

    }
    IEnumerator flash()
    {

        warning.gameObject.SetActive(true);
        // Gravity text for a tutorial level
        if (LevelName == "breakable_tile_tutorial")
        {
            p_gravityText.gameObject.SetActive(true);
        }
        var whenAreweDone = Time.time + 3;
        while (Time.time < whenAreweDone)
        {
            yield return new WaitForSeconds(0.5f);
            warning.gameObject.SetActive(!warning.gameObject.activeSelf);
            if (LevelName == "breakable_tile_tutorial")
            {
                p_gravityText.gameObject.SetActive(!p_gravityText.gameObject.activeSelf);
            }

        }
        warning.gameObject.SetActive(false);
        p_gravityText.gameObject.SetActive(false);
        rotation = 90.0f;
    }

    void DrawGridLines()
    {
        for (int i = 0; i < gridLength - 1; i++)
        {
            Debug.DrawLine(GetCameraCoordinates(i, 0, 2), GetCameraCoordinates(i, (int)gridLength - 2, 2), Color.green, 1000f);
            Debug.DrawLine(GetCameraCoordinates(0, i, 2), GetCameraCoordinates((int)gridLength - 2, i, 2), Color.green, 1000f);
        }

    }

    void PlaceObstacle(int x, int y, float penalty)
    {
        GameObject t = Instantiate(obstacle, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale, scale, 1);

        var script = t.GetComponent<ObstacleController>();
        script.SetPenalty(penalty);
    }

    void PlaceSpikeObstacle(int x, int y)
    {
        GameObject t = Instantiate(spikeObstacle, GetCameraCoordinates(x, y), Quaternion.identity);
        // t.transform.localScale = new Vector3(scale * 0.30f, scale * 0.30f, 1);
    }
    void PlaceSpikeObstacleTwoWide(int x, int y)
    {
        GameObject t = Instantiate(spikeObstacleTwoWide, GetCameraCoordinates(x, y), Quaternion.identity);
        // t.transform.localScale = new Vector3(scale * 0.30f, scale * 0.30f, 1);
    }
    // Corners
    void PlaceCornerNW(int x, int y)
    {
        GameObject t = Instantiate(triangle_nw, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.50f, scale * 0.50f, 1);

    }
    void PlaceCornerNE(int x, int y)
    {
        GameObject t = Instantiate(triangle_ne, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.50f, scale * 0.50f, 1);

    }
    void PlaceCornerSW(int x, int y)
    {
        GameObject t = Instantiate(triangle_sw, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.50f, scale * 0.50f, 1);

    }
    void PlaceCornerSE(int x, int y)
    {
        GameObject t = Instantiate(triangle_se, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.50f, scale * 0.50f, 1);

    }


    void PlaceBreakableWall(int x, int y)
    {
        GameObject t = Instantiate(breakableWall, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 2f, scale * 2f, 1);
    }
    void PlaceBreakableWallHint(int x, int y)
    {
        GameObject t = Instantiate(breakableWallHint, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 1.2f, scale * 1.2f, 1);
    }
    void PlaceGravityHint(int x, int y)
    {
        GameObject t = Instantiate(gravityHint, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 1.2f, scale * 1.2f, 1);
    }

    void PlaceShieldHint(int x, int y)
    {
        GameObject t = Instantiate(shieldHint, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 1.2f, scale * 1.2f, 1);
    }
    void PlaceShrinkHint(int x, int y)
    {
        GameObject t = Instantiate(shrinkHint, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 1.2f, scale * 1.2f, 1);
    }
    void PlaceWalkThroughHint(int x, int y)
    {
        GameObject t = Instantiate(walkThroughWallHint, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 1.2f, scale * 1.2f, 1);
    }
    void PlaceTimeHint(int x, int y)
    {
        GameObject t = Instantiate(timeHint, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 1.2f, scale * 1.2f, 1);
    }


    void PlacePowerUpWalkThru(int x, int y)
    {
        GameObject t = Instantiate(powerUpWalkThru, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale, scale, 1);

    }

    // dir (1:UP, 2:DOWN, 3:LEFT, 4:RIGHT)
    void PlaceOneWayDoor(int x, int y, int dir)
    {
        GameObject oneway = Instantiate(oneWayDoorSet, GetCameraCoordinates(x, y), Quaternion.identity);
        oneway.transform.localScale = new Vector3(scale * 3, scale, 1);
        switch (dir)
        {
            case 2:
                oneway.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180f));
                break;
            case 3:
                oneway.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                break;
            case 4:
                oneway.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 270f));
                break;
        }

    }

    void setFileClassVars(FileClass file)
    {
        screenHeight = file.screenHeight;
        screenWidth = file.screenWidth;
        gridLength = file.gridLength;
        playerCoordinates = file.playerCooridantes;
        winBlockCoor = file.winBlockCoor;
        target = file.target;
        mazeWallsList = new List<MazeWall>(file.mazeWallsList);
        blockList = file.blockList;
        objectList = file.objectList;
    }

}



