using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("스테이지")]
    public StageList stageList;
    private int currentEnemyIndex = 0;
    private int currentStageIndex = 0;
    
    [Header("플레이어 데이터")]
    public ShipStatsData playerStatsData;

    public int gold = 0;
    public float baseAttackPower;
    public float maxHp;

    [Header("버프 관리")]
    private float attackBuffMultiplier = 1.0f;
    
    [Header("인벤토리")]
    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();

    public float CurrentFinalAttackPower
    {
        get { return baseAttackPower * attackBuffMultiplier; }
    }
    
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
            baseAttackPower = playerStatsData.attackPower;
        }
    }

    private void Start()
    {
        StartStage(currentStageIndex);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Gold : " + gold);
    }

    public void AddItem(ItemData itemData, int quantity = 1)
    {
        if (inventory.ContainsKey(itemData))
        {
            inventory[itemData] += quantity;
        }
        else
        {
            inventory.Add(itemData, quantity);
        }
    }

    public void StartStage(int stageIndex)
    {
        currentStageIndex = stageIndex;
        currentEnemyIndex = 0;
        
        UIManager.Instance.UpdateStageText(stageList.stages[stageIndex].stageName);
        SpawnNextEnemy();
    }

    public void SpawnNextEnemy()
    {
        StageData currentStage = stageList.stages[currentStageIndex];
        if (currentEnemyIndex >= currentStage.enemyPrefabs.Length)
        {
            StartCoroutine(StageClearCoroutine());
            return;
        }

        GameObject enemySpawn = currentStage.enemyPrefabs[currentEnemyIndex];

        if (currentEnemyIndex >= currentStage.spawnPoints.Length)
        {
            return;
        }

        Vector3 spawnPoint = currentStage.spawnPoints[currentEnemyIndex];
        Instantiate(enemySpawn, spawnPoint, Quaternion.Euler(0, 180f, 0));

        currentEnemyIndex++;
    }

    IEnumerator StageClearCoroutine()
    {
        // TODO: 스테이지 클리어 UI
        yield return new WaitForSeconds(2.0f);
        
        int nextStageIndex = currentStageIndex + 1;

        if (nextStageIndex >= stageList.stages.Count)
        {
            nextStageIndex = 0;
        }
        
        StartStage(nextStageIndex);
    }

    public bool TryUpgradeAttack(int cost)
    {
        if (gold >= cost)
        {
            gold -= cost;
            baseAttackPower += 5; //예시, 추후 변수로 관리예정
            Debug.Log("공격력 업그레이드 성공");
            return true;
        }
        else
        {
            Debug.Log("골드 부족");
            return false;       
        }
    }

    public void ApplyBuff(ItemData itemData)
    {
        if (itemData.buffType == BuffType.AttackPower)
        {
            StartCoroutine(AttackBuffCoroutine(itemData.buffAmount, itemData.buffDuration));
        }
    }

    IEnumerator AttackBuffCoroutine(float amount, float duration)
    {
        attackBuffMultiplier = amount;

        yield return new WaitForSeconds(duration);

        attackBuffMultiplier = 1.0f;
    }
}
