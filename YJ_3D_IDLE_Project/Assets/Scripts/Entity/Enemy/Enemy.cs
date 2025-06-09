using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("데이터")]
    public ShipStatsData enemyStatsData;

    private float currentHp;
    private PlayerAI playerAI;
    
    void Start()
    {
        currentHp = enemyStatsData.maxHp;
        playerAI = FindObjectOfType<PlayerAI>();
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.AddGold(enemyStatsData.rewardGold);

        if (playerAI != null)
        {
            playerAI.target = null;
        }
        Destroy(gameObject);
    }
}
