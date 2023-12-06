using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform orientation;

    public float mouseSens;

    float mouseX;
    float mouseY;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        FPCamera();
    }

    void FPCamera()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSens;
        mouseY = Input.GetAxis("Mouse Y") * mouseSens;

        xRotation += -mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        orientation.localRotation = Quaternion.Euler(0, yRotation, 0);
    }
}
