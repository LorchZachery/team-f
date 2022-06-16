using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float rotateY;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rotateY = 0.0f;
        rotationSpeed = 150f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentTransform = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentTransform.x,
            currentTransform.y + (Time.deltaTime * rotationSpeed),
            currentTransform.z);

    }
}
