using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private float minSize;
    private float maxSize;
    private float currentSize;

    // Start is called before the first frame update
    void Start()
    {
        minSize = 2;
        maxSize = 6;
        currentSize = minSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10);
        }

        if (Input.GetKeyUp(KeyCode.RightBracket))
        {
            currentSize = Mathf.Max(currentSize - 1, minSize);
        }

        if (Input.GetKeyUp(KeyCode.LeftBracket))
        {
            currentSize = Mathf.Min(currentSize + 1, maxSize);

        }
        Camera.main.orthographicSize = currentSize;

    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        //Debug.Log("Player has been set");
    }
}
