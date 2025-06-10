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
        StartStage();
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
