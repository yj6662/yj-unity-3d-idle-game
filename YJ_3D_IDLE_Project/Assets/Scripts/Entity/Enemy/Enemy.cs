using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("데이터")]
    public ShipStatsData enemyStatsData;
    
    [Header("AI관리")]
    public float detectionRange = 40f;
    public float attackCooldown = 3f;

    private float currentHp;
    private Transform playerTransform;
    private Player player;
    
    void Start()
    {
        currentHp = enemyStatsData.maxHp;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        playerTransform = playerObject.transform;
        player = playerObject.GetComponent<Player>();

        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        while (true)
        {
            if (playerTransform != null &&
                Vector3.Distance(transform.position, playerTransform.position) <= detectionRange)
            {
                AttackPlayer();
            }

            yield return new WaitForSeconds(attackCooldown);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        Debug.Log("HP : " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.AddGold(enemyStatsData.rewardGold);
        Player.Instance.AddExp(enemyStatsData.rewardExp);
        
        GameManager.Instance.SpawnNextEnemy();
        
        Destroy(gameObject);
    }

    void AttackPlayer()
    {
        if (player != null)
        {
            player.TakeDamage(enemyStatsData.attackPower);
        }
    }
}
