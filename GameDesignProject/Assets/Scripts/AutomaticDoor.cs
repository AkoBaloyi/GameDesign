using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Automatic door that opens when player is nearby
/// No input required - just walk close to it!
/// </summary>
public class AutomaticDoor : MonoBehaviour
{
    [Header("Door Transform")]
    [Tooltip("The door object that will move (usually a child object)")]
    public Transform doorTransform;

    [Header("Door Positions")]
    [Tooltip("Local position when door is closed")]
    public Vector3 closePos = Vector3.zero;
    
    [Tooltip("Local position when door is open (e.g., Vector3.up * 2.5 for sliding up)")]
    public Vector3 openPos = new Vector3(0, 2.5f, 0);

    [Header("Detection Settings")]
    [Tooltip("How far away the player can be to trigger the door")]
    public float detectionDistance = 3f;
    
    [Tooltip("Layer mask for what can trigger the door (set to 'Player' layer)")]
    public LayerMask triggerLayerMask = ~0;

    [Header("Animation Settings")]
    [Tooltip("How long it takes for the door to open/close")]
    public float animationDuration = 1f;
    
    [Tooltip("Animation curve for smooth movement")]
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("State")]
    [Tooltip("Is the door currently open?")]
    public bool isOpen = false;

    [Header("Events")]
    public UnityEvent<bool> onDoorTriggered;

    private Coroutine animationCoroutine;
    private bool wasOpen = false;

    void Start()
    {
        // If door transform not assigned, try to find it
        if (doorTransform == null)
        {
            // Look for a child named "Door"
            doorTransform = transform.Find("Door");
            
            if (doorTransform == null)
            {
                Debug.LogError($"[AutomaticDoor] No door transform assigned on {gameObject.name}!");
                enabled = false;
                return;
            }
        }

        // Store the initial closed position
        closePos = doorTransform.localPosition;
        
        Debug.Log($"[AutomaticDoor] Initialized on {gameObject.name}");
        Debug.Log($"  - Close Position: {closePos}");
        Debug.Log($"  - Open Position: {openPos}");
        Debug.Log($"  - Detection Distance: {detectionDistance}");
    }

    void FixedUpdate()
    {
        // Check for objects in range using sphere overlap
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionDistance, triggerLayerMask);
        
        // Store previous state
        bool previousState = isOpen;
        
        // Door should be open if any colliders are detected
        isOpen = colliders.Length > 0;
        
        // If state changed, trigger the door animation
        if (isOpen != previousState)
        {
            Debug.Log($"[AutomaticDoor] State changed: {(isOpen ? "OPENING" : "CLOSING")} door {gameObject.name}");
            
            // Invoke event
            onDoorTriggered?.Invoke(isOpen);
            
            // Start door animation
            ToggleDoor(isOpen);
        }
    }

    /// <summary>
    /// Toggles the door open or closed with smooth animation
    /// </summary>
    public void ToggleDoor(bool open)
    {
        // Stop any existing animation
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        // Start new animation
        animationCoroutine = StartCoroutine(AnimateDoor(open));
    }

    /// <summary>
    /// Coroutine that smoothly moves the door between open and closed positions
    /// </summary>
    private IEnumerator AnimateDoor(bool open)
    {
        Vector3 startPos = doorTransform.localPosition;
        Vector3 targetPos = open ? openPos : closePos;
        
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            
            // Apply animation curve for smooth movement
            float curveValue = movementCurve.Evaluate(t);
            
            // Lerp between start and target position
            doorTransform.localPosition = Vector3.Lerp(startPos, targetPos, curveValue);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure we end exactly at target position
        doorTransform.localPosition = targetPos;
        
        Debug.Log($"[AutomaticDoor] Animation complete: {(open ? "OPEN" : "CLOSED")}");
    }

    /// <summary>
    /// Visualize the detection range in the Scene view
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isOpen ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
        
        // Draw a line showing the door movement
        if (doorTransform != null)
        {
            Gizmos.color = Color.cyan;
            Vector3 worldClosePos = transform.TransformPoint(closePos);
            Vector3 worldOpenPos = transform.TransformPoint(openPos);
            Gizmos.DrawLine(worldClosePos, worldOpenPos);
            
            // Draw small spheres at open/close positions
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(worldClosePos, 0.2f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(worldOpenPos, 0.2f);
        }
    }

    /// <summary>
    /// Manual trigger for testing in Inspector
    /// </summary>
    [ContextMenu("Test Open Door")]
    public void TestOpen()
    {
        ToggleDoor(true);
    }

    /// <summary>
    /// Manual trigger for testing in Inspector
    /// </summary>
    [ContextMenu("Test Close Door")]
    public void TestClose()
    {
        ToggleDoor(false);
    }
}
