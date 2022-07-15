using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public GameObject gravityPrompt;

    public GameObject obstaclePrompt;
    public GameObject wallPrompt;
    public GameObject hexPrompt;

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

            if (LevelName == "gravity_tutorial")
            {
                StartCoroutine(handleGravityPrompt());
            }
            if (LevelName == "obstacle_tutorial")
            {
                switch(script.score)
                {
                    case 4:
                        if (obstaclePrompt)
                        {
                            obstaclePrompt.SetActive(true);
                            StartCoroutine(closePrompt(obstaclePrompt));
                        }    
                        break;
                    case 8:
                        if (wallPrompt)
                        {
                            wallPrompt.SetActive(true);
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                wallPrompt.SetActive(false);
                                Destroy(wallPrompt);
                            }
                        }    
                        break;
                    case 32:
                        if (hexPrompt)
                        {
                            hexPrompt.SetActive(true);
                            StartCoroutine(closePrompt(hexPrompt));
                        }    
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
                            if (Input.GetKeyDown(KeyCode.M))
                            {
                                shieldPrompt.SetActive(false);
                                Destroy(shieldPrompt);
                            }
                        }
                        break;
                    case 16:
                        if (shrinkPrompt) 
                        {
                            shrinkPrompt.SetActive(true);
                            if (Input.GetKeyDown(KeyCode.N))
                            {
                                shrinkPrompt.SetActive(false);
                                Destroy(shrinkPrompt);
                            }
                        }
                        break;
                    case 128:
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
        gravityPrompt.SetActive(false);

        obstaclePrompt.SetActive(false);
        wallPrompt.SetActive(false);
        hexPrompt.SetActive(false);

        coinPrompt.SetActive(false);
        shieldPrompt.SetActive(false);
        shrinkPrompt.SetActive(false);
        walkThruPrompt.SetActive(false);
    }

    IEnumerator handleGravityPrompt()
    {
        yield return new WaitForSeconds(13.0f);
        gravityPrompt.SetActive(true);
        StartCoroutine(closePrompt(gravityPrompt));
    }

    IEnumerator closePrompt(GameObject obj)
    {
        yield return new WaitForSeconds(4.0f);
        Destroy(obj);
    }
}
