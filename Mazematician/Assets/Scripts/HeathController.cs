using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathController : MonoBehaviour
{
    float flashTime;
    bool flashActive;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flashTime > 0)
        {
            flashTime -= Time.deltaTime;
        }

        if(flashTime <= 0 && flashActive)
        {
            CancelInvoke("Flash");
            Destroy(gameObject);
        }
    }

    public void removeHealth()
    {
        flashTime = 0.2f;
        flashActive = true;
        InvokeRepeating("Flash", 0f, 0.1f);
    }


    public void Flash()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
