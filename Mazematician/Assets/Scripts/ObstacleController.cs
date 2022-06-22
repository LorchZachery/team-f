using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObstacleController : MonoBehaviour
{
    AnalyticsManager analyticsManager;

    private List<Collider2D> collist;

    // Start is called before the first frame update
    void Start()
    {

        collist = new List<Collider2D>();

        analyticsManager = AnalyticsManager.GetAnalyticsManager();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            analyticsManager.RegisterEvent(GameEvent.COLLISION, tag);
            var script = collision.gameObject.GetComponent<PlayerController>();
            if (!script.playerShield.activeInHierarchy) {
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
            }
            else {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
                collist.Add(collision.gameObject.GetComponent<CircleCollider2D>());
            }
        }
    }

    public void SetPenalty(float penalty)
    {
        GameObject penaltyObj = gameObject.transform.GetChild(0).gameObject;
        TextMeshPro penaltyText = penaltyObj.GetComponent<TextMeshPro>();
        penaltyText.text = "X " + penalty;
    }
}
