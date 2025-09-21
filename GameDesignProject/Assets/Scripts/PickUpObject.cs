using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform holdPoint)
    {
        // Make the rigidbody kinematic to disable physics and allow manual control
        rb.isKinematic = true;
        rb.useGravity = false; // Also disable gravity explicitly

        // Reset velocities to prevent weird spinning on pickup
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Parent the object to the hold point and snap it into position
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity; // Reset rotation as well
    }

    public void Drop()
    {
        // Unparent the object
        transform.SetParent(null);

        // Make the rigidbody dynamic again so it's affected by physics
        rb.isKinematic = false;
        rb.useGravity = true;
        
        // Reset velocities to prevent weird behavior
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        // Add a small downward force to help it fall naturally
        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
    }

    public void Throw(Vector3 impulse)
    {
        // Unparent the object
        transform.SetParent(null);

        // Make the rigidbody dynamic so it can be thrown
        rb.isKinematic = false;
        rb.useGravity = true;

        // Reset velocities before applying new force
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(impulse, ForceMode.Impulse);
    }

    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        // When kinematic, we can just move the transform directly.
        // This is smoother than using MovePosition for held objects.
        if (rb.isKinematic)
        {
            transform.position = targetPosition;
        }
    }
}
