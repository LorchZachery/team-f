using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            var script = collision.gameObject.GetComponent<PlayerController>();
            GameObject penaltyObj = gameObject.transform.GetChild(0).gameObject;
            TextMeshPro penaltyText = penaltyObj.GetComponent<TextMeshPro>();
            if (script.score > 2 && penaltyText.text == "X 0.5")
            {
                script.SetScore((int)(script.score * 0.5));
            }
            else if (script.score > 4 && penaltyText.text == "X 0.25")
            {
                script.SetScore((int)(script.score * 0.25));
            }
        }
    }

    public void SetPenalty(float penalty)
    {
        GameObject penaltyObj = gameObject.transform.GetChild(0).gameObject;
        TextMeshPro penaltyText = penaltyObj.GetComponent<TextMeshPro>();
        penaltyText.text = "X " + penalty;
    }
}
