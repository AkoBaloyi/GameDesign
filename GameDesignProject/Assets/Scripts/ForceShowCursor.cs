using UnityEngine;

/// <summary>
/// Forces cursor to be visible and unlocked in menu
/// </summary>
public class ForceShowCursor : MonoBehaviour
{
    void Update()
    {
        // Force cursor visible every frame
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
