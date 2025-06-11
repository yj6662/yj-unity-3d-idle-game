using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 스테이지 진행, 적 스폰, 맵 생성, 재화 관리, 데이터 저장/불러오기 등을 담당하는 싱글톤
    public static GameManager Instance;
    
    [Header("스테이지")]
    public StageList stageList;
    private int currentEnemyIndex = 0;
    private int currentStageIndex = 0;

    public int gold = 0;
    
    [Header("인벤토리")]
    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();

    // PlayerPrefs에 데이터를 저장하고 불러올 때 사용할 키 값
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
        LoadGame(); // 데이터 불러오기
        StartStage(currentStageIndex); // 저장된 스테이지 데이터부터 시작
    }
    private void OnApplicationQuit()
    {
        // 게임 종료 시
        SaveGame(); // 현재 데이터 저장
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

    public void AddItem(ItemData itemData, int quantity)
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

    private void StartStage(int stageIndex)
    {
        // 스테이지 시작
        currentStageIndex = stageIndex;
        currentEnemyIndex = 0;

        if (MapGenerator.Instance != null)
        {
            // 새로운 맵 생성
            MapGenerator.Instance.GenerateMap();
        }
        
        UIManager.Instance.UpdateStageText(stageList.stages[stageIndex].stageName);
        SpawnNextEnemy(); // 적 스폰
    }

    public void SpawnNextEnemy()
    {
        StageData currentStage = stageList.stages[currentStageIndex];
        
        // 모든 적을 처치하면 스테이지 클리어 처리
        if (currentEnemyIndex >= currentStage.enemyPrefabs.Length)
        {
            StartCoroutine(StageClearCoroutine());
            return;
        }

        GameObject enemySpawn = currentStage.enemyPrefabs[currentEnemyIndex];

        Vector3 spawnPoint = currentStage.spawnPoints[currentEnemyIndex];
        Instantiate(enemySpawn, spawnPoint, Quaternion.Euler(0, 180f, 0));

        currentEnemyIndex++;
    }

    IEnumerator StageClearCoroutine()
    {
        // 스테이지 클리어 처리 코루틴
        // 페이드 아웃 - 플레이어 위치 초기화 - 다음 스테이지 시작 - 페이드 인
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

    private void SaveGame()
    {
        // 현재 게임 상태 PlayerPrefs에 저장
        PlayerPrefs.SetInt(KEY_PLAYER_LEVEL, Player.Instance.level);
        PlayerPrefs.SetFloat(KEY_PLAYER_XP, Player.Instance.exp);
        PlayerPrefs.SetInt(KEY_CANNON_LEVEL, Player.Instance.cannonLevel);
        PlayerPrefs.SetInt(KEY_HULL_LEVEL, Player.Instance.hullLevel);
        PlayerPrefs.SetInt(KEY_SAIL_LEVEL, Player.Instance.sailLevel);
        PlayerPrefs.SetInt(KEY_GOLD, gold);
        PlayerPrefs.SetInt(KEY_STAGE_INDEX, currentStageIndex);
        PlayerPrefs.Save();
        
        Debug.Log("게임 데이터 저장 완료 (PlayerPrefs).");
    }

    private void LoadGame()
    {
        // 저장된 게임 상태 불러오기
        Player.Instance.level = PlayerPrefs.GetInt(KEY_PLAYER_LEVEL, 1);
        Player.Instance.exp = PlayerPrefs.GetFloat(KEY_PLAYER_XP, 0f);
        Player.Instance.cannonLevel = PlayerPrefs.GetInt(KEY_CANNON_LEVEL, 1);
        Player.Instance.hullLevel = PlayerPrefs.GetInt(KEY_HULL_LEVEL, 1);
        Player.Instance.sailLevel = PlayerPrefs.GetInt(KEY_SAIL_LEVEL, 1);
        gold = PlayerPrefs.GetInt(KEY_GOLD, 0);
        currentStageIndex = PlayerPrefs.GetInt(KEY_STAGE_INDEX, 0);
        
        Player.Instance.ApplyLoadedData();// 불러온 데이터 적용
    }
}
