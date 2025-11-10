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

        if (playerInRange && CanTakeNails())
        {
            bool interactPressed = false;

            if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                interactPressed = true;
            }

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

        NailgunWeapon nailgun = FindObjectOfType<NailgunWeapon>();
        if (nailgun == null || !nailgun.IsEquipped())
        {
            Debug.Log("Need to equip Nailgun first!");
            return;
        }

        if (nailgun.GetCurrentAmmo() >= nailgun.GetMaxAmmo())
        {
            Debug.Log("Nailgun is already full!");
            return;
        }

        currentBundles--;

        nailgun.LoadAmmo(nailsPerBundle);

        UpdateVisualState();

        PlayLoadingEffects();

        if (!CanTakeNails() && interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        
        Debug.Log($"Loaded {nailsPerBundle} nails. Bundles remaining: {currentBundles}");
    }
    
    void UpdateVisualState()
    {

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

        if (loadingEffect != null)
        {
            loadingEffect.Play();
        }

        if (crateAudio != null && takeNailsSound != null)
        {
            crateAudio.PlayOneShot(takeNailsSound);
        }

        if (tutorialManager != null)
        {
            tutorialManager.OnNailsLoaded();
        }

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

    public bool IsEmpty()
    {
        return currentBundles <= 0;
    }

    public int GetRemainingBundles()
    {
        return currentBundles;
    }
}