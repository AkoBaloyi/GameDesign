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
    public bool useManualBounds = true;
    public Vector2 manualCenter = new Vector2(0f, 0f); 
    public Vector2 manualSize = new Vector2(500f, 500f); 
    [Header("Debug")]
    public bool showDebugInfo = false; 
    public bool showDebugGizmos = false; 
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

        mapScale = 2f;
        
        mapWorldSize = new Vector2(minimapSize * mapScale, minimapSize * mapScale);
        
        Debug.Log($"Map scale LOCKED to: {mapScale} (covers {mapWorldSize.x}x{mapWorldSize.y} world units)");
        
        initialized = true;
    }

    private void Update()
    {
        if (!initialized || player == null || arrow == null) 
        {
            if (!initialized)
            {
                InitializeMinimap();
            }
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

    [ContextMenu("DEBUG: Show Current Values")]
    public void ShowCurrentValues()
    {
        if (player != null && arrow != null)
        {
            Vector3 playerPos = player.position;
            Vector2 arrowPos = arrow.anchoredPosition;
            
            Debug.Log("=== MINIMAP DEBUG ===");
            Debug.Log($"Player World Position: ({playerPos.x:F1}, {playerPos.z:F1})");
            Debug.Log($"Map Center: ({mapCenter.x:F1}, {mapCenter.y:F1})");
            Debug.Log($"Arrow Minimap Position: ({arrowPos.x:F1}, {arrowPos.y:F1})");
            Debug.Log($"Map Scale: {mapScale}");
            Debug.Log($"Distance from center: {Vector2.Distance(new Vector2(playerPos.x, playerPos.z), mapCenter):F1} world units");
            Debug.Log("Arrow should be at: " + ((new Vector2(playerPos.x, playerPos.z) - mapCenter) / mapScale));
        }
    }

    [ContextMenu("2. Auto-Size Map for Current Area")]
    public void AutoSizeForCurrentArea()
    {
        mapScale = 2f;
        
        mapWorldSize = new Vector2(minimapSize * mapScale, minimapSize * mapScale);
        manualSize = mapWorldSize;
        useManualBounds = true;
        
        Debug.Log($"Map scale LOCKED to: {mapScale}");
        Debug.Log($"This covers: {mapWorldSize.x}x{mapWorldSize.y} world units");
        Debug.Log($"Minimap will show {mapWorldSize.x/2f} units in each direction from center");
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
        if (!showDebugGizmos) return; 
        
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