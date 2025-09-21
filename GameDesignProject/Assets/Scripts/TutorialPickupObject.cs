using UnityEngine;

[RequireComponent(typeof(HighlightableObject))]
public class TutorialPickupObject : MonoBehaviour
{
    [Header("Pickup Type")]
    public PickupType objectType = PickupType.Nailgun;
    
    [Header("Effects")]
    public ParticleSystem pickupEffect;
    public AudioClip pickupSound;
    public float pickupRange = 3f;
    
    [Header("References")]
    public TutorialManager tutorialManager;
    public NailgunWeapon nailgunWeapon;
    
    public enum PickupType
    {
        Nailgun,
        NailAmmo,
        Generic
    }
    
    private HighlightableObject highlightable;
    private bool isPickedUp = false;
    
    void Start()
    {
        highlightable = GetComponent<HighlightableObject>();
        
        // Auto-find tutorial manager if not assigned
        if (tutorialManager == null)
        {
            tutorialManager = FindObjectOfType<TutorialManager>();
        }
        
        // Auto-find nailgun if this is a nailgun pickup
        if (objectType == PickupType.Nailgun && nailgunWeapon == null)
        {
            nailgunWeapon = GetComponent<NailgunWeapon>();
        }
    }
    
    void Update()
    {
        if (isPickedUp) return;
        
        // Check if player is looking at this object and in range
        if (IsPlayerLookingAt())
        {
            // Highlight the object
            if (highlightable != null)
            {
                highlightable.HighlightOn();
            }
            
            // Check for pickup input
            bool pickupPressed = false;
            
            // Keyboard input
            if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                pickupPressed = true;
            }
            
            // Gamepad input
            if (UnityEngine.InputSystem.Gamepad.current != null && UnityEngine.InputSystem.Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                pickupPressed = true;
            }
            
            if (pickupPressed)
            {
                PickupObject();
            }
        }
        else
        {
            // Turn off highlight when not looking
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
        
        // Cast ray from camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            return hit.collider.gameObject == gameObject;
        }
        
        return false;
    }
    
    void PickupObject()
    {
        if (isPickedUp) return;
        
        isPickedUp = true;
        
        // Handle different pickup types
        switch (objectType)
        {
            case PickupType.Nailgun:
                PickupNailgun();
                break;
            case PickupType.NailAmmo:
                PickupAmmo();
                break;
            case PickupType.Generic:
                PickupGeneric();
                break;
        }
        
        // Play effects
        PlayPickupEffects();
        
        // Notify tutorial
        NotifyTutorial();
        
        // Hide/destroy the pickup object
        gameObject.SetActive(false);
    }
    
    void PickupNailgun()
    {
        // Enable the nailgun weapon
        if (nailgunWeapon != null)
        {
            nailgunWeapon.EquipWeapon();
        }
        
        Debug.Log("Nailgun picked up and equipped!");
    }
    
    void PickupAmmo()
    {
        // Find and load ammo into nailgun
        NailgunWeapon nailgun = FindObjectOfType<NailgunWeapon>();
        if (nailgun != null && nailgun.IsEquipped())
        {
            nailgun.LoadAmmo(50); // Standard nail bundle
        }
        
        Debug.Log("Nail ammo picked up!");
    }
    
    void PickupGeneric()
    {
        // Handle generic pickups
        Debug.Log($"Picked up {gameObject.name}");
    }
    
    void PlayPickupEffects()
    {
        // Particle effect
        if (pickupEffect != null)
        {
            // Instantiate effect at pickup location
            GameObject effect = Instantiate(pickupEffect.gameObject, transform.position, transform.rotation);
            ParticleSystem particles = effect.GetComponent<ParticleSystem>();
            if (particles != null)
            {
                particles.Play();
                Destroy(effect, particles.main.duration + 1f);
            }
        }
        
        // Sound effect
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
    }
    
    void NotifyTutorial()
    {
        if (tutorialManager == null) return;
        
        // Notify tutorial based on pickup type
        switch (objectType)
        {
            case PickupType.Nailgun:
                tutorialManager.OnNailgunPickedUp();
                break;
            case PickupType.NailAmmo:
                tutorialManager.OnNailsLoaded();
                break;
        }
    }
    
    // Method to force pickup (for tutorial scripting)
    public void ForcePickup()
    {
        PickupObject();
    }
    
    // Method to reset pickup (for testing)
    [ContextMenu("Reset Pickup")]
    public void ResetPickup()
    {
        isPickedUp = false;
        gameObject.SetActive(true);
        
        if (highlightable != null)
        {
            highlightable.HighlightOff();
        }
    }
}