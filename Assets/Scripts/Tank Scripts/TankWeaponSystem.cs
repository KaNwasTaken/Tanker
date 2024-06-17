using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(TankTurret))]
public class TankWeaponSystem : MonoBehaviour
{
    public List<WeaponType> availableWeapons;
    public WeaponType currentWeapon;
    int currentWeaponIndex;

    [SerializeField] Image weaponSelectorIcon;
    [SerializeField] Image reloadCooldownIndicator;
    TankShellPool tankShellPool;

    [SerializeField] Transform shellPoint;
    InputAction.CallbackContext latestValidTouch;
    int currentFingerId;

    TankTurret turret;
    Camera activeCamera;

    bool isReloading;
    [SerializeField] ParticleSystem muzzleParticleSystem;

    float reloadTime;
    float currentReloadTime;

    private void Start()
    {
        tankShellPool = TankShellPool.Instance;

        if (currentWeapon == null && availableWeapons[0] != null)
        {
            currentWeaponIndex = 0;
            UpdateWeapon(availableWeapons[currentWeaponIndex]);
        }
        turret = GetComponent<TankTurret>();
    }

    public void CycleWeapon()
    {
        if (availableWeapons.Count <= 0) return;

        if ((currentWeaponIndex + 1) > availableWeapons.Count - 1)
        {
            currentWeaponIndex = 0;
        }
        else
        {
            currentWeaponIndex += 1;
        }
        UpdateWeapon(availableWeapons[currentWeaponIndex]);
    }

    void UpdateWeapon(WeaponType weapon)
    {
        weaponSelectorIcon.sprite = weapon.weaponSprite;
        activeCamera = Camera.main;
        currentWeapon = weapon;
        tankShellPool.ClearPool();
        tankShellPool.AddToPool(weapon.projectilePrefab, 10);
    }

    private void Update()
    {
        ProcessLatestTouch();

        // Reload Indicator Visuals
        if (isReloading)
        {
            reloadCooldownIndicator.fillAmount = currentReloadTime / reloadTime;
            currentReloadTime += Time.deltaTime;
        }
    }

    public void SetLatestTouch(InputAction.CallbackContext ctx, int fingerId)
    {
        latestValidTouch = ctx;
        currentFingerId = fingerId;
    }

    void ProcessLatestTouch()
    {
        if (latestValidTouch.action == null) return;
        if (turret.isRotating) return;

        if (latestValidTouch.action.IsInProgress())
        {
            if (!TouchTracker.Instance.IsFingerOverUI(latestValidTouch, currentFingerId))
            {
                Fire(latestValidTouch);
            }

        }
    }

    public void Fire(InputAction.CallbackContext ctx)
    {
        if (isReloading) return;

        StartCoroutine(ReloadCooldownCoroutine());
        GameObject projectile = tankShellPool.TakeFromPool(shellPoint.position, Quaternion.identity);

        // In case pool is empty, return
        if (projectile == null) return;

        Ray ray = activeCamera.ScreenPointToRay(ctx.ReadValue<Vector2>());
        Physics.Raycast(ray, out RaycastHit hitInfo);
        Vector3 worldPoint = new Vector3(hitInfo.point.x, shellPoint.position.y, hitInfo.point.z);

        Quaternion lookRot =  Quaternion.LookRotation(worldPoint - transform.position);
        lookRot = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);
        projectile.transform.rotation = lookRot;

        projectile.GetComponent<Projectile>().Initialized();
        muzzleParticleSystem.Play();
        projectile.GetComponent<Rigidbody>().AddForce((worldPoint - transform.position).normalized * currentWeapon.projectileSpeed);
    }

    IEnumerator ReloadCooldownCoroutine()
    {
        isReloading = true;
        reloadTime = currentWeapon.timeBetweenShots;
        currentReloadTime = 0;
        yield return new WaitForSeconds(currentWeapon.timeBetweenShots);
        isReloading = false;
    }
}
