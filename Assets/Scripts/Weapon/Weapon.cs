using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public bool equipped;

    public virtual void WeaponUpdate()
    {

    }

    public virtual void OnPrimaryActionDown()
    {

    }

    public virtual void OnPrimaryActionUp()
    {

    }
    
    public virtual void OnSecondaryActionDown()
    {

    }

    public virtual void OnSecondaryActionUp()
    {

    }

    public virtual Vector3 Sway(Vector3 pos)
    {
        return Vector3.zero;
    }

    public virtual void OnWeaponSwitch()
    {

    }
}
