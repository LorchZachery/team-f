using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * This class deals with logic of collision detection with other block
 */
public class BlockController : MonoBehaviour
{
    public int points;
    public int id;
    public TMP_Text pointsText;
    // Start is called before the first frame update
    void Start()
    {
        id = GetInstanceID();
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    { 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         * If colliding object is block, and has same points, 
         * then merge it with current block.
         */
        if(collision.gameObject.CompareTag("block"))
        {
            var script = collision.gameObject.GetComponent<BlockController>();
            if(script.points == points && script.id > id)
            {
                points += collision.gameObject.GetComponent<BlockController>().points;
                Destroy(collision.gameObject);
                UpdateText();
                UpdateColor();
            }
        }
    }

    public void SetPoints(int points)
    {
        this.points = points;
        
        UpdateText();
        //Debug.Log("Point updated" + points);
        UpdateColor();
    }

    void UpdateText()
    {
        pointsText.text = points.ToString();
    }
    void UpdateColor()
    {
        if (points == 2)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255f / 255f, 153f / 255f, 153f / 255f);
        }
        if(points == 4)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255f / 255f, 204f / 255f, 153f / 255f);
        }
        if(points == 8)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255f / 255f, 255f / 255f, 153f / 255f);
        }
        if(points == 16)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(204f / 255f, 255f / 255f, 153f / 255f);
        }
        if(points == 32)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(153f / 255f, 255f / 255f, 255f / 255f);
        }
        if(points == 64)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(102f / 255f, 102f / 255f, 255f / 255f);
        }
        if(points == 128)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(204f / 255f, 153f / 255f, 255f / 255f);
        }
        if(points == 256)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(224f / 255f, 224f / 255f, 224f / 255f);
        }
    }
}
