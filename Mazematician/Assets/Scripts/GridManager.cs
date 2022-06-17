using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

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

    public int target = 32;
    public Generator generator;
    
    public Vector2 playerCooridantes; 
    public Vector2 winBlockCoor;
    public List<Vector2> noGoCorr = new List<Vector2>();

    //TODO work for reset of map (blocklist, mazeWallList, winblockcorr)
    public List<Vector3> blockList = new List<Vector3>();


    private float rotation;


    public GameObject warningPrefab;
    private GameObject warning;
    
    //adds win block script to winblock
    //calculates to see if the player is at the target
    void Awake()
    {
        var script = winBlock.GetComponent<GameEndController>();
        script.targetScore = target;
    }

    //Maze Generation, player, blocks and obsticle placement
    void Start()
    {   
        // Setting screen length and height and translating it to a camera scale
        screenWidth = 24;
        screenHeight = Camera.main.orthographicSize * 2;

        // Instantiate warning red flash creation to alert user to gravity switch
        warning = Instantiate(warningPrefab, new Vector2(Screen.width, Screen.height), Quaternion.identity);
        warning.gameObject.SetActive(false);

        gridLength = 20; //10 + 2; // 8 x 8 grid + 1 top(left) wall + 1 bottom(right);
        // We need to scale the the tiles such that grid fits in camera(screen)
        scale = Mathf.Min(screenWidth, screenHeight) / gridLength;
        
        //saving the player cooridantes and generating a list of cooridinates where blocks
        //obsticles and walls should not be allow to generate. prevents crappy starting situations
        //for players
        playerCooridantes = new Vector2((int)gridLength - 2, (int)gridLength - 2);
        createNoGoCoorList();

       
        //maze generation script and then placement in the maze
        generator = new Generator(gridLength,screenWidth);
        mazeWallsList = generator.MazeGenerator();
        GenerateWalls();
        foreach (var wall in mazeWallsList)
        {

            if(!noGoCorr.Contains(new Vector2(wall.x,wall.y))){
                if (wall.isWall())
                {
                    GenerateTile(wall.x, wall.y);
                }
            }
        }

        DrawGridLines();

        //creating player
        GeneratePlayer();
        

        //creating win block 
        AddWinBlock(target);
        noGoCorr.Add(winBlockCoor);

        //creating powerup walkthru
        AddPowerUpWalkThru();

        //placing number blocks in maze
        PlaceBlocksInMaze();

        //giving gavity to objects
        ApplyGravity(GameObject.FindGameObjectsWithTag("block"));
        
        //invoking gravity to switch every 7 seconds, with a red screen flash before
        InvokeRepeating("rotateGameRoutine", 7.0f, 7.0f);
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

    //function that is called every 7 seconds that then starts a screen flash co routine
    void rotateGameRoutine(){
        
        StartCoroutine(flash());

    }
    IEnumerator flash()
    {

                warning.gameObject.SetActive(true);
                var dur = Time.time + 3;
                while(Time.time < dur){
                     
                    yield return new WaitForSeconds(0.5f);
                    warning.gameObject.SetActive(!warning.gameObject.activeSelf);
                }
                warning.gameObject.SetActive(false) ; 
                rotation = 90.0f;
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
        ApplyGravity(GameObject.FindGameObjectsWithTag("block"));
    }

    void TransformGameObjects(GameObject[] gameObjects, float z)
    {
        foreach(GameObject gameObject in gameObjects)
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

    void GeneratePlayer()
    {
        GameObject t = Instantiate(player, GetCameraCoordinates((int)gridLength - 2, (int)gridLength - 2), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.9f, scale * 0.9f, 1);
        var script = t.GetComponent<PlayerController>();
        script.SetScore(2);

        var cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.SetPlayer(t);

        GameObject dashboard = GameObject.Find("Dashboard");
        var dashBoardController = dashboard.GetComponent<DashBoardController>();
        dashBoardController.SetPlayer(t);
    }


    void GenerateCoin(int x, int y)
    {
        GameObject t = Instantiate(coin, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.7f, scale * 0.7f, 1);
    }



    void GenerateBlock(int x, int y, int points)
    {
        GameObject t = Instantiate(block, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.9f, scale * 0.9f, 1);

        var script = t.GetComponent<BlockController>();
        script.SetPoints(points);
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

    void PlaceWinBlock(int x, int y, int value)
    {
        GameObject t = Instantiate(winBlock, GetCameraCoordinates(x, y), Quaternion.identity);
        GameObject textChild = t.transform.GetChild(0).gameObject;
        TMP_Text textOf = textChild.GetComponent<TextMeshPro>();
        textOf.text = value.ToString();
        t.transform.localScale = new Vector3(scale, scale, 1);
    }

    /*
     * Generate grid based on coordinates
     */
    void GenerateTile(int x, int y)
    {
        GameObject t = Instantiate(tile, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale, scale, 1);

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

    Vector2 GetCameraCoordinates(int x, int y, int z)
    {
        float cartesianX = ((y + 1) - (gridLength + 1) / 2) * scale;
        float cartesianY = (-(x + 1) + (gridLength + 1) / 2) * scale;
        return new Vector3(cartesianX + (0.5f * scale), cartesianY - (0.5f * scale), z);
    }


    void createNoGoCoorList(){
        noGoCorr.Add(playerCooridantes);
        //noGoCorr.Add(new Vector2(playerCooridantes[0]+1,playerCooridantes[1]+1));
        noGoCorr.Add(new Vector2(playerCooridantes[0],playerCooridantes[1]+1));
        noGoCorr.Add(new Vector2(playerCooridantes[0]+1,playerCooridantes[1]));
        //noGoCorr.Add(new Vector2(playerCooridantes[0]-1,playerCooridantes[1]-1));
        noGoCorr.Add(new Vector2(playerCooridantes[0],playerCooridantes[1]-1));
        noGoCorr.Add(new Vector2(playerCooridantes[0]-1,playerCooridantes[1]));


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
            if(value >= (numNeeded/2) && !divided){
                mulitplier = 1;
                divided = true;
            }
            for(int i = 0; i < mulitplier;i++){
            bool taken = true;

            while (taken)
            {
                int x = random.Next((int)(screenWidth - 5));
                int y = random.Next((int)gridLength - 1);
                Vector2 coor = new Vector2(x,y);
                if(!noGoCorr.Contains(coor)){
                MazeWall temp = mazeWallsList.Find(r => r.x == x && r.y == y);
                if (temp != null)
                {
                    if (!temp.isWall() && !temp.isBlock())
                    {
                        
                        temp.setBlock();
                        GenerateBlock(x, y, value);
                        GenerateCoin(x+1, y);
                        blockList.Add(new Vector3(x,y, value));
                        taken = false;
                        
                    }
                }
            }
            
            }
            }
           
            total--;
            value = 2 * value ;
        }
    }



    void AddWinBlock(int value){
        
        bool end = false;
        while(!end){
            int x = random.Next((int)screenWidth-5);
            int y = random.Next((int)gridLength-1);
            Vector2 coor = new Vector2(x,y);
            if(coor != playerCooridantes){
             MazeWall temp = mazeWallsList.Find(r=> r.x == x && r.y ==y);
                if(temp != null){
                if(!temp.isWall() && !temp.isBlock()){
                    winBlockCoor = new Vector2(x,y);
                    PlaceWinBlock(x,y,value);
                    end = true;
                }
                }
            }

        }
       
    }

    void AddPowerUpWalkThru() {
        bool end = false;
        while (!end) {
            int x = random.Next((int)screenWidth - 5);
            int y = random.Next((int)gridLength - 1);
            Vector2 coor = new Vector2(x, y);
            if (coor != playerCooridantes && coor != winBlockCoor) 
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
        t.transform.localScale = new Vector3(scale * 0.5f, scale * 0.5f, 1);
    }
}



