using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject upgradePanel;
    public Button closeButton;
    
    [Header("버튼/텍스트")]
    public Button upgradeCannonButton;
    public Button upgradeHullButton;
    public Button upgradeSailButton;
    public TextMeshProUGUI cannonInfoText;
    public TextMeshProUGUI hullInfoText;
    public TextMeshProUGUI sailInfoText;
    void Start()
    {
        closeButton.onClick.AddListener(CloseUpgrade);
        upgradeCannonButton.onClick.AddListener(() => UpgradeManager.Instance.TryUpgradeCannon());
        upgradeHullButton.onClick.AddListener(() => UpgradeManager.Instance.TryUpgradeHull());
        upgradeSailButton.onClick.AddListener(() => UpgradeManager.Instance.TryUpgradeSail());
        Player.Instance.OnStatsUpdated += UpdatePanel;
        UpdatePanel(); 
        CloseUpgrade();
    }

    public void OpenUpgrade()
    {
        upgradePanel.SetActive(true);
    }
    private void CloseUpgrade()
    {
        upgradePanel.SetActive(false);
    }
    private void UpdatePanel()
    {
        if (Player.Instance == null) return;
        
        cannonInfoText.text = $"대포 Lv.{Player.Instance.cannonLevel}\nCost: {Player.Instance.GetCannonUpgradeCost()}";
        hullInfoText.text = $"선체 Lv.{Player.Instance.hullLevel}\nCost: {Player.Instance.GetHullUpgradeCost()}";
        sailInfoText.text = $"돛 Lv.{Player.Instance.sailLevel}\nCost: {Player.Instance.GetSailUpgradeCost()}";
    }
    private void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnStatsUpdated -= UpdatePanel;
        }
    }
}
