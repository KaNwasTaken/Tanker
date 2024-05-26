using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TMPro;

public class TankTurret : MonoBehaviour
{
    public Camera cam;


    public bool isRotating = false;

    [SerializeField] Canvas HUDCanvas;
    [SerializeField] TextMeshProUGUI DebugText;


    TankWeaponSystem weaponSystem;

    private void Start()
    {

        weaponSystem = GetComponent<TankWeaponSystem>();

    }


    public void RotateTurret(InputAction.CallbackContext ctx)
    {

        if (!isRotating)
        {
            isRotating = true;
            StartCoroutine(RotationCoroutine(ctx));
        }

    }

    IEnumerator RotationCoroutine(InputAction.CallbackContext ctx)
    {
        Quaternion currentRotation = ScreenpointToRotation(ctx.ReadValue<Vector2>());
        float ratio = 0f;

        while (ratio < 0.3)
        {
            if (ctx.action.IsInProgress())
            {
                currentRotation = ScreenpointToRotation(ctx.ReadValue<Vector2>());
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, ratio);
            ratio += 0.01f;
            yield return null;
        }

        isRotating = false;
        yield return null;
    }

    private Quaternion ScreenpointToRotation(Vector3 screenPoint)
    {
        Ray screenRay = cam.ScreenPointToRay(screenPoint);
        Physics.Raycast(screenRay, out RaycastHit hitInfo);
        Vector3 worldPoint = hitInfo.point;
        Quaternion lookRot = Quaternion.LookRotation(worldPoint - transform.position);
        lookRot = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);
        return lookRot;
    }
}
