using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObstacleController : MonoBehaviour
{
    AnalyticsManager analyticsManager;
    List<Collider2D> collist;
    private bool isCoroutine = false;

    // Start is called before the first frame update
    void Start()
    {
        analyticsManager = AnalyticsManager.GetAnalyticsManager();
        collist = new List<Collider2D>();
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
            if (!script.playerShield.activeInHierarchy) {
                foreach (Collider2D col in collist)
                {
                    Physics2D.IgnoreCollision(col, GetComponent<CircleCollider2D>(), false);
                }
                collist.Clear();

                GameObject penaltyObj = gameObject.transform.GetChild(0).gameObject;
                TextMeshPro penaltyText = penaltyObj.GetComponent<TextMeshPro>();
                if (penaltyText.text == "X 0.5")
                {
                    script.SetScore((int)(script.score * 0.5));
                }
                else if (penaltyText.text == "X 0.25")
                {
                    script.SetScore((int)(script.score * 0.25));
                }
                analyticsManager.RegisterEvent(GameEvent.COLLISION, penaltyText.text);
            }
            else {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
                collist.Add(collision.gameObject.GetComponent<CircleCollider2D>());
                if (!isCoroutine) {
                    isCoroutine = true;
                    StartCoroutine(handleShieldForObs());
                }
            }
        }
    }

    private IEnumerator handleShieldForObs()
    {
        yield return new WaitForSeconds(5.0f);
        isCoroutine = false;

        foreach (Collider2D col in collist)
        {
            Physics2D.IgnoreCollision(col, GetComponent<CircleCollider2D>(), false);
        }
        collist.Clear();
    }

    public void SetPenalty(float penalty)
    {
        GameObject penaltyObj = gameObject.transform.GetChild(0).gameObject;
        TextMeshPro penaltyText = penaltyObj.GetComponent<TextMeshPro>();
        penaltyText.text = "X " + penalty;
    }
}
