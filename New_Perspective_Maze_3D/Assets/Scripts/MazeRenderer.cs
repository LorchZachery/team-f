using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class MazeRenderer : MonoBehaviour
{

    [SerializeField]
    [Range(1,50)]
    private int width = 5;

    [SerializeField]
    [Range(1, 50)]
    private int height = 5;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform wallPrefab = null;

    [SerializeField]
    private Transform floorPrefab = null;

    [SerializeField]
    private GameObject player = null;


    public GameObject textobj;
    public TextMeshPro mytext;
    public GameObject textobj2;
    public TextMeshPro mytext2;
    public GameObject textobj3;
    public TextMeshPro mytext3;
    public GameObject textobj4;
    public TextMeshPro mytext4;
    public GameObject MergedObject;
    private float rotY;
    private float time = 0;
    private int[] numbers = { 8, 2, 2, 4, 8, 2, 4, 4, 8};
    private List<GameObject> gameObjList;
    private bool canRandomize = false;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(4.9f, 0, 0);
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
        RenderFloor(maze);
        PositionBalls(maze);
    }

    private int[] Shuffle(int[] a)
    {
        // Loops through array
        for (int i = a.Length - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = UnityEngine.Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overright when we swap the values
            int temp = a[i];

            // Swap the new and old values
            a[i] = a[rnd];
            a[rnd] = temp;
        }

        // Print
        for (int i = 0; i < a.Length; i++)
        {
            Debug.Log(a[i]);
        }

        return a;
    }

    private void RandomizeBallNumbers(int[] numbers, List<GameObject> gameObjList)
    {
        
            numbers = Shuffle(numbers);

            for (int i = 0; i < 8; i++)
            {
                textobj3 = gameObjList[i].transform.GetChild(0).gameObject;
                mytext3 = textobj3.GetComponent<TextMeshPro>();
                mytext3.text = numbers[i].ToString();
            }
        
    }
    private void PositionBalls(WallState[,] maze)
    {
        GameObject ball = (GameObject)Instantiate(Resources.Load("ball"));
        
        int[] x_num = { 2, 3, 5, 1, 6, 7, 4, 0, 3 };
        int[] y_num = { 7, 6, 5, 3, 4, 2, 1, 0, 4 };
        //int[] numbers = { 8, 2, 2, 4, 4 };
        x_num = Shuffle(x_num);
        //y_num = Shuffle(y_num);
        List<int> x_list = new List<int>(x_num);
        List<int> y_list = new List<int>(y_num);
        numbers = Shuffle(numbers);
        List<int> num_list = new List<int>(numbers);
        gameObjList = new List<GameObject>(8);
        //numbers = Shuffle(numbers);
        for (int i = 0; i < 9; i++)
        {
            GameObject player = (GameObject)Instantiate(ball, transform);
            //Rigidbody rb = player.AddComponent<Rigidbody>();
            //rb.mass = 0.5f;
            MergedObject = player;
            textobj3 = MergedObject.transform.GetChild(0).gameObject;
            mytext3 = textobj3.GetComponent<TextMeshPro>();
            mytext3.text = numbers[i].ToString();
            textobj4 = MergedObject.transform.GetChild(1).gameObject;
            mytext4 = textobj4.GetComponent<TextMeshPro>();
            mytext4.text = mytext3.text;
            gameObjList.Add(MergedObject);
            if (mytext3.text == mytext4.text)
            {
                Debug.Log("Success for: " + mytext3.text);
            }
            MergedObject.transform.position = new Vector3(-width / 2 + x_list[i], 0, -height / 2 + y_list[i]);
        }

        Debug.Log("time in positionball:" + time);

        

        
        

        //GameObject ball = (GameObject)Instantiate(Resources.Load("ball"));
        //List<int> x_list = new List<int>();
        //List<int> y_list = new List<int>();
        //int rand = 0;
        //x_list = new List<int>(new int[10]);
        //y_list = new List<int>(new int[10]);
        //for (int i = 0; i < 3; ++i)
        //{
        //    //GameObject player = (GameObject)Instantiate(ball, transform);
        //    //int x_pos = Random.Range(0, 10);
        //    //int z_pos = Random.Range(0, 10);
        //    rand = Random.Range(0, 10);

        //    while (x_list.Contains(rand))
        //    {
        //        rand = Random.Range(1, 10);
        //    }

        //    x_list[i] = rand;
        //    //if (x_pos != 3 && z_pos != 4)
        //    //{
        //    //    player.transform.position = new Vector3(-width / 2 + x_pos, 0, -height / 2 + z_pos);
        //    //}     
        //}

        //for (int j = 0; j < 3; ++j)
        //{
        //    rand = Random.Range(0, 10);

        //    while (y_list.Contains(rand))
        //    {
        //        rand = Random.Range(1, 10);

        //    }
        //    y_list[j] = rand;
        //}

        //for (int k = 0; k < 3; k++)
        //{
        //    GameObject player = (GameObject)Instantiate(ball, transform);
        //    if (x_list[k] != 4 && y_list[k] != 4)
        //    {
        //        Debug.Log("Indices:" + x_list[k] + "," + y_list[k]);
        //        player.transform.position = new Vector3(-width / 2 + x_list[k], 0, -height / 2 + y_list[k]);
        //    }

        //}

    }

    private void RenderFloor(WallState[,] maze)
    {
        GameObject tileBase = (GameObject)Instantiate(Resources.Load("Cube"));
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                GameObject tile = (GameObject)Instantiate(tileBase, transform);
                tile.transform.position = new Vector3(-width / 2 + i, 0, -height / 2 + j); 
            }
        }
    }
    private void Draw(WallState[,] maze)
    {

        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width, 1, height);
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                if (cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size/2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(wallPrefab, transform) as Transform;
                        rightWall.position = position + new Vector3(size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                        bottomWall.position = position + new Vector3(0, 0, -size / 2);
                        bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= 90)
        {
            time += Time.deltaTime;
        }
        if (time >= 10 && time < 15)
        {
            rotY += -Time.deltaTime * 18.0f;
            transform.rotation = Quaternion.Euler(0, rotY, 0);
            //RandomizeBallNumbers(numbers, gameObjList);
            //return;
        }
        if (time >= 20 && time < 15)
        {
            rotY += -Time.deltaTime * 18.0f;
            transform.rotation = Quaternion.Euler(0, rotY, 0);
            //RandomizeBallNumbers(numbers, gameObjList);
            //return;
        }
        if (time >= 30 && time < 35)
        {
            rotY += -Time.deltaTime * 18.0f;
            transform.rotation = Quaternion.Euler(0, rotY, 0);
        }
        if (time >= 40 && time < 45)
        {
            rotY += -Time.deltaTime * 18.0f;
            transform.rotation = Quaternion.Euler(0, rotY, 0);
            //RandomizeBallNumbers(numbers, gameObjList);
            //return;
        }
        if (time >= 50 && time < 55)
        {
            rotY += -Time.deltaTime * 18.0f;
            transform.rotation = Quaternion.Euler(0, rotY, 0);
        }
        if (time >= 60 && time < 65)
        {
            rotY += -Time.deltaTime * 18.0f;
            transform.rotation = Quaternion.Euler(0, rotY, 0);
            //RandomizeBallNumbers(numbers, gameObjList);
            //return;
        }
        if (time >= 70 && time < 75)
        {
            rotY += -Time.deltaTime * 18.0f;
            transform.rotation = Quaternion.Euler(0, rotY, 0);
        }
        if (time >= 80 && time < 85)
        {
            rotY += -Time.deltaTime * 18.0f;
            transform.rotation = Quaternion.Euler(0, rotY, 0);
            //RandomizeBallNumbers(numbers, gameObjList);
            //return;
        }
        //if (time >= 90 && time < 95)
        //{
        //    rotY += -Time.deltaTime * 18.0f;
        //    transform.rotation = Quaternion.Euler(0, rotY, 0);
        //}


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ball"))
        {
            textobj = collision.gameObject.transform.GetChild(0).gameObject;
            mytext = textobj.GetComponent<TextMeshPro>();
            textobj2 = gameObject.transform.GetChild(0).gameObject;
            mytext2 = textobj2.GetComponent<TextMeshPro>();
            if (mytext.text != null && mytext2.text != null && mytext.text.Equals(mytext2.text))
            {
                Debug.Log("Success!!");
                GameObject O = Instantiate(MergedObject, transform.position, Quaternion.identity) as GameObject;
                textobj3 = O.transform.GetChild(0).gameObject;
                mytext3 = textobj3.GetComponent<TextMeshPro>();
                mytext3.text = (Int32.Parse(mytext.text) + Int32.Parse(mytext2.text)).ToString();
                textobj4 = O.transform.GetChild(1).gameObject;
                mytext4 = textobj4.GetComponent<TextMeshPro>();
                mytext4.text = mytext3.text;
                //O.AddComponent<CameraMove>();
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
    }
}
