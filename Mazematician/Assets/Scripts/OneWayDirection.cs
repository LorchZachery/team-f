using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayDirection : MonoBehaviour
{

    float diff = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Flash", 0, 0.1f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Flash()
    {
        Color color = GetComponent<SpriteRenderer>().color;

        if (color.a >= 0.9 || color.a <= 0.1) diff = -diff;
        color.a += diff;

        GetComponent<SpriteRenderer>().color = color;

        Debug.Log("flash::" + GetComponent<SpriteRenderer>().color.a);

    }
}
