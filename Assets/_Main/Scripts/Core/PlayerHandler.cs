using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    #region Variables

    [Range(1f, 10f)]
    [SerializeField] private int maxLife = 10; // Maximum life of the player
    [SerializeField] private FillBar lifeBar; // UI bar to display the player's life

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes the player's life bar and subscribes to the event for when life reaches zero.
    /// </summary>
    private void Start()
    {
        lifeBar.InitBar(maxLife); // Set up the life bar with the max life value
        lifeBar.onBarReachesZero.AddListener(Die); // Subscribe to the event when life reaches zero
    }

    #endregion

    #region Combat

    /// <summary>
    /// Handles the player's attack using data from a slot.
    /// </summary>
    /// <param name="data">The slot data used to perform the attack.</param>
    public void Attack(SO_SlotData data)
    {
        if (data != null)
        {
            Debug.Log($"Attacking with {data.SlotName}, dealing {data.SlotDamage} damage!");
            GameManager.Singleton.AttackMonster(data.SlotDamage, data.VFXType); // Attack the monster
        }
    }

    /// <summary>
    /// Reduces the player's life by the specified amount.
    /// </summary>
    /// <param name="amount">The amount of damage received.</param>
    public void RecieveDamage(float amount)
    {
        lifeBar.UpdateBar(amount);
    }

    #endregion

    #region Death Logic

    /// <summary>
    /// Handles the player's death, unsubscribing from events and ending the game.
    /// </summary>
    public void Die()
    {
        Debug.Log("RIP player dead"); // Log the player's death
        lifeBar.onBarReachesZero.RemoveListener(Die); // Unsubscribe from the event to prevent multiple calls
        GameManager.Singleton.GameOver(); // Trigger game over in the GameManager
    }

    #endregion
}
