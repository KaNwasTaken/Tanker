using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchTracker : MonoBehaviour
{
    // UI Raycast Variables
    GraphicRaycaster graphicRaycaster;
    PointerEventData pointer;
    List<RaycastResult> raycastResults;
    [SerializeField] Canvas hudCanvas;

    [HideInInspector] public InputAction[] touchActions = new InputAction[10];
    public bool[] fingersOnUi = new bool[10];

    public static TouchTracker Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        graphicRaycaster = hudCanvas.GetComponent<GraphicRaycaster>();

        touchActions[0] = TankInput.playerInput.Player.FirstTouch;
        touchActions[1] = TankInput.playerInput.Player.SecondTouch;
    }
    private void Update()
    {
        CheckFingerLiftedOffUi();
    }
    public bool IsFingerOverUI(InputAction.CallbackContext ctx)
    {
        pointer = new(EventSystem.current);
        pointer.position = ctx.ReadValue<Vector2>();
        raycastResults = new();

        graphicRaycaster.Raycast(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsFingerOverUI(InputAction.CallbackContext ctx, int touchID)
    {
        if (fingersOnUi[touchID])
        {
            return true;
        }
        else
        {
            return IsFingerOverUI(ctx);
        }
    }

    void CheckFingerLiftedOffUi()
    {
        for (int i = 0; i < fingersOnUi.Length; i++)
        {
            if (fingersOnUi[i])
            {
                if (!touchActions[i].IsInProgress())
                {
                    fingersOnUi[i] = false;
                }
            }
        }
    }
}
