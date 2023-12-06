using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
    bool canReload = true;

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

    [Space]
    [Header("Recoil Firearm Rotation")]
    Vector3 firearmCurrentRotation;
    Vector3 firearmTargetRotation;

    [Space]
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
        if(currentAmmo > 0)
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
        canReload = true;
    }

    public bool CanReload() => !isReloading && !isBursting;
    public bool IsReloading() => isReloading;

    public IEnumerator Reloading()
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

        if(IsReloading())
            yield return new WaitForSeconds(reloadTime);

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
            if(currentAmmo <= 0)
                break;

            if(IsReloading())
                yield return new WaitForSeconds(reloadTime);

            Recoil();
            Shoot();

            yield return new WaitForSeconds(1 / (fireRate / 60));
        }

        isAutomatic = false;
    }

    bool CanShootBurst() => !isBursting && canBurst;

    IEnumerator Burst()
    {
        isBursting = true;
        canBurst = false;

        for(int i = 0; i < burstAmount; i++)
        {
            if(currentAmmo <= 0)
                break;

            if(IsReloading())
                yield return new WaitForSeconds(reloadTime);

            Shoot();
            Recoil();

            yield return new WaitForSeconds(timeBetweenBurst);
        }

        isBursting = false;
    }

    void Shoot()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, firearmData.raycastLength))
        {
            if(hit.transform.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                damagable.Damagable(damage);
            }
        }

        currentAmmo--;
    }

    public override Vector3 Sway(Vector3 pos)
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        input.x = Mathf.Clamp(input.x, -firearmData.swayClamp, firearmData.swayClamp);
        input.y = Mathf.Clamp(input.y, -firearmData.swayClamp, firearmData.swayClamp);

        Vector3 target = new Vector3(input.x, input.y, 0);

        Vector3 newPos = Vector3.Lerp(pos, target + Vector3.zero, Time.deltaTime * firearmData.smoothing);

        return newPos;
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

        reloadTime = firearmData.reloadTime;

        singleShotCooldown = firearmData.baseSingleShotCooldown;
        fireRate = firearmData.baseFireRate;

        burstAmount = firearmData.baseBurstAmount;
        timeBetweenBurst = firearmData.baseTimeBetweenBurst;
        burstCooldown = firearmData.baseBurstCooldown;
    }

    public override void OnWeaponSwitch()
    {
        StopAllCoroutines();

        isSingleShooting = false;
        isAutomatic = false;
        isBursting = false;

        OnPrimaryActionUp();
    }
}
