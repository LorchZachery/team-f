using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameEndController : MonoBehaviour
{
    int targetScore;
    // Start is called before the first frame update
    void Start()
    {
        targetScore = 64;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var script = collision.gameObject.GetComponent<PlayerController>();
            if (script.score == targetScore)
            {
                script.NotifyPlayerWin();
            }
        }
    }
}
