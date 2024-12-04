using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterHandler : MonoBehaviour
{
    [SerializeField] private string monsterName = "EVIL Monster";
    [SerializeField] private TextMeshProUGUI monsterNameText;
    [SerializeField] private SpriteRenderer monsterRenderer;

    [SerializeField] private FillBar lifeBar;
    [SerializeField] private FillBar attackBar;

    [Range(1, 100)]
    [SerializeField] private float maxLife = 25;

    [SerializeField] private float monsterDamage = 2;
    
    [Range (1, 30)]
    [SerializeField] private float attackTotalTime = 5f;
      
    private float attackTimer = 0;
    private float attackBarLoadTimer = 0;

    public FillBar LifeBar { get => lifeBar; set => lifeBar = value; }

    public void InitMonster(Color monsterColor, float maxMonsterLife)
    {
        maxLife = maxMonsterLife;
        LifeBar.InitBar(maxLife);
        attackBar.InitBar(attackTotalTime, false);
        monsterNameText.text = monsterName;
        monsterRenderer.color = monsterColor;
        LifeBar.onBarReachesZero.AddListener(Die);

        StartCoroutine(FakeUpdate());
    }


    private IEnumerator FakeUpdate()
    {         
        while (true) 
        {
            LoadAttack();
            UpdateAttackBar();
            yield return new WaitForEndOfFrame();
        }     
    }

    private void LoadAttack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackTotalTime)
        {
            GameManager.Singleton.AttackPlayer(monsterDamage);
            Debug.Log("RAW Monster attacking! " + monsterDamage);
            attackTimer = 0;
        }      
    }

    private void UpdateAttackBar()
    {
        attackBarLoadTimer += Time.deltaTime;
        float chargeValue = 1/attackTotalTime;       
        
        if (attackBarLoadTimer >= chargeValue)
        {
            attackBar.UpdateBar(chargeValue, false);
            attackBarLoadTimer = 0;
        }
    }

    public void ReciebeDamage(float damage)
    {
        LifeBar.UpdateBar(damage);     
    }

    private void Die()
    {
        LifeBar.onBarReachesZero.RemoveListener(Die);
        gameObject.SetActive(false);
    }
}
