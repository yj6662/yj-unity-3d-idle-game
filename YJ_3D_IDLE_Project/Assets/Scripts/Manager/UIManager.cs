using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("UI")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI stageText;
    public Button upgradeButton;
    public Button useItemButton;
    
    [Header("업그레이드/아이템 정보")]
    public int upgradeCost;
    public ItemData useItem;
    
    [Header("인벤토리")]
    public InventoryUI inventoryUI;
    public Button inventoryButton;

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
    void Start()
    {
        upgradeButton.onClick.AddListener(OnClickUpgrade);
        useItemButton.onClick.AddListener(OnClickUseItem);
        inventoryButton.onClick.AddListener(OnClickInventoryOpen);
    }
    
    void Update()
    {
        if (GameManager.Instance != null)
        {
            goldText.text = GameManager.Instance.gold.ToString();
        }
    }

    public void OnClickUpgrade()
    {
        GameManager.Instance.TryUpgradeAttack(upgradeCost);
    }
    
    public void OnClickUseItem()
    {
        GameManager.Instance.ApplyBuff(useItem);
    }
    
    public void OnClickInventoryOpen()
    {
        inventoryUI.OpenInventory();
    }

    public void UpdateStageText(string text)
    {
        if (stageText != null)
        {
            stageText.text = text;
        }
            
    }
}
