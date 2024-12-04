using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerData")]
public class SO_PlayerData : ScriptableObject
{
    #region Variables

    public string PlayerName = "Player"; // Name of the player
    public GameResults PlayerGameResults; // Holds game-related results for the player

    #endregion

    #region Public Methods

    /// <summary>
    /// Resets the player's game results to default values.
    /// </summary>
    public void ResetResults()
    {
        PlayerGameResults = new GameResults();
    }

    /// <summary>
    /// Sets a new player name and resets the game results.
    /// </summary>
    /// <param name="newPlayerName">The new name for the player.</param>
    public void SetNewPlayerName(string newPlayerName)
    {
        ResetResults();
        PlayerName = newPlayerName;
    }

    /// <summary>
    /// Calculates the player's score based on game performance.
    /// </summary>
    public void CalculateScore()
    {
        if (PlayerGameResults == null)
        {
            Debug.LogWarning("PlayerGameResults is null. Cannot calculate score.");
            return;
        }

        // Base calculation
        int baseScore = PlayerGameResults.TotalFindPairs * 100;

        // Penalties
        int clickPenalty = PlayerGameResults.TotalClicks * 2;
        int timePenalty = PlayerGameResults.TotalGameTime;

        // Final score
        PlayerGameResults.Score = Mathf.Max(0, baseScore - clickPenalty - timePenalty);

        Debug.Log($"Score calculated: {PlayerGameResults.Score} (Base: {baseScore}, Click Penalty: {clickPenalty}, Time Penalty: {timePenalty})");
    }

    #endregion
}

[System.Serializable]
public class GameResults
{
    #region Variables

    public int TotalClicks = 0; // Total clicks made by the player
    public int TotalGameTime = 0; // Total time spent in the game (seconds)
    public int TotalFindPairs = 0; // Total pairs found by the player
    public int Score = 0; // Final score of the player

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor to initialize game results with default or specified values.
    /// </summary>
    /// <param name="totalClicks">Total clicks made by the player.</param>
    /// <param name="totalGameTime">Total time spent in the game.</param>
    /// <param name="totalFindPairs">Total pairs found by the player.</param>
    /// <param name="score">Final score of the player.</param>
    public GameResults(int totalClicks = 0, int totalGameTime = 0, int totalFindPairs = 0, int score = 0)
    {
        TotalClicks = totalClicks;
        TotalGameTime = totalGameTime;
        TotalFindPairs = totalFindPairs;
        Score = score;
    }

    #endregion
}
