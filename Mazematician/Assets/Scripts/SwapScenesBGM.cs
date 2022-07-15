using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScenesBGM : MonoBehaviour
{

    private void Start()
    {
        NotDestroyBGM.instance.GetComponent<AudioSource>().Pause();
    }
    private void OnDestroy()
    {
        if (NotDestroyBGM.instance != null)
        {
            NotDestroyBGM.instance.GetComponent<AudioSource>().Play();
        }

    }

}