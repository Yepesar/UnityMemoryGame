using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerPositionText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerScoreText;

    public void SetPlayerData(string position, string name, string score)
    {
        playerPositionText.text = position;
        playerNameText.text = name;
        playerScoreText.text = score;
    }
}
