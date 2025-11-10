using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;




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

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        if (promptUI != null) promptUI.SetActive(false);
        if (inspectionPanel != null) inspectionPanel.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool wasInRange = playerInRange;
        playerInRange = distance <= inspectionRange;

        UpdatePrompt();

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

                promptText.text = ">>> PRESS E TO INSPECT <<<";
                promptText.fontSize = 36; // Big and obvious
                promptText.color = Color.yellow; // Bright yellow

                if (promptText.outlineWidth == 0)
                {
                    promptText.outlineWidth = 0.2f;
                    promptText.outlineColor = Color.black;
                }
            }
        }
    }

    void Inspect()
    {
        Debug.Log($"[InspectableObject] {gameObject.name} inspected!");
        
        hasBeenInspected = true;

        if (promptUI != null) promptUI.SetActive(false);

        if (inspectionPanel != null && inspectionText != null)
        {
            inspectionPanel.SetActive(true);
            inspectionText.text = inspectionResult;

            Invoke(nameof(HideInspectionPanel), inspectionDisplayTime);
        }

        if (audioSource != null && inspectionSound != null)
        {
            audioSource.PlayOneShot(inspectionSound);
        }

        onInspected?.Invoke();

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
