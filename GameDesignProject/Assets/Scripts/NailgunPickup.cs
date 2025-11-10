using UnityEngine;

[RequireComponent(typeof(HighlightableObject))]
public class NailgunPickup : MonoBehaviour
{
    [Header("References")]
    public NailgunWeapon nailgunWeapon;
    public GunSlotManager gunSlotManager;
    public TutorialManager tutorialManager;
    
    [Header("Settings")]
    public float pickupRange = 3f;
    
    private HighlightableObject highlightable;
    private bool isPickedUp = false;
    
    void Start()
    {
        highlightable = GetComponent<HighlightableObject>();

        if (gunSlotManager == null)
        {
            gunSlotManager = FindObjectOfType<GunSlotManager>();
        }
        
        if (tutorialManager == null)
        {
            tutorialManager = FindObjectOfType<TutorialManager>();
        }
        
        if (nailgunWeapon == null)
        {
            nailgunWeapon = GetComponent<NailgunWeapon>();
        }
        
        if (highlightable == null)
        {
            Debug.LogError($"NailgunPickup on {gameObject.name} needs HighlightableObject component!");
        }
    }
    
    void Update()
    {
        if (isPickedUp) return;

        if (IsPlayerLookingAt())
        {

            if (highlightable != null)
            {
                highlightable.HighlightOn();
            }

            bool pickupPressed = false;

            if (UnityEngine.InputSystem.Keyboard.current != null && 
                UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                pickupPressed = true;
            }

            if (UnityEngine.InputSystem.Gamepad.current != null && 
                UnityEngine.InputSystem.Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                pickupPressed = true;
            }
            
            if (pickupPressed)
            {
                PickupNailgun();
            }
        }
        else
        {

            if (highlightable != null)
            {
                highlightable.HighlightOff();
            }
        }
    }
    
    bool IsPlayerLookingAt()
    {
        Camera playerCamera = Camera.main;
        if (playerCamera == null) return false;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            return hit.collider.gameObject == gameObject;
        }
        
        return false;
    }
    
    void PickupNailgun()
    {
        if (isPickedUp) return;
        
        isPickedUp = true;

        if (gunSlotManager != null && nailgunWeapon != null)
        {
            gunSlotManager.EquipNailgun(nailgunWeapon);
        }
        else
        {
            Debug.LogError("GunSlotManager or NailgunWeapon not found!");
        }

        if (highlightable != null)
        {
            highlightable.HighlightOff();
        }
        
        Debug.Log("Nailgun picked up and equipped in gun slot!");
    }

    [ContextMenu("Force Pickup Nailgun")]
    public void ForcePickup()
    {
        PickupNailgun();
    }

    [ContextMenu("Reset Pickup")]
    public void ResetPickup()
    {
        isPickedUp = false;
        if (highlightable != null)
        {
            highlightable.HighlightOff();
        }
    }
}