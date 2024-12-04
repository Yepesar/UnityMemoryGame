using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerData")]
public class SO_PlayerData : ScriptableObject
{
    public string PlayerName = "Player";
    public GameResults PlayerGameResults;

    public void ResetResults()
    {
        PlayerGameResults = new GameResults();
    }

    public void SetNewPlayerName(string newPlayerName)
    {
        ResetResults();
        PlayerName = newPlayerName;
    }

}

[System.Serializable]
public class GameResults
{
    public int TotalClicks = 0;
    public int TotalGameTime = 0;
    public int TotalFindPairs = 0;
    public int Score = 0;

    public GameResults(int totalClicks = 0, int totalGameTime = 0, int totalFindPairs = 0, int score = 0)
    {
        TotalClicks = totalClicks;
        TotalGameTime = totalGameTime;
        TotalFindPairs = totalFindPairs;
        Score = score;
    }
}

