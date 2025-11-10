using UnityEngine;




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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        if (arrowPrefab != null)
        {
            basePosition = transform.position + Vector3.up * floatHeight;
            arrowInstance = Instantiate(arrowPrefab, basePosition, Quaternion.identity);
            arrowInstance.transform.SetParent(transform);

            Renderer renderer = arrowInstance.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = waypointColor;

                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", waypointColor * 2f);
            }
        }
        else
        {

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

        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        arrowInstance.transform.position = basePosition + Vector3.up * bobOffset;

        arrowInstance.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            bool shouldShow = distance >= minDistanceToShow && distance <= maxDistanceToShow;
            arrowInstance.SetActive(shouldShow);
        }
    }

    void CreateSimpleArrow()
    {

        basePosition = transform.position + Vector3.up * floatHeight;
        arrowInstance = new GameObject("Waypoint Arrow");
        arrowInstance.transform.position = basePosition;
        arrowInstance.transform.SetParent(transform);


        GameObject shaft = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        shaft.transform.SetParent(arrowInstance.transform);
        shaft.transform.localPosition = new Vector3(0, -1f, 0); // Center of shaft below arrow base
        shaft.transform.localScale = new Vector3(0.3f, 1f, 0.3f); // Taller shaft

        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
        head.transform.SetParent(arrowInstance.transform);
        head.transform.localPosition = new Vector3(0, -2.2f, 0); // Well below shaft
        head.transform.localScale = new Vector3(0.8f, 0.4f, 0.8f); // Wider, flatter
        head.transform.localRotation = Quaternion.Euler(45, 45, 0); // Diamond pointing down

        Renderer shaftRenderer = shaft.GetComponent<Renderer>();
        Renderer headRenderer = head.GetComponent<Renderer>();
        
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = waypointColor;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", waypointColor * 2f);
        
        if (shaftRenderer != null) shaftRenderer.material = mat;
        if (headRenderer != null) headRenderer.material = mat;

        Destroy(shaft.GetComponent<Collider>());
        Destroy(head.GetComponent<Collider>());

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
