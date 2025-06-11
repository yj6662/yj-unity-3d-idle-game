using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("플레이어 상태")]
    public Slider expSlider;
    public TextMeshProUGUI levelText;
    
    [Header("UI")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI stageText;

    
    [Header("업그레이드")]
    public UpgradeUI upgradeUI;
    public Button upgradeButton;
    
    [Header("인벤토리")]
    public InventoryUI inventoryUI;
    public Button inventoryButton;
    
    [Header("상태창")]
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
        inventoryButton.onClick.AddListener(inventoryUI.OpenInventory);
        upgradeButton.onClick.AddListener(upgradeUI.OpenUpgrade);
        statusButton.onClick.AddListener(statusUI.OpenStatus);
        
        if (Player.Instance != null)
        {
            Player.Instance.OnXPChanged += UpdateXpUI;
            Player.Instance.OnLevelUP += UpdateLevelUI;
            
            UpdateLevelUI(Player.Instance.level);
        }
    }
    
    void Update()
    {
        if (GameManager.Instance != null)
        {
            goldText.text = GameManager.Instance.gold.ToString();
        }
    }
    

    public void UpdateStageText(string text)
    {
        if (stageText != null)
        {
            stageText.text = text;
        }
    }

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
            levelText.text = $"Lv. {level}";
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
        fadeImage.gameObject.SetActive(false);
    }
}
