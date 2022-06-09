using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject tile;
    public GameObject player;
    public GameObject block;
    public GameObject obstacle;
    public GameObject winBlock;
    public GameObject spikeObstacle;

    void Start()
    {
        screenWidth = 16;
        screenHeight = Camera.main.orthographicSize * 2;

        gridLength = 10 + 2; // 8 x 8 grid + 1 top(left) wall + 1 bottom(right);

        /* We need to scale the the tiles such that grid fits in camera(screen) */
        scale = Mathf.Min(screenWidth, screenHeight) / gridLength;

        GenerateWalls();
        //GenerateTile(1, 2);
        //GenerateTile(1, 4);
        //GenerateTile(1, 6);
        //GenerateTile(1, 8);

        //GenerateTile(2, 1);
        //GenerateTile(2, 3);
        GenerateTile(2, 5);
        GenerateTile(2, 7);

        GenerateTile(3, 2);
        //GenerateTile(3, 4);
        //GenerateTile(3, 6);
        //GenerateTile(3, 8);

        //GenerateTile(4, 1);
        //GenerateTile(4, 3);
        GenerateTile(4, 5);
        //GenerateTile(4, 7);

        GenerateTile(5, 2);
        GenerateTile(5, 4);
        GenerateTile(5, 6);
        //GenerateTile(5, 8);

        //GenerateTile(6, 1);
        //GenerateTile(6, 3);
        //GenerateTile(6, 5);
        //GenerateTile(6, 7);

        GenerateTile(7, 2);
        //GenerateTile(7, 4);
        //GenerateTile(7, 6);
        //GenerateTile(7, 8);

        //GenerateTile(8, 1);
        //GenerateTile(8, 3);
        //GenerateTile(8, 5);
        //GenerateTile(8, 7);

        GenerateTile(9, 6);
        GenerateTile(9, 8);
        GenerateTile(10, 6);
        GenerateTile(10, 8);

        DrawGridLines();

        GeneratePlayer();
        GenerateBlock(2, 7, 16);
        GenerateBlock(4, 4, 32);
        GenerateBlock(4, 2, 4);
        GenerateBlock(5, 3, 16);
        GenerateBlock(5, 5, 16);
        GenerateBlock(6, 1, 4);
        GenerateBlock(7, 3, 2);
        GenerateBlock(7, 4, 2);
        GenerateBlock(10, 7, 2);
        PlaceObstacle(1, 1, 0.5f);
        PlaceObstacle(10, 1, 0.25f);
        PlaceWinBlock(1, 10);
        PlaceSpikeObstacle(8, 8);


    }

    // Update is called once per frame
    void Update()
    {
    }



    void GeneratePlayer()
    {
        GameObject t = Instantiate(player, GetCameraCoordinates((int)gridLength - 2, (int)gridLength - 2), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.9f, scale * 0.9f, 1);
        var script = t.GetComponent<PlayerController>();
        script.SetScore(2);
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

    void PlaceWinBlock(int x, int y)
    {
        GameObject t = Instantiate(winBlock, GetCameraCoordinates(x, y), Quaternion.identity);
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


}
