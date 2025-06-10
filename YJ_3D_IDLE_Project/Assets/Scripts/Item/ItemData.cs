using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    AttackPower,
    Health,
    Speed
}
[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("아이템 정보")]
    public string itemName;
    public Sprite icon;
    
    [Header("버프 효과")]
    public BuffType buffType;
    public float buffAmount;
    public float buffDuration;
}
