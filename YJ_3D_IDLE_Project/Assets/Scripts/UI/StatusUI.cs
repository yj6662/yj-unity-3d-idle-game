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
    
    
    void Start()
    {
        // 플레이어 정보 관련 이벤트를 모두 구독하여 UI 업데이트
        if (Player.Instance != null)
        {
            Player.Instance.OnStatsUpdated += UpdateUI;
            Player.Instance.OnHpChanged += (currentHp, maxHp) => UpdateUI();
            Player.Instance.OnLevelUP += (level) => UpdateUI();
        }
        
        closeButton.onClick.AddListener(CloseStatus);
        CloseStatus();
    }
    
    private void UpdateUI()
    {
        if (Player.Instance == null) return;
        
        levelText.text = $"레벨: {Player.Instance.level}";
        healthText.text = $"체력: {Player.Instance.currentHp:F0} / {Player.Instance.maxHp:F0}";
        attackPowerText.text = $"공격력: {Player.Instance.baseAttackPower:F0}";
        speedText.text = $"이동 속도: {Player.Instance.moveSpeed:F1}";
    }

    public void OpenStatus()
    {
        if (statusPanel != null)
        {
            statusPanel.SetActive(true);
            UpdateUI();
        }
    }
    
    private void CloseStatus()
    { 
        statusPanel.SetActive(false);
    }
}
