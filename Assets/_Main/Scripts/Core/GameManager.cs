using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private PlayerHandler player;
    [SerializeField] private SO_PlayerData playerData;
    [SerializeField] private SlotsCreator slotsCreator;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Gradient monsterColors;
    [SerializeField] private Transform monsterParent;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float revealTime = 0.25f;
    [SerializeField] private float gameOverDelay = 1.0f;

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

    #endregion

    #region Initialization

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        playerData.ResetResults();
        slotsCreator.InitGenerator();
        CreateNewMonster();
        StartCoroutine(UpdateTimeTimer());
    }

    #endregion

    #region Game Logic

    /// <summary>
    /// Ends the game and handles game over logic.
    /// </summary>
    public void GameOver()
    {
        StartCoroutine(GameOverSystem());
    }

    /// <summary>
    /// Coroutine to handle the game over process.
    /// </summary>
    private IEnumerator GameOverSystem()
    {
        yield return new WaitForSeconds(gameOverDelay);

        StopAllCoroutines();
        actualMonster.gameObject.SetActive(false);
        SavePlayerData();
        Debug.Log("GAME OVER!");

        onGameOver?.Invoke();
    }

    /// <summary>
    /// Checks if all pairs have been revealed to determine if the game is complete.
    /// </summary>
    private void ValidateAllPairs()
    {
        if (actualPairs >= slotsCreator.Pairs)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Validates if two selected slots form a matching pair.
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
    /// Coroutine to compare two selected slots for a match.
    /// </summary>
    private IEnumerator ComparingSystem()
    {
        bool datNull = validationPairA.GetSlotData() == null && validationPairB.GetSlotData() == null;
        bool match = validationPairA.GetSlotData() == validationPairB.GetSlotData();

        if (!datNull && match)
        {
            Debug.Log("Match found!");

            validationPairA.Lock();
            validationPairB.Lock();

            actualPairs++;
            ValidateAllPairs();

            onPairHit?.Invoke(validationPairA.GetSlotData());
        }
        else
        {
            yield return new WaitForSeconds(revealTime);

            validationPairA.UnRevealSlot();
            validationPairB.UnRevealSlot();

            Debug.Log("Not a match!");
        }

        validationPairA = null;
        validationPairB = null;
        pairCheckCoroutine = null;
    }

    #endregion

    #region Player and Monster Interaction

    /// <summary>
    /// Inflicts damage to the player.
    /// </summary>
    public void AttackPlayer(float damage)
    {
        player.RecieveDamage(damage);
    }

    /// <summary>
    /// Inflicts damage to the monster and spawns visual effects.
    /// </summary>
    public void AttackMonster(float damage, VFXTypes vFXType = VFXTypes.None)
    {
        VFXManager.Singleton.SpawnVFX(vFXType, actualMonster.transform);
        actualMonster.ReciebeDamage(damage);
    }

    /// <summary>
    /// Creates a new monster with random attributes.
    /// </summary>
    public void CreateNewMonster()
    {
        GameObject newMonster = Instantiate(monsterPrefab, monsterParent);
        newMonster.transform.localPosition = Vector3.zero;
        actualMonster = newMonster.GetComponent<MonsterHandler>();

        float rand = Random.Range(0f, 1f);
        Color newMonsterColor = monsterColors.Evaluate(rand);
        actualMonster.InitMonster(newMonsterColor, slotsCreator.GetSlotsTotalDamage());
    }

    #endregion

    #region Timer and Data Saving

    /// <summary>
    /// Coroutine to update the game timer and save progress.
    /// </summary>
    private IEnumerator UpdateTimeTimer()
    {
        while (true)
        {
            int minutes = Mathf.FloorToInt(gameElapsedTime / 60);
            int seconds = Mathf.FloorToInt(gameElapsedTime % 60);

            timerText.text = $"{minutes:00}:{seconds:00}";

            yield return new WaitForSeconds(1);
            gameElapsedTime += 1f;

            SavePlayerData();
        }
    }

    /// <summary>
    /// Saves the player's current game data.
    /// </summary>
    private void SavePlayerData()
    {
        playerData.PlayerGameResults.TotalClicks = totalClicks;
        playerData.PlayerGameResults.TotalGameTime = Mathf.FloorToInt(gameElapsedTime);
        playerData.PlayerGameResults.TotalFindPairs = actualPairs;
        playerData.CalculateScore();
    }

    #endregion
}
