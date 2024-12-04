using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    #region Variables

    [SerializeField] private SO_SlotData slotData; // Slot's data reference
    [SerializeField] private Button slotButton; // Button to interact with the slot
    [SerializeField] private GameObject slotSelection; // Visual indicator for selection
    [SerializeField] private Sprite coverSlotSprite; // Sprite for the covered state
    [SerializeField] private Sprite uncoverSlotSprite; // Sprite for the uncovered state
    [SerializeField] private Image slotImage; // Background image of the slot
    [SerializeField] private Image slotIconImage; // Icon to display slot data

    private bool isLocked = false;

    public bool IsLocked => isLocked; // Read-only property to check if the slot is locked

    #endregion

    #region Initialization

    private void Start()
    {
        // Assign the button's click event
        slotButton.onClick.AddListener(HandleSlotClick);
    }

    #endregion

    #region Slot Interaction

    /// <summary>
    /// Handles the click event on the slot button.
    /// Toggles between revealing and unrevealing the slot.
    /// </summary>
    private void HandleSlotClick()
    {
        if (slotSelection.gameObject.activeInHierarchy)
        {
            UnRevealSlot();
        }
        else
        {
            RevealSlot();
        }
    }

    /// <summary>
    /// Reveals the slot, showing its icon and triggering the selection event.
    /// </summary>
    public void RevealSlot()
    {
        if (slotData == null) // Empty slot
        {
            slotSelection.gameObject.SetActive(false);
            slotIconImage.gameObject.SetActive(false);
            slotImage.sprite = coverSlotSprite;
            slotButton.interactable = false;
        }
        else
        {
            slotSelection.gameObject.SetActive(true);
            slotIconImage.gameObject.SetActive(true);
            slotImage.sprite = uncoverSlotSprite;

            // Assign the slot data's icon if available
            slotIconImage.sprite = slotData != null ? slotData.SlotIcon : null;

            // Notify the GameManager about the selection
            GameManager.Singleton?.ValidatePair(this);
        }
    }

    /// <summary>
    /// Hides the slot's content, resetting it to the covered state.
    /// </summary>
    public void UnRevealSlot()
    {
        slotSelection.gameObject.SetActive(false);
        slotIconImage.gameObject.SetActive(false);
        slotImage.sprite = coverSlotSprite;
    }

    /// <summary>
    /// Locks the slot, making it non-interactable and hiding the selection.
    /// </summary>
    public void Lock()
    {
        slotButton.interactable = false;
        slotSelection.gameObject.SetActive(false);
        isLocked = true;
    }

    #endregion

    #region Slot Data Management

    /// <summary>
    /// Assigns data to the slot.
    /// </summary>
    /// <param name="data">The data to assign to this slot.</param>
    public void SetSlotData(SO_SlotData data)
    {
        slotData = data;
    }

    /// <summary>
    /// Retrieves the slot's data.
    /// </summary>
    /// <returns>The slot's data.</returns>
    public SO_SlotData GetSlotData()
    {
        return slotData;
    }

    #endregion

    #region Cleanup

    /// <summary>
    /// Cleans up the event listener when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        slotButton.onClick.RemoveAllListeners();
    }

    #endregion
}
