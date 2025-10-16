using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SecurityDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public string requiredKeycardID = "DOOR_01"; 
    public Animator doorAnimator;
    public string animatorBoolName = "Open";

    [Header("UI Settings")]
    public GameObject interactionPromptPrefab;
    public float uiHeightOffset = 2f;
    public float fadeDuration = 0.3f;
    public string lockedMessage = "KEYCARD REQUIRED";
    public string openMessage = "PRESS [F] TO OPEN";

    private GameObject promptInstance;
    private TextMeshProUGUI interactionText;
    private bool playerInRange;
    private PlayerInventory playerInventory;

    private void Start()
    {
        
        promptInstance = Instantiate(interactionPromptPrefab, 
            transform.position + Vector3.up * uiHeightOffset, 
            Quaternion.identity);
        promptInstance.transform.SetParent(transform);
        interactionText = promptInstance.GetComponentInChildren<TextMeshProUGUI>();
        interactionText.alpha = 0; 
    }

    private void Update()
    {
        if (promptInstance != null)
        {
            
            promptInstance.transform.LookAt(Camera.main.transform);
            promptInstance.transform.Rotate(0, 180, 0); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();
            UpdatePromptText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionText.alpha = 0;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInRange && context.performed)
        {
            if (playerInventory.HasKeycard(requiredKeycardID))
            {
                ToggleDoor();
            }
            else
            {
                interactionText.text = lockedMessage;
               
            }
        }
    }

    private void ToggleDoor()
    {
        bool isOpen = !doorAnimator.GetBool(animatorBoolName);
        doorAnimator.SetBool(animatorBoolName, isOpen);
        interactionText.alpha = 0;
    }

    private void UpdatePromptText()
    {
        if (playerInventory.HasKeycard(requiredKeycardID))
            interactionText.text = openMessage;
        else
            interactionText.text = lockedMessage;
        
        interactionText.alpha = 1; 
    }
}