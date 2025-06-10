using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipStatsData", menuName = "ScriptableObjects/ShipStatsData")]
public class ShipStatsData : ScriptableObject
{
    [Header("능력치")]
    public float initialMaxHp = 100f;      // 최대 체력
    public float initialAttackPower = 10f; // 공격력
    public float initialMoveSpeed = 5f;    // 이동 속도
    
    [Header("강화 성장치")]
    public float hpPerUpgrade = 10f;
    public float attackPerUpgrade = 5f;
    public float speedPerUpgrade = 0.2f;
    
    [Header("보상 정보")]
    public int rewardGold;
    public int rewardExp;
}
