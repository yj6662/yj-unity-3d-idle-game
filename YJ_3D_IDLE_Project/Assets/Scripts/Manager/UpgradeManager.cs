using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TryUpgradeCannon()
    {
        int cost = Player.Instance.GetCannonUpgradeCost();

        if (GameManager.Instance.gold >= cost)
        {
            GameManager.Instance.SpendGold(cost);
            Player.Instance.UpgradeCannon();
        }
        else
        {
            //TODO: 골드 부족 알림 UI
        }
    }
    
    public void TryUpgradeHull()
    {
        int cost = Player.Instance.GetHullUpgradeCost();

        if (GameManager.Instance.gold >= cost)
        {
            GameManager.Instance.SpendGold(cost);
            Player.Instance.UpgradeHull();
        }
        else
        {
            //TODO: 골드 부족 알림 UI
        }
    }
    
    public void TryUpgradeSail()
    {
        int cost = Player.Instance.GetSailUpgradeCost();

        if (GameManager.Instance.gold >= cost)
        {
            GameManager.Instance.SpendGold(cost);
            Player.Instance.UpgradeSail();
        }
        else
        {
            //TODO: 골드 부족 알림 UI
        }
    }
}
