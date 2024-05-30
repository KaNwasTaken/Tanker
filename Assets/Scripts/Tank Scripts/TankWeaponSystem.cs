using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TankTurret))]
public class TankWeaponSystem : MonoBehaviour
{
    public List<WeaponType> availableWeapons;
    public WeaponType currentWeapon;
    TankShellPool tankShellPool;

    [SerializeField] Transform shellPoint;
    InputAction.CallbackContext latestValidTouch;
    int currentFingerId;

    //public delegate void WeaponSystemEvents();
    //public event WeaponSystemEvents WeaponFired;

    TankTurret turret;
    Camera activeCamera;

    bool isReloading;
    [SerializeField] ParticleSystem muzzleParticleSystem;

    private void Start()
    {
        tankShellPool = TankShellPool.Instance;

        if (currentWeapon == null && availableWeapons[0] != null)
        {
            UpdateWeapon(availableWeapons[0]);
        }
        turret = GetComponent<TankTurret>();
    }

    void UpdateWeapon(WeaponType weapon)
    {
        activeCamera = Camera.main;
        currentWeapon = weapon;
        tankShellPool.shellPool.Clear();
        tankShellPool.AddToPool(weapon.projectilePrefab, 10);
    }

    private void Update()
    {
        ProcessLatestTouch();
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
        yield return new WaitForSeconds(currentWeapon.timeBetweenShots);
        isReloading = false;
    }
}
