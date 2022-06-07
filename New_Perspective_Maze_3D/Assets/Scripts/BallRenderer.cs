using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class BallRenderer : MonoBehaviour
{
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private float speed = 2.0f;
    public GameObject textobj;
    public TextMeshPro mytext;
    public GameObject textobj2;
    public TextMeshPro mytext2;
    public GameObject textobj3;
    public TextMeshPro mytext3;
    public GameObject textobj4;
    public TextMeshPro mytext4;
    public GameObject MergedObject;
    public CameraMove camera;
    public String winningTotal;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        winningTotal = "16";
    }

    // Update is called once per frame
    void Update()
    {
        //horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");
        Physics.gravity = new Vector3(0, 0, -9.8f);

    }

    private void FixedUpdate()
    {
        //Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput);
        //transform.Translate(movement * speed * Time.deltaTime);
        //rb.position += new Vector3(horizontalInput, 0.0f, verticalInput);
        //horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");
        //int isDiagonal = horizontalInput * verticalInput != 0 ? 0 : 1;
        ////rb.velocity = new Vector3(horizontalInput * isDiagonal * speed, 0.0f, verticalInput * isDiagonal * speed);
        //rb.AddForce(new Vector3(horizontalInput * isDiagonal, 0.0f, verticalInput * isDiagonal) * speed);
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
                //GameObject O = Instantiate(MergedObject, transform.position, Quaternion.identity) as GameObject;
                GameObject O = collision.gameObject;
                textobj3 = O.transform.GetChild(0).gameObject;
                mytext3 = textobj3.GetComponent<TextMeshPro>();
                textobj4 = O.transform.GetChild(1).gameObject;
                mytext4 = textobj4.GetComponent<TextMeshPro>();
                int num = Int32.Parse(mytext.text) + Int32.Parse(mytext2.text);
                if (num > 10)
                {
                    mytext3.fontSize = 16;
                    mytext4.fontSize = 16;
                }
                mytext3.text = (Int32.Parse(mytext.text) + Int32.Parse(mytext2.text)).ToString();
                mytext4.text = mytext3.text;

                
                
                //Destroy(collision.gameObject);
                Destroy(gameObject);

                if (collision.gameObject.tag == "Player")
                {
                    if (mytext4.text == winningTotal)
                    {
                        SceneManager.LoadScene(2);
                        return;
                    }
                }
                
            }
        }
        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
    }

    

}
