using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FirearmData", menuName = "Firearm Data", order = 0)]
public class FirearmData : WeaponData
{
    [Header("Firearm Data")]
    public FireMode fireMode;

    public enum FireMode
    {
        singleshot,
        burst,
        automatic,
        shotgun,
    }
}
