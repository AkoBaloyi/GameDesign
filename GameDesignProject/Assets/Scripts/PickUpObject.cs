// Fixed PickUpObject.cs that properly handles gravity and physics:

using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private Transform originalParent;
    private bool isBeingHeld = false;
    
    [Header("Pickup Settings")]
    public bool canBePickedUp = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        originalParent = transform.parent;
        
        // Make sure we have required components
        if (rb == null)
        {
            Debug.LogError($"PickUpObject on {gameObject.name} needs a Rigidbody!");
        }
        if (col == null)
        {
            Debug.LogError($"PickUpObject on {gameObject.name} needs a Collider!");
        }
    }

    public void PickUp(Transform holdPoint)
    {
        if (!canBePickedUp) return;
        
        Debug.Log($"Picking up {gameObject.name}");
        
        isBeingHeld = true;
        
        // Store physics state
        rb.useGravity = false;
        rb.isKinematic = true; // Make kinematic to prevent physics interference
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        // Disable collision while held
        col.isTrigger = true;

        // Parent to hold point
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        if (!isBeingHeld) return;
        
        Debug.Log($"Dropping {gameObject.name}");
        
        isBeingHeld = false;
        
        // Restore physics - THIS IS THE KEY FIX!
        transform.SetParent(originalParent);
        
        // Re-enable physics AFTER unparenting
        rb.isKinematic = false; // Allow physics again
        rb.useGravity = true;   // Enable gravity
        col.isTrigger = false;  // Re-enable collision
        
        // Clear any residual velocity
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Throw(Vector3 impulse)
    {
        if (!isBeingHeld) return;
        
        Debug.Log($"Throwing {gameObject.name}");
        
        isBeingHeld = false;
        
        // Restore physics
        transform.SetParent(originalParent);
        rb.isKinematic = false; // Allow physics
        rb.useGravity = true;   // Enable gravity
        col.isTrigger = false;  // Re-enable collision
        
        // Apply throw force
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        // Only move if we're being held and parented
        if (isBeingHeld && transform.parent != null)
        {
            transform.position = targetPosition;
        }
    }
    
    // Public getters
    public bool IsBeingHeld() { return isBeingHeld; }
}