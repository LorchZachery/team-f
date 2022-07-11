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
    [SerializeField] private AudioSource mergeBlockSound;

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
                mergeBlockSound.Play();
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
        if (points == 4)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(251f / 255f, 140f / 255f, 69f / 255f);
        }
        if (points == 8)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255f / 255f, 255f / 255f, 153f / 255f);
        }
        if (points == 16)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(115f / 255f, 240f / 255f, 187f / 255f);
        }
        if (points == 32)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(196f / 255f, 245f / 255f, 243f / 255f);
        }
        if (points == 64)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(102f / 255f, 206f / 255f, 245f / 255f);
        }
        if (points == 128)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(219f / 255f, 196f / 255f, 245f / 255f);
        }
        if (points == 256)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(185f / 255f, 182f / 255f, 185f / 255f);
        }
        if (points == 512)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(245f / 255f, 224f / 255f, 46f / 255f);
        }
        if (points == 1024)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(245f / 255f, 120f / 255f, 219f / 255f);
        }
        if (points == 2048)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(250f / 255f, 206f / 255f, 133f / 255f);
        }
        if (points == 4096)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255f / 255f, 167f / 255f, 138f / 255f);
        }
    }
}
