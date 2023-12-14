using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Data")]
    public ItemData itemData;
    public ItemRank itemRank;

    public enum ItemRank
    {
        common,
        uncommon,
        rare,
        epic,
        legendary,
    }

    public virtual void InitializeItem()
    {

    }
}
