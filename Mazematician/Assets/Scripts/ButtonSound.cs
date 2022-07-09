using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{

    public AudioSource src;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverSound()
    {
        src.PlayOneShot(hoverSound);
    }
    public void ClickSound()
    {
        src.PlayOneShot(clickSound);
    }

}
