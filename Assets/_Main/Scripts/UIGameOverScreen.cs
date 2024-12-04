using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverScreen : MonoBehaviour
{
    [SerializeField] private SO_PlayerData playerData;
    [SerializeField] private GameObject contentGameObject;
    [SerializeField] private TextMeshProUGUI resultTitleText;
    [SerializeField] private TextMeshProUGUI totalClicksText;
    [SerializeField] private TextMeshProUGUI totalTimeText;
    [SerializeField] private TextMeshProUGUI pairsText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void DisplayScreen()
    {
        contentGameObject.gameObject.SetActive(true);

        resultTitleText.text = "Game Over";
        totalClicksText.text = "Total Clicks: " + playerData.PlayerGameResults.TotalClicks;
        totalTimeText.text = "Total Time: " + playerData.PlayerGameResults.TotalGameTime;
        pairsText.text = "Pairs: " + playerData.PlayerGameResults.TotalFindPairs;
        scoreText.text = "Score: " + playerData.PlayerGameResults.Score;
    }
}
