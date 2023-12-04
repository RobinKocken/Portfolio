using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("General")]
    public ItemData itemData;
    public bool equipped;

    public virtual void InitializeItem()
    {

    }
}