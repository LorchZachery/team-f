using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MovePlayer : MonoBehaviour
{
   public float moveSpeed = 5f;
   public Transform movePoint;
   public LayerMask whatStopsMovement;
   
   
   [SerializeField] GameObject gridManager;
    GridManager grid;

    public GameObject textobj;
    public TextMeshPro mytext;
    bool isGameOver = false;
    
    void Awake()
    {
        grid = gridManager.GetComponent<GridManager>();
    }
    //transform.position += new Vector3(x, y, 0) * isDiagonal* Time.deltaTime;
    // Start is called before the first frame update
    void Start()
    { 
        Vector3 tmp =  movePoint.transform.position;
        tmp.x = Mathf.Round(tmp.x);
        tmp.y = Mathf.Round(tmp.y);
        movePoint.transform.position = tmp; 
        movePoint.parent =null;
        // Text object that keeps track of player score
        textobj = gameObject.transform.GetChild(0).gameObject;
        mytext = textobj.GetComponent<TextMeshPro>();
        Debug.Log("INITIAL PLAYER SCORE " + mytext.text );
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        
        if(Vector3.Distance(transform.position, movePoint.position) <= 0f)
        {
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f){
            if(movePoint.position[0] + Input.GetAxisRaw("Horizontal") < grid.width && movePoint.position[0] + Input.GetAxisRaw("Horizontal") >= 0){
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"),0f,0f), .2f, whatStopsMovement)){
                        movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"),0f,0f);
                
                }
            }
            }
            else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f){
            if(movePoint.position[1] + Input.GetAxisRaw("Vertical") < grid.height && movePoint.position[1] + Input.GetAxisRaw("Vertical") >= 0){

                if(!Physics2D.OverlapCircle(movePoint.position +new Vector3(0f,Input.GetAxisRaw("Vertical"),0f), .2f, whatStopsMovement)){
                    movePoint.position += new Vector3(0f,Input.GetAxisRaw("Vertical"),0f);
                }
                }
            }
        }

        // Calls this when final score is reached
        if (mytext.text == "32" && !isGameOver) {
            isGameOver = true;
            Debug.Log("END GAME PLAYER SCORE " + mytext.text );
            grid.gameOver(Int32.Parse(mytext.text));
        }
    }
}
