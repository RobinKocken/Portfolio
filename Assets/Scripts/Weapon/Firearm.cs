using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearm : Weapon
{
    [Space]
    [Header("Firearm Data")]
    public FirearmData firearmData;
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
    bool isAutomatic;
    bool canAutomatic = true;

    [Space]
    [Header("Burst Mode")]
    public float burstAmount;
    public float timeBetweenBurst;
    public float burstCooldown;
    bool isBursting;
    bool canBurst = true;

    [Space]
    [Header("Recoil Camera Rotation")]
    Vector3 camCurrentRotation;
    Vector3 camTargetRotation;

    [Header("Recoil Firearm Rotation")]
    Vector3 firearmCurrentRotation;
    Vector3 firearmTargetRotation;

    [Header("Recoil Firearm Position")]
    Vector3 firearmCurrentPosition;
    Vector3 firearmTargetPosition;

    public override void InitializeItem()
    {
        SetFirearmData();
    }

    public override void WeaponUpdate()
    {
        SetPosAndRot();
    }

    public override void OnPrimaryActionDown()
    {
        if(!isReloading && currentAmmo > 0)
        {
            switch(firearmData.fireMode)
            {
                case FirearmData.FireMode.singleshot:
                {
                    if(CanShootSingleShoot())
                        StartCoroutine(SingleShot());

                    break;
                }
                case FirearmData.FireMode.automatic:
                {
                    if(CanShootAutomatic())
                        StartCoroutine(Automatic());

                    break;
                }
                case FirearmData.FireMode.burst:
                {
                    if(CanShootBurst())
                        StartCoroutine(Burst());

                    break;
                }
                default:
                {
                    return;
                }
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

    IEnumerator Automatic()
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

    bool CanShootBurst() => !isBursting && canBurst;

    IEnumerator Burst()
    {
        for(int i = 0; i < burstAmount; i++)
        {
            if(currentAmmo <= 0)
                break;

            Shoot();
            Recoil();

            yield return new WaitForSeconds(timeBetweenBurst);
        }

        yield return new WaitForSeconds(0);
        isBursting = false;
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
        camTargetRotation += new Vector3(firearmData.camRecoil.x, Random.Range(-firearmData.camRecoil.y, firearmData.camRecoil.y), Random.Range(-firearmData.camRecoil.z, firearmData.camRecoil.z));
        firearmTargetRotation += new Vector3(firearmData.firearmRecoil.x, Random.Range(-firearmData.firearmRecoil.y, firearmData.firearmRecoil.y), Random.Range(-firearmData.firearmRecoil.z, firearmData.firearmRecoil.z));
        firearmTargetPosition = new Vector3(firearmData.localPlacmentPos.x, firearmData.localPlacmentPos.y, firearmData.firearmRecoilBackUp + firearmData.localPlacmentPos.z);
    }

    void SetPosAndRot()
    {
        camTargetRotation = Vector3.Lerp(camTargetRotation, Vector3.zero, firearmData.camReturnSpeed * Time.deltaTime);
        camCurrentRotation = Vector3.Lerp(camCurrentRotation, camTargetRotation, firearmData.camSnappiness * Time.deltaTime);

        firearmTargetRotation = Vector3.Lerp(firearmTargetRotation, Vector3.zero, firearmData.firearmReturnSpeed * Time.deltaTime);
        firearmCurrentRotation = Vector3.Lerp(firearmCurrentRotation, firearmTargetRotation, firearmData.firearmSnappiness * Time.deltaTime);

        firearmTargetPosition = Vector3.Lerp(firearmTargetPosition, firearmData.localPlacmentPos, firearmData.backUpReturnSpeed * Time.deltaTime);
        firearmCurrentPosition = Vector3.Lerp(firearmCurrentPosition, firearmTargetPosition, firearmData.backUpSnappiness * Time.deltaTime);

        Camera.main.transform.localRotation = Quaternion.Euler(camCurrentRotation);
        transform.localRotation = Quaternion.Euler(firearmCurrentRotation);
        transform.localPosition = firearmCurrentPosition;
    }

    void SetFirearmData()
    {
        firearmCurrentPosition = firearmData.localPlacmentPos;

        damage = firearmData.baseDamage;

        maxAmmo = firearmData.baseMaxAmmo;
        currentAmmo = maxAmmo;

        singleShotCooldown = firearmData.baseSingleShotCooldown;
        fireRate = firearmData.baseFireRate;

        burstAmount = firearmData.baseBurstAmount;
        timeBetweenBurst = firearmData.baseTimeBetweenBurst;
        burstCooldown = firearmData.baseBurstCooldown;
    }
}
