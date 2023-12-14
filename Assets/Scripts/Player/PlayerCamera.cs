using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    PlayerInput input;

    public Transform orientation;

    public float mouseSens;

    Vector2 mouse;
    float xRotation;
    float yRotation;

    void Start()
    {
        input = new PlayerInput();
        input.Player.Enable();
        input.Player.Camera.performed += OnCamPerformed;
        input.Player.Camera.canceled += OnCamCancelled;
    }

    void Update()
    {
        FPCamera();
    }

    void OnCamPerformed(InputAction.CallbackContext context)
    {

    }

    void OnCamCancelled(InputAction.CallbackContext context)
    {

    }

    void FPCamera()
    {
        mouse = input.Player.Camera.ReadValue<Vector2>();

        xRotation += -mouse.y;
        yRotation += mouse.x;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        orientation.localRotation = Quaternion.Euler(0, yRotation, 0);
    }
}
