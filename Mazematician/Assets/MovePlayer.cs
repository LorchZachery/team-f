using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovePlayer : MonoBehaviour
{
    int ID;
    //int counter = 2;
    float x;
    float y;
    int ballSpeed = 7;
    //TextMeshPro score;


    //transform.position += new Vector3(x, y, 0) * isDiagonal* Time.deltaTime;
    // Start is called before the first frame update test
    void Start()
    {
        ID = GetInstanceID();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        int isDiagonal = x * y != 0 ? 0 : 1;
        //counter += 2;
        //score.text = counter.ToString();

        GetComponent<Rigidbody2D>().velocity = new Vector2(x * ballSpeed * isDiagonal, y * ballSpeed * isDiagonal);
        //GetComponent<Rigidbody2D>().transform.position += new Vector3(x, y, 0) * isDiagonal * Time.deltaTime * ballSpeed;
    }
}
