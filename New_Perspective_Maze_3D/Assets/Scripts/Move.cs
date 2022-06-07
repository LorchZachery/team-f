using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Move : MonoBehaviour
{
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private float speed = 2.5f;
    public GameObject textobj;
    public TextMeshPro mytext;
    public GameObject textobj2;
    public TextMeshPro mytext2;
    public GameObject textobj3;
    public TextMeshPro mytext3;
    private Vector3 _lastPosition;
    private Vector3 currentPosition;
    private int count = 0;
    public float timeRemaining = 30;
    public bool timerIsRunning = false;
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = 0.2f;
    
    //[SerializeField]
    //public GameObject MergedObject;
    //Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _lastPosition = gameObject.transform.position;
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        //horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");
        //currentPosition = transform.position;
        //checkLogs(currentPosition, _lastPosition);
        //_lastPosition = transform.position;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        //Debug.Log(string.Format("Current Time: {0:00}:{1:00}", minutes, seconds));
    }

    private void checkLogs(Vector3 currentPosition, Vector3 _lastPosition)
    {
        float diff = _lastPosition.x - currentPosition.x;
        
        if (Vector3.Distance(currentPosition, _lastPosition) >= 1f)
        {
            Debug.Log("Success! - Difference: " + diff);
            Debug.Log("Count: " + count);
            count++;
        }
        //Debug.Log("Current Position: " + currentPosition.x);
        //Debug.Log("Last Position: " + _lastPosition.x);
        //float diff = _lastPosition.x - currentPosition.x;
        //Debug.Log("Difference:" + diff);
    }

    private void FixedUpdate()
    {
        //if (Input.GetKey(KeyCode.W) && !isMoving)
        //{
        //    rb.AddForce(new Vector3(0.0f, 0.0f, 1.0f));
        //    StartCoroutine(MovePlayer(new Vector3(0,0,1.0f)));
        //}
        //if (Input.GetKey(KeyCode.A) && !isMoving)
        //{
        //    rb.AddForce(new Vector3(-1.0f, 0.0f, 0.0f));
        //    StartCoroutine(MovePlayer(Vector3.left));
        //}
        //if (Input.GetKey(KeyCode.S) && !isMoving)
        //{
        //    rb.AddForce(new Vector3(0.0f, 0.0f, -1.0f));
        //    StartCoroutine(MovePlayer(new Vector3(0, 0, -1.0f)));
        //}
        //if (Input.GetKey(KeyCode.D) && !isMoving)
        //{
        //    rb.AddForce(new Vector3(1.0f, 0.0f, 0.0f));
        //    StartCoroutine(MovePlayer(Vector3.right));
        //}




        //Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput);
        //transform.Translate(movement * speed * Time.deltaTime);
        //rb.position += new Vector3(horizontalInput, 0.0f, verticalInput);
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        int isDiagonal = horizontalInput * verticalInput != 0 ? 0 : 1;
        //rb.velocity = new Vector3(horizontalInput * isDiagonal * speed, 0.0f, verticalInput * isDiagonal * speed);
        rb.AddForce(new Vector3(horizontalInput * isDiagonal, 0.0f, verticalInput * isDiagonal) * speed);
        currentPosition = transform.position;
        checkLogs(currentPosition, _lastPosition);
        _lastPosition = transform.position;

        //if (timerIsRunning)
        //{
        //    if (timeRemaining > 0)
        //    {
        //        timeRemaining -= Time.deltaTime;
        //        DisplayTime(timeRemaining);
        //    }
        //    else
        //    {
        //        Debug.Log("Time has run out!");
        //        timeRemaining = 0;
        //        timerIsRunning = false;
        //    }
        //}
        //float diff = _lastPosition.x - currentPosition.x;
        //Debug.Log("Difference:" + diff);
        //if (Mathf.Approximately(currentPosition.x - _lastPosition.x, 0.03f))
        //{
        //    Debug.Log("Success!");
        //}


    }

    //private IEnumerator MovePlayer(Vector3 direction)
    //{
    //    isMoving = true;

    //    float elapsedTime = 0;

    //    origPos = transform.position;
    //    targetPos = origPos + direction;

    //    while (elapsedTime < timeToMove)
    //    {
    //        transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    transform.position = targetPos;
    //    rb.AddForce(transform.position);
    //    isMoving = false;
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ball"))
    //    {
    //        textobj = collision.gameObject.transform.GetChild(0).gameObject;
    //        mytext = textobj.GetComponent<TextMeshPro>();
    //        textobj2 = gameObject.transform.GetChild(0).gameObject;
    //        mytext2 = textobj2.GetComponent<TextMeshPro>();
    //        if (mytext.text != null && mytext2.text != null && mytext.text.Equals(mytext2.text))
    //        {
    //            Debug.Log("Success!!");
    //            Debug.Log("collided object" + collision.gameObject.tag);
    //            Debug.Log("Other object" + gameObject.tag);

    //            GameObject O = Instantiate(MergedObject, transform.position, Quaternion.identity) as GameObject;
    //            textobj3 = O.transform.GetChild(0).gameObject;
    //            mytext3 = textobj3.GetComponent<TextMeshPro>();
    //            mytext3.text = (Int32.Parse(mytext.text) + Int32.Parse(mytext2.text)).ToString();
    //            Destroy(collision.gameObject);
    //            Destroy(gameObject);
    //        }
    //    }
    //    //horizontalInput = Input.GetAxis("Horizontal");
    //    //verticalInput = Input.GetAxis("Vertical");
    //}
}
