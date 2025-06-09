using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipStatsData", menuName = "ScriptableObjects/ShipStatsData")]
public class ShipStatsData : ScriptableObject
{
    [Header("능력치")]
    public float maxHp = 100f;      // 최대 체력
    public float attackPower = 10f; // 공격력
    public float moveSpeed = 5f;    // 이동 속도
}
