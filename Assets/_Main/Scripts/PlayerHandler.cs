using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [Range(1f, 10f)]
    [SerializeField] private int maxLife = 10;
    [SerializeField] private FillBar lifeBar;

    // Start is called before the first frame update
    void Start()
    {
        lifeBar.InitBar(maxLife);
        lifeBar.onBarReachesZero.AddListener(Die);
    }

    public void Attack(SO_SlotData data)
    {
        Debug.Log("Attacking with " + data.SlotName + " , " + data.SlotDamage + " of Damage!");
        GameManager.Singleton.AttackMonster(data.SlotDamage);
    }

    public void RecieveDamage(float amount)
    {
        lifeBar.UpdateBar(amount);
    }

    public void Die()
    {
        Debug.Log("RIP player dead");
        lifeBar.onBarReachesZero.RemoveListener(Die);
    }
}
