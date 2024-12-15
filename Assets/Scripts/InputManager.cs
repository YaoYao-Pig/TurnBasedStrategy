#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; set; }
    private PlayerInpuActions playerInputActions;

    private void Awake()
    {
        Instance = this;
        playerInputActions= new PlayerInpuActions();
        playerInputActions.Player.Enable();


    }
    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool IsLeftMouseButtonDown()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }
    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        Vector2 inputMoveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y += -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x += -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x += 1f;
        }
        return inputMoveDir;
#endif
    }

    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else

        float rotateAmount = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            rotateAmount += 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotateAmount += -1f;
        }
        return rotateAmount;
#endif
    }

    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        float zoomAmount = 0;
        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount -= 1f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount += 1f;
        }
        return zoomAmount;
#endif
    }

}
