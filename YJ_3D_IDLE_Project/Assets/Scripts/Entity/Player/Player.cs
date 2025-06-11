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
    public GameObject healthBar; //체력바 프리팹
    public Transform healthBarAnchor; // 체력바가 생성될 위치
    private FloatingHealthBar floatingHealthBar;
    
    private Animator animator;
    
    // 플레이어의 상태가 변경될 때 다른 객체에 알림
    public event Action<float, float> OnHpChanged;
    public event Action<float, float> OnXPChanged;
    public event Action<int> OnLevelUP;
    public event Action OnStatsUpdated;
    
    private float attackBuffMultiplier = 1.0f; // 버프 적용을 위한 배율

    public float CurrentFinalAttackPower => baseAttackPower * attackBuffMultiplier; // 최종 공격력 계산

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        animator = GetComponent<Animator>();
        
        CalculateStats(); // 최초 스탯 계산
    }

    private void Start()
    {
        currentHp = maxHp;
        
        // 체력바 UI 생성
        if (healthBar != null && healthBarAnchor != null)
        {
            GameObject healthBarObj = Instantiate(healthBar, healthBarAnchor.position, Quaternion.identity, healthBarAnchor);
            floatingHealthBar = healthBarObj.GetComponent<FloatingHealthBar>();
        }

        // 체력 변경 이벤트에 체력바 업데이트 함수 연결
        if (floatingHealthBar != null)
        {
            OnHpChanged += floatingHealthBar.UpdateHealthBar;
        }
        
        // 게임 시작 시 UI와 데이터의 초기 상태를 반영하기 위해 이벤트를 한 번씩 호출
        OnHpChanged?.Invoke(currentHp, maxHp);
        OnXPChanged?.Invoke(exp, nextLevelExp);
        OnLevelUP?.Invoke(level);
        OnStatsUpdated?.Invoke();
    }

    void CalculateStats()
    {
        // 플레이어의 레벨과 강화 정도에 따라 최종 스탯 계산
        if (baseStats == null) return;
        
        // 레벨업에 따른 스탯 보너스
        float hpBonus = (level - 1) * 10; 
        float attackBonus = (level - 1) * 2;
        
        // 최종 스탯 = 기본 스탯 + (강화 보너스) + (레벨 보너스)
        maxHp = baseStats.initialMaxHp + ((hullLevel - 1) * baseStats.hpPerUpgrade) + hpBonus;
        baseAttackPower = baseStats.initialAttackPower + ((cannonLevel - 1) * baseStats.attackPerUpgrade) + attackBonus;
        moveSpeed = baseStats.initialMoveSpeed + ((sailLevel - 1) * baseStats.speedPerUpgrade);
    }
    public void TakeDamage(float damage)
    {
        // 데미지 처리
        currentHp -= damage;
        OnHpChanged?.Invoke(currentHp, maxHp); // 체력 변경 이벤트를 호출하여 UI에 알림
        
        animator.SetTrigger("isDamaged");
        
        // 카메라 흔들림 효과
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

    public void OnAttack()
    {
        animator.SetTrigger("isAttack");
    }

    public void AddExp(int amount)
    {
        // 경험치 획득 및 레벨업
        exp += amount;

        while (exp >= nextLevelExp)
        {
            LevelUP();
        }
        
        OnXPChanged?.Invoke(exp, nextLevelExp); // 경험치 변경 이벤트를 호출하여 UI에 알림
    }

    void LevelUP()
    {
        // 레벨업 처리 및 요구 경험치/스탯 증가
        exp -= nextLevelExp;
        level++;
        nextLevelExp *= 1.2f;
        UpdateStats();
        Heal(maxHp);
        
        OnLevelUP?.Invoke(level); // 레벨업 이벤트를 호출하여 UI에 알림
    }

    void Die()
    {
        animator.SetTrigger("isDie");
    }
    

    public IEnumerator ApplyAttackBuff(float amount, float duration)
    {
        // 공격력 버프 적용
        attackBuffMultiplier = amount;
        yield return new WaitForSeconds(duration);
        attackBuffMultiplier = 1.0f; // 버프 해제
    }

    // 각 파츠 업그레이드 함수
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
        // 스탯 갱신 및 이벤트 호출
        CalculateStats();
        OnStatsUpdated?.Invoke();
    }
    
    // 각 파츠 업그레이드 비용 계산
    public int GetCannonUpgradeCost() => 10 * cannonLevel;
    public int GetHullUpgradeCost() => 10 * hullLevel;
    public int GetSailUpgradeCost() => 10 * sailLevel;
    
    private void Heal(float amount)
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
    
    public void ApplyLoadedData()
    {
        // 저장된 게임 데이터로 플레이어의 상태와 스탯 갱신
        CalculateStats();
        
        currentHp = Mathf.Min(currentHp, maxHp);
        OnHpChanged?.Invoke(currentHp, maxHp);
        OnXPChanged?.Invoke(exp, nextLevelExp);
        OnLevelUP?.Invoke(level);
        OnStatsUpdated?.Invoke();
    }
}

