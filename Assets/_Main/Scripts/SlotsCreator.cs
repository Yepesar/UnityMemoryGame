using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotsCreator : MonoBehaviour
{
    [Range(2, 8)]
    [SerializeField] private int rows = 4; // Number of rows
    [Range(2, 8)]
    [SerializeField] private int columns = 3; // Number of columns

    [Range(1, 10)]
    [SerializeField] private int pairs = 4; // Number of pairs

    [SerializeField] private SO_SlotData[] slotDataPool;
    [SerializeField] private GameObject slotObjectPrefab;
    [SerializeField] private Transform slotsParent;

    private List<GameObject> createdSlots = new List<GameObject>();
    private GridLayoutGroup gridLayoutGroup;

    private UISlot validationPairA = null;
    private UISlot validationPairB = null;
    private IEnumerator pairCheckCoroutine = null;

    private void Start()
    {
        // Validate and initialize the Grid Layout Group
        gridLayoutGroup = slotsParent.GetComponent<GridLayoutGroup>();
        if (!gridLayoutGroup)
        {
            Debug.LogError("The parent object must have a Grid Layout Group component.");
            return;
        }

        ConfigureGridLayout();
        CreateSlots();
        AssignSlotData();
    }

    /// <summary>
    /// Configures the Grid Layout Group for proper alignment of slots.
    /// </summary>
    private void ConfigureGridLayout()
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;

        float parentWidth = ((RectTransform)slotsParent).rect.width;
        float parentHeight = ((RectTransform)slotsParent).rect.height;

        float spacing = 10f; // Spacing between slots
        gridLayoutGroup.spacing = new Vector2(spacing, spacing);

        float cellWidth = (parentWidth - ((columns - 1) * spacing)) / columns;
        float cellHeight = (parentHeight - ((rows - 1) * spacing)) / rows;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
    }

    /// <summary>
    /// Instantiates slots based on the number of rows and columns.
    /// </summary>
    private void CreateSlots()
    {
        createdSlots.Clear();

        for (int i = 0; i < rows * columns; i++)
        {
            GameObject slotInstance = Instantiate(slotObjectPrefab, slotsParent);
            createdSlots.Add(slotInstance);
        }
    }

    /// <summary>
    /// Assigns data to slots and randomizes their order.
    /// </summary>
    private void AssignSlotData()
    {
        int totalSlots = rows * columns;
        int totalPairs = Mathf.Min(pairs, totalSlots / 2);

        // Log a warning if the number of pairs exceeds available slots
        if (pairs > totalSlots / 2)
        {
            Debug.LogWarning($"Number of pairs ({pairs}) exceeds available slots. Adjusted to {totalSlots / 2}.");
        }

        List<SO_SlotData> assignedSlotData = new List<SO_SlotData>();
        for (int i = 0; i < totalPairs; i++)
        {
            assignedSlotData.Add(slotDataPool[i]);
            assignedSlotData.Add(slotDataPool[i]);
        }

        while (assignedSlotData.Count < totalSlots)
        {
            assignedSlotData.Add(null); // Fill remaining slots with null
        }

        ShuffleList(assignedSlotData);

        for (int i = 0; i < createdSlots.Count; i++)
        {
            UISlot uiSlot = createdSlots[i].GetComponent<UISlot>();
            if (uiSlot != null)
            {
                uiSlot.SetSlotData(assignedSlotData[i]);
                uiSlot.OnSlotSelected += () => ValidatePair(uiSlot); // Subscribe to slot selection
            }
        }
    }

    /// <summary>
    /// Shuffles a list using the Fisher-Yates algorithm.
    /// </summary>
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
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
