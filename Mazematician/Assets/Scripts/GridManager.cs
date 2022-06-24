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

}

/**
 * This class deals with the logic of generating grid from prefabs
 * */


public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update
    float screenWidth;
    float screenHeight;

    float gridLength;
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
    public GameObject coin;
    public GameObject powerUpWalkThru;
    public GameObject breakableWall;
    public GameObject oneWayDoorSet;

    public int target = 32;
    public Generator generator;

    public Vector2 playerCoordinates;
    public Vector2 winBlockCoor;
    public List<Vector2> noGoCorr = new List<Vector2>();

    //TODO work for reset of map (blocklist, mazeWallList, winblockcorr)
    public List<Vector3> blockList = new List<Vector3>();
    public List<Vector4> objectList = new List<Vector4>();

    private FileClass fileObject = new FileClass();


    private float rotation;


    public GameObject warningPrefab;
    private GameObject warning;


    private bool read = false;
    public string LevelName;

    AnalyticsManager analyticsManager;

    //adds win block script to winblock
    //calculates to see if the player is at the target
    void Awake()
    {
        var script = winBlock.GetComponent<GameEndController>();
        script.targetScore = target;
        LevelName = LevelsController.LevelName;
        analyticsManager = AnalyticsManager.GetAnalyticsManager();
        analyticsManager.Reset(LevelsController.LevelName);
    }

    //Maze Generation, player, blocks and obsticle placement
    void Start()

    {


        // Instantiate warning red flash creation to alert user to gravity switch
        warning = Instantiate(warningPrefab, new Vector2(Screen.width, Screen.height), Quaternion.identity);
        warning.gameObject.SetActive(false);
        TextAsset levelFile = Resources.Load<TextAsset>("Levels/" + LevelName);
        Debug.Log(levelFile);

        //if (!File.Exists("Assets/Resources/Levels/" + LevelName + ".txt")) 
        if (levelFile == null)
        {
            //setting screen length and height and translating it to a camera scale
            screenWidth = 24;

            gridLength = 20; //10 + 2; // 8 x 8 grid + 1 top(left) wall + 1 bottom(right);
            /* We need to scale the the tiles such that grid fits in camera(screen) */

            //saving the player cooridantes and generating a list of cooridinates where blocks
            //obsticles and walls should not be allow to generate. prevents crappy starting situations
            //for players
            playerCoordinates = new Vector2((int)gridLength - 2, (int)gridLength - 2);
            generator = new Generator(gridLength, screenWidth);
            mazeWallsList = generator.MazeGenerator();
        }
        else
        {
            //fileObject.ReadFile(LevelName);
            string fileData = levelFile.text;
            fileData = fileData.Replace("\r", "");
            string[] levelData = fileData.Split("\n");
            Debug.Log(levelData.Length);
            fileObject.ReadTextAsset(levelData);
            setFileClassVars(fileObject);

            setFileClassVars(fileObject);

            read = true;
        }

        screenHeight = Camera.main.orthographicSize * 2;
        scale = Mathf.Min(screenWidth, screenHeight) / gridLength;
        createNoGoCoorList();
        GenerateWalls();
        foreach (var wall in mazeWallsList)
        {

            if (!noGoCorr.Contains(new Vector2(wall.x, wall.y)))
            {
                if (wall.isWall())
                {
                    GenerateTile(wall.x, wall.y);
                }
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
        else
        {
            //creating win block 
            AddWinBlock(target);
            noGoCorr.Add(winBlockCoor);
        }



        if (blockList.Count != 0)
        {
            foreach (Vector3 block in blockList)
            {
                GenerateBlock((int)block[0], (int)block[1], (int)block[2]);
            }
        }
        else
        {
            //placing number blocks in maze
            PlaceBlocksInMaze();
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
                 if (obj[3] == OConst.breakableTile)
                {
                    PlaceBreakableWall((int)obj[0], (int)obj[1]);
                }

            }
        }
        //if not read hardcode things for testing
        if (!read)
        {
            AddPowerUpWalkThru();
            // PlaceOneWayDoor(16, 16);
            PlaceSpikeObstacle(16, 16);
            PlaceObstacle(14, 14, 0.5f);
            PlaceBreakableWall(12, 12);
        }




        //giving gavity to objects
        ApplyGravity(GameObject.FindGameObjectsWithTag("block"));

        //invoking gravity to switch every 7 seconds, with a red screen flash before
        if(LevelName != "ag_tutorial")
        {
            InvokeRepeating("rotateGameRoutine", 7.0f, 7.0f);
        }

        InitAnalyticsData();
    }


    void InitAnalyticsData()
    {
        int totalCoins = 0;
        foreach(var obj in objectList)
        {
            if(obj[3] == OConst.coin)
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

    void createNoGoCoorList()
    {
        noGoCorr.Add(playerCoordinates);
        // Hard coding spike obstacle and 8 directions around
        for (int i = 15; i < 18; i++)
        {
            for (int j = 15; j < 18; j++)
            {
                noGoCorr.Add(new Vector2(i, j));
            }
        }
        // Hard coding points obstacle
        noGoCorr.Add(new Vector2(14, 14));

        //noGoCorr.Add(new Vector2(playerCoordinates[0]+1,playerCoordinates[1]+1));
        noGoCorr.Add(new Vector2(playerCoordinates[0], playerCoordinates[1] + 1));
        noGoCorr.Add(new Vector2(playerCoordinates[0] + 1, playerCoordinates[1]));
        //noGoCorr.Add(new Vector2(playerCoordinates[0]-1,playerCoordinates[1]-1));
        noGoCorr.Add(new Vector2(playerCoordinates[0], playerCoordinates[1] - 1));
        noGoCorr.Add(new Vector2(playerCoordinates[0] - 1, playerCoordinates[1]));
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

    public void AddWinBlock(int value)
    {
        Debug.Log("IN ADD WIN BLOCK");
        bool end = false;
        while (!end)
        {
            int x = random.Next((int)screenWidth - 5);
            int y = random.Next((int)gridLength - 1);
            Vector2 coor = new Vector2(x, y);
            if (coor != playerCoordinates)
            {
                MazeWall temp = mazeWallsList.Find(r => r.x == x && r.y == y);
                if (temp != null)
                {
                    if (!temp.isWall() && !temp.isBlock())
                    {
                        winBlockCoor = new Vector2(x, y);
                        PlaceWinBlock(x, y, value);
                        end = true;
                    }
                }
            }

        }

    }

    void PlaceWinBlock(int x, int y, int value)
    {
        GameObject t = Instantiate(winBlock, GetCameraCoordinates(x, y), Quaternion.identity);
        GameObject textChild = t.transform.GetChild(0).gameObject;
        TMP_Text textOf = textChild.GetComponent<TextMeshPro>();
        textOf.text = value.ToString();
        t.transform.localScale = new Vector3(scale, scale, 1);
    }

    void PlaceBlocksInMaze()
    {

        double numNeeded = Math.Log((double)target, 2);
        int value = 2;
        int mulitplier = (int)numNeeded;
        int total = (int)numNeeded;
        bool divided = false;
        while (total > 0)
        {
            if (value >= (numNeeded / 2) && !divided)
            {
                mulitplier = 1;
                divided = true;
            }
            for (int i = 0; i < mulitplier; i++)
            {
                bool taken = true;

                while (taken)
                {
                    int x = random.Next((int)(screenWidth - 5));
                    int y = random.Next((int)gridLength - 1);
                    Vector2 coor = new Vector2(x, y);
                    if (!noGoCorr.Contains(coor))
                    {
                        MazeWall temp = mazeWallsList.Find(r => r.x == x && r.y == y);
                        if (temp != null)
                        {
                            if (!temp.isWall() && !temp.isBlock())
                            {

                                temp.setBlock();
                                GenerateBlock(x, y, value);
                                GenerateCoin(x + 1, y);
                                blockList.Add(new Vector3(x, y, value));
                                taken = false;

                            }
                        }
                    }

                }
            }

            total--;
            value = 2 * value;
        }
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
        TransformGameObjects(GameObject.FindGameObjectsWithTag("breakableTile"), angle);
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

        StartCoroutine(flash());

    }
    IEnumerator flash()
    {

        warning.gameObject.SetActive(true);
        var whenAreweDone = Time.time + 3;
        while (Time.time < whenAreweDone)
        {

            yield return new WaitForSeconds(0.5f);
            warning.gameObject.SetActive(!warning.gameObject.activeSelf);
        }
        warning.gameObject.SetActive(false);
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

    void PlaceBreakableWall(int x, int y)
    {
        GameObject t = Instantiate(breakableWall, GetCameraCoordinates(x, y), Quaternion.identity);
        // t.transform.localScale = new Vector3(scale * 0.30f, scale * 0.30f, 1);
    }

    void AddPowerUpWalkThru()
    {
        bool end = false;
        while (!end)
        {
            int x = random.Next((int)screenWidth - 5);
            int y = random.Next((int)gridLength - 1);
            Vector2 coor = new Vector2(x, y);
            if (coor != playerCoordinates && coor != winBlockCoor)
            {
                MazeWall temp = mazeWallsList.Find(r => r.x == x && r.y == y);
                if (temp != null)
                {
                    if (!temp.isWall() && !temp.isBlock())
                    {
                        noGoCorr.Add(new Vector2(x, y));
                        PlacePowerUpWalkThru(x, y);
                        end = true;
                    }
                }
            }
        }
    }

    void PlacePowerUpWalkThru(int x, int y)
    {
        GameObject t = Instantiate(powerUpWalkThru, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale, scale, 1);
        Debug.Log("Power Up Add");
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
    void setWriteFileClassVars(FileClass file)
    {
        file.screenHeight = screenHeight;
        file.screenWidth = screenWidth;
        file.gridLength = gridLength;
        file.playerCooridantes = playerCoordinates;
        file.winBlockCoor = winBlockCoor;
        file.target = target;
        file.mazeWallsList = mazeWallsList;
        file.blockList = blockList;
        file.objectList = objectList;
    }
}



