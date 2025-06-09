using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("스테이지")]
    public StageData currentStage;
    public Transform enemySpawnPoint;
    private int currentEnemyIndex = 0;
    
    [Header("플레이어 데이터")]
    public ShipStatsData playerStatsData;

    public int gold = 0;
    public float currentAttackPower;
    public float maxHp;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (playerStatsData != null)
        {
            maxHp = playerStatsData.maxHp;
            currentAttackPower = playerStatsData.attackPower;
        }
    }

    private void Start()
    {
        StartStage();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Gold : " + gold);
    }

    public void StartStage()
    {
        currentEnemyIndex = 0;
        SpawnNextEnemy();
    }

    public void SpawnNextEnemy()
    {
        if (currentEnemyIndex >= currentStage.enemyPrefabs.Length)
        {
            // TODO: 다음 스테이지
            return;
        }

        GameObject enemySpawn = currentStage.enemyPrefabs[currentEnemyIndex];
        Instantiate(enemySpawn, enemySpawnPoint.position, enemySpawnPoint.rotation);

        currentEnemyIndex++;
    }

    public bool TryUpgradeAttack(int cost)
    {
        if (gold >= cost)
        {
            gold -= cost;
            currentAttackPower += 5; //예시, 추후 변수로 관리예정
            Debug.Log("공격력 업그레이드 성공");
            return true;
        }
        else
        {
            Debug.Log("골드 부족");
            return false;       
        }
    }
}
