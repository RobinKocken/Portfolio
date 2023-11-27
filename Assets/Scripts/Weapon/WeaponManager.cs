using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Select")]
    public WeaponSlot[] weaponSlot;
    public Item selectedWeapon;
    public float currentScrollNum;

    [System.Serializable]
    public struct WeaponSlot
    {
        public Item item;

        [Space]

        public ItemData.ItemType[] itemType;
    }

    public struct WeaponInput
    {
        public KeyCode primaryAction;
        public KeyCode secondaryAction;
        public KeyCode reload;
    }

    void Start()
    {
        InitializeWeaponManager();
    }

    void Update()
    {
        
    }

    void InitializeWeaponManager()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            for(int j = 0; j < weaponSlot.Length; j++)
            {
                if(weaponSlot[j].item == null)
                {
                    if(transform.GetChild(i).TryGetComponent<Item>(out Item item))
                    {
                        if(item.itemData != null && item.equipped == false)
                        {
                            for(int t = 0; t < weaponSlot[j].itemType.Length; t++)
                            {
                                if(item.itemData.itemType == weaponSlot[j].itemType[t])
                                {
                                    weaponSlot[j].item = item;
                                    item.equipped = true;
                                }
                            }
                        }
                    }
                }
            }              
        }
    }

    void SelectWeapon()
    {
        float oldScrollNum = currentScrollNum;

        currentScrollNum += Input.mouseScrollDelta.y;
        currentScrollNum = Mathf.Clamp(currentScrollNum, -weaponSlot.Length - 1, 0);

        if(oldScrollNum != currentScrollNum)
        {
            if(selectedWeapon != null)
            {
                if(selectedWeapon.TryGetComponent<Weapon>(out Weapon weapon))
                {

                }
            }
        }
    }
}
