using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Data")]
    public ItemData itemData;
    public bool equipped;

    public virtual void InitializeItem()
    {

    }
}
