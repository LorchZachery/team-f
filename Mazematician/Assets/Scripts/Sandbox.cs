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
    public const int coin = 7;
    public const int spike = 8;
    public const int obstacle = 9;
    public const int powerWalkThru = 10;
    public const int onewayUp = 11;
    public const int onewayDown = 12;
    public const int onewayLeft = 13;
    public const int onewayRight = 14;
    public const int breakableTile = 15;
    public const int spikeTwo = 16;
}



/*
To use sandbox mode in unity inspect make sure to give level a name (or use level name to load a level 
you are currently designing)
you also can change the screen width and gridlength in unity editor
To add a tile press "t" and click somewhere
to add a oneway wall UP press "1" and click somewhere
to add a oneway wall DOWN press "2" and click somewhere
to add a oneway wall LEFT press "3" and click somewhere
to add a oneway wall RIGHT press "4" and click somewhere
to add a breakable tile, press "V" and click somewhere
to add a block press "b", click where you want it and  provided a number in input box, press enter
to add a win block press "g" click where you want it and provide the target number in the input box, press enter
to add reducing obstacle press "O" and click where you want and provide reducing value (usually .5)
to add spike obstacle press "K" and click where you want it press "L" for two spike
to add the walk thru wall power up press "w" and place it where you want it
to delete any object you added press "d" and click on that object
to play the game press "p" 
to exit play mode or just reset player and balls press "e"
to save current set up press "s" this will use provided level name so if it will re-write files if the names are the same
"[" zooms in and "]" zooms out
*/

public class Sandbox : MonoBehaviour
{
    // Start is called before the first frame update
    public float screenWidth = 0f;
    private float screenHeight;

    public float gridLength = 0f;
    float scale;

   

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
    public GameObject oneWayDoorSet;
    public GameObject breakableWall;


    //value player must reach to win
    public int target = 32;


    
    public List<Vector2> noGoCorr = new List<Vector2>();

    //For level loading and saving
    public Vector2 playerCooridantes; 
    public Vector2 winBlockCoor;
    public List<MazeWall> mazeWallsList = new List<MazeWall>();
    public List<Vector3> blockList = new List<Vector3>();
    //x,y,pen,type
    public List<Vector4> objectList = new List<Vector4>();


    private float time;
    private float rotation;


    public GameObject warningPrefab;
    private GameObject warning;

    //level name to load or save as (or both)
    public string LevelName; 
    //mode for what to start at
    private int mode = 0;

    //sandbox variables
    public GameObject textbox;
    private string blockInput = null;
    public List<Tuple<GameObject,Vector2>> tileList = new List<Tuple<GameObject,Vector2>>();
    public List<Tuple<GameObject,Vector3>> blockListObjects = new List<Tuple<GameObject,Vector3>>();
    public List<Tuple<List<GameObject>,Vector4>> objectListObjects = new List<Tuple<List<GameObject>,Vector4>>();
    
    //to delete and reset player and win box
    private GameObject playerObject;
    private GameObject winBlockObject;

    private FileClass fileObject = new FileClass();

    //adds win block script to winblock
    //calculates to see if the player is at the target
    void Awake()
    {
        var script = winBlock.GetComponent<GameEndController>();
        script.targetScore = target;
    }

    
    void Start()
    {   

        //warning red flash creation to alert user to gravity switch
        warning = Instantiate(warningPrefab, new Vector2(Screen.width, Screen.height), Quaternion.identity);
        warning.gameObject.SetActive(false);

        //hids input box for blocks
        textbox.gameObject.SetActive(false);

        //if the level name is not a file create a new map
        if(!File.Exists("Assets/Resources/Levels/" + LevelName + ".txt") || new FileInfo("Assets/Resources/Levels/" + LevelName + ".txt").Length == 0)
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
            for(int x =1; x < screenWidth+5; x++){
            for(int y = 1; y < gridLength+5; y++){
                MazeWall temp = new MazeWall(x,y);
                temp.removeWall();
                mazeWallsList.Add(temp);
                 
            }
        }
        
        }
        //if the level name is a file load that verision
        else
        {
            //ReadFile(LevelName);
            fileObject.ReadFile(LevelName);
            setFileClassVars(fileObject);
        }

