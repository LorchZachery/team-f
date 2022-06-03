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
    public GameObject obstacle;

    public Transform camera;

    void Start()
    {
        screenWidth = 16;
        screenHeight = Camera.main.orthographicSize * 2;

        gridLength = 8 + 2; // 8 x 8 grid + 1 top(left) wall + 1 bottom(right);

        /* We need to scale the the tiles such that grid fits in camera(screen) */
        scale = Mathf.Min(screenWidth, screenHeight) / gridLength;

        GenerateWalls();
        //GenerateBlock(1, 2);
        //GenerateBlock(1, 4);
        //GenerateBlock(1, 6);
        //GenerateBlock(1, 8);

        //GenerateBlock(2, 1);
        GenerateBlock(2, 3);
        GenerateBlock(2, 5);
        GenerateBlock(2, 7);

        GenerateBlock(3, 2);
        GenerateBlock(3, 4);
        GenerateBlock(3, 6);
        //GenerateBlock(3, 8);

        //GenerateBlock(4, 1);
        GenerateBlock(4, 3);
        GenerateBlock(4, 5);
        GenerateBlock(4, 7);

        GenerateBlock(5, 2);
        GenerateBlock(5, 4);
        GenerateBlock(5, 6);
        //GenerateBlock(5, 8);

        //GenerateBlock(6, 1);
        GenerateBlock(6, 3);
        GenerateBlock(6, 5);
        GenerateBlock(6, 7);

        GenerateBlock(7, 2);
        GenerateBlock(7, 4);
        GenerateBlock(7, 6);
        //GenerateBlock(7, 8);

        //GenerateBlock(8, 1);
        //GenerateBlock(8, 3);
        //GenerateBlock(8, 5);
        //GenerateBlock(8, 7);

        DrawGridLines();

        GeneratePlayer();
        GenerateObstacle();
    }

    // Update is called once per frame
    void Update()
    {
    }

    

    void GeneratePlayer()
    {
        GameObject t = Instantiate(player, GetCameraCoordinates((int) gridLength-2, (int)gridLength-2), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.9f, scale * 0.9f, 1);
        t.AddComponent<MovePlayer>();
    }

    void GenerateObstacle()
    {
        GameObject t = Instantiate(obstacle, GetCameraCoordinates(1, 1), Quaternion.identity);
        t.transform.localScale = new Vector3(scale * 0.9f, scale * 0.9f, 1);

        GameObject t1 = Instantiate(obstacle, GetCameraCoordinates(2, 2), Quaternion.identity);
        t1.transform.localScale = new Vector3(scale * 0.9f, scale * 0.9f, 1);
    }


    void GenerateWalls()
    {
        for (int i = 0; i < 10; i++)
        {
            //top : x = 0, y = i
            GameObject t = Instantiate(tile, GetCameraCoordinates(0, i), Quaternion.identity);
            t.transform.localScale = new Vector3(scale, scale, 1);

            //bottom: x = 9, y = i
            GameObject t2 = Instantiate(tile, GetCameraCoordinates(9, i), Quaternion.identity);
            t2.transform.localScale = new Vector3(scale, scale, 1);


        }

        for(int i = 1; i < 9; i++)
        {
            //left x = i, y = 0
            GameObject t = Instantiate(tile, GetCameraCoordinates(i, 0), Quaternion.identity);
            t.transform.localScale = new Vector3(scale, scale, 1);

            //right x = i, y = 9
            GameObject t2 = Instantiate(tile, GetCameraCoordinates(i, 9), Quaternion.identity);
            t2.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    void GenerateWalls1()
    {

        Debug.Log(scale);
        for (int i = 0; i < 10; i++)
        {
            //generate top and bottom walls
            //top : x = 0, y = i
            //bottom: x = 9, y = i
            GameObject t = Instantiate(tile, new Vector3(0, i * scale), Quaternion.identity);
            t.transform.localScale = new Vector3(scale, scale, 1);

            GameObject t2 = Instantiate(tile, new Vector3(9 * scale, i * scale), Quaternion.identity);
            t2.transform.localScale = new Vector3(scale, scale, 1);


        }

        for (int i = 1; i < 9; i++)
        {
            //todo generate side walls
            //left x = i, y = 0
            //right x = i, y = 9

            GameObject t = Instantiate(tile, new Vector3(i * scale, 0), Quaternion.identity);
            t.transform.localScale = new Vector3(scale, scale, 1);

            GameObject t2 = Instantiate(tile, new Vector3(i * scale, 9 * scale), Quaternion.identity);
            t2.transform.localScale = new Vector3(scale, scale, 1);
        }

        /* Centering the camera */
        camera.transform.position = new Vector3((gridLength / 2 * scale) - (scale / 2), (gridLength / 2 * scale) - (scale / 2), -10);
    }

    void DrawGridLines()
    {
        for(int i = 0; i < 9; i++)
        {
            Debug.DrawLine(GetCameraCoordinates(i, 0, 2), GetCameraCoordinates(i, 8, 2), Color.green, 1000f);
            Debug.DrawLine(GetCameraCoordinates(0, i, 2), GetCameraCoordinates(8, i, 2), Color.green, 1000f);
        }
        
    }

    /*
     * Generate grid based on coordinates
     */
    void GenerateBlock(int x, int y)
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
        return new Vector3(cartesianX + (0.5f*scale), cartesianY - (0.5f*scale), z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("obstacle"))
        {
            Destroy(collision.gameObject);
        }
    }
}
