using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI itemQuantity;

    public void SetSlot(ItemData itemData, int quantity)
    {
        icon.sprite = itemData.icon;
        icon.gameObject.SetActive(true);
        itemQuantity.text = quantity.ToString();
        itemQuantity.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        icon.gameObject.SetActive(false);
        itemQuantity.gameObject.SetActive(false);
    }
}
