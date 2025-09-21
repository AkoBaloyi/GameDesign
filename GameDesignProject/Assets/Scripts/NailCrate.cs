using UnityEngine;

public class NailCrate : MonoBehaviour
{
    [Header("Crate Settings")]
    public int nailsPerBundle = 50;
    public int maxBundles = 5;
    public int currentBundles = 5;
    
    [Header("Visual Feedback")]
    public GameObject[] nailBundleVisuals; // Visual representations of nail bundles
    public Material fullCrateMaterial;
    public Material emptyCrateMaterial;
    public Renderer crateRenderer;
    
    [Header("Effects")]
    public ParticleSystem loadingEffect;
    public AudioSource crateAudio;
    public AudioClip takeNailsSound;
    public AudioClip emptySound;
    
    [Header("UI Feedback")]
    public GameObject interactionPrompt; // "Press E to load nails"
    
    [Header("References")]
    public TutorialManager tutorialManager;
    
    // Private variables
    private bool playerInRange = false;
    private FPController playerController;
    
    void Start()
    {
        UpdateVisualState();
        
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
    
    void Update()
    {
        // Handle interaction input when player is in range
        if (playerInRange && CanTakeNails())
        {
            bool interactPressed = false;
            
            // Check for keyboard input
            if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                interactPressed = true;
            }
            
            // Check for gamepad input
            if (UnityEngine.InputSystem.Gamepad.current != null && UnityEngine.InputSystem.Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                interactPressed = true;
            }
            
            if (interactPressed)
            {
                TakeNails();
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerController = other.GetComponent<FPController>();
            
            // Show interaction prompt
            if (interactionPrompt != null && CanTakeNails())
            {
                interactionPrompt.SetActive(true);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerController = null;
            
            // Hide interaction prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }
    
    bool CanTakeNails()
    {
        return currentBundles > 0;
    }
    
    void TakeNails()
    {
        if (!CanTakeNails()) 
        {
            PlayEmptySound();
            return;
        }
        
        // Find the player's nailgun
        NailgunWeapon nailgun = FindObjectOfType<NailgunWeapon>();
        if (nailgun == null || !nailgun.IsEquipped())
        {
            Debug.Log("Need to equip Nailgun first!");
            return;
        }
        
        // Check if nailgun is full
        if (nailgun.GetCurrentAmmo() >= nailgun.GetMaxAmmo())
        {
            Debug.Log("Nailgun is already full!");
            return;
        }
        
        // Take one bundle
        currentBundles--;
        
        // Load nails into nailgun
        nailgun.LoadAmmo(nailsPerBundle);
        
        // Update visual state
        UpdateVisualState();
        
        // Play effects
        PlayLoadingEffects();
        
        // Hide prompt if empty
        if (!CanTakeNails() && interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        
        Debug.Log($"Loaded {nailsPerBundle} nails. Bundles remaining: {currentBundles}");
    }
    
    void UpdateVisualState()
    {
        // Update nail bundle visuals
        if (nailBundleVisuals != null)
        {
            for (int i = 0; i < nailBundleVisuals.Length; i++)
            {
                if (nailBundleVisuals[i] != null)
                {
                    nailBundleVisuals[i].SetActive(i < currentBundles);
                }
            }
        }
        
        // Update crate material
        if (crateRenderer != null)
        {
            if (currentBundles > 0)
            {
                crateRenderer.material = fullCrateMaterial;
            }
            else
            {
                crateRenderer.material = emptyCrateMaterial;
            }
        }
    }
    
    void PlayLoadingEffects()
    {
        // Particle effect
        if (loadingEffect != null)
        {
            loadingEffect.Play();
        }
        
        // Sound effect
        if (crateAudio != null && takeNailsSound != null)
        {
            crateAudio.PlayOneShot(takeNailsSound);
        }
        
        // Notify tutorial manager
        if (tutorialManager != null)
        {
            tutorialManager.OnNailsLoaded();
        }
        
        // Optional: Camera shake for feedback
        StartCoroutine(CameraShake());
    }
    
    void PlayEmptySound()
    {
        if (crateAudio != null && emptySound != null)
        {
            crateAudio.PlayOneShot(emptySound);
        }
    }
    
    System.Collections.IEnumerator CameraShake()
    {
        Transform playerCamera = Camera.main?.transform;
        if (playerCamera == null) yield break;
        
        Vector3 originalPos = playerCamera.localPosition;
        float shakeDuration = 0.2f;
        float shakeAmount = 0.02f;
        
        float elapsed = 0f;
        
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;
            
            playerCamera.localPosition = originalPos + new Vector3(x, y, 0f);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        playerCamera.localPosition = originalPos;
    }
    
    // Method to refill the crate (for testing or gameplay mechanics)
    [ContextMenu("Refill Crate")]
    public void RefillCrate()
    {
        currentBundles = maxBundles;
        UpdateVisualState();
        
        if (interactionPrompt != null && playerInRange)
        {
            interactionPrompt.SetActive(true);
        }
    }
    
    // Method to check if crate is empty
    public bool IsEmpty()
    {
        return currentBundles <= 0;
    }
    
    // Method to get remaining bundles
    public int GetRemainingBundles()
    {
        return currentBundles;
    }
}