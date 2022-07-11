using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    private float shakeTime, shakePower, shakeFade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            var script = collision.gameObject.GetComponent<PlayerController>();
            if (!script.playerShield.activeInHierarchy)
            {
                StartShake(.3f, .3f);
            }
        }
    }

    private void LateUpdate()
    {
        if(shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            float xShake = Random.Range(-1f, 1f) * shakePower;
            float yShake = Random.Range(-1f, 1f) * shakePower;

            Camera.main.transform.position += new Vector3(xShake, yShake, 0);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFade * Time.deltaTime);
        }
    }

    public void StartShake(float length, float power)
    {
        shakeTime = length;
        shakePower = power;

        shakeFade = power / length;
    }
}
