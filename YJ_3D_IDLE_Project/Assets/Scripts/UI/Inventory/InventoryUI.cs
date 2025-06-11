using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("UI")] 
    public GameObject inventoryPanel;
    public Button closeButton;

    [Header("슬롯")] 
    public Transform slotGrid;
    public GameObject slotPrefab;

    void Start()
    {
        closeButton.onClick.AddListener(CloseInventory);
        CloseInventory();
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        UpdateInventory();
    }
    private void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    void UpdateInventory()
    {
        foreach (Transform slot in slotGrid)
        {
            Destroy(slot.gameObject);
        }

        if (GameManager.Instance != null)
        {
            foreach (var item in GameManager.Instance.inventory)
            {
                GameObject slot = Instantiate(slotPrefab, slotGrid);
                
                InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
                inventorySlot.SetSlot(item.Key, item.Value);
            }
        }
    }
}
