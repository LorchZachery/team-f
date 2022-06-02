using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Merge : MonoBehaviour
{
    int ID;
    public GameObject MergedObject;
    public GameObject textobj;
    public TextMeshPro mytext;
    public GameObject textobj2;
    public TextMeshPro mytext2;
    public GameObject textobj3;
    public TextMeshPro mytext3;
    // Start is called before the first frame update
    void Start()
    {
        ID = GetInstanceID();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("MergeBlock") || collision.gameObject.CompareTag("New Block"))
        {
            textobj = collision.gameObject.transform.GetChild(0).gameObject;
            mytext = textobj.GetComponent<TextMeshPro>();
            textobj2 = gameObject.transform.GetChild(0).gameObject;
            mytext2 = textobj2.GetComponent<TextMeshPro>();
            //if (collision.gameObject.GetComponent<SpriteRenderer>().color == GetComponent<SpriteRenderer>().color)
            if (mytext.text != null && mytext2.text != null && mytext.text.Equals(mytext2.text))
            {
                //if (ID < collision.gameObject.GetComponent<MovePlayer>().GetInstanceID()) { return; }
                //textobj = collision.gameObject.transform.GetChild(0).gameObject;
                //mytext = textobj.GetComponent<TextMeshPro>();
                //textobj2 = gameObject.transform.GetChild(0).gameObject;
                //mytext2 = textobj2.GetComponent<TextMeshPro>();
                //Debug.Log($"Name of Child 1: {mytext.text}");
                //Debug.Log($"Name of Child 2: {mytext2.text}");
                Debug.Log($"SENDING MESSAGE FROM {gameObject.name} With the ID number of {collision.gameObject.GetComponent<MovePlayer>().GetInstanceID()} and object for {ID}");
                //Debug.Log($"Text of {gameObject.name}");
                GameObject O = Instantiate(MergedObject, transform.position, Quaternion.identity) as GameObject;
                textobj3 = O.transform.GetChild(0).gameObject;
                mytext3 = textobj3.GetComponent<TextMeshPro>();
                mytext3.text = (Int32.Parse(mytext.text) + Int32.Parse(mytext2.text)).ToString();
                //mytext.text = (Int32.Parse(mytext.text) + Int32.Parse(mytext2.text)).ToString();
                Debug.Log($"Sum after merging blocks: {Int32.Parse(mytext.text) + Int32.Parse(mytext2.text)}");
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
