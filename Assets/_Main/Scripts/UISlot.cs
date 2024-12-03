using UnityEngine;
using UnityEngine.UI;
using System;

public class UISlot : MonoBehaviour
{
    [SerializeField] private SO_SlotData slotData; // Slot's data
    [SerializeField] private Button slotButton; // Button to interact with the slot
    [SerializeField] private GameObject slotSelection; // Highlight or selection indicator
    [SerializeField] private Sprite coverSlotSprite; // Sprite for covered state
    [SerializeField] private Sprite uncoverSlotSprite; // Sprite for uncovered state
    [SerializeField] private Image slotImage; // Background image of the slot
    [SerializeField] private Image slotIconImage; // Icon image to show slot data

    private bool isLocked = false;

    public bool IsLocked {get => isLocked;}

    private void Start()
    {
        // Assign the button's click event
        slotButton.onClick.AddListener(HandleSlotClick);
    }
   
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
    /// Locks the slot, making it non-interactable and hiding the selection.
    /// </summary>
    public void Lock()
    {
        slotButton.interactable = false;
        slotSelection.gameObject.SetActive(false);
        isLocked = true;
    }

    /// <summary>
    /// Reveals the slot, showing its icon and triggering the selection event.
    /// </summary>
    public void RevealSlot()
    {
        slotSelection.gameObject.SetActive(true);
        slotIconImage.gameObject.SetActive(true);
        slotImage.sprite = uncoverSlotSprite;

        // Assign the slot data's icon if available
        slotIconImage.sprite = slotData != null ? slotData.SlotIcon : null;

        // Invoke the OnSlotSelected event
        GameManager.Singleton?.ValidatePair(this);
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

    /// <summary>
    /// Cleans up the event listener when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        slotButton.onClick.RemoveAllListeners();
    }
}
