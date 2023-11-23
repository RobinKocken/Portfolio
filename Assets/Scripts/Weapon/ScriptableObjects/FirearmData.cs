using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FirearmData", menuName = "Firearm Data", order = 0)]
public class FirearmData : WeaponData
{
    public enum FirearmMode
    {
        singleshot,
        burst,
        automatic,
        shotgun,
    }
}
