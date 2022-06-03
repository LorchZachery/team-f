using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private float rotZ;
    public float RotationSpeed;
    public bool ClockwiseRotation;

    [SerializeField] GameObject gridManager;
    GridManager grid;

    void Awake()
    {
        grid = gridManager.GetComponent<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ClockwiseRotation == false) {
            rotZ += Time.deltaTime * RotationSpeed;
        } else {
            rotZ += -Time.deltaTime * RotationSpeed;
        }

        transform.rotation = Quaternion.Euler(0,0,rotZ);
    }
}
