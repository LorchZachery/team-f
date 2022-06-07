using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
  [SerializeField] public int width, height;
  
  [SerializeField] private Tile _tilePrefab;
  
  [SerializeField] private Transform _cam;

  [SerializeField] GameObject _blockPrefab;


  [SerializeField] GameObject _playerPrefab;
  [SerializeField] GameObject _newBlock;
  private Dictionary<Vector2, Tile > _tiles; 

  [SerializeField] GameObject _platformObstaclePrefab;

  [SerializeField] GameObject _gameOverScreenPrefab;
  GameObject gameOverScreen;
  GameObject player;

 /* [SerializeField] private Wall _wallPrefab;*/
 
 void Awake(){
   GenerateGrid();
   GenerateBounds();
   GenerateWalls();
   GenerateBlocks();
   placePlayer();
   placeObstacle();
 }
    
    
	
  void GenerateGrid(){
  _tiles = new Dictionary<Vector2, Tile>();
	for (int x =0; x < width; x++){
		for(int y = 0; y < height; y++){
			var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
			spawnedTile.name = $"Tile {x} {y}";

      var isOffset = (x % 2 == 0 && y%2 != 0) || (x %2 != 0 && y %2 ==0 );
      
      spawnedTile.Init(isOffset);
      
      _tiles[new Vector2(x,y)] = spawnedTile;

		}
	}
  _cam.transform.position = new Vector3((float)width/2 - 0.5f, (float)height/2 - 0.5f,-10);
  }

  void GenerateBounds(){
     int StopsMovement = LayerMask.NameToLayer("StopsMovement");
    for(int y= -1; y < height+1; y++){
      Tile spawnedBound = Instantiate(_tilePrefab, new Vector3(-1,y), Quaternion.identity);
      spawnedBound.name = $"Bound -1 {y}";
      spawnedBound.makeBound();
      spawnedBound.gameObject.layer = StopsMovement;
      Rigidbody2D rigid = spawnedBound.gameObject.AddComponent<Rigidbody2D>();
      rigid.bodyType = RigidbodyType2D.Static;
      PolygonCollider2D collider = spawnedBound.gameObject.AddComponent<PolygonCollider2D>();

    }
    for(int y= -1; y < height+1; y++){
      Tile spawnedBound = Instantiate(_tilePrefab, new Vector3(width,y), Quaternion.identity);
      spawnedBound.name = $"Bound {width + 1} {y}";
      spawnedBound.makeBound();
      spawnedBound.gameObject.layer = StopsMovement;
      Rigidbody2D rigid = spawnedBound.gameObject.AddComponent<Rigidbody2D>();
      rigid.bodyType = RigidbodyType2D.Static;
      PolygonCollider2D collider = spawnedBound.gameObject.AddComponent<PolygonCollider2D>();

    }
    for(int x= 0; x < width+1; x++){
      Tile spawnedBound = Instantiate(_tilePrefab, new Vector3(x,-1), Quaternion.identity);
      spawnedBound.name = $"Bound {x} -1";
      spawnedBound.makeBound();
      spawnedBound.gameObject.layer = StopsMovement;
      Rigidbody2D rigid = spawnedBound.gameObject.AddComponent<Rigidbody2D>();
      rigid.bodyType = RigidbodyType2D.Static;
      PolygonCollider2D collider = spawnedBound.gameObject.AddComponent<PolygonCollider2D>();

    }
    for(int x= 0; x < width+1; x++){
      Tile spawnedBound = Instantiate(_tilePrefab, new Vector3(x,height), Quaternion.identity);
      spawnedBound.name = $"Bound {x} {height+1}";
      spawnedBound.makeBound();
      spawnedBound.gameObject.layer = StopsMovement;
      Rigidbody2D rigid = spawnedBound.gameObject.AddComponent<Rigidbody2D>();
      rigid.bodyType = RigidbodyType2D.Static;
      PolygonCollider2D collider = spawnedBound.gameObject.AddComponent<PolygonCollider2D>();

    }
  }

  void GenerateWalls(){
      List<Vector2> positions = new List<Vector2>() {new Vector2(1,2), new Vector2(2,2), new Vector2(3,2),new Vector2(3,3),new Vector2(3,4),new Vector2(3,5),
      new Vector2(4,4)};
      foreach(Vector2 pos in positions){
        WallMaker(pos);
      }



  }
  public void WallMaker(Vector2 pos){
    int StopsMovement = LayerMask.NameToLayer("StopsMovement");

    Tile _newWall = GetTileAtPosition(pos);
    _newWall.makeWall();
     _newWall.name = $"Wall {pos[0]} {pos[1]}";
     _newWall.gameObject.layer = StopsMovement;
    Rigidbody2D rigid = _newWall.gameObject.AddComponent<Rigidbody2D>();
    rigid.bodyType = RigidbodyType2D.Static;
    

    PolygonCollider2D collider = _newWall.gameObject.AddComponent<PolygonCollider2D>();
  }
  void GenerateBlocks(){
    
    List<Tuple<Vector3,string>> blocks = new List<Tuple<Vector3, string>> { new Tuple<Vector3, String> (new Vector3(6,6),"2"),
    new Tuple<Vector3, String> (new Vector3(6,4),"4"),new Tuple<Vector3, String> (new Vector3(2,5),"2"), new Tuple<Vector3, String> (new Vector3(7,6),"2"),
    new Tuple<Vector3, String> (new Vector3(1,7),"8"), new Tuple<Vector3, String> (new Vector3(4,6),"16")};

    foreach( Tuple<Vector3,string> pair in blocks ){
         BlockMaker(pair.Item1,pair.Item2);
    }
  
     

  }

  public void BlockMaker(Vector3 pos, string value){
    GameObject spawnedBlock4 = Instantiate(_blockPrefab, pos, Quaternion.identity) as GameObject;
            
     
      GameObject textObj = spawnedBlock4.transform.GetChild(0).gameObject;
      TextMeshPro mytext = textObj.GetComponent<TextMeshPro>();
      spawnedBlock4.AddComponent<Merge>();
      Merge script2  = spawnedBlock4.GetComponent<Merge>();
     
      script2.MergedObject =  _newBlock;
      mytext.text = value;
     
  }

  void placePlayer(){
    player = Instantiate(_playerPrefab, new Vector3(5,5), Quaternion.identity);
    player.name = "Player";
  }

  void placeObstacle() {
    var platformObstacle = Instantiate(_platformObstaclePrefab, new Vector3(5,1), Quaternion.identity);
    platformObstacle.name = "PlatformObstacle";

    // int StopsMovement = LayerMask.NameToLayer("StopsMovement");
    // platformObstacle.gameObject.layer = StopsMovement;
    // Rigidbody2D rigid = platformObstacle.gameObject.AddComponent<Rigidbody2D>();
    // rigid.bodyType = RigidbodyType2D.Static;
    // PolygonCollider2D collider = platformObstacle.gameObject.AddComponent<PolygonCollider2D>();
  }

  public Tile GetTileAtPosition(Vector2 pos){
    if(_tiles.TryGetValue(pos, out var tile)) {
      return tile;
    }
    return null; 
  }

  // Function that triggers game over screen
  public void gameOver(int score) {
    Debug.Log("BACK IN GRID MANAGER");
    gameOverScreen = Instantiate(_gameOverScreenPrefab, new Vector3(3.5f,3.5f), Quaternion.identity);
    gameOverScreen.name = "GameOverScreen";
  }

  // Function that restarts game
  public void RestartButton() {
    // Deletes all game objects other than camera
    GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
    Debug.Log("LENGTH OF ALL OBJECTS LIST " + allObjects.Length);
    foreach(GameObject go in allObjects) {
      if(!go.GetComponent<Camera>() && !go.GetComponent<Button>())
       Destroy(go); 
    }

    // Generating Grid again without adjusting camera
    _tiles = new Dictionary<Vector2, Tile>();
    for (int x =0; x < width; x++){
      for(int y = 0; y < height; y++){
        var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
        spawnedTile.name = $"Tile {x} {y}";

        var isOffset = (x % 2 == 0 && y%2 != 0) || (x %2 != 0 && y %2 ==0 );
        
        spawnedTile.Init(isOffset);
        
        _tiles[new Vector2(x,y)] = spawnedTile;

      }
    }
    // Re-generating rest of grid
    GenerateBounds();
    GenerateWalls();
    GenerateBlocks();
    placePlayer();
    placeObstacle();
  }
   
}
