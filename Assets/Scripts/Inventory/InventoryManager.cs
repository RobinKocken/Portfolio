using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    PlayerInput input;

    [Header("Inventory Data")]
    public GameObject inventoryHolder;
    public Holder[] holder;

    [System.Serializable]
    public struct Holder
    {
        public Transform slotHolder;
        [Space]
        public ItemData.ItemType[] itemType;
        [Space]
        public List<InventorySlot> slot;
    }

    [Space]
    [Header("Mouse Data")]
    public CursorHolder cursorHolder;

    [System.Serializable]
    public struct CursorHolder
    {
        public ItemData itemData;
        public int itemAmount;
        [Space]
        public Vector3 offset;
        [Space]
        public Transform cursor;
        public Image cursorIconRenderer;
        public TMP_Text cursorAmountText;
    }

    void Start()
    {
        InitializeSlots();
        inventoryHolder.SetActive(false);

        input = new PlayerInput();
        input.Player.Enable();
        //input.Player.Inventory.performed += OnInventoryPerformed;
        input.Player.Inventory.canceled += OnInventoryCancelled;
    }

    void Update()
    {
        CursorTracking();
    }

    void OnInventoryPerformed(InputAction.CallbackContext context)
    {
        CursorManager.Unlock();
        inventoryHolder.SetActive(true);
    }

    void OnInventoryCancelled(InputAction.CallbackContext context)
    {
        bool active = inventoryHolder.activeSelf;
        inventoryHolder.SetActive(!active);
    }

    void InitializeSlots()
    {
        for(int i = 0; i < holder.Length; i++)
        {
            if(holder[i].slotHolder != null)
            {
                for(int j = 0; j < holder[i].slotHolder.childCount; j++)
                {
                    if(holder[i].slotHolder.GetChild(j).TryGetComponent<InventorySlot>(out InventorySlot inventorySlot))
                    {
                        inventorySlot.InitializeSlot(this, j, i);
                        holder[i].slot.Add(inventorySlot);
                    }
                }
            }
        }
    }

    public int AddItem(ItemData itemData, int itemAmount)
    {
        for(int i = 0; i < holder.Length; i++)
        {
            for(int j = 0; j < holder[i].itemType.Length; j++)
            {
                if(holder[i].itemType[j] == itemData.itemType)
                {
                    for(int s = 0; s < holder[i].slot.Count; s++)
                    {
                        if(holder[i].slot[s].itemData != null)
                        {
                            if(holder[i].slot[s].itemData == itemData)
                            {
                                if(itemData.maxAmount <= -1)
                                {
                                    holder[i].slot[s].AddAmount(itemData, itemAmount);
                                    return itemAmount;
                                }
                                else if(itemAmount <= itemData.maxAmount - holder[i].slot[s].itemAmount)
                                {
                                    holder[i].slot[s].AddAmount(itemData, itemAmount);
                                    return itemAmount;
                                }
                                else if(itemAmount > itemData.maxAmount - holder[i].slot[s].itemAmount)
                                {
                                    int maxAmount = itemData.maxAmount - holder[i].slot[s].itemAmount;
                                    itemAmount -= maxAmount;

                                    holder[i].slot[s].AddAmount(itemData, maxAmount);

                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }

        for(int i = 0; i < holder.Length; i++)
        {
            for(int j = 0; j < holder[i].itemType.Length; j++)
            {
                if(holder[i].itemType[j] == itemData.itemType)
                {
                    for(int s = 0; s < holder[i].slot.Count; s++)
                    {
                        if(holder[i].slot[s].itemData == null)
                        {
                            if(itemAmount <= itemData.maxAmount - holder[i].slot[s].itemAmount)
                            {
                                holder[i].slot[s].SetItem(itemData, itemAmount);
                                return itemAmount;
                            }
                            else if(itemAmount > itemData.maxAmount - holder[i].slot[s].itemAmount)
                            {
                                int maxAmount = itemData.maxAmount - holder[i].slot[s].itemAmount;
                                itemAmount -= maxAmount;

                                holder[i].slot[s].SetItem(itemData, itemAmount);

                                //AddItem(itemData, itemAmount);
                                //return;
                                continue;
                            }
                        }
                    }
                }
            }
        }

        return itemAmount;
    }

    public int RemoveItem(ItemData itemData, int itemAmount, int slotID, int holderID)
    {
        if(slotID > -1 && holderID > -1)
        {
            if(holder[holderID].slot[slotID].itemData != null)
            {
                if(itemAmount <= holder[holderID].slot[slotID].itemAmount)
                {
                    holder[holderID].slot[slotID].RemoveAmount(itemAmount);

                    return itemAmount;
                }
                else if(itemAmount > holder[holderID].slot[slotID].itemAmount)
                {
                    int maxAmount = holder[holderID].slot[slotID].itemAmount;
                    holder[holderID].slot[slotID].RemoveAmount(maxAmount);

                    return itemAmount;
                }
            }
        }
        
        for(int i = 0; i < holder.Length; i++)
        {
            for(int s = 0; s < holder[i].slot.Count; s++)
            {
                if(holder[i].slot[s].itemData != null)
                {
                    if(holder[i].slot[s].itemData == itemData)
                    {
                        if(itemAmount <= holder[i].slot[s].itemAmount)
                        {
                            holder[i].slot[s].RemoveAmount(itemAmount);

                            return itemAmount;
                        }
                        else if(itemAmount > holder[i].slot[s].itemAmount)
                        {
                            int maxAmount = holder[holderID].slot[slotID].itemAmount;
                            itemAmount -= maxAmount;

                            holder[holderID].slot[slotID].RemoveAmount(maxAmount);

                            continue;
                        }
                    }
                }
            }
        }

        return itemAmount;
    }

    public void PickUpDropItems(int SlotID, int holderID)
    {
        if(cursorHolder.itemData == null)
        {
            if(holder[holderID].slot[SlotID].itemData != null)
            {
                AddItemToCursor(holder[holderID].slot[SlotID].itemData, holder[holderID].slot[SlotID].itemAmount);
                holder[holderID].slot[SlotID].RemoveAmount(holder[holderID].slot[SlotID].itemAmount);
            }
        }
        else if(cursorHolder.itemData != null)
        {
            for(int i = 0; i < holder[holderID].itemType.Length; i++)
            {
                if(holder[holderID].itemType[i] == cursorHolder.itemData.itemType)
                {
                    if(holder[holderID].slot[SlotID].itemData == null)
                    {
                        holder[holderID].slot[SlotID].AddAmount(cursorHolder.itemData, cursorHolder.itemAmount);
                        RemoveItemFromCursor(cursorHolder.itemAmount);
                    }
                    else if(holder[holderID].slot[SlotID].itemData != null)
                    {
                        if(cursorHolder.itemAmount <= holder[holderID].slot[SlotID].itemData.maxAmount - holder[holderID].slot[SlotID].itemAmount)
                        {
                            holder[holderID].slot[SlotID].AddAmount(cursorHolder.itemData, cursorHolder.itemAmount);
                            RemoveItemFromCursor(cursorHolder.itemAmount);
                            Debug.Log("All Amount");
                        }
                        else if(cursorHolder.itemAmount > holder[holderID].slot[SlotID].itemData.maxAmount - holder[holderID].slot[SlotID].itemAmount)
                        {
                            int maxAmount = holder[holderID].slot[SlotID].itemData.maxAmount - holder[holderID].slot[SlotID].itemAmount;
                            RemoveItemFromCursor(maxAmount);
                            holder[holderID].slot[SlotID].AddAmount(cursorHolder.itemData, maxAmount);
                            Debug.Log("Some Amount");
                        }
                    }
                }
            }
        }
    }

    void CursorTracking()
    {
        cursorHolder.cursor.position = Input.mousePosition + cursorHolder.offset;
    }

    void AddItemToCursor(ItemData itemData, int itemAmount)
    {
        cursorHolder.itemData = itemData;
        cursorHolder.itemAmount = itemAmount;

        cursorHolder.cursor.gameObject.SetActive(true);

        cursorHolder.cursorIconRenderer.sprite = itemData.icon;
        cursorHolder.cursorAmountText.text = itemAmount.ToString();
    }

    void RemoveItemFromCursor(int itemAmount)
    {
        if(cursorHolder.itemAmount - itemAmount == 0)
        {
            cursorHolder.itemData = null;
            cursorHolder.itemAmount = 0;

            cursorHolder.cursor.gameObject.SetActive(false);

            cursorHolder.cursorIconRenderer.sprite = null;
            cursorHolder.cursorAmountText.text = 0.ToString();
        }
        else if(cursorHolder.itemAmount - itemAmount > 0)
        {
            cursorHolder.itemAmount -= itemAmount;

            cursorHolder.cursorAmountText.text = cursorHolder.itemAmount.ToString();
        }
    }
}