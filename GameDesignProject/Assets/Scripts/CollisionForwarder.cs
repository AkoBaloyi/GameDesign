using UnityEngine;




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

            parentTransform.SendMessage("OnCollisionEnter", collision, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (parentTransform != null)
        {

            parentTransform.SendMessage("OnTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
        }
    }
}
