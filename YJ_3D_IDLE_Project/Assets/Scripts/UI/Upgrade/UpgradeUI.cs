using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject upgradePanel;
    public Button closeButton;
    void Start()
    {
        closeButton.onClick.AddListener(CloseUpgrade);
        CloseUpgrade();
    }

    public void OpenUpgrade()
    {
        upgradePanel.SetActive(true);
    }
    public void CloseUpgrade()
    {
        upgradePanel.SetActive(false);
    }
}
