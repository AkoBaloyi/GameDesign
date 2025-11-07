using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles F key interaction and forwards it to nearby interactable objects
/// Attach this to your Player GameObject
/// </summary>
public class PlayerInteractionHandler : MonoBehaviour
{
    [Header("Settings")]
    public float interactionRange = 3f;
    public LayerMask interactionLayer = ~0;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
    }

    // This method is called by the Input System when F key is pressed
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("[PlayerInteractionHandler] OnInteract called by Input System!");

        // Find all DoorInteractor components in the scene
        DoorInteractor[] doors = FindObjectsOfType<DoorInteractor>();
        
        Debug.Log($"[PlayerInteractionHandler] Found {doors.Length} doors in scene");
        
        foreach (DoorInteractor door in doors)
        {
            // Check if player is in range of this door using distance
            float distance = Vector3.Distance(transform.position, door.transform.position);
            
            Debug.Log($"[PlayerInteractionHandler] Door '{door.gameObject.name}' distance: {distance:F2}");
            
            // IMPORTANT: Call OnInteract on ALL doors, let them decide if player is in range
            // The door's own playerInRange check will handle this
            door.OnInteract(context);
        }

        // Also broadcast to PowerBay components
        PowerBay[] powerBays = FindObjectsOfType<PowerBay>();
        Debug.Log($"[PlayerInteractionHandler] Found {powerBays.Length} power bays in scene");
        
        foreach (PowerBay bay in powerBays)
        {
            bay.OnInteract(context);
        }

        // Broadcast to FactoryConsole components
        FactoryConsole[] consoles = FindObjectsOfType<FactoryConsole>();
        Debug.Log($"[PlayerInteractionHandler] Found {consoles.Length} consoles in scene");
        
        foreach (FactoryConsole console in consoles)
        {
            console.OnInteract(context);
        }
    }

    // Fallback: Also check for manual F key press in case Input System isn't wired up
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("[PlayerInteractionHandler] F key detected manually, creating fake context");
            
            // Create a fake context and call OnInteract
            InputAction.CallbackContext fakeContext = new InputAction.CallbackContext();
            OnInteract(fakeContext);
        }
    }

    // Visualize interaction range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
