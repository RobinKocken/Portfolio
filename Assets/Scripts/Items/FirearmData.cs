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
    public Vector3 localPlacmentPos;
    public float baseDamage;
    public float raycastLength;

    [Space]
    [Header("Ammuniton Data")]
    public int baseMaxAmmo;

    [Space]
    [Header("Single Shot Data")]
    public float baseSingleShotCooldown;

    [Space]
    [Header("Automatic Mode")]
    public float baseFireRate;

    [Space]
    [Header("Burst Mode")]
    public int baseBurstAmount;
    public float baseTimeBetweenBurst;
    public float baseBurstCooldown;

    [Space]
    [Header("Sway")]
    public float swayClamp;
    public float smoothing;

    [Space]
    [Header("Recoil Camera Rotation")]
    public Vector3 camRecoil;
    public float camSnappiness;
    public float camReturnSpeed;

    [Header("Recoil Firearm Rotation")]
    public Vector3 firearmRecoil;
    public float firearmSnappiness;
    public float firearmReturnSpeed;

    [Header("Recoil Firearm Position")]
    public float firearmRecoilBackUp;
    public float backUpSnappiness;
    public float backUpReturnSpeed;
}
