using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager
{
    string level;
    float coinsCollected;
    float coinsSpent;
    float timeToReachTarget;
    float pointsAtDeath;
    string exitReason;
    float remainingTime;

    IDictionary<string, object> powerUpPurchased;
    IDictionary<string, object> powerUpSpent;
    IDictionary<string, object> collidedObstacles;

    private static AnalyticsManager instance;

    public static AnalyticsManager GetAnalyticsManager()
    {
        if (instance == null)
        {
            instance = new AnalyticsManager();
            instance.Reset("0");
        }
        return instance;
    }

    public void RegisterEvent(GameEvent gameEvent, object data)
    {

        Debug.Log("Event registered");
        switch (gameEvent)
        {
            case GameEvent.COINS_COLLECTED:
                {
                    coinsCollected++;
                    break;
                }
            case GameEvent.COINS_SPENT:
                {
                    coinsSpent = (int)data;
                    break;
                }
            case GameEvent.PLAYER_WON:
                {
                    timeToReachTarget = (float)data;
                    break;
                }
            case GameEvent.PLAYER_LOST:
                {
                    pointsAtDeath = (int)data;
                    break;
                }
            case GameEvent.POWER_UP_PURCHASED:
                {
                    string powerUp = (string)data;
                    int count = 1;
                    if (powerUpPurchased.ContainsKey(powerUp))
                    {
                        count = ((int)powerUpPurchased[powerUp]) + 1;
                    }
                    powerUpPurchased[powerUp] = count;
                    break;
                }
            case GameEvent.POWER_UP_USED:
                {
                    string powerUp = (string)data;
                    int count = 1;
                    if (powerUpSpent.ContainsKey(powerUp))
                    {
                        count = ((int)powerUpSpent[powerUp]) + 1;
                    }
                    powerUpSpent[powerUp] = count;
                    break;
                }
            case GameEvent.COLLISION:
                {
                    Debug.Log("Registeded collision");
                    string powerUp = (string)data;
                    int count = 1;
                    if (collidedObstacles.ContainsKey(powerUp))
                    {
                        count = ((int)collidedObstacles[powerUp]) + 1;
                    }
                    collidedObstacles[powerUp] = count;
                    break;
                }
            case GameEvent.EXIT_REASON:
                {
                    exitReason = (string)data;
                    break;
                }
            case GameEvent.TIME_SPENT:
                {
                    remainingTime = (float)data;
                    break;
                }

            default: break;

        }
    }

    public void Reset(string level)
    {
        this.level = level;
        coinsCollected = 0;
        coinsSpent = 0;
        timeToReachTarget = 0;
        pointsAtDeath = 0;
        powerUpSpent = new Dictionary<string, object>();
        powerUpSpent["level"] = level;
        powerUpPurchased = new Dictionary<string, object>();
        powerUpPurchased["level"] = level;
        collidedObstacles = new Dictionary<string, object>();
        collidedObstacles["level"] = level;
    }

    public void Publish()
    {
        Debug.Log("Publishing");
        IDictionary<string, object> analytics = new Dictionary<string, object>();
        analytics.Add("level", level);
        analytics.Add("coinsCollected", coinsCollected);
        analytics.Add("coinsSpent", coinsCollected - coinsSpent);
        analytics.Add("timeToReachTarget", timeToReachTarget);
        analytics.Add("pointsAtDeath", pointsAtDeath);
        analytics.Add("exitReason", exitReason);
        analytics.Add("remainingTime", remainingTime);

        AnalyticsResult analyticsResult = Analytics.CustomEvent("userData", analytics);
        Debug.Log(analyticsResult);

        analyticsResult = Analytics.CustomEvent("powerUpPurchased", powerUpPurchased);
        Debug.Log(analyticsResult);

        analyticsResult = Analytics.CustomEvent("powerUpSpent", powerUpSpent);
        Debug.Log(analyticsResult);

        analyticsResult = Analytics.CustomEvent("collidedObstacles", collidedObstacles);
        Debug.Log(analyticsResult);
    }

}
