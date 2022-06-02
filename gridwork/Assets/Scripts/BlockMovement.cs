using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 lastPostion; 
     [SerializeField] GameObject gridManager;
    GridManager grid;
    
    void Awake()
    {
        grid = gridManager.GetComponent<GridManager>();
    }
    void Start()
    {
        lastPostion = gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3  round = new Vector3(
            Mathf.Round(gameObject.transform.position.x),
            Mathf.Round(gameObject.transform.position.y),
            Mathf.Round(gameObject.transform.position.z)
        );
        lastPostion = new Vector3( Mathf.Round(lastPostion[0]),Mathf.Round(lastPostion[1]),Mathf.Round(lastPostion[2]));

        Debug.Log("last " + lastPostion);
        Debug.Log("curr roun " + round);

        if(lastPostion != round){
            Vector3 targetPos = lastPostion;
            
            float xmovement = Mathf.Abs(lastPostion[0] - gameObject.transform.position.x);
            float ymovement = Mathf.Abs(lastPostion[1] - gameObject.transform.position.y);
            if(xmovement > ymovement){
                if(lastPostion[0] > gameObject.transform.position.x){
                    if(lastPostion[0] - 1f >= 0){
                        targetPos[0] = lastPostion[0] - 1f;
                        }
                    }
                
                else if(lastPostion[0] < gameObject.transform.position.x){
                    if(lastPostion[0] + 1f < grid.width){
                        targetPos[0] = lastPostion[0] + 1f;
                    }
                }
            }
            else if(ymovement > xmovement){
                if(lastPostion[1] > gameObject.transform.position.y){
                    if(lastPostion[1] - 1f >= 0){
                        targetPos[1] = lastPostion[1] - 1f;
                    }
                }
            
                else if(lastPostion[1] < gameObject.transform.position.y){
                    if(lastPostion[1] + 1f < grid.height){
                        targetPos[1] = lastPostion[1] + 1f;
                    }
                }
            }

           
            

            
            
            gameObject.transform.position = targetPos;

        }
         lastPostion = gameObject.transform.position;

        

    }
}
