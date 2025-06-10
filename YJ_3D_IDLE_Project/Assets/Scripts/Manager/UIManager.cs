using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("플레이어 상태")]
    public Slider hpSlider;
    public Slider expSlider;
    
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

        if (Player.Instance != null)
        {
            Player.Instance.OnHpChanged += UpdateHpUI;
            Player.Instance.OnXPChanged += UpdateXpUI;
        }
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

    void UpdateHpUI(float currentHp, float maxHp)
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHp / maxHp;
        }
    }

    void UpdateXpUI(float currentXp, float nextLevelXp)
    {
        if (expSlider != null)
        {
            expSlider.value = currentXp / nextLevelXp;
        }
    }
    
}
