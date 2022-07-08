using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float rotateY;
    public float rotationSpeed;
    private ParticleSystem particle;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        rotateY = 0.0f;
        rotationSpeed = 150f;
        particle = GetComponentInChildren<ParticleSystem>();
        particle.Pause();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentTransform = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentTransform.x,
            currentTransform.y + (Time.deltaTime * rotationSpeed),
            currentTransform.z);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("block"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
        }
    }

    public void DestroyCoin()
    {
        StartCoroutine(AnimateDestroy());
    }

    private IEnumerator AnimateDestroy()
    {
        particle.Play();
        audioSource.Play();
        spriteRenderer.enabled = false;
        circleCollider2D.enabled = false;
        yield return new WaitForSeconds(particle.main.startLifetime.constantMax);
        Destroy(gameObject);
    }

}
