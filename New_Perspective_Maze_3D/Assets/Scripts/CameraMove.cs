using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform targetObject;

    public Vector3 cameraOffset;

    public float smoothFactor = 0.5f;

    public bool lookAtTarget = false;

    public GameObject gameObject2;
    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - targetObject.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Debug.Log("Target Object: " + targetObject);
        //if (targetObject == null)
        //{
        //    GameObject something = (GameObject)Instantiate(gameObject2, transform);
        //    targetObject = something.transform;
        //    //Vector3 newPos = targetObject.transform.position + cameraOffset;
        //    //transform.position = Vector3.Slerp(transform.position, cameraOffset, smoothFactor);
        //}
        Vector3 newPosition = targetObject.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        if (lookAtTarget)
        {
            transform.LookAt(targetObject);
        }

    }

    public void assignObject(Transform transform)
    {
        //targetObject = Instantiate(gameObject, transform).transform;
        targetObject = transform;
        //Destroy(gameObject);
    }
}
