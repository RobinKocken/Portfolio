using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void Unlock()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
