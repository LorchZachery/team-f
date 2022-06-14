using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int target = 32;
    public Generator generator;
    public Vector2 playerCooridantes; 
    public Vector2 winBlockCoor;
    private float time;
    private float rotation;


    void Awake()
    {
        var script = winBlock.GetComponent<GameEndController>();
        script.targetScore = target;
    }
    void Start()
    {
        
        screenWidth = 24;
        screenHeight = Camera.main.orthographicSize * 2;

        gridLength = 20; //10 + 2; // 8 x 8 grid + 1 top(left) wall + 1 bottom(right);

        /* We need to scale the the tiles such that grid fits in camera(screen) */
        scale = Mathf.Min(screenWidth, screenHeight) / gridLength;
        playerCooridantes = GetCameraCoordinates((int)gridLength - 2, (int)gridLength - 2);
        generator = new Generator(gridLength,screenWidth);
        mazeWallsList = generator.MazeGenerator();
        GenerateWalls();
        foreach (var wall in mazeWallsList)
        {
            if (wall.isWall())
            {
                GenerateTile(wall.x, wall.y);
            }
        }
        //GenerateTile(1, 2);
        //GenerateTile(1, 4);
        //GenerateTile(1, 6);
        //GenerateTile(1, 8);

        //GenerateTile(2, 1);
        //GenerateTile(2, 3);
        //GenerateTile(2, 5);
        //GenerateTile(2, 7);

        //GenerateTile(3, 2);
        //GenerateTile(3, 4);
        //GenerateTile(3, 6);
        //GenerateTile(3, 8);

        //GenerateTile(4, 1);
        //GenerateTile(4, 3);
        //GenerateTile(4, 5);
        //GenerateTile(4, 7);

        //GenerateTile(5, 2);
        //GenerateTile(5, 4);
        //GenerateTile(5, 6);
        //GenerateTile(5, 8);

        //GenerateTile(6, 1);
        //GenerateTile(6, 3);
        //GenerateTile(6, 5);
        //GenerateTile(6, 7);

        //GenerateTile(7, 2);
        //GenerateTile(7, 4);
        //GenerateTile(7, 6);
        //GenerateTile(7, 8);

        //GenerateTile(8, 1);
        //GenerateTile(8, 3);
        //GenerateTile(8, 5);
        //GenerateTile(8, 7);

        // GenerateTile(9, 6);
        // GenerateTile(9, 8);
        // GenerateTile(10, 6);
        // GenerateTile(10, 8);

        //GenerateTile(10, 10);
        //GenerateTile(1,1);
        DrawGridLines();

        GeneratePlayer();

        
        
        AddWinBlock(target);
        PlaceBlocksInMaze();
        
        /*GenerateBlock(2, 7, 16);
        GenerateBlock(4, 4, 32);
        GenerateBlock(4, 2, 4);
        GenerateBlock(5, 3, 16);
        GenerateBlock(5, 5, 16);
        GenerateBlock(6, 1, 4);
        GenerateBlock(7, 3, 2);p
        GenerateBlock(7, 4, 2);
        GenerateBlock(10, 7, 2);
        GenerateBlock(7, 6, 2);
        PlaceObstacle(9, 8);
        PlaceWinBlock(1, 10);
        PlaceObstacle(1, 1, 0.5f);
        PlaceObstacle(10, 1, 0.25f);
        PlaceSpikeObstacle(8, 8);
        PlaceWinBlock(1, 10);*/

        
        ApplyGravity(GameObject.FindGameObjectsWithTag("block"));


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //time = 1.5f;
            rotation = 90f;
        }

        if (rotation > 0)
        {
            float currentRotation = Time.deltaTime * 90 / 1.5f;
            currentRotation = Math.Min(currentRotation, rotation);
            //if ((rotation - currentRotation) < 0)
            //{
            //    currentRotation = rotation;
            //}
            //time -= Time.deltaTime;
            rotation -= currentRotation;
            RotateGame(currentRotation);
        }

        
    }

    void RotateGame(float angle)
    {
        Camera.main.transform.Rotate(0, 0, angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("block"), angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("obstacle"), angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("player"), angle);
        TransformGameObjects(GameObject.FindGameObjectsWithTag("target"), angle);
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
        Debug.Log(gameObjects[0].transform.eulerAngles.ToString());
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

    void PlaceBlocksInMaze()
    {
        int blocksPlaced = 0;
        double numNeeded = Math.Log((double)target, 2);
        int value = 2;
        int mulitplier = (int)numNeeded;
        while (mulitplier > 0)
        {
            for(int i = 0; i < mulitplier;i++){
            bool taken = true;

            while (taken)
            {
                int x = random.Next((int)(screenWidth - 5));
                int y = random.Next((int)gridLength - 1);
                Vector2 coor = new Vector2(x,y);
                if(coor != winBlockCoor){
                MazeWall temp = mazeWallsList.Find(r => r.x == x && r.y == y);
                if (temp != null)
                {
                    if (!temp.isWall() && !temp.isBlock())
                    {
                        
                        MazeWall upWall = mazeWallsList.Find(r=> r.x == temp.x+1 && r.y == temp.y+1);
                        if(upWall != null){
                            if(!upWall.isWall()){
                                upWall.removeWall();
                            }
                            }
                        MazeWall downWall = mazeWallsList.Find(r=> r.x == temp.x-1 && r.y == temp.y-1);
                        if(downWall != null){
                            if(!downWall.isWall()){
                                downWall.removeWall();
                            }
                        }


                        
                            MazeWall neighbor = generator.getNext(temp, random.Next(3));
                            
                            if(neighbor != null){
                            if(!neighbor.isWall()){
                                neighbor.removeWall();
                            }
                            }
                        
                        temp.setBlock();
                        GenerateBlock(x, y, value);
                        taken = false;
                        blocksPlaced++;
                    }
                }
            }
            
            }
            }
           
            mulitplier = mulitplier - 1;
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




}



