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

        if (tutorialManager == null)
        {
            tutorialManager = FindObjectOfType<TutorialManager>();
        }

        if (objectType == PickupType.Nailgun && nailgunWeapon == null)
        {
            nailgunWeapon = GetComponent<NailgunWeapon>();
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

            if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                pickupPressed = true;
            }

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
    
    void PickupObject()
    {
        if (isPickedUp) return;
        
        isPickedUp = true;

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

        PlayPickupEffects();

        NotifyTutorial();

        gameObject.SetActive(false);
    }
    
    void PickupNailgun()
    {

        if (nailgunWeapon != null)
        {
            nailgunWeapon.EquipWeapon();
        }

        gameObject.SetActive(false);
        
        Debug.Log("Nailgun picked up and equipped to gun slot!");
    }
    
    void PickupAmmo()
    {

        NailgunWeapon nailgun = FindObjectOfType<NailgunWeapon>();
        if (nailgun != null && nailgun.IsEquipped())
        {
            nailgun.LoadAmmo(50); // Standard nail bundle
        }
        
        Debug.Log("Nail ammo picked up!");
    }
    
    void PickupGeneric()
    {

        Debug.Log($"Picked up {gameObject.name}");
    }
    
    void PlayPickupEffects()
    {

        if (pickupEffect != null)
        {

            GameObject effect = Instantiate(pickupEffect.gameObject, transform.position, transform.rotation);
            ParticleSystem particles = effect.GetComponent<ParticleSystem>();
            if (particles != null)
            {
                particles.Play();
                Destroy(effect, particles.main.duration + 1f);
            }
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
    }
    
    void NotifyTutorial()
    {
        if (tutorialManager == null) return;

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

    public void ForcePickup()
    {
        PickupObject();
    }

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