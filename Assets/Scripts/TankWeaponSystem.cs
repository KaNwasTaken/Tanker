using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TankTurret))]
public class TankWeaponSystem : MonoBehaviour
{
    public List<WeaponType> availableWeapons;
    WeaponType currentWeapon;

    [SerializeField] Transform shellPoint;
    InputAction.CallbackContext latestValidTouch;
    int currentFingerId;

    TankTurret turret;

    Camera activeCamera;

    bool isReloading;
    private void Start()
    {
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
        if (!isReloading)
        {
            StartCoroutine(ReloadCooldownCoroutine());
            GameObject projectile = Instantiate(currentWeapon.projectilePrefab, shellPoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            Ray ray = activeCamera.ScreenPointToRay(ctx.ReadValue<Vector2>());
            Physics.Raycast(ray, out RaycastHit hitInfo);
            Vector3 worldPoint = new Vector3(hitInfo.point.x, shellPoint.position.y, hitInfo.point.z);

            projectile.transform.rotation = Quaternion.LookRotation(worldPoint - transform.position);
            rb.AddForce((worldPoint - shellPoint.position).normalized * 5000);
        }

    }

    IEnumerator ReloadCooldownCoroutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(currentWeapon.timeBetweenShots);
        isReloading = false;
    }
}
