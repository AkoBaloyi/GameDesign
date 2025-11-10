using UnityEngine;



public class MenuCameraController : MonoBehaviour
{
    [Header("Rotation")]
    public bool enableRotation = true;
    public float rotationSpeed = 3f; // Degrees per second
    public Vector3 rotationAxis = Vector3.up; // Rotate around Y axis
    
    [Header("Sway (Optional)")]
    public bool enableSway = false;
    public float swayAmount = 0.5f;
    public float swaySpeed = 1f;
    
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {

        if (enableRotation)
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }

        if (enableSway)
        {
            float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            float swayY = Mathf.Cos(Time.time * swaySpeed * 0.7f) * swayAmount * 0.5f;
            
            transform.position = startPosition + new Vector3(swayX, swayY, 0);
        }
    }
}
