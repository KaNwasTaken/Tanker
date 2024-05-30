using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class TankInput : MonoBehaviour
{
    public static PlayerInput playerInput;

    TankMotor tankMotor;
    TankTurret tankTurret;
    TankWeaponSystem weaponSystem;

    InputAction[] fingerActions = new InputAction[10];

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        playerInput = new PlayerInput();
    }
    private void Start()
    {
        tankMotor = GetComponent<TankMotor>();
        tankTurret = transform.GetChild(0).GetComponent<TankTurret>();
        weaponSystem = transform.GetChild(0).GetComponent<TankWeaponSystem>();

        playerInput.Player.FlipUp.performed += ctx => tankMotor.FlipUpTank();

        playerInput.Player.FirstTouch.performed += ctx => ProcessTouch(ctx, 0);
        fingerActions[0] = playerInput.Player.FirstTouch;

        playerInput.Player.SecondTouch.performed += ctx => ProcessTouch(ctx, 1);
        fingerActions[1] = playerInput.Player.SecondTouch;
    }
    private void OnEnable()
    {
        playerInput.Player.Movement.Enable();
        playerInput.Player.FirstTouch.Enable();
        playerInput.Player.SecondTouch.Enable();
        playerInput.Player.FlipUp.Enable();
    }

    public void ProcessTouch(InputAction.CallbackContext ctx, int touchId)
    {
        if (TouchTracker.Instance.IsFingerOverUI(ctx, touchId))
        {
            TouchTracker.Instance.fingersOnUi[touchId] = true;
            return;
        }
        tankTurret.RotateTurret(ctx);
        weaponSystem.SetLatestTouch(ctx, touchId);
    }

    private void FixedUpdate()
    {
        tankMotor.MoveTank(playerInput.Player.Movement.ReadValue<Vector2>());
    }

}
