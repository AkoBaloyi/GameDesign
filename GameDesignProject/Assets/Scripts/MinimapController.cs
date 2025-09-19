using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [Header("References")]
    public Transform player;                    
    public RectTransform arrow;                 
    public RectTransform minimapBackground;     

    [Header("World Settings")]
    public Vector2 worldSize = new Vector2(100f, 100f);    
    public Vector2 worldCenter = Vector2.zero;              

    [Header("Minimap Settings")]
    public float minimapSize = 250f;            
    public float arrowSize = 12f;               

    [Header("Debug")]
    public bool showDebugInfo = false;
    public bool clampArrowToEdge = true;        

    private void Update()
    {
        if (player == null || arrow == null || minimapBackground == null) 
        {
            Debug.LogWarning("MinimapController: Missing references!");
            return;
        }

        UpdateArrowPosition();
        UpdateArrowRotation();
        
        if (showDebugInfo)
        {
            DebugInfo();
        }
    }

    private void UpdateArrowPosition()
    {
        Vector3 playerWorldPos = player.position;
        
        float relativeX = playerWorldPos.x - worldCenter.x;
        float relativeZ = playerWorldPos.z - worldCenter.y;
        
        float normalizedX = relativeX / (worldSize.x * 0.5f);  
        float normalizedZ = relativeZ / (worldSize.y * 0.5f);
        
        float minimapX = normalizedX * (minimapSize * 0.5f);   
        float minimapY = normalizedZ * (minimapSize * 0.5f);
        
        if (clampArrowToEdge)
        {
            float maxOffset = (minimapSize * 0.5f) - (arrowSize * 0.5f);
            minimapX = Mathf.Clamp(minimapX, -maxOffset, maxOffset);
            minimapY = Mathf.Clamp(minimapY, -maxOffset, maxOffset);
        }
        
        arrow.anchoredPosition = new Vector2(minimapX, minimapY);
    }

    private void UpdateArrowRotation()
    {
        float playerYaw = player.eulerAngles.y;
        
        arrow.localEulerAngles = new Vector3(0f, 0f, -playerYaw);
    }

    private void DebugInfo()
    {
        Vector3 playerPos = player.position;
        Vector2 arrowPos = arrow.anchoredPosition;
        float playerRotation = player.eulerAngles.y;
        
        Debug.Log($"Player World: ({playerPos.x:F1}, {playerPos.z:F1}) | " +
                  $"Arrow Minimap: ({arrowPos.x:F1}, {arrowPos.y:F1}) | " +
                  $"Player Rotation: {playerRotation:F1}°");
    }

    [ContextMenu("Set World Center to Player")]
    public void SetWorldCenterToPlayer()
    {
        if (player != null)
        {
            worldCenter = new Vector2(player.position.x, player.position.z);
            Debug.Log($"World center set to player position: {worldCenter}");
        }
    }

    [ContextMenu("Auto Detect World Size")]
    public void AutoDetectWorldSize()
    {
        GameObject plane = GameObject.FindWithTag("Ground");
        if (plane == null)
        {
            plane = GameObject.Find("Plane");
        }
        
        if (plane != null)
        {
            Vector3 planeScale = plane.transform.localScale;
            worldSize = new Vector2(planeScale.x * 10f, planeScale.z * 10f); 
            Debug.Log($"Auto-detected world size: {worldSize}");
        }
        else
        {
            Debug.LogWarning("Could not find plane. Make sure it's tagged 'Ground' or named 'Plane'");
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        
        Vector3 worldCenterPos = new Vector3(worldCenter.x, 0f, worldCenter.y);
        Vector3 worldSizeVector = new Vector3(worldSize.x, 0.1f, worldSize.y);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(worldCenterPos, worldSizeVector);
        
        
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(player.position, 1f);
            
            
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(player.position, player.forward * 5f);
        }
        
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(worldCenterPos, 0.5f);
    }
}