        scale = Mathf.Min(screenWidth, screenHeight) / gridLength;
        //createNoGoCoorList();       
        GenerateWalls();
        

        //adding walls if we are loading a level
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
        
        //if we are loading a level create winblock
        if(winBlockCoor != new Vector2(0,0))
        {
            PlaceWinBlock((int)winBlockCoor[0],(int)winBlockCoor[1],target);
        }
        
        //if we are loading a level add blocks 
        if(blockList.Count != 0)
        {
            foreach(Vector3 block in blockList)
            {
                GenerateBlock((int)block[0],(int)block[1],(int)block[2]);
            }
        }

        if(objectList.Count != 0)
        {
            foreach(Vector4 obj in objectList)
            {
                if(obj[3] == OConst.normObj )
                {
                    PlaceObstacle((int)obj[0],(int)obj[1],obj[2]);
                }
                if(obj[3] == OConst.spike)
                {
                    PlaceSpikeObstacle((int)obj[0],(int)obj[1]);
                }
                if(obj[3] == OConst.wallkThru)
                {
                    PlacePowerUpWalkThru((int)obj[0],(int)obj[1]);
                }
                if(obj[3] == OConst.coin)
                {
                    GenerateCoin((int)obj[0],(int)obj[1]);
                }
                if(obj[3] == OConst.oneway)
                {
                    PlaceOneWayDoor((int)obj[0],(int)obj[1], (int)obj[2]);
                }
                if (obj[3] == OConst.breakableTile)
                {
                    PlaceBreakableWall((int)obj[0], (int)obj[1]);
                }
                 if (obj[3] == OConst.spikeTwo)
                {
                    PlaceSpikeObstacleTwoWide((int)obj[0], (int)obj[1]);

                }

            }
        }
       
        
       
       
        
    }

   //to start testing putting the game in play mode
    void startTesting()
    {
        //giving gavity to objects
        ApplyGravity(GameObject.FindGameObjectsWithTag("block"));
        
        //invoking gravity to switch every 7 seconds, with a red screen flash before
        InvokeRepeating("rotateGameRoutine", 7.0f, 7.0f);

    }

    //return objects to there upright position and flipping the camera back
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

    //function to reset the testing enviroment back to the orignal state
    void stopTesting()
    {
        //stops the rotation and flashing
        CancelInvoke();
        //puts everything right side up
        ReturnRotation();
        
        //have to destory blocks and then re-instantate them because player could have
        //merged with blocks meaning they dont exist anymore
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

        List<Tuple<List<GameObject>,Vector4>> tempObj = new List<Tuple<List<GameObject>,Vector4>>(objectListObjects);
        foreach( var objTuple in tempObj)
        {
            objectListObjects.Remove(objTuple);
            objectList.Remove(objectList.Find(r => r[0] == objTuple.Item2[0] && r[1] == objTuple.Item2[1] && r[3] == objTuple.Item2[3]));
            foreach(var gameobject in objTuple.Item1)
            {
                Destroy(gameobject);
            }

               if(objTuple.Item2[3] == OConst.normObj )
                {
                    PlaceObstacle((int)objTuple.Item2[0],(int)objTuple.Item2[1],objTuple.Item2[2]);
                }
                if(objTuple.Item2[3] == OConst.spike)
                {
                    PlaceSpikeObstacle((int)objTuple.Item2[0],(int)objTuple.Item2[1]);
                }
                if(objTuple.Item2[3] == OConst.wallkThru)
                {
                    PlacePowerUpWalkThru((int)objTuple.Item2[0],(int)objTuple.Item2[1]);
                }
                if(objTuple.Item2[3] == OConst.coin)
                {
                    GenerateCoin((int)objTuple.Item2[0],(int)objTuple.Item2[1]);
                }
                if(objTuple.Item2[3] == OConst.oneway)
                {
                    PlaceOneWayDoor((int)objTuple.Item2[0],(int)objTuple.Item2[1], (int)objTuple.Item2[2]);
                }
                if (objTuple.Item2[3] == OConst.breakableTile)
                {
                    PlaceBreakableWall((int)objTuple.Item2[0], (int)objTuple.Item2[1]);
                }
                 if (objTuple.Item2[3] == OConst.spikeTwo)
                {
                    PlaceSpikeObstacleTwoWide((int)objTuple.Item2[0], (int)objTuple.Item2[1]);

                }
            objectList.Add(new Vector4(objTuple.Item2[0],objTuple.Item2[1],objTuple.Item2[2],objTuple.Item2[3]));
        }

        //reset the player
        playerObject.transform.position = GetCameraCoordinates((int)playerCooridantes[0],(int)playerCooridantes[1]);
        var script = playerObject.GetComponent<PlayerController>();
        script.SetScore(2);
    }

   

    // Update is called once per frame
    //on update there is a create to rotate the screen slowly
    void Update()
    {
        
        //press T to enter tile creation mode
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Place Tile Mode");
            mode = Constants.tile;
        }
        //press 1 to enter oneway wall Up mode
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Oneway Up Mode");
            mode = Constants.onewayUp;
        }
        //press 2 to enter oneway wall Down mode
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Oneway Down Mode");
            mode = Constants.onewayDown;
        }
        //press 3 to enter oneway wall Left mode
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Oneway Left Mode");
            mode = Constants.onewayLeft;
        }
        //press 4 to enter oneway wall Right mode
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Oneway Right Mode");
            mode = Constants.onewayRight;
        }
        //press B to enter block creation mode
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Place Block mode");
            mode = Constants.block;
        }
        //press G to enter winblock creation mode
         if(Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Place Win block mode");
            mode = Constants.win;
        }
        //press D to enter delete mode
        if(Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("DELETE MODE");
            mode = Constants.delete;
        }
        //press C to enter coin mode
        if(Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Place Coin");
            mode = Constants.coin;
        }

        //press K to enter spike mode
        if(Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Place Spike");
            mode = Constants.spike;
        }
        //press L to enter Twospike mode
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Place TwoSpike");
            mode = Constants.spikeTwo;
        }

        //press O to enter obstacle mode
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Place obstacle");
            mode = Constants.obstacle;
        }

        //press V to enter breakable tile mode
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Place breakable tile");
            mode = Constants.breakableTile;
        }


        //press W to enter obstacle mode
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Place power Walk Thru");
            mode = Constants.powerWalkThru;
        }

        //press P to enter play mode
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("PLAY MODE");
            mode = Constants.play;
        }
        //press E to exit play mode, or reset player back to start
        //do not have to be in play mode for E to work 
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("EXIT PLAY MODE");
            mode = Constants.exitPlay;
        }

        //press S to save the current map
        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("SAVING");
            File.Delete("Assets/Resources/Levels/" + LevelName + ".txt");
            setWriteFileClassVars(fileObject);
            fileObject.writeToFile(LevelName);
        }
        
        //adding coin to map where user clicks
        if (Input.GetMouseButtonDown(0) && mode == Constants.coin) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Vector2 tilePos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"CREATE COIN {tilePos}");
            GenerateCoin((int)tilePos[0],(int)tilePos[1]);
            
           objectList.Add(new Vector4(tilePos[0],tilePos[1],-1,OConst.coin));
        }


        //adding spike to map where user clicks
        if (Input.GetMouseButtonDown(0) && mode == Constants.spike) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Vector2 tilePos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"CREATE SPIKE {tilePos}");
            PlaceSpikeObstacle((int)tilePos[0],(int)tilePos[1]);
            
           objectList.Add(new Vector4(tilePos[0],tilePos[1],-1,OConst.spike));
        }
         //adding Two spike to map where user clicks
        if (Input.GetMouseButtonDown(0) && mode == Constants.spikeTwo) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Vector2 tilePos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"CREATE Two SPIKE {tilePos}");
            PlaceSpikeObstacleTwoWide((int)tilePos[0],(int)tilePos[1]);
            
           objectList.Add(new Vector4(tilePos[0],tilePos[1],-1,OConst.spikeTwo));
        }

        //adding breakable tile to map where user clicks
        if (Input.GetMouseButtonDown(0) && mode == Constants.breakableTile)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 tilePos = GetTileCoordinates(mousePos[0], mousePos[1]);
            Debug.Log($"CREATE BREAKABLE TILE {tilePos}");
            PlaceBreakableWall((int)tilePos[0], (int)tilePos[1]);

            objectList.Add(new Vector4(tilePos[0], tilePos[1], -1, OConst.breakableTile));
        }


        //adding powerup walkthru to map where user clicks
        if (Input.GetMouseButtonDown(0) && mode == Constants.powerWalkThru) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Vector2 tilePos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"CREATE walkthru {tilePos}");
            PlacePowerUpWalkThru((int)tilePos[0],(int)tilePos[1]);
            
           objectList.Add(new Vector4(tilePos[0],tilePos[1],-1,OConst.wallkThru));
        }


        //adding oneway wall to map where user clicks
        if (Input.GetMouseButtonDown(0) && mode >= Constants.onewayUp && mode <= Constants.onewayRight) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Vector2 tilePos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"CREATE oneway {tilePos}");
            int dir = mode - 10;
            PlaceOneWayDoor((int)tilePos[0],(int)tilePos[1],dir);
            
           objectList.Add(new Vector4(tilePos[0],tilePos[1],dir,OConst.oneway));
        }

         //adding obsticle to map where user clicks
        if (Input.GetMouseButtonDown(0) && mode == Constants.obstacle) {
            Debug.Log("starting obstcale co-r");

            StartCoroutine(obstaclekMakingFunction());
        }

        //adding tiles to map where user clicks
        if (Input.GetMouseButtonDown(0) && mode == Constants.tile) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
           
            Vector2 tilePos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"CREATE TILE {tilePos}");
            GenerateTile((int)tilePos[0],(int)tilePos[1]);
            //todo change to find and add wall
            //mazeWallsList.Add(new MazeWall((int)tilePos[0],(int)tilePos[1]));
            MazeWall wall = mazeWallsList.Find(r=> r.x == (int)tilePos[0] && r.y == (int)tilePos[1]);
           
            
            wall.setWall();
        }

        


         //adding win block where user clicks
         if( Input.GetMouseButtonDown(0) && mode == Constants.win)
         {
            Debug.Log("starting win block co-r");

            StartCoroutine(winBlockMakingFunction());

         }

        //adding block where user clicks
        if( Input.GetMouseButtonDown(0) && mode == Constants.block)
        {
            Debug.Log("starting block co-r");
           StartCoroutine(blockMakingFunction());
                
        }
        //delete clicked item
        if (Input.GetMouseButtonDown(0) && mode == Constants.delete) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            
            Vector2 tilePos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"DELETE attempt {tilePos}");
            Tuple<GameObject,Vector2> tupleObject = tileList.Find(r => r.Item2[0] == (int)tilePos[0] && r.Item2[1] == (int)tilePos[1]);
            if(tupleObject != null)
            {
                Debug.Log($"DELTING TILE");
                tileList.Remove(tupleObject);
                //mazeWallsList.Remove(mazeWallsList.Find(r=> r.x == tilePos[0] && r.y == tilePos[1]));
                MazeWall temp = mazeWallsList.Find(r=> r.x == tilePos[0] && r.y == tilePos[1]);
                temp.removeWall();
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
                    Tuple<List<GameObject>,Vector4> objTuple = objectListObjects.Find(r => r.Item2[0] == (int)tilePos[0] && r.Item2[1] == (int)tilePos[1]);
                    if(objTuple != null)
                    {
                        Debug.Log($"Delete obj {objTuple.Item2[3]}");
                        objectListObjects.Remove(objTuple);
                        objectList.Remove(objectList.Find(r => r[0] == objTuple.Item2[0] && r[1] == objTuple.Item2[1]));
                        foreach(var gameobject in objTuple.Item1)
                        {
                            Destroy(gameobject);
                        }
                        
                        
                    }
                    else{

                        if(tilePos[0] == winBlockCoor[0] && tilePos[1] == winBlockCoor[1])
                        {
                            Debug.Log("DELETE WINBLOCK");
                            winBlockCoor = new Vector2(0,0);
                            Destroy(winBlockObject);
                        }
                    }
                }
            }

            
        }
        //entering testing mode
        if(mode == Constants.play)
        {
            startTesting();
            mode = Constants.nothing;
        }
        //exiting testing mode or reseting map
        if(mode == Constants.exitPlay)
        {
            stopTesting();
            mode = Constants.nothing;
        }

        //rotation for actual game play
        if (rotation > 0)
        {
            float currentRotation = Time.deltaTime * 90 / 1.5f;
            currentRotation = Math.Min(currentRotation, rotation);

            rotation -= currentRotation;
            RotateGame(currentRotation);

        }
       
    }
    

    //function to create obstacle with input value
    private IEnumerator obstaclekMakingFunction()
    {
            mode = Constants.nothing;
            Debug.Log("Enter Obj maker");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 blockPos = GetTileCoordinates(mousePos[0],mousePos[1]);
            Debug.Log($"Got Obj pos {blockPos}");
            
           if(textbox != null)
           {
            textbox.transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane) );
            textbox.gameObject.SetActive(true);
            yield return waitForInput();
            while(!float.TryParse(blockInput, out _))
            {
                Debug.Log("ERROR IN ENTER VALUE TRY AGAIN");
                blockInput = null;
                yield return waitForInput();
            }
            Debug.Log($"CREATE OBJ {blockPos} value {blockInput}");
            PlaceObstacle((int)blockPos[0], (int)blockPos[1], float.Parse(blockInput));
            
            objectList.Add(new Vector4((int)blockPos[0], (int)blockPos[1], float.Parse(blockInput),OConst.normObj));
            blockInput = null;
            textbox.gameObject.SetActive(false);
            
           } 
    }

    //function to create winblock with input value
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
            while(!int.TryParse(blockInput, out _))
            {
                Debug.Log("ERROR IN ENTER VALUE TRY AGAIN");
                blockInput = null;
                yield return waitForInput();
            }
            Debug.Log($"CREATE WIN BLOCK {blockPos} value {blockInput}");
            PlaceWinBlock((int)blockPos[0], (int)blockPos[1], Int32.Parse(blockInput));
            winBlockCoor = new Vector2((int)blockPos[0], (int)blockPos[1]);
            target = Int32.Parse(blockInput);
            
            blockInput = null;
            textbox.gameObject.SetActive(false);
            
           } 
    }



    //function to create normal block with input value
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
            while(!int.TryParse(blockInput, out _))
            {
                Debug.Log("ERROR IN ENTER VALUE TRY AGAIN");
                blockInput = null;
                yield return waitForInput();
            }
            Debug.Log($"CREATE BLOCK {blockPos} value {blockInput}");
            GenerateBlock((int)blockPos[0], (int)blockPos[1], Int32.Parse(blockInput));
            blockList.Add(new Vector3((int)blockPos[0], (int)blockPos[1], Int32.Parse(blockInput)));
            
            blockInput = null;
            textbox.gameObject.SetActive(false);
            
           } 
    }

    //waits for input from input box when block needs a value
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

    //gets input string from input box
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
        var cameraController = Camera.main.GetComponent<CameraControllerSandBox>();
        cameraController.SetPlayerSandBox(t);
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

     void GenerateCoin(int x, int y)
    {
        GameObject t = Instantiate(coin, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.7f, scale * 0.7f, 1);
        List<GameObject> coinObjList = new List<GameObject> {t};
        objectListObjects.Add(new Tuple<List<GameObject>,Vector4>(coinObjList,new Vector4(x,y,0,OConst.coin)));
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
        List<GameObject> obstacleObjList = new List<GameObject> {t};
        objectListObjects.Add(new Tuple<List<GameObject>,Vector4>(obstacleObjList,new Vector4(x,y,penalty,OConst.normObj)));

    }

    void PlaceSpikeObstacle(int x, int y)
    {
        GameObject t = Instantiate(spikeObstacle, GetCameraCoordinates(x, y), Quaternion.identity);
        // t.transform.localScale = new Vector3(scale * 0.30f, scale * 0.30f, 1);
        GameObject spiketop = t.transform.GetChild(3).gameObject;
        spiketop.tag = "Untagged";
        List<GameObject> spikeObjList = new List<GameObject> {t};
        objectListObjects.Add(new Tuple<List<GameObject>,Vector4>(spikeObjList,new Vector4(x,y,0,OConst.spike)));

    }
    void PlaceSpikeObstacleTwoWide(int x, int y)
    {
        GameObject t = Instantiate(spikeObstacleTwoWide, GetCameraCoordinates(x, y), Quaternion.identity);
        // t.transform.localScale = new Vector3(scale * 0.30f, scale * 0.30f, 1);
        List<GameObject> spikeObjList = new List<GameObject> {t};
        objectListObjects.Add(new Tuple<List<GameObject>,Vector4>(spikeObjList,new Vector4(x,y,0,OConst.spikeTwo)));
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

    void PlacePowerUpWalkThru(int x, int y)
    {
        GameObject t = Instantiate(powerUpWalkThru, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.5f, scale * 0.5f, 1);
        List<GameObject> powerWalkObjList = new List<GameObject> {t};
        objectListObjects.Add(new Tuple<List<GameObject>,Vector4>(powerWalkObjList,new Vector4(x,y,0,OConst.wallkThru)));

    }
    //oneway in 4-direction, dir = 1:UP, 2:DOWN, 3:LEFT, 4:RIGHT
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
        List<GameObject> oneWayDoorObj = new List<GameObject>{oneway};
        objectListObjects.Add(new Tuple<List<GameObject>,Vector4>(oneWayDoorObj,new Vector4(x,y,dir,OConst.oneway)));


    }

    void PlaceBreakableWall(int x, int y)
    {
        GameObject t = Instantiate(breakableWall, GetCameraCoordinates(x, y), Quaternion.identity);
        t.transform.localScale = new Vector3(scale*2f, scale*2f, 1);
        List<GameObject> breakList = new List<GameObject> { t };
        objectListObjects.Add(new Tuple<List<GameObject>, Vector4>(breakList, new Vector4(x, y, 0, OConst.breakableTile)));

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
    //outer tiles for wall of map, prevents walkthruwalls from passing outside map area
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

    
    void setFileClassVars(FileClass file)
    {
        screenHeight = file.screenHeight;
        screenWidth = file.screenWidth;
        gridLength = file.gridLength;
        playerCooridantes = file.playerCooridantes;
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
        file.playerCooridantes = playerCooridantes;
        file.winBlockCoor = winBlockCoor;
        file.target = target;
        file.mazeWallsList = mazeWallsList;
        file.blockList = blockList;
        file.objectList = objectList;
    }
}



