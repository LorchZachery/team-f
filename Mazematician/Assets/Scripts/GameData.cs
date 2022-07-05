using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class deals with storing game data to be saved into JSON file
*/
[System.Serializable]
public class GameData
{
    public int levelNumber;
    public int timeBestScore;

    public GameData(int levelNumber, int timeBestScore)
    {
        this.levelNumber = levelNumber;
        this.timeBestScore = timeBestScore;
    }

}
