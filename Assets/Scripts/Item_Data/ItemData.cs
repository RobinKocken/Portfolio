using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item Data", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("General")]
    public string itemName;
    [TextArea(5, 20)]
    public string itemDescription;
    [Header("For unlimeted Amount use -1")]
    public int maxAmount;

    [Space]
    public GameObject prefab;
    public Sprite icon;

    [Space]

    public ItemType itemType;

    public enum ItemType
    {
        none,
        melee,
        firearm,
        ammunition,
        craftable,
        consumable,
        junk,
    }
}
