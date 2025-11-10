using System.Collections;
using UnityEngine;
using UnityEngine.Events;




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

        if (doorTransform == null)
        {

            doorTransform = transform.Find("Door");
            
            if (doorTransform == null)
            {
                Debug.LogError($"[AutomaticDoor] No door transform assigned on {gameObject.name}!");
                enabled = false;
                return;
            }
        }

        closePos = doorTransform.localPosition;
        
        Debug.Log($"[AutomaticDoor] Initialized on {gameObject.name}");
        Debug.Log($"  - Close Position: {closePos}");
        Debug.Log($"  - Open Position: {openPos}");
        Debug.Log($"  - Detection Distance: {detectionDistance}");
    }

    void FixedUpdate()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionDistance, triggerLayerMask);

        bool previousState = isOpen;

        isOpen = colliders.Length > 0;

        if (isOpen != previousState)
        {
            Debug.Log($"[AutomaticDoor] State changed: {(isOpen ? "OPENING" : "CLOSING")} door {gameObject.name}");

            onDoorTriggered?.Invoke(isOpen);

            ToggleDoor(isOpen);
        }
    }



    public void ToggleDoor(bool open)
    {

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(AnimateDoor(open));
    }



    private IEnumerator AnimateDoor(bool open)
    {
        Vector3 startPos = doorTransform.localPosition;
        Vector3 targetPos = open ? openPos : closePos;
        
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;

            float curveValue = movementCurve.Evaluate(t);

            doorTransform.localPosition = Vector3.Lerp(startPos, targetPos, curveValue);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        doorTransform.localPosition = targetPos;
        
        Debug.Log($"[AutomaticDoor] Animation complete: {(open ? "OPEN" : "CLOSED")}");
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = isOpen ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);

        if (doorTransform != null)
        {
            Gizmos.color = Color.cyan;
            Vector3 worldClosePos = transform.TransformPoint(closePos);
            Vector3 worldOpenPos = transform.TransformPoint(openPos);
            Gizmos.DrawLine(worldClosePos, worldOpenPos);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(worldClosePos, 0.2f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(worldOpenPos, 0.2f);
        }
    }



    [ContextMenu("Test Open Door")]
    public void TestOpen()
    {
        ToggleDoor(true);
    }



    [ContextMenu("Test Close Door")]
    public void TestClose()
    {
        ToggleDoor(false);
    }
}
