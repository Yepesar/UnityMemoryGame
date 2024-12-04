using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerHandler player;
    [SerializeField] private SO_PlayerData playerData;
    [SerializeField] private SlotsCreator slotsCreator;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Gradient monsterColors;
    [SerializeField] private Transform monsterParent;
    [SerializeField] private TextMeshProUGUI timerText;

    private MonsterHandler actualMonster;

    public static GameManager Singleton;

    private UISlot validationPairA = null;
    private UISlot validationPairB = null;
    private IEnumerator pairCheckCoroutine = null;

    public UnityEvent onGameOver;
    public UnityEvent<SO_SlotData> onPairHit;

    private int actualPairs = 0;
    private int totalClicks = 0;
    private float gameElapsedTime = 0f;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(Singleton);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerData.ResetResults();
        slotsCreator.InitGenerator();
        CreateNewMonster();
        StartCoroutine(UpdateTimeTimer());         
    }

    public void GameOver()
    {
        StopAllCoroutines();
        actualMonster.gameObject.SetActive(false);
        SavePlayerData();
        Debug.Log("GAME OVER!");

        onGameOver?.Invoke();
    }

    /// <summary>
    /// Check if the player unreveal all the pairs
    /// </summary>
    private void ValidateAllPairs()
    {
        int amount = slotsCreator.Pairs;
        if (actualPairs >= amount)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Validates if two selected slots form a pair.
    /// </summary>
    public void ValidatePair(UISlot slot)
    {
        totalClicks++;

        if (validationPairA == null)
        {
            validationPairA = slot;
        }
        else if (validationPairB == null)
        {
            validationPairB = slot;
        }

        if (validationPairA != null && validationPairB != null && pairCheckCoroutine == null)
        {
            pairCheckCoroutine = ComparingSystem();
            StartCoroutine(pairCheckCoroutine);
        }
    }

    /// <summary>
    /// Coroutine to compare two selected slots.
    /// </summary>
    private IEnumerator ComparingSystem()
    {
        if (validationPairA.GetSlotData() == validationPairB.GetSlotData())
        {
            Debug.Log("Match found!");
            
            validationPairA.Lock();
            validationPairB.Lock();
            
            actualPairs++;
            Debug.Log("Founded Pairs: " + actualPairs);
            ValidateAllPairs();
             
            onPairHit?.Invoke(validationPairA.GetSlotData());
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

            validationPairA.UnRevealSlot();
            validationPairB.UnRevealSlot();
            
            Debug.Log("Not a match!");
        }
      
        validationPairA = null;
        validationPairB = null;
        pairCheckCoroutine = null;              
    }

    public void AttackPlayer(float damage)
    {
        player.RecieveDamage(damage);
    }

    public void AttackMonster(float damage)
    {
        actualMonster.ReciebeDamage(damage);
    }

    public void CreateNewMonster()
    {
        GameObject newMonster = Instantiate(monsterPrefab, monsterParent);
        newMonster.transform.localPosition = Vector3.zero;
        actualMonster = newMonster.GetComponent<MonsterHandler>();
        float rand = Random.Range(0f, 1f);
        Color newMonsterColor = monsterColors.Evaluate(rand);
        actualMonster.InitMonster(newMonsterColor,slotsCreator.GetSlotsTotalDamage());
    }

    private IEnumerator UpdateTimeTimer()
    {
        while (true)
        {
            // Convert the elapsed time into minutes and seconds
            int minutes = Mathf.FloorToInt(gameElapsedTime / 60);
            int seconds = Mathf.FloorToInt(gameElapsedTime % 60);

            // Format the time as MM:SS
            string formattedTime = $"{minutes:00}:{seconds:00}";

            // Update the timer text
            timerText.text = formattedTime;

            // Wait for one second
            yield return new WaitForSeconds(1);

            // Increment the elapsed time
            gameElapsedTime += 1f;

            SavePlayerData();
        }
    }

    private void SavePlayerData()
    {
        playerData.PlayerGameResults.TotalClicks = totalClicks;
        playerData.PlayerGameResults.TotalGameTime = Mathf.FloorToInt(gameElapsedTime % 60);
        playerData.PlayerGameResults.TotalFindPairs = actualPairs;
        //playerData.PlayerGameResults.Score = 100;
    }

}
