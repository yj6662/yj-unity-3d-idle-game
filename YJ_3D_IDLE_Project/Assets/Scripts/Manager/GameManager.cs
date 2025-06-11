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

    private const string KEY_PLAYER_LEVEL = "PlayerLevel";
    private const string KEY_PLAYER_XP = "PlayerXP";
    private const string KEY_CANNON_LEVEL = "CannonLevel";
    private const string KEY_HULL_LEVEL = "HullLevel";
    private const string KEY_SAIL_LEVEL = "SailLevel";
    private const string KEY_GOLD = "Gold";
    private const string KEY_STAGE_INDEX = "StageIndex";
    
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
        LoadGame();
        StartStage(currentStageIndex);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
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

        if (MapGenerator.Instance != null)
        {
            MapGenerator.Instance.GenerateMap();
        }
        
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

    public void SaveGame()
    {
        PlayerPrefs.SetInt(KEY_PLAYER_LEVEL, Player.Instance.level);
        PlayerPrefs.SetFloat(KEY_PLAYER_XP, Player.Instance.exp);
        PlayerPrefs.SetInt(KEY_CANNON_LEVEL, Player.Instance.cannonLevel);
        PlayerPrefs.SetInt(KEY_HULL_LEVEL, Player.Instance.hullLevel);
        PlayerPrefs.SetInt(KEY_SAIL_LEVEL, Player.Instance.sailLevel);
        PlayerPrefs.SetInt(KEY_GOLD, this.gold);
        PlayerPrefs.SetInt(KEY_STAGE_INDEX, this.currentStageIndex);
        PlayerPrefs.Save();
        
        Debug.Log("게임 데이터 저장 완료 (PlayerPrefs).");
    }

    public void LoadGame()
    {
        Player.Instance.level = PlayerPrefs.GetInt(KEY_PLAYER_LEVEL, 1);
        Player.Instance.exp = PlayerPrefs.GetFloat(KEY_PLAYER_XP, 0f);
        Player.Instance.cannonLevel = PlayerPrefs.GetInt(KEY_CANNON_LEVEL, 1);
        Player.Instance.hullLevel = PlayerPrefs.GetInt(KEY_HULL_LEVEL, 1);
        Player.Instance.sailLevel = PlayerPrefs.GetInt(KEY_SAIL_LEVEL, 1);
        this.gold = PlayerPrefs.GetInt(KEY_GOLD, 0);
        this.currentStageIndex = PlayerPrefs.GetInt(KEY_STAGE_INDEX, 0);
        Player.Instance.ApplyLoadedData();
    }
}
