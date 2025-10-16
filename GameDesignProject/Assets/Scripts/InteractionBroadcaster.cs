using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Broadcasts interaction input to nearby interactable objects (doors, etc.)
/// Attach this to the Player GameObject
/// </summary>
public class InteractionBroadcaster : MonoBehaviour
{
    [Header("Settings")]
    public float interactionRange = 3f;
    public LayerMask interactableLayer = ~0;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    // This is called by the Input System when F key is pressed
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Raycast from camera to find interactable objects
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
        {
            // Try to find DoorInteractor
            DoorInteractor door = hit.collider.GetComponent<DoorInteractor>();
            if (door != null)
            {
                door.OnInteract(context);
                return;
            }

            // Try to find SecurityDoor
            SecurityDoor securityDoor = hit.collider.GetComponent<SecurityDoor>();
            if (securityDoor != null)
            {
                securityDoor.OnInteract(context);
                return;
            }

            // Add more interactable types here as needed
        }
    }
}
