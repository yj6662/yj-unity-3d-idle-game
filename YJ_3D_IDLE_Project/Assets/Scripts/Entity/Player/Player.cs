using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    public ShipStatsData baseStats;

    [Header("능력치")]
    public float maxHp;
    public float currentHp;
    public float baseAttackPower;
    public float moveSpeed;

    [Header("레벨,경험치")]
    public int level = 1;
    public int exp = 0;
    public int nextLevelExp = 100;
    
    public event Action<float, float> OnHpChanged;
    public event Action<float, float> OnXPChanged; 
    
    private float attackBuffMultiplier = 1.0f;

    public float CurrentFinalAttackPower => baseAttackPower * attackBuffMultiplier;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        if (baseStats != null)
        {
            maxHp = baseStats.maxHp;
            currentHp = maxHp;
            baseAttackPower = baseStats.attackPower;
            moveSpeed = baseStats.moveSpeed;
        }
    }

    private void Start()
    {
        OnHpChanged?.Invoke(currentHp, maxHp);
        OnXPChanged?.Invoke(exp, nextLevelExp);
    }
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
        OnHpChanged?.Invoke(currentHp, maxHp);
    }

    public void AddExp(int amount)
    {
        exp += amount;
        
        if (exp >= nextLevelExp)
        {
            level++;
            nextLevelExp += 5 * level * level;
        }
        OnXPChanged?.Invoke(exp, nextLevelExp);
    }

    void Die()
    {
        Time.timeScale = 0;
        //TODO: 게임 오버 처리
    }

    public void UpgradeAttack(float amount)
    {
        baseAttackPower += amount;
    }

    public IEnumerator ApplyAttackBuff(float amount, float duration)
    {
        attackBuffMultiplier = amount;

        yield return new WaitForSeconds(duration);
        attackBuffMultiplier = 1.0f;
    }
}

