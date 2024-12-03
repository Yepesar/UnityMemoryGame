using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SlotsCreator slotsCreator;
    
    public static GameManager Singleton;

    private UISlot validationPairA = null;
    private UISlot validationPairB = null;
    private IEnumerator pairCheckCoroutine = null;

    private int actualPairs = 0;

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
        slotsCreator.InitGenerator();
    }

    /// <summary>
    /// Check if the player unreveal all the pairs
    /// </summary>
    private void ValidateAllPairs()
    {
        int amount = slotsCreator.Pairs;
        if (actualPairs >= amount)
        {
            actualPairs = 0;
            Debug.Log("GAME OVER!");
            //GAME OVER
        }
    }

    /// <summary>
    /// Validates if two selected slots form a pair.
    /// </summary>
    public void ValidatePair(UISlot slot)
    {
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
}
