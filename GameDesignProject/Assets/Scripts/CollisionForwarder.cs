using UnityEngine;

/// <summary>
/// Forwards collision events from child to parent
/// Attach to child object with collider
/// </summary>
public class CollisionForwarder : MonoBehaviour
{
    private Transform parentTransform;

    void Start()
    {
        parentTransform = transform.parent;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (parentTransform != null)
        {
            // Forward to parent
            parentTransform.SendMessage("OnCollisionEnter", collision, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (parentTransform != null)
        {
            // Forward to parent
            parentTransform.SendMessage("OnTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
        }
    }
}
