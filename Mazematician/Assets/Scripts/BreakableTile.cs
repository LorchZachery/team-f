using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This class deals with the effect of executing power up through key code "V": break wall
 */

public class BreakableTile : MonoBehaviour
{
    private ParticleSystem particle;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player") && Input.GetKey(KeyCode.V))
        {
            StartCoroutine(Break());
        }
    }

    private IEnumerator Break()
    {
        particle.Play();

        audioSource.Play();

        sr.enabled = false;
        bc.enabled = false;

        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        Destroy(gameObject);
    }
}
