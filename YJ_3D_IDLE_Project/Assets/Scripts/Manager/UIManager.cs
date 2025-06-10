using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("플레이어 상태")]
    // public Slider hpSlider;
    public Slider expSlider;
    public TextMeshProUGUI levelText;
    
    [Header("UI")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI stageText;
    public Button useItemButton;
    
    [Header("업그레이드/아이템 정보")]
    public ItemData useItem;
    
    [Header("파츠 업그레이드 UI")]
    public UpgradeUI upgradeUI;
    public Button upgradeButton;
    public Button upgradeCannonButton;
    public Button upgradeHullButton;
    public Button upgradeSailButton;
    public TextMeshProUGUI cannonInfoText;
    public TextMeshProUGUI hullInfoText;
    public TextMeshProUGUI sailInfoText;
    
    [Header("인벤토리")]
    public InventoryUI inventoryUI;
    public Button inventoryButton;
    
    [Header("상태창 UI")]
    public StatusUI statusUI;
    public Button statusButton;
    
    [Header("페이드 효과")]
    public Image fadeImage;

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
        upgradeCannonButton.onClick.AddListener(() => UpgradeManager.Instance.TryUpgradeCannon());
        upgradeHullButton.onClick.AddListener(() => UpgradeManager.Instance.TryUpgradeHull());
        upgradeSailButton.onClick.AddListener(() => UpgradeManager.Instance.TryUpgradeSail());
        inventoryButton.onClick.AddListener(OnClickInventoryOpen);
        upgradeButton.onClick.AddListener(OnClickUpgradeOpen);
        statusButton.onClick.AddListener(statusUI.OpenPanel);
        
        if (Player.Instance != null)
        {
            //Player.Instance.OnHpChanged += UpdateHpUI;
            Player.Instance.OnXPChanged += UpdateXpUI;
            Player.Instance.OnLevelUP += UpdateLevelUI;
            Player.Instance.OnStatsUpdated += UpdatePartUpgradeUI;
            
            UpdateLevelUI(Player.Instance.level);
            UpdatePartUpgradeUI();
        }
    }
    
    void Update()
    {
        if (GameManager.Instance != null)
        {
            goldText.text = GameManager.Instance.gold.ToString();
        }
    }
    
    public void OnClickUseItem()
    {
        GameManager.Instance.ApplyBuff(useItem);
    }
    
    public void OnClickInventoryOpen()
    {
        inventoryUI.OpenInventory();
    }

    public void OnClickUpgradeOpen()
    {
        upgradeUI.OpenUpgrade();
    }

    public void UpdateStageText(string text)
    {
        if (stageText != null)
        {
            stageText.text = text;
        }
    }

    /*void UpdateHpUI(float currentHp, float maxHp)
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHp / maxHp;
        }
    }*/

    void UpdateXpUI(float currentXp, float nextLevelXp)
    {
        if (expSlider != null)
        {
            expSlider.value = currentXp / nextLevelXp;
        }
    }

    void UpdateLevelUI(int level)
    {
        if (levelText != null)
        {
            levelText.text = "Lv. " + level.ToString();
        }
    }

    void UpdatePartUpgradeUI()
    {
        if (Player.Instance != null)
        {
            cannonInfoText.text = $"공격력을 증가시킵니다.\n대포 Lv.{Player.Instance.cannonLevel}\nCost: {Player.Instance.GetCannonUpgradeCost()}";
            hullInfoText.text = $"체력을 증가시킵니다.\n선체 Lv.{Player.Instance.hullLevel}\nCost: {Player.Instance.GetHullUpgradeCost()}";
            sailInfoText.text = $"속도를 증가시킵니다.\n돛Lv.{Player.Instance.sailLevel}\nCost: {Player.Instance.GetSailUpgradeCost()}";
        }
    }

    public IEnumerator FadeOut(float duration = 1.0f)
    {
        fadeImage.gameObject.SetActive(true);
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / duration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }
    
    public IEnumerator FadeIn(float duration = 1.0f)
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / duration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);
    }
}
