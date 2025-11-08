using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Allows player to inspect objects and trigger story events
/// Used for console and power bay inspection
/// </summary>
public class InspectableObject : MonoBehaviour
{
    [Header("Inspection")]
    public string inspectionMessage = "Press E to inspect";
    public string inspectionResult = "Nothing unusual here.";
    public float inspectionRange = 3f;
    
    [Header("UI")]
    public GameObject promptUI;
    public TextMeshProUGUI promptText;
    public GameObject inspectionPanel;
    public TextMeshProUGUI inspectionText;
    public float inspectionDisplayTime = 3f;
    
    [Header("Trigger")]
    public bool canInspect = true;
    public bool inspectOnce = true;
    
    [Header("Events")]
    public UnityEngine.Events.UnityEvent onInspected;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip inspectionSound;
    
    private bool hasBeenInspected = false;
    private bool playerInRange = false;
    private Transform player;

    void Start()
    {
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        // Hide UI initially
        if (promptUI != null) promptUI.SetActive(false);
        if (inspectionPanel != null) inspectionPanel.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        // Check distance to player
        float distance = Vector3.Distance(transform.position, player.position);
        bool wasInRange = playerInRange;
        playerInRange = distance <= inspectionRange;

        // Update prompt visibility
        UpdatePrompt();

        // Handle inspection input
        if (playerInRange && canInspect && !hasBeenInspected)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                Inspect();
            }
        }
    }

    void UpdatePrompt()
    {
        bool shouldShow = playerInRange && canInspect && !hasBeenInspected;
        
        if (promptUI != null)
        {
            promptUI.SetActive(shouldShow);
            
            if (shouldShow && promptText != null)
            {
                promptText.text = inspectionMessage;
            }
        }
    }

    void Inspect()
    {
        Debug.Log($"[InspectableObject] {gameObject.name} inspected!");
        
        hasBeenInspected = true;

        // Hide prompt
        if (promptUI != null) promptUI.SetActive(false);

        // Show inspection result
        if (inspectionPanel != null && inspectionText != null)
        {
            inspectionPanel.SetActive(true);
            inspectionText.text = inspectionResult;
            
            // Hide after delay
            Invoke(nameof(HideInspectionPanel), inspectionDisplayTime);
        }

        // Play sound
        if (audioSource != null && inspectionSound != null)
        {
            audioSource.PlayOneShot(inspectionSound);
        }

        // Trigger event
        onInspected?.Invoke();

        // Disable if inspect once
        if (inspectOnce)
        {
            canInspect = false;
        }
    }

    void HideInspectionPanel()
    {
        if (inspectionPanel != null)
        {
            inspectionPanel.SetActive(false);
        }
    }

    public void EnableInspection()
    {
        canInspect = true;
        hasBeenInspected = false;
    }

    public void DisableInspection()
    {
        canInspect = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = canInspect ? Color.cyan : Color.gray;
        Gizmos.DrawWireSphere(transform.position, inspectionRange);
    }
}
