using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public enum State
    {
        Moving,
        Attacking
    }
    
    public State state = State.Moving;
    
    [Header("데이터")]
    public ShipStatsData shipStatsData;
    [Header("상태")]
    public float detectionRange = 10f;
    public float attackCooldown = 2f;
    public Transform target;

    private Coroutine attackCoroutine;

    private void Update()
    {
        switch (state)
        {
            case State.Moving:
                Move();
                break;
            case State.Attacking:
                Attack();
                break;
        }
    }

    void Move()
    {
        if (target == null)
        {
            FindNewTarget();
        }
        transform.Translate(Vector3.forward * shipStatsData.moveSpeed * Time.deltaTime);
        
        if (target != null && Vector3.Distance(transform.position, target.position) < detectionRange)
        {
            ChangeState(State.Attacking);
        }
    }

    void FindNewTarget()
    {
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyObject != null)
        {
            target = enemyObject.transform;
        }
    }

    void Attack()
    {
        
    }

    void ChangeState(State newState)
    {
        state = newState;

        switch (state)
        {
            case State.Moving:
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }
                break;
            case State.Attacking:
                attackCoroutine = StartCoroutine(AttackCoroutine());
                break;
        }
    }

    IEnumerator AttackCoroutine()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        
        while (state == State.Attacking)
        {
            if (enemy != null)
            {
                float attackDamage = GameManager.Instance.currentAttackPower;
                Debug.Log(attackDamage + "공격");
                enemy.TakeDamage(attackDamage);
            }
            else
            {
                TargetDefeated();
                break;
            }
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    public void TargetDefeated()
    {
        target = null;
        ChangeState(State.Moving);
    }
}
