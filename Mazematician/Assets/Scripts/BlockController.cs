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
            }
        }

        /*
         * Code to prevent diagonal movement
         */

        Vector2 direction = collision.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
        float x = direction.x;
        float y = direction.y;
        if(Mathf.Abs(x) > Mathf.Abs(y))
        {
            int dir = x > 0 ? 1 : -1;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1 * dir, 0);
        }
        else if(Mathf.Abs(x) < Mathf.Abs(y))
        {
            int dir = y > 0 ? 1 : -1;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1 * dir);
        }
    }

    public void SetPoints(int points)
    {
        this.points = points;
        UpdateText();
        Debug.Log("Point updated" + points);
    }

    void UpdateText()
    {
        pointsText.text = points.ToString();
    }
}
