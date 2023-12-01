using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [Header("General")]
    public string itemName;
    [TextArea(5, 20)]
    public string itemDescription;
    [Space]
    public GameObject prefab;

    [Space]

    public ItemType itemType;
    public ItemRank itemRank;

    public enum ItemType
    {
        none,
        melee,
        firearm,
        ammonition,
        craftable,
        consumable,
        junk,
    }

    public enum ItemRank
    {
        common,
        uncommon,
        rare,
        epic,
        legendary,
    }
}
