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
    
    [Header("이펙트")]
    public GameObject cannonFirePrefab;
    public Transform firePoint;

    private float maxHp;
    private float currentHp;
    private Transform playerTransform;
    private Player player;
    private Animator animator;
    public event Action<float, float> OnHpChanged;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        maxHp = enemyStatsData.initialMaxHp;
        currentHp = maxHp;
        
        // 플레이어 오브젝트를 찾아서 저장
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        playerTransform = playerObject.transform;
        player = playerObject.GetComponent<Player>();

        // 체력바 생성 및 이벤트 구독
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
        // 공격 로직 시작
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        while (true) // 적이 살아있는 동안
        {
            if (playerTransform != null &&
                Vector3.Distance(transform.position, playerTransform.position) <= detectionRange) // 플레이어가 감지 범위 안에 있다면
            {
                AttackPlayer(); // 공격
            }
            
            yield return new WaitForSeconds(attackCooldown); // 쿨타임 동안 대기
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        OnHpChanged?.Invoke(currentHp, maxHp); // 체력 변경을 UI에 알림
        
        animator.SetTrigger("isDamaged");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("isDie");
        GameManager.Instance.AddGold(enemyStatsData.rewardGold); // 플레이어 골드 증가
        Player.Instance.AddExp(enemyStatsData.rewardExp); // 플레이어 경험치 증가
        
        GameManager.Instance.SpawnNextEnemy(); // 다음 적을 스폰하도록 게임매니저에 요청
        
        Destroy(gameObject, 1.0f); // 애니메이션 실행 후 오브젝트 파괴
    }

    void AttackPlayer()
    {
        // 플레이어를 공격
        if (player != null)
        {
            animator.SetTrigger("isAttack");
            if (cannonFirePrefab != null && firePoint != null)
            {
                Instantiate(cannonFirePrefab, firePoint.position, firePoint.rotation); // 대포 발사 이펙트
            }
            player.TakeDamage(enemyStatsData.initialAttackPower);
        }
    }

    private void OnDestroy()
    {
        // 오브젝트가 파괴될 때 이벤트 구독 해제
        if (floatingHealthBar != null)
        {
            OnHpChanged -= floatingHealthBar.UpdateHealthBar;
        }
    }
}
