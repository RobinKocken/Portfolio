using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Select")]
    public Weapon currentWeapon;
    public WeaponSlot[] weaponSlot;
    public float currentScrollNum;

    [System.Serializable]
    public struct WeaponSlot
    {
        public Weapon weapon;
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
        SelectWeapon();

        if(currentWeapon != null)
        {
            PlayerInput();
            currentWeapon.WeaponUpdate();
            transform.localPosition = currentWeapon.Sway(transform.localPosition);
        }
    }

    void PlayerInput()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            currentWeapon.OnPrimaryActionDown();
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            currentWeapon.OnPrimaryActionUp();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(currentWeapon.transform.TryGetComponent<Firearm>(out Firearm firearm))
            {
                if(firearm.CanReload())
                {
                    StartCoroutine(firearm.Reloading());
                }
            }            
        }
    }

    void InitializeWeaponManager()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            for(int j = 0; j < weaponSlot.Length; j++)
            {
                if(weaponSlot[j].weapon == null)
                {
                    if(transform.GetChild(i).TryGetComponent<Weapon>(out Weapon weapon))
                    {                       
                        if(weapon.itemData != null && weapon.equipped == false)
                        {
                            for(int t = 0; t < weaponSlot[j].itemType.Length; t++)
                            {
                                if(weaponSlot[j].itemType[t] == weapon.itemData.itemType)
                                {                                  
                                    weaponSlot[j].weapon = weapon;
                                    weapon.equipped = true;

                                    weapon.InitializeItem();

                                    if(j != Mathf.Abs((int)currentScrollNum))
                                    {
                                        weapon.gameObject.SetActive(false);

                                    }
                                    else if(j == Mathf.Abs((int)currentScrollNum))
                                    {
                                        currentWeapon = weapon;
                                    }                                    
                                }
                            }
                        }
                    }
                }
            }              
        }

        for(int i = 0; i < weaponSlot.Length; i++)
        {
            if(weaponSlot[i].weapon != null)
            {
                weaponSlot[i].weapon.transform.SetSiblingIndex(i);
            }
        }
    }

    void SelectWeapon()
    {
        float oldScrollNum = currentScrollNum;

        currentScrollNum += Input.mouseScrollDelta.y;
        currentScrollNum = Mathf.Clamp(currentScrollNum, -weaponSlot.Length + 1, 0);

        if(oldScrollNum != currentScrollNum)
        {
            if(currentWeapon != null)
            {
                currentWeapon.OnWeaponSwitch();
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = null;
            }

            if(weaponSlot[Mathf.Abs((int)currentScrollNum)].weapon != null)
            {
                if(weaponSlot[Mathf.Abs((int)currentScrollNum)].weapon.TryGetComponent<Weapon>(out Weapon newWeapon))
                {
                    currentWeapon = newWeapon;
                    currentWeapon.gameObject.SetActive(true);
                }
            }
        }
    }

    public bool AddWeapon(Weapon weapon)
    {
        for(int i = 0; i < weaponSlot.Length; i++)
        {
            if(weaponSlot[i].weapon == null)
            {
                GameObject weaponObject = Instantiate(weapon.itemData.prefab, transform.position, Quaternion.identity, transform);

                if(weaponObject.TryGetComponent<Weapon>(out Weapon newWeapon))
                {
                    weaponSlot[i].weapon = newWeapon;
                    weaponSlot[i].weapon.InitializeItem();
                }             

                return true;
            }
        }

        return false;
    }
}
