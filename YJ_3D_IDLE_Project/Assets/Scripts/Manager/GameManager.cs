using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public ShipStatsData playerStatsData;

    public int gold = 0;
    public float currentAttackPower;
    public float maxHp;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Gold : " + gold);
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
