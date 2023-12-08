using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("Item Data")]
    public ItemData itemData;
    public int itemAmount;

    [Space]
    [Header("Slot Data")]
    public int slotID;
    public int holderID;
    [Space]
    public Image iconRenderer;
    public TMP_Text amountText;

    public void InitializeItem()
    {
        if(itemData != null)
        {
            iconRenderer.sprite = itemData.icon;
            amountText.text = itemAmount.ToString();

            iconRenderer.sprite = itemData.icon;
            amountText.text = this.itemAmount.ToString();
        }
    }

    public void SetItem(ItemData itemData, int itemAmount)
    {
        this.itemData = itemData;
        this.itemAmount = itemAmount;

        iconRenderer.sprite = itemData.icon;
        amountText.text = this.itemAmount.ToString();
    }

    public void AddAmount(ItemData itemData ,int itemAmount)
    {
        if(this.itemAmount == 0 && itemAmount > 0)
        {
            SetItem(itemData, itemAmount);
        }
        else if(itemAmount > 0)
        {
            this.itemAmount += itemAmount;
            amountText.text = this.itemAmount.ToString();
        }
    }

    public void RemoveAmount(int itemAmount)
    {
        if(this.itemAmount - itemAmount == 0)
        {
            ClearItem();
        }
        else if(this.itemAmount - itemAmount > 0)
        {
            this.itemAmount -= itemAmount;
            amountText.text = this.itemAmount.ToString();
        }
    }

    public void ClearItem()
    {
        itemData = null;
        itemAmount = 0;

        iconRenderer.sprite = null;
        amountText.text = itemAmount.ToString();
    }
}
