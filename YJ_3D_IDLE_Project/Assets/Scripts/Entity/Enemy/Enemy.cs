using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("데이터")]
    public ShipStatsData enemyStatsData;
    
    [Header("UI")]
    public GameObject healthBar;
    public Transform healthBarAnchor;
    private FloatingHealthBar floatingHealthBar;
    
    [Header("AI관리")]
    public float detectionRange = 40f;
    public float attackCooldown = 3f;

    private float maxHp;
    private float currentHp;
    private Transform playerTransform;
    private Player player;
    
    public event Action<float, float> OnHpChanged;
    
    void Start()
    {
        maxHp = enemyStatsData.initialMaxHp;
        currentHp = maxHp;
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        playerTransform = playerObject.transform;
        player = playerObject.GetComponent<Player>();

        if (healthBar != null && healthBarAnchor != null)
        {
            GameObject healthBarObj = Instantiate(healthBar, healthBarAnchor.position, Quaternion.identity, healthBarAnchor);
            floatingHealthBar = healthBarObj.GetComponentInChildren<FloatingHealthBar>();

            if (floatingHealthBar != null)
            {
                OnHpChanged += floatingHealthBar.UpdateHealthBar;
                OnHpChanged?.Invoke(currentHp, maxHp);
            }
        }
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
        OnHpChanged?.Invoke(currentHp, maxHp);

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
            player.TakeDamage(enemyStatsData.initialAttackPower);
        }
    }

    private void OnDestroy()
    {
        if (floatingHealthBar != null)
        {
            OnHpChanged -= floatingHealthBar.UpdateHealthBar;
        }
    }
}
