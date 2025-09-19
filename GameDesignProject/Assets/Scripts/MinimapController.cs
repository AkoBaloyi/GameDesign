using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public RectTransform arrow;
    public RectTransform minimapBackground;

    [Header("Minimap Settings")]
    public float minimapSize = 250f;
    public float arrowSize = 12f;
    public float mapScale = 2f; 

    [Header("Dynamic Bounds")]
    public bool usePlayerStartAsCenter = true;
    public float initialMapRange = 50f; 
    
    [Header("Manual Override")]
    public bool useManualBounds = false;
    public Vector2 manualCenter = Vector2.zero;
    public Vector2 manualSize = new Vector2(100f, 100f);

    [Header("Debug")]
    public bool showDebugInfo = true;
    public bool clampArrowToEdge = true;

    private Vector2 mapCenter;
    private Vector2 mapWorldSize;
    private bool initialized = false;

    private void Start()
    {
        InitializeMinimap();
    }

    private void InitializeMinimap()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned to minimap!");
            return;
        }

        if (useManualBounds)
        {
            mapCenter = manualCenter;
            mapWorldSize = manualSize;
            Debug.Log($"Using manual bounds - Center: {mapCenter}, Size: {mapWorldSize}");
        }
        else if (usePlayerStartAsCenter)
        {
            mapCenter = new Vector2(player.position.x, player.position.z);
            mapWorldSize = new Vector2(initialMapRange * 2f, initialMapRange * 2f);
            Debug.Log($"Minimap centered on player start: {mapCenter}, Size: {mapWorldSize}");
        }

        mapScale = mapWorldSize.x / minimapSize;
        
        initialized = true;
    }

    private void Update()
    {
        if (!initialized || player == null || arrow == null) return;

        UpdateArrowPosition();
        UpdateArrowRotation();
        
        if (showDebugInfo)
        {
            DebugInfo();
        }
    }

    private void UpdateArrowPosition()
    {
        float worldX = player.position.x - mapCenter.x;
        float worldZ = player.position.z - mapCenter.y;
        
        float minimapX = worldX / mapScale;
        float minimapZ = worldZ / mapScale;
        
        if (clampArrowToEdge)
        {
            float maxOffset = (minimapSize * 0.5f) - (arrowSize * 0.5f);
            minimapX = Mathf.Clamp(minimapX, -maxOffset, maxOffset);
            minimapZ = Mathf.Clamp(minimapZ, -maxOffset, maxOffset);
        }
        
        arrow.anchoredPosition = new Vector2(minimapX, minimapZ);
    }

    private void UpdateArrowRotation()
    {
        float playerYaw = player.eulerAngles.y;
        arrow.localEulerAngles = new Vector3(0f, 0f, -playerYaw);
    }

    [ContextMenu("1. Set Map Center to Current Player Position")]
    public void SetMapCenterToCurrentPlayerPosition()
    {
        if (player != null)
        {
            mapCenter = new Vector2(player.position.x, player.position.z);
            useManualBounds = true;
            manualCenter = mapCenter;
            Debug.Log($"Map center set to current player position: {mapCenter}");
        }
    }

    [ContextMenu("2. Auto-Size Map for Current Area")]
    public void AutoSizeForCurrentArea()
    {
        mapWorldSize = new Vector2(200f, 200f); 
        manualSize = mapWorldSize;
        mapScale = mapWorldSize.x / minimapSize;
        useManualBounds = true;
        
        Debug.Log($"Map sized for building area: {mapWorldSize} world units");
        Debug.Log($"Map scale: 1 minimap pixel = {mapScale:F2} world units");
    }

    [ContextMenu("3. Reset to Player Start Position")]
    public void ResetToPlayerStart()
    {
        usePlayerStartAsCenter = true;
        useManualBounds = false;
        initialized = false;
        Start();
    }

    private void DebugInfo()
    {
        if (player == null || arrow == null) return;
        
        Vector3 playerPos = player.position;
        Vector2 arrowPos = arrow.anchoredPosition;
        float playerRotation = player.eulerAngles.y;
        
        float distanceFromCenter = Vector2.Distance(
            new Vector2(playerPos.x, playerPos.z), 
            mapCenter
        );
        
        Debug.Log($"Player: ({playerPos.x:F1}, {playerPos.z:F1}) | " +
                  $"Arrow: ({arrowPos.x:F1}, {arrowPos.y:F1}) | " +
                  $"Distance from center: {distanceFromCenter:F1} | " +
                  $"Map scale: {mapScale:F2}");
    }

    private void OnDrawGizmosSelected()
    {
        if (!initialized && Application.isPlaying) return;
        
        Vector2 centerToUse = useManualBounds ? manualCenter : mapCenter;
        Vector2 sizeToUse = useManualBounds ? manualSize : mapWorldSize;
        
        Vector3 center3D = new Vector3(centerToUse.x, 0f, centerToUse.y);
        Vector3 size3D = new Vector3(sizeToUse.x, 0.1f, sizeToUse.y);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(center3D, size3D);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(center3D, 2f);
        
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(player.position, 3f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(center3D, player.position);
        }
    }
}