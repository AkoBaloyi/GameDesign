using UnityEngine;
using UnityEngine.InputSystem;



public class DoorDebugHelper : MonoBehaviour
{
    [Header("Debug Settings")]
    public bool showDebugLogs = true;
    public bool showGizmos = true;
    public Color gizmoColor = Color.green;

    private DoorInteractor doorInteractor;
    private Collider triggerCollider;
    private Animator doorAnimator;

    void Start()
    {

        doorInteractor = GetComponent<DoorInteractor>();
        triggerCollider = GetComponent<Collider>();
        doorAnimator = GetComponentInChildren<Animator>();

        if (showDebugLogs)
        {
            Debug.Log("=== DOOR DEBUG INFO ===");
            Debug.Log($"Door GameObject: {gameObject.name}");
            Debug.Log($"DoorInteractor found: {doorInteractor != null}");
            Debug.Log($"Trigger Collider found: {triggerCollider != null}");
            if (triggerCollider != null)
            {
                Debug.Log($"  - Is Trigger: {triggerCollider.isTrigger}");
                Debug.Log($"  - Collider Type: {triggerCollider.GetType().Name}");
            }
            Debug.Log($"Animator found: {doorAnimator != null}");
            if (doorAnimator != null)
            {
                Debug.Log($"  - Animator on: {doorAnimator.gameObject.name}");
                Debug.Log($"  - Has 'Open' parameter: {HasParameter(doorAnimator, "Open")}");
            }

            Debug.Log($"Children count: {transform.childCount}");
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Debug.Log($"  - Child {i}: {child.name}");
                if (child.GetComponent<Animator>())
                    Debug.Log($"    → Has Animator");
                if (child.GetComponent<Collider>())
                    Debug.Log($"    → Has Collider");
            }
        }
    }

    void Update()
    {

        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (showDebugLogs)
            {
                Debug.Log($"[DoorDebug] F key pressed! Checking door state...");
                if (doorInteractor != null)
                {
                    Debug.Log($"  - Player in range: {doorInteractor.isOpen}");
                    Debug.Log($"  - Door is open: {doorInteractor.isOpen}");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (showDebugLogs)
        {
            Debug.Log($"[DoorDebug] Trigger entered by: {other.gameObject.name} (Tag: {other.tag})");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (showDebugLogs)
        {
            Debug.Log($"[DoorDebug] Trigger exited by: {other.gameObject.name}");
        }
    }

    bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Collider col = GetComponent<Collider>();
        if (col != null && col.isTrigger)
        {
            Gizmos.color = gizmoColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            if (col is BoxCollider box)
            {
                Gizmos.DrawWireCube(box.center, box.size);
            }
            else if (col is SphereCollider sphere)
            {
                Gizmos.DrawWireSphere(sphere.center, sphere.radius);
            }
        }
    }
}
