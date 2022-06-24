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






public class FileClass
{
    public float screenHeight;
    public float screenWidth; 
    public float gridLength;
    public Vector2 playerCooridantes;
    public Vector2 winBlockCoor;
    public int target;
    public List<MazeWall> mazeWallsList = new List<MazeWall>();
    public List<Vector3> blockList = new List<Vector3>();
    //(x,y,penility,type)
    public List<Vector4> objectList = new List<Vector4>();


   public void ReadFile(string level)
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
                            MazeWall temp = new MazeWall(Int32.Parse(values[0]), Int32.Parse(values[1]));
                        
                            if(values[2] == "0"){
                                temp.removeWall();
                            }
                            mazeWallsList.Add(temp);
                        }

                    }
                    if(line == "MazeBlocks")
                    {
                        
                        while((line = sr.ReadLine()) != "Objects")
                        {
                            string[] values = line.Split(',');
                            blockList.Add(new Vector3(Int32.Parse(values[0]),Int32.Parse(values[1]),Int32.Parse(values[2])));
                        }
                    }
                    if(line == "Objects")
                    {
                        while((line =sr.ReadLine()) != "END")
                        {
                            string[] values = line.Split(',');
                            objectList.Add(new Vector4(Int32.Parse(values[0]),Int32.Parse(values[1]),float.Parse(values[2]),Int32.Parse(values[3])));
                        }
                    }
                }
        }
       
    }

    public void ReadTextAsset(string[] level)
    {



        int i = 0;
        string line;
        // Read and display lines from the file until the end of
        // the file is reached.
        while (i < level.Length)
        {
            line = level[i++];
            if (line == "width height gridlength")
            {
                line = level[i++];
                string[] values = line.Split(',');
                screenWidth = float.Parse(values[0]);
                screenHeight = float.Parse(values[1]);
                gridLength = float.Parse(values[2]);
            }
            if (line == "playerCoor")
            {
                line = level[i++];
                string[] values = line.Split(',');
                playerCooridantes = new Vector2(float.Parse(values[0]), float.Parse(values[1]));
            }
            if (line == "winBlock")
            {
                line = level[i++];
                string[] values = line.Split(',');
                winBlockCoor = new Vector2(float.Parse(values[0]), float.Parse(values[1]));
                target = Int32.Parse(values[2]);
            }
            if (line == "MazeWalls")
            {

                while ((line = level[i++]) != "MazeBlocks")
                {

                    string[] values = line.Split(',');
                    MazeWall temp = new MazeWall(Int32.Parse(values[0]), Int32.Parse(values[1]));

                    if (values[2] == "0")
                    {
                        temp.removeWall();
                    }
                    mazeWallsList.Add(temp);
                }

            }
            if (line == "MazeBlocks")
            {

                while ((line = level[i++]) != "Objects")
                {
                    string[] values = line.Split(',');
                    blockList.Add(new Vector3(Int32.Parse(values[0]), Int32.Parse(values[1]), Int32.Parse(values[2])));
                }
            }
            if (line == "Objects")
            {
                while ((line = level[i++]) != "END")
                {
                    string[] values = line.Split(',');
                    objectList.Add(new Vector4(Int32.Parse(values[0]), Int32.Parse(values[1]), float.Parse(values[2]), Int32.Parse(values[3])));
                }
            }

        }

    }

    public void writeToFile(string LevelName)
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
         foreach (MazeWall tile in mazeWallsList)
        {
            if (tile.isWall())
            {
                writer.WriteLine($"{tile.x},{tile.y},1");
            }else{
                writer.WriteLine($"{tile.x},{tile.y},0");
            }

        }
        writer.WriteLine("MazeBlocks");
        foreach(Vector3 block in blockList){
            writer.WriteLine($"{block[0]},{block[1]},{block[2]}");
        }
        writer.WriteLine("Objects");
        foreach(Vector4 obj in objectList){
            writer.WriteLine($"{obj[0]},{obj[1]},{obj[2]},{obj[3]}");
        }
        writer.WriteLine("END");
        writer.Close();

        
       
    }
}
