using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UILeaderBoard : MonoBehaviour
{
    #region Variables

    private readonly string folderPath = "Assets/_Main/JSONs/GameResults"; // Path to the folder containing JSON files

    [SerializeField] private Transform playersParent; // Parent transform for the leaderboard entries
    [SerializeField] private GameObject leaderBoardPlayerPrefab; // Prefab for individual leaderboard entries
    [Range(1, 6)]
    [SerializeField] private int maxPlayers = 6; // Maximum number of players to display

    #endregion

    #region Initialization

    /// <summary>
    /// Populates the leaderboard when the script starts.
    /// </summary>
    private void Start()
    {
        PopulateLeaderboard();
    }

    #endregion

    #region Leaderboard Population

    /// <summary>
    /// Populates the leaderboard UI by reading and sorting JSON files.
    /// </summary>
    private void PopulateLeaderboard()
    {
        // Ensure the folder exists
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"Folder not found: {folderPath}");
            return;
        }

        // Get all JSON files in the folder
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
        if (jsonFiles.Length == 0)
        {
            Debug.LogWarning("No JSON files found in the folder.");
            return;
        }

        // Parse JSON files and store players in a list
        List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

        foreach (string jsonFile in jsonFiles)
        {
            string jsonContent = File.ReadAllText(jsonFile);

            // Deserialize the JSON content
            GameResultJson result = JsonUtility.FromJson<GameResultJson>(jsonContent);

            if (result?.results == null)
            {
                Debug.LogWarning($"Invalid JSON structure in file: {jsonFile}");
                continue;
            }

            // Extract player name from file name
            string playerName = Path.GetFileNameWithoutExtension(jsonFile).Replace("RESULTS", "");

            // Create a leaderboard entry
            leaderboardEntries.Add(new LeaderboardEntry
            {
                PlayerName = playerName,
                Score = result.results.score
            });
        }

        // Sort leaderboard by score in descending order
        leaderboardEntries.Sort((a, b) => b.Score.CompareTo(a.Score));

        // Populate the leaderboard UI
        for (int i = 0; i < Mathf.Min(maxPlayers, leaderboardEntries.Count); i++)
        {
            var entry = leaderboardEntries[i];
            GameObject playerEntry = Instantiate(leaderBoardPlayerPrefab, playersParent);

            LeaderboardPlayer leaderboardPlayer = playerEntry.GetComponent<LeaderboardPlayer>();
            if (leaderboardPlayer != null)
            {
                leaderboardPlayer.SetPlayerData((i + 1).ToString(), entry.PlayerName, entry.Score.ToString());
            }
        }
    }

    #endregion
}

[System.Serializable]
public class LeaderboardEntry
{
    public string PlayerName; // Name of the player
    public int Score; // Player's score
}
