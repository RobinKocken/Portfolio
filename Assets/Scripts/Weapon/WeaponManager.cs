using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject playerCamera;

    [Header("Input")]
    public KeyCode leftMouseKey;
    public KeyCode rightMouseKey;
    public KeyCode reload;

    [Header("Weapon Select")]
    public List<WeaponSlot> weaponSlots;
    public WeaponSlot selectedWeapon;
    public int weaponSlotNumber;

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
            if(transform.GetChild(i).TryGetComponent<WeaponSlot>(out WeaponSlot weaponSlot))
            {
                weaponSlots.Add(weaponSlot);
            }
        }
    }
}
