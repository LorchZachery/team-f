using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;

/**
 * This class deals with the logic of generating grid from prefabs
 * */

static class Constants
{
    public const int tile = 0;
    public const int block = 1;
    public const int delete = 2;
    public const int play = 3;
    public const int exitPlay = 4;
    public const int nothing = 5;
    public const int win = 6;
}


public class Sandbox : MonoBehaviour
{
    // Start is called before the first frame update
    public float screenWidth = 0f;
    private float screenHeight;

    public float gridLength = 0f;
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
    public List<Vector2> noGoCorr = new List<Vector2>();

    //TODO work for reset of map (blocklist, mazeWallList, winblockcorr)
    public List<Vector3> blockList = new List<Vector3>();


    private float time;
    private float rotation;


    public GameObject warningPrefab;
    private GameObject warning;


    public string LevelName; 
    private int mode = 0;

    
    public GameObject textbox;
    private string blockInput = null;
    public List<Tuple<GameObject,Vector2>> tileList = new List<Tuple<GameObject,Vector2>>();
    public List<Tuple<GameObject,Vector3>> blockListObjects = new List<Tuple<GameObject,Vector3>>();
    

    private GameObject playerObject;
    private GameObject winBlockObject;

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

        //warning red flash creation to alert user to gravity switch
        warning = Instantiate(warningPrefab, new Vector2(Screen.width, Screen.height), Quaternion.identity);
        warning.gameObject.SetActive(false);
        textbox.gameObject.SetActive(false);
        if(!File.Exists("Assets/Levels/" + LevelName + ".txt") || new FileInfo("Assets/Levels/" + LevelName + ".txt").Length == 0)
        {
            //setting screen length and height and translating it to a camera scale
            if(screenWidth == 0){
                screenWidth = 24;
            }
            screenHeight = Camera.main.orthographicSize * 2;
            if(gridLength == 0){
                gridLength = 20; 
            }
        
        
            //saving the player cooridantes and generating a list of cooridinates where blocks
            //obsticles and walls should not be allow to generate. prevents crappy starting situations
            //for players
            playerCooridantes = new Vector2((int)gridLength - 2, (int)gridLength - 2);
            
        }
        else
        {
            ReadFile(LevelName);
        }

        scale = Mathf.Min(screenWidth, screenHeight) / gridLength;
        createNoGoCoorList();       
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

        
        GeneratePlayer(playerCooridantes);
        
        if(winBlockCoor != new Vector2(0,0))
        {
            PlaceWinBlock((int)winBlockCoor[0],(int)winBlockCoor[1],target);
        }
        

