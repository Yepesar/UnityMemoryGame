using System.Collections;
using TMPro;
using UnityEngine;

public class MonsterHandler : MonoBehaviour
{
    #region Variables

    [SerializeField] private string monsterName = "EVIL Monster";
    [SerializeField] private TextMeshProUGUI monsterNameText;
    [SerializeField] private SpriteRenderer monsterRenderer;
    [SerializeField] private GameObject deadVFX;

    [SerializeField] private FillBar lifeBar;
    [SerializeField] private FillBar attackBar;

    [Range(1, 100)]
    [SerializeField] private float maxLife = 25;

    [SerializeField] private float monsterDamage = 2;

    [Range(1, 30)]
    [SerializeField] private float attackTotalTime = 5f;

    private float attackTimer = 0;
    private float attackBarLoadTimer = 0;

    public FillBar LifeBar { get => lifeBar; set => lifeBar = value; }

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes the monster's attributes, such as color, max life, and attack timers.
    /// </summary>
    /// <param name="monsterColor">The color to assign to the monster's renderer.</param>
    /// <param name="maxMonsterLife">The maximum life value of the monster.</param>
    public void InitMonster(Color monsterColor, float maxMonsterLife)
    {
        maxLife = maxMonsterLife;
        LifeBar.InitBar(maxLife); // Initialize the life bar
        attackBar.InitBar(attackTotalTime, false); // Initialize the attack bar
        monsterNameText.text = monsterName; // Set the monster's name
        monsterRenderer.color = monsterColor; // Assign the color to the monster's sprite renderer

        // Subscribe to the life bar's "reaches zero" event
        LifeBar.onBarReachesZero.AddListener(Die);

        // Start the attack and update logic
        StartCoroutine(FakeUpdate());
    }

    #endregion

    #region Attack Logic

    /// <summary>
    /// Coroutine to continuously handle attack loading and updating the attack bar.
    /// </summary>
    private IEnumerator FakeUpdate()
    {
        while (true)
        {
            LoadAttack();
            UpdateAttackBar();
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Loads the monster's attack and triggers it when the timer exceeds the attack threshold.
    /// </summary>
    private void LoadAttack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackTotalTime)
        {
            GameManager.Singleton.AttackPlayer(monsterDamage); // Monster attacks the player
            Debug.Log($"Monster attacks! Damage: {monsterDamage}");
            attackTimer = 0;
        }
    }

    /// <summary>
    /// Updates the visual representation of the attack bar to indicate charge progress.
    /// </summary>
    private void UpdateAttackBar()
    {
        attackBarLoadTimer += Time.deltaTime;
        float chargeValue = 1 / attackTotalTime;

        if (attackBarLoadTimer >= chargeValue)
        {
            attackBar.UpdateBar(chargeValue, false); // Update attack bar progress
            attackBarLoadTimer = 0;
        }
    }

    #endregion

    #region Life Logic

    /// <summary>
    /// Reduces the monster's life based on the damage received.
    /// </summary>
    /// <param name="damage">The amount of damage to inflict.</param>
    public void ReciebeDamage(float damage)
    {
        LifeBar.UpdateBar(damage);
    }

    /// <summary>
    /// Handles the monster's death, including spawning VFX and deactivating the game object.
    /// </summary>
    private void Die()
    {
        // Instantiate death VFX and detach it from the monster
        GameObject vfx = Instantiate(deadVFX, transform);
        vfx.transform.parent = null;

        // Remove the listener from the life bar and deactivate the monster
        LifeBar.onBarReachesZero.RemoveListener(Die);
        gameObject.SetActive(false);
    }

    #endregion
}
