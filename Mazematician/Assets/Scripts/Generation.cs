using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWall
{
    public int x;
    public int y;
    private bool visited =false;
    private bool aWall = true;
    private bool aBlock = false;
    public MazeWall(int x_set, int y_set){
        this.x = x_set;
        this.y =y_set;
    }
    public bool isVisited(){
        return this.visited;
    }
    
    public void setVisited(){
        this.visited = true;
    }
     public bool isWall(){
        return this.aWall;
    }
    public void removeWall(){
        this.aWall = false;
    }
   public void setBlock(){
       this.aBlock = true;
   }
   public bool isBlock(){
       return this.aBlock;
   }
}

public class Generator
{
   public System.Random random = new System.Random(); 
   private List<MazeWall> mazeWallsList = new List<MazeWall>();
   private float gridLength;
   private float screenWidth;

    public Generator(float gl, float sw)
    {
        this.gridLength = gl;
        this.screenWidth = sw;
    }

   public List<MazeWall> MazeGenerator(){
        
        //inilizing list of maze wall objects
        for(int x =1; x < screenWidth-5; x++){
            for(int y = 1; y < gridLength-1; y++){
                 mazeWallsList.Add(new MazeWall(x,y) );
                 
            }
        }
       
        

        Queue<MazeWall> Q = new Queue<MazeWall>();
        //currently starting maze generation from where the player is starting to allow the player never to
        //be placed on a wall, can probably change this down the line and just have a check for when 
        //player is placed

        MazeWall N = mazeWallsList.Find(r => r.x == (int)gridLength -2 && r.y == (int)gridLength -2);
        
       
        //Algo for creating maze, following DFS generation
        int spacesCount = 0;
        bool exit = false;
        while(true){
           Q.Enqueue(N);
           N.setVisited();
           while(checkVisitedNeighbors(N)){
               if(Q.Count == 0){
                   exit = true;
                    break;
               }
               N = Q.Dequeue();
              N.setVisited();
            
           }
           if(exit){
               break;
           }
           
              
            MazeWall next = selectNeighbor(N);
            if(N.isWall()){ 
                    N.removeWall();
                    //need because the center of the blocks are the coordinates not the corners
                    translationWork(N,next);
                    
                   spacesCount++;
                
            }
            N = next;
            
        }
        
        
       
      return mazeWallsList; 
    }

    private void translationWork(MazeWall N, MazeWall next)
    {
        MazeWall nextDoor = null;
                    if(N.x - next.x > 0){
                        nextDoor = mazeWallsList.Find(r => r.x == next.x-1 &&  r.y == next.y+1);
                        if(nextDoor != null){
                            nextDoor.setVisited();
                        }
                    }
                    else if(N.x - next.x < 0){
                        nextDoor = mazeWallsList.Find(r => r.x == next.x+1 &&  r.y == next.y-1);
                        if(nextDoor != null){
                            nextDoor.setVisited();
                        }
                    }
                    else if(N.y - next.y > 0){
                        nextDoor = mazeWallsList.Find(r => r.x ==next.x+1 && r.y ==next.y-1);
                        if(nextDoor != null){
                            nextDoor.setVisited();
                        }
                    }
                    else if(N.y - next.y < 0){
                        nextDoor = mazeWallsList.Find(r => r.x ==next.x-1 && r.y ==next.y+1);
                       if(nextDoor != null){
                            nextDoor.setVisited();
                        }
                    }
    }
  
   private bool checkVisitedNeighbors(MazeWall N)
    {
        MazeWall neighbor = mazeWallsList.Find(r => r.x == N.x+1 && r.y == N.y);
        if(neighbor != null){
            if(!neighbor.isVisited()){
                return false;
            }
        } 
        neighbor =mazeWallsList.Find(r => r.x == N.x-1 && r.y == N.y);
        if(neighbor != null){
            if(!neighbor.isVisited()){
                return false;
            }
        } 
        neighbor =mazeWallsList.Find(r => r.x == N.x && r.y == N.y+1);
        if(neighbor != null){
            if(!neighbor.isVisited()){
                return false;
            }
        } 
        neighbor =mazeWallsList.Find(r => r.x == N.x && r.y == N.y-1);
        if(neighbor != null){
            if(!neighbor.isVisited()){
                return false;
            }
        } 
        return true;
    }
    
    private MazeWall selectNeighbor(MazeWall N)
    {
      
        int possibleDirection = random.Next(4);
        MazeWall possibleNext = getNext(N, possibleDirection);
       
        if(possibleNext != null){
        if(!possibleNext.isVisited())
            {
                return possibleNext;
            }
        }
        int index = 1;
        
        while(index < 4){
            possibleDirection  = (index + possibleDirection) % 4;
           
            possibleNext = getNext(N,possibleDirection);
            
            if(possibleNext != null){
                if(!possibleNext.isVisited())
                {
                    return possibleNext;
                }
            }
            index++;
        }
        return null;
        
    }
    public MazeWall getNext(MazeWall N, int direction){
        if(direction == 0){
            return mazeWallsList.Find(r=> r.x == N.x+1 && r.y == N.y);
        }
        if(direction == 1){
            return mazeWallsList.Find(r=> r.x == N.x-1 && r.y == N.y);
        }
        if(direction == 2){
            return mazeWallsList.Find(r=> r.x == N.x && r.y == N.y+1);
        }
        if(direction == 3){
            return mazeWallsList.Find(r=> r.x == N.x && r.y == N.y-1);
        }
        return null;
    }
}
