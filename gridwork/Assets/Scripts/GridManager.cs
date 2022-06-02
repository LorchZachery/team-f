using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridManager : MonoBehaviour
{
  [SerializeField] public int width, height;
  
  [SerializeField] private Tile _tilePrefab;
  
  [SerializeField] private Transform _cam;

  [SerializeField] GameObject _blockPrefab;


  [SerializeField] GameObject _playerPrefab;
  [SerializeField] GameObject _newBlock;
  private Dictionary<Vector2, Tile > _tiles; 


 /* [SerializeField] private Wall _wallPrefab;*/
 
 void Awake(){
   GenerateGrid();
   GenerateWalls();
   GenerateBlocks();
   placePlayer();
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


  void GenerateWalls(){
       int StopsMovement = LayerMask.NameToLayer("StopsMovement");

    Tile _newWall = GetTileAtPosition(new Vector2(2,2));
    _newWall.makeWall();
     _newWall.name = $"Wall 2 2";
     _newWall.gameObject.layer = StopsMovement;
    Rigidbody2D rigid = _newWall.gameObject.AddComponent<Rigidbody2D>();
    rigid.bodyType = RigidbodyType2D.Static;
    

    PolygonCollider2D collider = _newWall.gameObject.AddComponent<PolygonCollider2D>();


  }

  void GenerateBlocks(){
    

      GameObject spawnedBlock = Instantiate(_blockPrefab, new Vector3(1,1), Quaternion.identity) as GameObject;
      spawnedBlock.name = "Block2";
      spawnedBlock.AddComponent<Merge>();
      Merge script  = spawnedBlock.GetComponent<Merge>();
     
      script.MergedObject = _newBlock;
      
      GameObject spawnedBlock4 = Instantiate(_blockPrefab, new Vector3(6,6), Quaternion.identity) as GameObject;
            
      spawnedBlock4.name = "Block4";
      GameObject textObj = spawnedBlock4.transform.GetChild(0).gameObject;
      TextMeshPro mytext = textObj.GetComponent<TextMeshPro>();
      spawnedBlock4.AddComponent<Merge>();
      Merge script2  = spawnedBlock4.GetComponent<Merge>();
     
      script2.MergedObject =  _newBlock;
      mytext.text = "4";
     

  }

  void placePlayer(){
    var player = Instantiate(_playerPrefab, new Vector3(5,5), Quaternion.identity);
    player.name = "Player";
  }

  public Tile GetTileAtPosition(Vector2 pos){
    if(_tiles.TryGetValue(pos, out var tile)) {
      return tile;
    }
    return null; 
  }
   
}
