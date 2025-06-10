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

    public int gold = 0;
    
    [Header("인벤토리")]
    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    
    
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

    public void SpendGold(int amount)
    {
        gold -= amount;
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
        yield return StartCoroutine(UIManager.Instance.FadeOut());
        
        Player.Instance.GetComponent<PlayerAI>().ResetPosition();
        
        int nextStageIndex = currentStageIndex + 1;

        if (nextStageIndex >= stageList.stages.Count)
        {
            nextStageIndex = 0;
        }
        
        StartStage(nextStageIndex);
        
        yield return StartCoroutine(UIManager.Instance.FadeIn());
    }

    public void ApplyBuff(ItemData itemData)
    {
        if (itemData.buffType == BuffType.AttackPower)
        {
            StartCoroutine(Player.Instance.ApplyAttackBuff(itemData.buffAmount, itemData.buffDuration));
        }
    }
}
