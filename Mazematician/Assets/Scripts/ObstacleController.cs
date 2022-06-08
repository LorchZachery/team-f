using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("player"))
        {
            var script = collision.gameObject.GetComponent<PlayerController>();
            if(script.score > 2)
            {
                script.SetScore((int)(script.score * 0.5));
            }
        }
    }
}
