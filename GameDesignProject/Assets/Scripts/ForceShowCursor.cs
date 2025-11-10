using UnityEngine;



public class ForceShowCursor : MonoBehaviour
{
    void Update()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
