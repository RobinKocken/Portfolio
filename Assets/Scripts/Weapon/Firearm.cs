using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Firearm : Weapon
{
    [Space]
    [Header("Firearm Data")]
    FirearmData firearmData;
    public float damage;

    [Space]
    [Header("Ammunition Data")]
    public int maxAmmo;
    public int currentAmmo;

    [Space]
    [Header("Reload Data")]
    public float reloadTime;
    bool isReloading;

    [Space]
    [Header("Single Shot Data")]
    public float singleShotCooldown;
    bool isSingleShooting;
    bool canSingleShoot = true;

    [Space]
    [Header("Automatic Data")]
    public float fireRate;
    float timeSinceLastShot;
    bool isAutomatic;
    bool canAutomatic = true;

    [Space]
    [Header("Burst Mode")]
    public float burstAmount;
    bool isBursting;
    bool canBurst = true;


    public override void InitializeItem()
    {
        if(itemData.GetComponent<FirearmData>() != null) 
        {
            firearmData = itemData.GetComponent<FirearmData>();
        }
        
    }

    public override void OnPrimaryActionDown()
    {
        switch(firearmData.fireMode)
        {
            case FirearmData.FireMode.singleshot:
            {
                break;
            }
            case FirearmData.FireMode.automatic:
            {
                break;
            }
            case FirearmData.FireMode.burst:
            {
                break;
            }
            default:
            {
                return;
            }
        }
    }

    public override void OnPrimaryActionUp()
    {
        canSingleShoot = true;
        canAutomatic = true;
        canBurst = true;
    }

    public bool IsReloading() => isReloading;

    IEnumerator Reloading()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime / 2);
        currentAmmo = maxAmmo;
        yield return new WaitForSeconds(reloadTime / 2);

        isReloading = false;
    }

    bool CanShootSingleShoot() => !isSingleShooting && canSingleShoot;

    IEnumerator SingleShot()
    {
        isSingleShooting = true;
        canSingleShoot = false;

        Shoot();
        Recoil();

        yield return new WaitForSeconds(singleShotCooldown);
        isSingleShooting = false;
    }

    bool CanShootAutomatic() => !isAutomatic && canAutomatic;

    IEnumerator Automtic()
    {
        isAutomatic = true;
        canAutomatic = false;

        while(!canAutomatic)
        {
            Recoil();
            Shoot();

            yield return new WaitForEndOfFrame();
        }

        isAutomatic = false;
    }


    void Shoot()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, firearmData.raycastLength))
        {
            if(hit.transform.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                
            }
        }

        currentAmmo--;
    }

    void Recoil()
    {

    }
}
