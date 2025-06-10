using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    public ShipStatsData baseStats;
    
    public float maxHp { get; private set; }
    public float currentHp { get; private set; }
    public float baseAttackPower { get; private set; }
    public float moveSpeed { get; private set; }

    [Header("레벨,경험치")]
    public int level = 1;
    public float exp = 0;
    public float nextLevelExp = 100;

    [Header("파츠 강화")] 
    public int cannonLevel = 1;
    public int hullLevel = 1;
    public int sailLevel = 1;
    
    [Header("UI")]
    public GameObject healthBar;
    public Transform healthBarAnchor;
    private FloatingHealthBar floatingHealthBar;
    
    
    public event Action<float, float> OnHpChanged;
    public event Action<float, float> OnXPChanged;
    public event Action<int> OnLevelUP;
    public event Action OnStatsUpdated;
    
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

        CalculateStats();
    }

    private void Start()
    {
        currentHp = maxHp;
        
        if (healthBar != null && healthBarAnchor != null)
        {
            GameObject healthBarObj = Instantiate(healthBar, healthBarAnchor.position, Quaternion.identity, healthBarAnchor);
            floatingHealthBar = healthBarObj.GetComponent<FloatingHealthBar>();
        }

        if (floatingHealthBar != null)
        {
            OnHpChanged += floatingHealthBar.UpdateHealthBar;
        }
        OnHpChanged?.Invoke(currentHp, maxHp);
        OnXPChanged?.Invoke(exp, nextLevelExp);
        OnLevelUP?.Invoke(level);
        OnStatsUpdated?.Invoke();
    }

    void CalculateStats()
    {
        if (baseStats == null) return;
        
        float hpBonus = (level - 1) * 10; 
        float attackBonus = (level - 1) * 2;
        
        maxHp = baseStats.initialMaxHp + ((hullLevel - 1) * baseStats.hpPerUpgrade) + hpBonus;
        baseAttackPower = baseStats.initialAttackPower + ((cannonLevel - 1) * baseStats.attackPerUpgrade) + attackBonus;
        moveSpeed = baseStats.initialMoveSpeed + ((sailLevel - 1) * baseStats.speedPerUpgrade);
    }
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        OnHpChanged?.Invoke(currentHp, maxHp);
        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.ShakeCamera(2.0f, 0.2f);
        }
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }

    }

    public void AddExp(int amount)
    {
        exp += amount;

        while (exp >= nextLevelExp)
        {
            LevelUP();
        }
        
        OnXPChanged?.Invoke(exp, nextLevelExp);
    }

    void LevelUP()
    {
        exp -= nextLevelExp;
        level++;
        nextLevelExp *= 1.2f;
        UpdateStats();
        Heal(maxHp);
        
        OnLevelUP?.Invoke(level);
    }

    void Die()
    {
        Time.timeScale = 0;
        //TODO: 게임 오버 처리
    }
    

    public IEnumerator ApplyAttackBuff(float amount, float duration)
    {
        attackBuffMultiplier = amount;

        yield return new WaitForSeconds(duration);
        attackBuffMultiplier = 1.0f;
    }

    public void UpgradeCannon()
    {
        cannonLevel++;
        UpdateStats();
    }
    public void UpgradeHull()
    {
        hullLevel++;
        UpdateStats();
        Heal(baseStats.hpPerUpgrade);
    }
    public void UpgradeSail()
    {
        sailLevel++;
        UpdateStats();
    }
    
    void UpdateStats()
    {
        CalculateStats();
        OnStatsUpdated?.Invoke();
    }
    public int GetCannonUpgradeCost() => 10 * cannonLevel;
    public int GetHullUpgradeCost() => 10 * hullLevel;
    public int GetSailUpgradeCost() => 10 * sailLevel;
    
    public void Heal(float amount)
    {
        currentHp = Mathf.Min(currentHp + amount, maxHp);
        OnHpChanged?.Invoke(currentHp, maxHp);
    }

    private void OnDestroy()
    {
        if (floatingHealthBar != null)
        {
            OnHpChanged -= floatingHealthBar.UpdateHealthBar;
        }
    }

}

