using UnityEngine;
using UnityEngine.InputSystem;




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

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("[PlayerInteractionHandler] OnInteract called by Input System!");

        DoorInteractor[] doors = FindObjectsOfType<DoorInteractor>();
        
        Debug.Log($"[PlayerInteractionHandler] Found {doors.Length} doors in scene");
        
        foreach (DoorInteractor door in doors)
        {

            float distance = Vector3.Distance(transform.position, door.transform.position);
            
            Debug.Log($"[PlayerInteractionHandler] Door '{door.gameObject.name}' distance: {distance:F2}");


            door.OnInteract(context);
        }

        PowerBay[] powerBays = FindObjectsOfType<PowerBay>();
        Debug.Log($"[PlayerInteractionHandler] Found {powerBays.Length} power bays in scene");
        
        foreach (PowerBay bay in powerBays)
        {
            bay.OnInteract(context);
        }

        FactoryConsole[] consoles = FindObjectsOfType<FactoryConsole>();
        Debug.Log($"[PlayerInteractionHandler] Found {consoles.Length} consoles in scene");
        
        foreach (FactoryConsole console in consoles)
        {
            console.OnInteract(context);
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("[PlayerInteractionHandler] F key detected manually, creating fake context");

            InputAction.CallbackContext fakeContext = new InputAction.CallbackContext();
            OnInteract(fakeContext);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
