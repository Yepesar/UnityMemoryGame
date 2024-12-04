using TMPro;
using UnityEngine;

public class UIGameOverScreen : MonoBehaviour
{
    #region Variables

    [SerializeField] private SO_PlayerData playerData; // Reference to player data containing game results
    [SerializeField] private GameObject contentGameObject; // GameObject containing the UI elements for the game over screen
    [SerializeField] private TextMeshProUGUI totalClicksText; // UI element for total clicks
    [SerializeField] private TextMeshProUGUI totalTimeText; // UI element for total time
    [SerializeField] private TextMeshProUGUI pairsText; // UI element for total pairs found
    [SerializeField] private TextMeshProUGUI scoreText; // UI element for the score

    #endregion

    #region Public Methods

    /// <summary>
    /// Displays the game over screen with the player's game results.
    /// </summary>
    public void DisplayScreen()
    {
        // Activate the content GameObject
        contentGameObject.SetActive(true);

        // Update UI elements with player's results
        totalClicksText.text = $"Total Clicks: {playerData.PlayerGameResults.TotalClicks}";
        totalTimeText.text = $"Total Time: {playerData.PlayerGameResults.TotalGameTime}s";
        pairsText.text = $"Pairs: {playerData.PlayerGameResults.TotalFindPairs}";
        scoreText.text = $"Score: {playerData.PlayerGameResults.Score}";
    }

    #endregion
}
