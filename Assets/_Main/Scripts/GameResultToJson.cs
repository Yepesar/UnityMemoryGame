using System.IO;
using UnityEngine;

public class GameResultToJson : MonoBehaviour
{
    [SerializeField] private SO_PlayerData playerData; // Reference to SO_PlayerData

    private readonly string folderPath = "Assets/_Main/JSONs/GameResults";

    /// <summary>
    /// Saves the player's game results to a JSON file.
    /// </summary>
    public void SaveResultsToJson()
    {
        if (playerData == null)
        {
            Debug.LogError("Player data reference is null. Cannot save results.");
            return;
        }

        // Create the folder if it doesn't exist
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Prepare the results object
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

        // Convert results to JSON
        string jsonContent = JsonUtility.ToJson(results, true);

        // Generate the file name using PlayerName
        string fileName = $"RESULTS{playerData.PlayerName}.json";
        string filePath = Path.Combine(folderPath, fileName);

        // Save the JSON to a file
        File.WriteAllText(filePath, jsonContent);

        Debug.Log($"Game results saved successfully to: {filePath}");
    }
}

[System.Serializable]
public class GameResultJson
{
    public GameResultDetails results;
}

[System.Serializable]
public class GameResultDetails
{
    public int total_clicks;
    public int total_time;
    public int pairs;
    public int score;
}
