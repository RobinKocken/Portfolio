using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FirearmData", menuName = "Firearm Data", order = 0)]
public class FirearmData : WeaponData
{
    [Space]
    [Header("Firearm Mode")]
    public FireMode fireMode;
    public enum FireMode
    {
        singleshot,
        burst,
        automatic,
        shotgun,
    }

    [Space]
    [Header("Firearm Data")]
    public float baseDamage;
    public float raycastLength;

    [Space]
    [Header("Single Shot Data")]
    public float baseSingleShotCooldown;

    [Space]
    [Header("Automatic Mode")]
    public float baseFireRate;

    [Space]
    [Header("Burst Mode")]
    public int baseBurstAmount;

}
