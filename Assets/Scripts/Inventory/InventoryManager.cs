using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Data")]
    public InventoryHolder[] inventoryHolder;

    [System.Serializable]
    public struct InventoryHolder
    {
        public Transform holder;
        [Space]
        public ItemData.ItemType[] itemType;
        [Space]
        public List<InventorySlot> inventorySlot;
    }

    [Space]
    [Header("Mouse Data")]
    public ItemData itemDataHolder;
    public int itemAmountHolder;
    [Space]
    public Vector2 offset;
    [Space]
    public Transform cursor;
    public Image cursorIconRenderer;
    public TMP_Text cursorAmountText;

    void Start()
    {
        InitializeSlots();
    }

    void InitializeSlots()
    {
        for(int i = 0; i < inventoryHolder.Length; i++)
        {
            if(inventoryHolder[i].holder != null)
            {
                for(int j = 0; j < inventoryHolder[i].holder.childCount; j++)
                {
                    if(inventoryHolder[i].holder.GetChild(j).TryGetComponent<InventorySlot>(out InventorySlot inventorySlot))
                    {
                        inventorySlot.slotID = j;
                        inventorySlot.holderID = i;
                        inventorySlot.InitializeItem();
                        inventoryHolder[i].inventorySlot.Add(inventorySlot);
                    }
                }
            }
        }
    }

    public void AddItem(ItemData itemData, int itemAmount)
    {
        for(int i = 0; i < inventoryHolder.Length; i++)
        {
            for(int j = 0; j < inventoryHolder[i].itemType.Length; j++)
            {
                if(inventoryHolder[i].itemType[j] == itemData.itemType)
                {
                    for(int s = 0; s < inventoryHolder[i].inventorySlot.Count; s++)
                    {
                        if(inventoryHolder[i].inventorySlot[s].itemData != null)
                        {
                            if(inventoryHolder[i].inventorySlot[s].itemData == itemData)
                            {
                                if(itemData.maxAmount <= -1)
                                {
                                    inventoryHolder[i].inventorySlot[s].AddAmount(itemData, itemAmount);
                                    return;
                                }
                                else if(itemAmount <= itemData.maxAmount - inventoryHolder[i].inventorySlot[s].itemAmount)
                                {
                                    inventoryHolder[i].inventorySlot[s].AddAmount(itemData, itemAmount);
                                    return;
                                }
                                else if(itemAmount > itemData.maxAmount - inventoryHolder[i].inventorySlot[s].itemAmount)
                                {
                                    int maxAmount = itemData.maxAmount - inventoryHolder[i].inventorySlot[s].itemAmount;
                                    itemAmount -= maxAmount;

                                    inventoryHolder[i].inventorySlot[s].AddAmount(itemData, maxAmount);

                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }

        for(int i = 0; i < inventoryHolder.Length; i++)
        {
            for(int j = 0; j < inventoryHolder[i].itemType.Length; j++)
            {
                if(inventoryHolder[i].itemType[j] == itemData.itemType)
                {
                    for(int s = 0; s < inventoryHolder[i].inventorySlot.Count; s++)
                    {
                        if(inventoryHolder[i].inventorySlot[s].itemData == null)
                        {
                            if(itemAmount <= itemData.maxAmount - inventoryHolder[i].inventorySlot[s].itemAmount)
                            {
                                inventoryHolder[i].inventorySlot[s].SetItem(itemData, itemAmount);
                                return;
                            }
                            else if(itemAmount > itemData.maxAmount - inventoryHolder[i].inventorySlot[s].itemAmount)
                            {
                                int maxAmount = itemData.maxAmount - inventoryHolder[i].inventorySlot[s].itemAmount;
                                itemAmount -= maxAmount;

                                inventoryHolder[i].inventorySlot[s].SetItem(itemData, itemAmount);

                                AddItem(itemData, itemAmount);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    public void RemoveItem(ItemData itemData, int itemAmount, int slotID, int holderID)
    {
        if(slotID > -1 && holderID > -1)
        {
            if(inventoryHolder[holderID].inventorySlot[slotID].itemData != null)
            {
                if(itemAmount <= inventoryHolder[holderID].inventorySlot[slotID].itemAmount)
                {
                    inventoryHolder[holderID].inventorySlot[slotID].RemoveAmount(itemAmount);

                    return;
                }
                else if(itemAmount > inventoryHolder[holderID].inventorySlot[slotID].itemAmount)
                {
                    int maxAmount = inventoryHolder[holderID].inventorySlot[slotID].itemAmount;
                    inventoryHolder[holderID].inventorySlot[slotID].RemoveAmount(maxAmount);

                    return;
                }
            }
        }
        
        for(int i = 0; i < inventoryHolder.Length; i++)
        {

        }
    }
}
