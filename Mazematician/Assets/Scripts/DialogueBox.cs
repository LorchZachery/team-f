using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// Collect coins
// For spike - back side 


public class DialogueBox : MonoBehaviour
{
    public GameObject coinPrompt;
    public GameObject shieldPrompt;
    public GameObject shrinkPrompt;
    public GameObject walkThruPrompt;
    
    GameObject player;
    string LevelName;

    // Start is called before the first frame update
    void Start()
    {
        disableAllPrompts();
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            LevelName = LevelsController.LevelName;
            var script = player.GetComponent<PlayerController>();

            if (LevelName == "obstacle_tutorial")
            {
                switch(script.score)
                {
                    case 4:
                        coinPrompt.SetActive(true);
                        StartCoroutine(closePrompt(coinPrompt));
                        break;
                    case 8:
                        shieldPrompt.SetActive(true);
                        StartCoroutine(closePrompt(shieldPrompt));
                        break;
                    case 16:
                        shrinkPrompt.SetActive(true);
                        StartCoroutine(closePrompt(shrinkPrompt));
                        break;
                    case 128:
                        walkThruPrompt.SetActive(true);
                        StartCoroutine(closePrompt(walkThruPrompt));
                        break;
                }
            }
            else if (LevelName == "newtutorial31")
            {
                switch(script.score)
                {
                    case 4:
                        if (coinPrompt) 
                        {
                            coinPrompt.SetActive(true);
                            StartCoroutine(closePrompt(coinPrompt));
                        }
                        break;
                    case 8:
                        if (shieldPrompt) 
                        {
                            shieldPrompt.SetActive(true);
                            StartCoroutine(closePrompt(shieldPrompt));
                        }
                        break;
                    case 16:
                        if (shrinkPrompt) 
                        {
                            shrinkPrompt.SetActive(true);
                            StartCoroutine(closePrompt(shrinkPrompt));
                        }
                        break;
                    case 128:
                    case 32:
                        if (walkThruPrompt) 
                        {
                            walkThruPrompt.SetActive(true);
                            StartCoroutine(closePrompt(walkThruPrompt));
                        }
                        break;
                }
            }
        }
    }

    void disableAllPrompts()
    {
        coinPrompt.SetActive(false);
        shieldPrompt.SetActive(false);
        shrinkPrompt.SetActive(false);
        walkThruPrompt.SetActive(false);
    }

    IEnumerator closePrompt(GameObject obj)
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(obj);
    }
}
