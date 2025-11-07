using UnityEngine;

/// <summary>
/// Floating arrow/marker that points to objective
/// Attach to Power Cell, Power Bay, Console
/// </summary>
public class ObjectiveWaypoint : MonoBehaviour
{
    [Header("Visual Settings")]
    public GameObject arrowPrefab; // Assign a 3D arrow model or sprite
    public Color waypointColor = Color.yellow;
    public float floatHeight = 2f; // Height above object
    public float bobSpeed = 1f;
    public float bobAmount = 0.3f;
    public float rotationSpeed = 50f;
    
    [Header("Visibility")]
    public bool showOnStart = true;
    public float minDistanceToShow = 5f; // Only show if player is far away
    public float maxDistanceToShow = 50f; // Hide if too far
    
    private GameObject arrowInstance;
    private Transform playerTransform;
    private Vector3 basePosition;
    private bool isVisible = true;

    void Start()
    {
        // Find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Create arrow above this object
        if (arrowPrefab != null)
        {
            basePosition = transform.position + Vector3.up * floatHeight;
            arrowInstance = Instantiate(arrowPrefab, basePosition, Quaternion.identity);
            arrowInstance.transform.SetParent(transform);
            
            // Set color
            Renderer renderer = arrowInstance.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = waypointColor;
                
                // Enable emission for glow
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", waypointColor * 2f);
            }
        }
        else
        {
            // Create simple arrow using primitives
            CreateSimpleArrow();
        }

        if (!showOnStart)
        {
            HideWaypoint();
        }
    }

    void Update()
    {
        if (arrowInstance == null || !isVisible) return;

        // Bob up and down
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        arrowInstance.transform.position = basePosition + Vector3.up * bobOffset;

        // Rotate
        arrowInstance.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Check distance to player
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            
            // Hide if too close or too far
            bool shouldShow = distance >= minDistanceToShow && distance <= maxDistanceToShow;
            arrowInstance.SetActive(shouldShow);
        }
    }

    void CreateSimpleArrow()
    {
        // Create a simple arrow using Unity primitives
        basePosition = transform.position + Vector3.up * floatHeight;
        arrowInstance = new GameObject("Waypoint Arrow");
        arrowInstance.transform.position = basePosition;
        arrowInstance.transform.SetParent(transform);

        // Create cone pointing down
        GameObject cone = GameObject.CreatePrimitive(PrimitiveType.Cone);
        cone.transform.SetParent(arrowInstance.transform);
        cone.transform.localPosition = Vector3.zero;
        cone.transform.localRotation = Quaternion.Euler(180, 0, 0);
        cone.transform.localScale = new Vector3(0.5f, 1f, 0.5f);

        // Set color
        Renderer renderer = cone.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = waypointColor;
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", waypointColor * 2f);
            renderer.material = mat;
        }

        // Remove collider
        Destroy(cone.GetComponent<Collider>());

        // Add light
        Light light = arrowInstance.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = waypointColor;
        light.range = 10f;
        light.intensity = 2f;
    }

    public void ShowWaypoint()
    {
        isVisible = true;
        if (arrowInstance != null)
        {
            arrowInstance.SetActive(true);
        }
    }

    public void HideWaypoint()
    {
        isVisible = false;
        if (arrowInstance != null)
        {
            arrowInstance.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (arrowInstance != null)
        {
            Destroy(arrowInstance);
        }
    }
}
