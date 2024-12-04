using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotsCreator : MonoBehaviour
{
    #region Variables

    [SerializeField] private SO_BlocksData blockData; // Reference to block data

    [Range(2, 8)]
    [SerializeField] private int rows = 4; // Number of rows
    [Range(2, 8)]
    [SerializeField] private int columns = 3; // Number of columns

    [Range(1, 10)]
    [SerializeField] private int pairs = 4; // Number of pairs

    [SerializeField] private SO_SlotData[] slotDataPool; // Slot data pool
    [SerializeField] private GameObject slotObjectPrefab; // Prefab for slot objects
    [SerializeField] private Transform slotsParent; // Parent for the slot objects

    private List<GameObject> createdSlots = new List<GameObject>();
    private GridLayoutGroup gridLayoutGroup;

    public List<GameObject> CreatedSlots => createdSlots;
    public int Pairs => pairs;

    #endregion

    #region Initialization

    private void Awake()
    {
        // Validate and initialize the Grid Layout Group
        gridLayoutGroup = slotsParent.GetComponent<GridLayoutGroup>();
        if (!gridLayoutGroup)
        {
            Debug.LogError("The parent object must have a Grid Layout Group component.");
            return;
        }

        // Adjust rows, columns, and pairs if blockData is available
        if (blockData != null)
        {
            AdjustGridFromRandomBlock();
        }
    }

    /// <summary>
    /// Adjusts rows, columns, and pairs based on a random block from blockData.
    /// </summary>
    private void AdjustGridFromRandomBlock()
    {
        Block randomBlock = blockData.GetRandomBlock();

        if (randomBlock != null)
        {
            rows = Mathf.Clamp(randomBlock.R, 2, 8);
            columns = Mathf.Clamp(randomBlock.C, 2, 8);
            pairs = Mathf.Clamp(randomBlock.number, 1, 10);

            Debug.Log($"Random block selected: Rows = {rows}, Columns = {columns}, Pairs = {pairs}");
        }
        else
        {
            Debug.LogWarning("No valid block selected from blockData.");
        }
    }

    #endregion

    #region Slot Creation and Assignment

    /// <summary>
    /// Initialize the generator and create the slots.
    /// </summary>
    public void InitGenerator()
    {
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
            pairs = totalSlots / 2;
        }
        else
        {
            pairs = totalPairs;
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

    #endregion

    #region Utility Methods

    /// <summary>
    /// Calculates the total damage from all slots.
    /// </summary>
    /// <returns>Total damage divided by 2.</returns>
    public float GetSlotsTotalDamage()
    {
        float totalDamage = 0;
        for (int i = 0; i < createdSlots.Count; i++)
        {
            UISlot uiSlot = createdSlots[i].GetComponent<UISlot>();
            if (uiSlot != null && uiSlot.GetSlotData() != null)
            {
                totalDamage += uiSlot.GetSlotData().SlotDamage;
            }
        }

        return totalDamage / 2;
    }

    #endregion
}
