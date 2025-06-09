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
        transform.Translate(Vector3.forward * shipStatsData.moveSpeed * Time.deltaTime);
        
        if (target != null && Vector3.Distance(transform.position, target.position) < detectionRange)
        {
            ChangeState(State.Attacking);
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
        while (state == State.Attacking)
        {
            //TODO: 실제 공격 로직 넣기
            Debug.Log(GameManager.Instance.currentAttackPower + "공격");

            yield return new WaitForSeconds(attackCooldown);
        }
    }
}
