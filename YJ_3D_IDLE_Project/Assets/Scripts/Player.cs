using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ShipStatsData baseStats;

    public float maxHp;
    public float currentHp;
    public float baseAttackPower;
    public float moveSpeed;

    private float attackBuffMultiplier = 1.0f;

    public float CurrentFinalAttackPower => baseAttackPower * attackBuffMultiplier;

    private void Awake()
    {
        if (baseStats == null)
        {
            maxHp = baseStats.maxHp;
            currentHp = maxHp;
            baseAttackPower = baseStats.attackPower;
            moveSpeed = baseStats.moveSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
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

