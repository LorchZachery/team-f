using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource src;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void HoverSound()
    {
        src.PlayOneShot(hoverSound);
    }

    public void ClickSound()
    {
        src.PlayOneShot(clickSound);
    }

}
