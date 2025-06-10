using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackPowerText;
    public TextMeshProUGUI speedText;
    public GameObject statusPanel;
    public Button closeButton;

    private Player player;
    
    void Start()
    {
        player = Player.Instance;
        if (player != null)
        {
            player.OnStatsUpdated += UpdateUI;
            player.OnHpChanged += UpdateHealthUI;
            player.OnLevelUP += (level) => UpdateUI();

            UpdateUI();
        }
        
        closeButton.onClick.AddListener(ClosePanel);
        
        if (statusPanel != null)
        {
            ClosePanel();
        }
    }

    private void UpdateHealthUI(float currentHp, float maxHp)
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (player == null || !statusPanel.activeInHierarchy) return;
        
        levelText.text = $"레벨: {player.level}";
        healthText.text = $"체력: {player.currentHp:F0} / {player.maxHp:F0}";
        attackPowerText.text = $"공격력: {player.baseAttackPower:F0}";
        speedText.text = $"이동 속도: {player.moveSpeed:F1}";
    }

    public void OpenPanel()
    {
        if (statusPanel != null)
        {
            statusPanel.SetActive(true);
            UpdateUI();
        }
    }
    
    public void ClosePanel()
    {
        if (statusPanel != null)
        {
            statusPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnStatsUpdated -= UpdateUI;
            player.OnHpChanged -= UpdateHealthUI;
            player.OnLevelUP -= (level) => UpdateUI();
        }
    }
}
