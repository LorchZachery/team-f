using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Merge : MonoBehaviour
{
    public int ID;
    public GameObject MergedObject;
    
    public GameObject textobj;
    public TextMeshPro mytext;
    public GameObject textobj2;
    public TextMeshPro mytext2;
    public GameObject textobj3;
    public TextMeshPro mytext3;
    private Collision2D current_collision;
    private int count =0;
    // Start is called before the first frame update
    void Start()
    {
        current_collision = null;
        ID = GetInstanceID();
    }

    // Update is called once per frame
    void Update()
    {
        if(current_collision != null && current_collision.gameObject != this.gameObject ){
        /*Debug.Log($"collison x,y: {current_collision.transform.position.x} , {current_collision.transform.position.y} this x,y: {this.gameObject.transform.position.x} , {this.gameObject.transform.position.y}");*/

            if((Mathf.Abs((current_collision.transform.position.x)  - (this.gameObject.transform.position.x)) < 1.5f && Mathf.Abs((current_collision.transform.position.y) - (this.gameObject.transform.position.y)) < 0.5f) ||
            (Mathf.Abs((current_collision.transform.position.y)  - (this.gameObject.transform.position.y)) < 1.5f && Mathf.Abs((current_collision.transform.position.x) - (this.gameObject.transform.position.x)) < 0.5f)
            ){


            textobj = current_collision.gameObject.transform.GetChild(0).gameObject;
            mytext = textobj.GetComponent<TextMeshPro>();
            textobj2 = gameObject.transform.GetChild(0).gameObject;
            mytext2 = textobj2.GetComponent<TextMeshPro>();

           
            
            //if (collision.gameObject.GetComponent<SpriteRenderer>().color == GetComponent<SpriteRenderer>().color)
            
            if (mytext.text != null && mytext2.text != null && current_collision != null)
            {
               if( mytext.text.Equals(mytext2.text)){
               // Debug.Log($"SENDING MESSAGE FROM {gameObject.name} With the ID number of {current_collision.gameObject.GetComponent<MovePlayer>().GetInstanceID()} and object for {ID}");
                //Debug.Log($"Text of {gameObject.name}");
                if(current_collision.gameObject.CompareTag("Player Tag")){
                    GameObject O = current_collision.gameObject;
                    textobj3 = O.transform.GetChild(0).gameObject;
                    mytext3 = textobj3.GetComponent<TextMeshPro>();
                    mytext3.text = (Int32.Parse(mytext.text) + Int32.Parse(mytext2.text)).ToString();
                    Debug.Log($"Sum after merging blocks: {Int32.Parse(mytext.text) + Int32.Parse(mytext2.text)}");
                 
                
               
                
                    Debug.Log("Desotry " + gameObject.name );
                    Destroy(gameObject);
                }
                else if(!this.gameObject.CompareTag("Player Tag")){
                    if(this.gameObject.GetInstanceID() > current_collision.gameObject.GetInstanceID()){
                        GameObject O = current_collision.gameObject;
                        textobj3 = O.transform.GetChild(0).gameObject;
                        mytext3 = textobj3.GetComponent<TextMeshPro>();
                        mytext3.text = (Int32.Parse(mytext.text) + Int32.Parse(mytext2.text)).ToString();
                        Debug.Log($"Sum after merging blocks: {Int32.Parse(mytext.text) + Int32.Parse(mytext2.text)}");

                        Debug.Log("Desotry " + gameObject.name );
                        Destroy(gameObject);
                    }
                }
                
                
            }
                }
            }
        }
        current_collision = null;
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.CompareTag("MergeBlock") || collision.gameObject.CompareTag("New Block") || collision.gameObject.CompareTag("Player Tag"))
        {   
            /*if(!this.gameObject.CompareTag("MergeBlock")){*/
            current_collision = collision;
             
            
        }
    }
}
