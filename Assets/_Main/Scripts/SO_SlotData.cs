using UnityEngine;

[CreateAssetMenu(fileName = "New Slot Data", menuName = "ScriptableObjects/NewSlotData")]
public class SO_SlotData : ScriptableObject
{
    [Header("Slot Information")]
    public string SlotName; // Name of the slot (for identification)

    [Header("Visuals")]
    public Sprite SlotIcon; // Icon displayed on the slot

    [Header("Gameplay")]
    [Range(1, 100)]
    public int SlotDamage = 1; // Damage value associated with this slot
}
