using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script controls a UI arrow on a minimap to point towards a specific target.
/// It should be attached to the UI Image object of the arrow.
/// </summary>
public class MinimapArrow : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The target object the arrow should point to (e.g., a quest objective, exit, etc.).")]
    public Transform target;

    [Tooltip("The player's transform. The arrow's direction is relative to this.")]
    public Transform player;

    [Tooltip("The RectTransform of the arrow's UI Image.")]
    public RectTransform arrowRectTransform;

    void LateUpdate()
    {
        if (target == null || player == null || arrowRectTransform == null)
        {
            // Disable the arrow if references are not set
            if (arrowRectTransform != null && arrowRectTransform.gameObject.activeSelf)
            {
                arrowRectTransform.gameObject.SetActive(false);
            }
            return;
        }

        // Ensure the arrow is active if references are set
        if (!arrowRectTransform.gameObject.activeSelf)
        {
            arrowRectTransform.gameObject.SetActive(true);
        }

        // 1. Get the direction vector from the player to the target
        Vector3 directionToTarget = target.position - player.position;

        // 2. We only care about the horizontal direction (X and Z axes)
        directionToTarget.y = 0;

        // 3. Get the player's forward direction, also only on the horizontal plane
        Vector3 playerForward = player.forward;
        playerForward.y = 0;

        // 4. Calculate the angle between the player's forward direction and the direction to the target
        // The result is in degrees, and SignedAngle gives a positive or negative value
        // depending on whether the target is to the left or right.
        float angle = Vector3.SignedAngle(playerForward, directionToTarget, Vector3.up);

        // 5. Apply the rotation to the arrow's RectTransform.
        // We rotate around the Z-axis for UI elements. The angle is negated because UI rotation
        // is typically clockwise for positive values.
        arrowRectTransform.localEulerAngles = new Vector3(0, 0, -angle);
    }

    /// <summary>
    /// Public method to change the target the arrow points to at runtime.
    /// </summary>
    /// <param name="newTarget">The new transform to point towards.</param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}