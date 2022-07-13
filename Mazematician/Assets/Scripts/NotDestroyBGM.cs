using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDestroyBGM : MonoBehaviour
{
    public static NotDestroyBGM instance;

    void Awake()
    {
        // Debug.Log("not destroy script, instance:", instance);
        if (instance != null)
        {
            // Destroy(gameObject);
            // Debug.Log("NOT null case");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            // Debug.Log(" null case");
        }

    }
}