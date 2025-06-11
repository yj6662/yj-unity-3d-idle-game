using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    private Player player;
    public enum State
    {
        Moving,
        Attacking
    }
    
    public State state = State.Moving; // 초기 플레이어 상태
    
    [Header("상태")]
    public float detectionRange = 40f; // 적 감지 범위
    public float attackCooldown = 2f; // 공격 쿨타임
    public Transform target; // 현재 공격 대상

    [Header("이펙트")] public GameObject cannonFirePrefab;
    public Transform firePoint;

    private Coroutine attackCoroutine;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Moving:
                Move();
                break;
            case State.Attacking:
                break;
        }
    }

    void Move()
    {
        // 현재 공격중인 타겟이 없으면 새로운 타겟을 찾음
        if (target == null)
        {
            FindNewTarget();
        }
        transform.Translate(Vector3.forward * player.moveSpeed * Time.deltaTime);
        
        // 타겟이 있고, 감지 범위 안에 들어오면 공격 상태로 전환
        if (target != null && Vector3.Distance(transform.position, target.position) < detectionRange)
        {
            ChangeState(State.Attacking);
        }
    }

    void FindNewTarget()
    {
        // 새로운 공격 대상을 찾기 위함
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy"); // Enemy 태그를 가진 오브젝트를 찾음
        if (enemyObject != null)
        {
            target = enemyObject.transform;
        }
    }
    

    void ChangeState(State newState)
    {
        // 플레이어의 상태 변경
        state = newState;

        switch (state)
        {
            // 상태에 따라 다른 로직 실행
            case State.Moving:
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }
                break;
            case State.Attacking:
                // 공격 상태에 진입하면 공격 코루틴 시작
                attackCoroutine = StartCoroutine(AttackCoroutine());
                break;
        }
    }

    IEnumerator AttackCoroutine()
    {
        // 공격 상태일 때의 코루틴
        Enemy enemy = target.GetComponent<Enemy>();
        
        while (state == State.Attacking) // 공격 상태인 동안 반복
        {
            if (enemy != null) // 적이 살아있는 경우
            {
                player.OnAttack(); // 애니메이션

                if (cannonFirePrefab != null && firePoint != null)
                {
                    // 대포 발사 이펙트
                    Instantiate(cannonFirePrefab, firePoint.position, firePoint.rotation);
                }
                
                float attackDamage = player.CurrentFinalAttackPower; // 최종 공격력으로 적에게 데미지
                enemy.TakeDamage(attackDamage);
            }
            else
            {
                //적이 죽었을 때
                TargetDefeated();
                break;
            }
            yield return new WaitForSeconds(attackCooldown); // 쿨타임 대기
        }
    }

    private void TargetDefeated()
    {
        target = null;
        ChangeState(State.Moving); // 다시 이동 상태로 전환
    }
    
    public void ResetPosition()
    {
        // 플레이어 위치 초기화
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
