using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager
{
    string level;
    float totalCoins;
    float coinsCollected;
    float coinsRemaining;
    float timeToReachTarget;
    float pointsAtDeath;
    string exitReason;
    float remainingTime;
    float totalTime;

    IDictionary<string, object> powerUpUsed;
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
                // Player Controller: when player collides with coin
                {
                    coinsCollected++;
                    break;
                }
            case GameEvent.COINS_REMAINING:
                // Player Controller: End of game
                {
                    coinsRemaining = (int)data;
                    break;
                }
            case GameEvent.COINS_TOTAL:
                // Grid Manager: Initalize data
                {
                    totalCoins = (int)data;
                    break;
                }
            case GameEvent.PLAYER_WON:
                // Player Controller: Won
                {
                    timeToReachTarget = (float)data;
                    break;
                }
            case GameEvent.PLAYER_LOST:
                // Player Controller: Lost
                {
                    pointsAtDeath = (int)data;
                    break;
                }
            case GameEvent.POWER_UP_USED:
                /* PowerUpWalkthrough: Player controller
                 * Shield: Player controller
                 * Shrink: Dashboard controller
                 * Extra time: Dashboard controller
                 */
                {
                    string powerUp = (string)data;
                    int count = 1;
                    if (powerUpUsed.ContainsKey(powerUp))
                    {
                        count = ((int)powerUpUsed[powerUp]) + 1;
                    }
                    powerUpUsed[powerUp] = count;
                    break;
                }
            case GameEvent.COLLISION:
                /* Player controller: Spike
                 * Obstacle controller: X0.5 , X 0.25
                 * TODO One way door;
                 */
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
                /* Time Loss: DashBoard controller
                 * Obstacle Loss: Player Controller
                 * Spike Loss: Player Controller
                 * Quit: Dashboard controller
                 * Restart: Dashboard controller
                 * Win: Player Controller
                 */
                {
                    exitReason = (string)data;
                    break;
                }
            case GameEvent.TIME_SPENT:
                {
                    /* At end of level: Player Controller
                     */
                    remainingTime = (float)data;
                    break;
                }
            case GameEvent.TOTAL_TIME:
                /* Initialize: Grid Manager
                 * Extra time power up: Dashboard controller
                 */
                {
                    totalTime += (float)data;
                    break;
                }

            default: break;

        }
    }

    public void Reset(string level)
    {
        this.level = level;
        coinsCollected = 0;
        coinsRemaining = 0;
        timeToReachTarget = 0;
        pointsAtDeath = 0;
        totalTime = 0;
        remainingTime = 0;
        totalCoins = 0;
        powerUpUsed = new Dictionary<string, object>();
        powerUpUsed["level"] = level;
        collidedObstacles = new Dictionary<string, object>();
        collidedObstacles["level"] = level;
    }

    public void Publish()
    {
        Debug.Log("Publishing");
        IDictionary<string, object> analytics = new Dictionary<string, object>();
        analytics.Add("level", level);
        analytics.Add("coinsCollected", coinsCollected);
        analytics.Add("coinsSpent", coinsCollected - coinsRemaining);
        analytics.Add("totalCoins", totalCoins);
        analytics.Add("timeToReachTarget", timeToReachTarget);
        analytics.Add("pointsAtDeath", pointsAtDeath);
        analytics.Add("exitReason", exitReason);
        analytics.Add("remainingTime", remainingTime);

        AnalyticsResult analyticsResult = Analytics.CustomEvent("userData", analytics);
        Debug.Log(analyticsResult);

        analyticsResult = Analytics.CustomEvent("powerUpUsed", powerUpUsed);
        Debug.Log(analyticsResult);

        analyticsResult = Analytics.CustomEvent("collidedObstacles", collidedObstacles);
        Debug.Log(analyticsResult);
    }

}
