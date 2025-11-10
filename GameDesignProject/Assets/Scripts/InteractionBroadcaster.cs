using UnityEngine;
using UnityEngine.InputSystem;




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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
        {

            DoorInteractor door = hit.collider.GetComponent<DoorInteractor>();
            if (door != null)
            {
                door.OnInteract(context);
                return;
            }

            SecurityDoor securityDoor = hit.collider.GetComponent<SecurityDoor>();
            if (securityDoor != null)
            {
                securityDoor.OnInteract(context);
                return;
            }

        }
    }
}
