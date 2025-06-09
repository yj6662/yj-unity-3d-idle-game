using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI goldText;
    public Button upgradeButton;
    
    [Header("업그레이드 정보")]
    public int upgradeCost;
    
    void Start()
    {
        upgradeButton.onClick.AddListener(OnClickUpgrade);
    }
    
    void Update()
    {
        if (GameManager.instance != null)
        {
            goldText.text = GameManager.instance.gold.ToString();
        }
    }

    public void OnClickUpgrade()
    {
        GameManager.instance.TryUpgradeAttack(upgradeCost);
    }
}
