using UnityEngine;
using UnityEngine.UI;




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

            if (arrowRectTransform != null && arrowRectTransform.gameObject.activeSelf)
            {
                arrowRectTransform.gameObject.SetActive(false);
            }
            return;
        }

        if (!arrowRectTransform.gameObject.activeSelf)
        {
            arrowRectTransform.gameObject.SetActive(true);
        }

        Vector3 directionToTarget = target.position - player.position;

        directionToTarget.y = 0;

        Vector3 playerForward = player.forward;
        playerForward.y = 0;



        float angle = Vector3.SignedAngle(playerForward, directionToTarget, Vector3.up);



        arrowRectTransform.localEulerAngles = new Vector3(0, 0, -angle);
    }




    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}