        if(blockList.Count != 0)
        {
            foreach(Vector3 block in blockList)
            {
                GenerateBlock((int)block[0],(int)block[1],(int)block[2]);
            }
        }
       
        
       
       
        
    }

    //Function to run test on level that has been created 
    //TODO figure out how to call
    void startTesting()
    {
        //giving gavity to objects
        ApplyGravity(GameObject.FindGameObjectsWithTag("block"));
        
        //invoking gravity to switch every 7 seconds, with a red screen flash before
        InvokeRepeating("rotateGameRoutine", 7.0f, 7.0f);

    }

    void ReturnRotation()
    {
        Camera.main.transform.rotation = Quaternion.identity;
        rightSideUp(GameObject.FindGameObjectsWithTag("block"));
        rightSideUp(GameObject.FindGameObjectsWithTag("obstacle"));
        rightSideUp(GameObject.FindGameObjectsWithTag("player"));
        rightSideUp(GameObject.FindGameObjectsWithTag("target"));

    }

    void rightSideUp(GameObject[] gameObjects)
    {
        foreach(GameObject gameObject in gameObjects)
        {
            gameObject.transform.rotation = Quaternion.identity;
        }
    }


    void stopTesting()
    {
        CancelInvoke();
        ReturnRotation();
        
        //RemoveGravity(GameObject.FindGameObjectsWithTag("block"));
        List<Tuple<GameObject,Vector3>> temp = new List<Tuple<GameObject,Vector3>>(blockListObjects);
        foreach(var blockTuple in temp)
        {
            //moveBlock((int)blockTuple.Item2[0],(int)blockTuple.Item2[1],blockTuple.Item1);
            blockListObjects.Remove(blockTuple);
            blockList.Remove(blockList.Find(r => r[0] == blockTuple.Item2[0] && r[1] == blockTuple.Item2[1]));
            Destroy(blockTuple.Item1);
            

            GenerateBlock((int)blockTuple.Item2[0], (int)blockTuple.Item2[1], (int)blockTuple.Item2[2]);
            blockList.Add(new Vector3((int)blockTuple.Item2[0], (int)blockTuple.Item2[1], (int)blockTuple.Item2[2]));
            


        }
        
        playerObject.transform.position = GetCameraCoordinates((int)playerCooridantes[0],(int)playerCooridantes[1]);
        var script = playerObject.GetComponent<PlayerController>();
        script.SetScore(2);
    }

   

    // Update is called once per frame
    //on update there is a create to rotate the screen slowly
    void Update()
    {
        
        
        if(Input.GetKeyDown(KeyCode.T))
        {
            mode = Constants.tile;
        }
         if(Input.GetKeyDown(KeyCode.B))
        {
            mode = Constants.block;
        }
         if(Input.GetKeyDown(KeyCode.G))
        {
            mode = Constants.win;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            mode = Constants.delete;
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("PLAY MODE");
            mode = Constants.play;
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("EXIT PLAY MODE");
            mode = Constants.exitPlay;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("SAVING");
            File.Delete("Assets/Levels/" + LevelName + ".txt");
            writeToFile(LevelName);
        }
        //adding tiles to map
        if (Input.GetMouseButtonDown(0) && mode == Constants.tile) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
           
            Vector2 tilePos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"CREATE TILE {tilePos}");
            GenerateTile((int)tilePos[0],(int)tilePos[1]);
            mazeWallsList.Add(new MazeWall((int)tilePos[0],(int)tilePos[1]));
        }
         if( Input.GetMouseButtonDown(0) && mode == Constants.win)
         {
            Debug.Log("starting win block co-r");

            StartCoroutine(winBlockMakingFunction());

         }


        if( Input.GetMouseButtonDown(0) && mode == Constants.block)
        {
            Debug.Log("starting block co-r");
           StartCoroutine(blockMakingFunction());
                
        }

        if (Input.GetMouseButtonDown(0) && mode == Constants.delete) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            
            Vector2 tilePos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"DELETE attempt {tilePos}");
            Tuple<GameObject,Vector2> tupleObject = tileList.Find(r => r.Item2[0] == (int)tilePos[0] && r.Item2[1] == (int)tilePos[1]);
            if(tupleObject != null)
            {
                Debug.Log($"DELTING TILE");
                tileList.Remove(tupleObject);
                mazeWallsList.Remove(mazeWallsList.Find(r=> r.x == tilePos[0] && r.y == tilePos[1]));
                Destroy(tupleObject.Item1);
                

            }
            else
            {
                Tuple<GameObject,Vector3> blockTupleObject = blockListObjects.Find(r => r.Item2[0] == (int)tilePos[0] && r.Item2[1] == (int)tilePos[1]);
                if(blockTupleObject != null)
                {
                    Debug.Log("DELETING BLOCK");
                    blockListObjects.Remove(blockTupleObject);
                    blockList.Remove(blockList.Find(r => r[0] == blockTupleObject.Item2[0] && r[1] == blockTupleObject.Item2[1]));
                    Destroy(blockTupleObject.Item1);
                }
                else
                {
                    if(tilePos[0] == winBlockCoor[0] && tilePos[1] == winBlockCoor[1])
                    {
                        Debug.Log("DELETE WINBLOCK");
                        winBlockCoor = new Vector2(0,0);
                        Destroy(winBlockObject);
                    }
                }
            }

            
        }

        if(mode == Constants.play)
        {
            startTesting();
            mode = Constants.nothing;
        }
        if(mode == Constants.exitPlay)
        {
            stopTesting();
            mode = Constants.nothing;
        }


        if (rotation > 0)
        {
            float currentRotation = Time.deltaTime * 90 / 1.5f;
            currentRotation = Math.Min(currentRotation, rotation);

            rotation -= currentRotation;
            RotateGame(currentRotation);

        }
       
    }



    private IEnumerator winBlockMakingFunction()
    {
            mode = Constants.nothing;
            Debug.Log("Enter Win maker");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 blockPos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"Got win pos {blockPos}");
            
           if(textbox != null)
           {
            textbox.transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane) );
            textbox.gameObject.SetActive(true);
            yield return waitForInput();
            Debug.Log($"CREATE WIN BLOCK {blockPos} value {blockInput}");
            PlaceWinBlock((int)blockPos[0], (int)blockPos[1], Int32.Parse(blockInput));
            winBlockCoor = new Vector2((int)blockPos[0], (int)blockPos[1]);
            target = Int32.Parse(blockInput);
            
            blockInput = null;
            textbox.gameObject.SetActive(false);
            
           } 
    }




    private IEnumerator blockMakingFunction()
    {
            mode = Constants.nothing;
            Debug.Log("Enter Block maker");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 blockPos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"Got block pos {blockPos}");
            
           if(textbox != null)
           {
            textbox.transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane) );
            textbox.gameObject.SetActive(true);
            yield return waitForInput();
            Debug.Log($"CREATE BLOCK {blockPos} value {blockInput}");
            GenerateBlock((int)blockPos[0], (int)blockPos[1], Int32.Parse(blockInput));
            blockList.Add(new Vector3((int)blockPos[0], (int)blockPos[1], Int32.Parse(blockInput)));
            
            blockInput = null;
            textbox.gameObject.SetActive(false);
            
           } 
    }

    private IEnumerator waitForInput()
    {
        bool done = false;
        while(!done)
        {
            if(blockInput != null)
            {
                done = true;
            }
            yield return null;
        }
    }

    public void ReadStringInput(string s)
    {
        blockInput = s;
       
    }

    //function that is called every 7 seconds that then starts a screen flash co routine
    void rotateGameRoutine(){
        
        StartCoroutine(flash());

    }
    IEnumerator flash()
    {

                warning.gameObject.SetActive(true);
                var whenAreweDone = Time.time + 3;
                while(Time.time < whenAreweDone){
                     
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
        //Debug.Log(gameObjects[0].transform.eulerAngles.ToString());
        foreach (GameObject gameObject in gameObjects)
        {
            ConstantForce2D constantForce = gameObject.GetComponent<ConstantForce2D>();
            Vector2 direction = Camera.main.transform.up * -1;
            constantForce.force = direction * 50f;
        }
        //Debug.Log(gameObjects[0].transform.x);
    }


    void RemoveGravity(GameObject[] gameObjects)
    {
        //Debug.Log(gameObjects[0].transform.eulerAngles.ToString());
        foreach (GameObject gameObject in gameObjects)
        {
            ConstantForce2D constantForce = gameObject.GetComponent<ConstantForce2D>();
            Vector2 direction = Camera.main.transform.up * -1;
            constantForce.force = direction * 0;
        }
        //Debug.Log(gameObjects[0].transform.x);
    }

    void GeneratePlayer(Vector2 coordinates)
    {
        GameObject t = Instantiate(player, GetCameraCoordinates((int)coordinates[0], (int)coordinates[1]), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.9f, scale * 0.9f, 1);
        var script = t.GetComponent<PlayerController>();
        script.SetScore(2);
        var cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.SetPlayer(t);
        playerObject = t;
    }

    void GenerateBlock(int x, int y, int points)
    {
        GameObject t = Instantiate(block, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.9f, scale * 0.9f, 1);

        var script = t.GetComponent<BlockController>();
        script.SetPoints(points);
        blockListObjects.Add(new Tuple<GameObject,Vector3>(t,new Vector3(x,y,points)));


    }
    void moveBlock(int x, int y, GameObject t)
    {
       t.transform.position = GetCameraCoordinates(x,y);
    
    }


    void GenerateWalls()
    {
        for (int i = 0; i < gridLength; i++)
        {
            //top : x = 0, y = i
            GenerateTile(0, i);

            //bottom: x = 9, y = i
            GenerateTile((int)gridLength - 1, i);
        }

        for (int i = 1; i < gridLength - 1; i++)
        {
            //left x = i, y = 0
            GenerateTile(i, 0);
            //right x = i, y = 9
            GenerateTile(i, (int)gridLength - 1);
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
        winBlockObject = t;
    }

    /*
     * Generate grid based on coordinates
     */
    void GenerateTile(int x, int y)
    {
        GameObject t = Instantiate(tile, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale, scale, 1);
        tileList.Add(new Tuple<GameObject,Vector2>(t,new Vector2(x,y)));

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


    Vector2 GetTileCoordinates(float cartesianX, float cartesianY)
    {
        cartesianX = cartesianX - (0.5f * scale);
        cartesianY = cartesianY - (0.5f * scale);
        int y = (int)Mathf.Ceil(((cartesianX / scale) + ((gridLength + 1) /2)) -1);
        int x = (int)(-((cartesianY/scale) - ((gridLength +1)/2))) -1;
        return new Vector2(x,y);
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
    void writeToFile(string LevelName)
    {
        string path = "Assets/Levels/" + LevelName + ".txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("width height gridlength");
        writer.WriteLine($"{screenWidth},{screenHeight},{gridLength}");
        writer.WriteLine("playerCoor");
        writer.WriteLine($"{playerCooridantes[0]},{playerCooridantes[1]}");
        writer.WriteLine("winBlock");
        //Vector3 winBlockVector = new Vector3(winBlockCoor[0],winBlockCoor[1],target);
        writer.WriteLine($"{winBlockCoor[0]},{winBlockCoor[1]},{target}");
        writer.WriteLine("MazeWalls");
        foreach(MazeWall tile in mazeWallsList){
            if(tile.isWall()){
                writer.WriteLine($"{tile.x},{tile.y}");
            }
            
        }
        writer.WriteLine("MazeBlocks");
        foreach(Vector3 block in blockList){
            writer.WriteLine($"{block[0]},{block[1]},{block[2]}");
        }
        writer.WriteLine("END");
        writer.Close();

        //ReadFile(path);
       
    }
    void ReadFile(string level)
    {
        
        string path = "Assets/Levels/" + level + ".txt";
        using (StreamReader sr = new StreamReader(path))
        {
                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    if(line == "width height gridlength")
                    {
                        line = sr.ReadLine();
                        string[] values = line.Split(',');
                        screenWidth = float.Parse(values[0]);
                        screenHeight = float.Parse(values[1]);
                        gridLength = float.Parse(values[2]);
                    }
                    if(line == "playerCoor")
                    {
                       line = sr.ReadLine();
                       string[] values = line.Split(',');
                       playerCooridantes = new Vector2(float.Parse(values[0]),float.Parse(values[1]));
                    }
                    if(line == "winBlock")
                    {
                        line = sr.ReadLine();
                       string[] values = line.Split(',');
                       winBlockCoor = new Vector2(float.Parse(values[0]),float.Parse(values[1]));
                       target = Int32.Parse(values[2]);
                    }
                    if(line == "MazeWalls")
                    {
                        
                        while((line = sr.ReadLine()) != "MazeBlocks")
                        {
                            string[] values = line.Split(',');
                            mazeWallsList.Add(new MazeWall(Int32.Parse(values[0]),Int32.Parse(values[1])));
                        }

                    }
                    if(line == "MazeBlocks")
                    {
                        
                        while((line = sr.ReadLine()) != "END")
                        {
                            string[] values = line.Split(',');
                            blockList.Add(new Vector3(Int32.Parse(values[0]),Int32.Parse(values[1]),Int32.Parse(values[2])));
                        }
                    }
                }
        }
       
    }
}



