using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public Weapon weapon;

    public void WeaponInput(KeyCode leftMouseKey, KeyCode rightMouseKey, KeyCode reloadKey)
    {
        if(Input.GetKeyDown(leftMouseKey))
        {
            weapon.OnLeftMouseDown();
        }
        if(Input.GetKeyUp(leftMouseKey))
        {
            weapon.OnLeftMouseUp();
        }

        if(Input.GetKeyDown(rightMouseKey))
        {
            weapon.OnRightMouseDown();
        }
        if(Input.GetKeyUp(rightMouseKey))
        {
            weapon.OnRightMouseUp();
        }

        if(Input.GetKeyDown(reloadKey))
        {

        }
        if(Input.GetKeyUp(reloadKey))
        {

        }
    }

    public void WeaponUpdate()
    {
        weapon.WeaponSway();
    }
}
