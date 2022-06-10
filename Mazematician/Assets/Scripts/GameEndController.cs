using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEndController : MonoBehaviour
{
    public int targetScore;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setTargetScore(int value)
    {
        this.targetScore = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            var script = collision.gameObject.GetComponent<PlayerController>();
            if (script.score == targetScore)
            {
                // script.NotifyPlayerWin();
                SceneManager.LoadScene("GameOverWon");
            }
        }
    }
}
