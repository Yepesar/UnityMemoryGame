using System.IO;
using UnityEngine;

public class GameResultToJson : MonoBehaviour
{
    #region Variables

    [SerializeField] private SO_PlayerData playerData; // Reference to SO_PlayerData

    private readonly string folderPath = "Assets/_Main/JSONs/GameResults"; // Path to save the JSON files

    #endregion

    #region Public Methods

    /// <summary>
    /// Saves the player's game results to a JSON file in the specified folder.
    /// </summary>
    public void SaveResultsToJson()
    {
        if (playerData == null)
        {
            Debug.LogError("Player data reference is null. Cannot save results.");
            return;
        }

        // Ensure the folder exists
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Prepare the results object for JSON serialization
        GameResultJson results = new GameResultJson
        {
            results = new GameResultDetails
            {
                total_clicks = playerData.PlayerGameResults.TotalClicks,
                total_time = playerData.PlayerGameResults.TotalGameTime,
                pairs = playerData.PlayerGameResults.TotalFindPairs,
                score = playerData.PlayerGameResults.Score
            }
        };

        // Serialize results to JSON format
        string jsonContent = JsonUtility.ToJson(results, true);

        // Generate the file name using the player's name
        string fileName = $"RESULTS{playerData.PlayerName}.json";
        string filePath = Path.Combine(folderPath, fileName);

        // Write the JSON content to a file
        File.WriteAllText(filePath, jsonContent);

        Debug.Log($"Game results saved successfully to: {filePath}");
    }

    #endregion
}

[System.Serializable]
public class GameResultJson
{
    public GameResultDetails results; // Container for the game result details
}

[System.Serializable]
public class GameResultDetails
{
    public int total_clicks; // Total clicks made by the player
    public int total_time;   // Total time spent in the game (in seconds)
    public int pairs;        // Total pairs found by the player
    public int score;        // Final score achieved by the player
}
