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
    public float initialMapRange = 750f; // Increased to cover a larger area by default 
    
    [Header("Manual Override")]
    public bool useManualBounds = true;
    public Vector2 manualCenter = new Vector2(-181.4f, 96.7f); // Player start position (X, Z)
    public Vector2 manualSize = new Vector2(1500f, 1500f); // Increased from 500 to make the arrow move slower
    
    [Header("Calibration Settings")]
    public Vector2 uiOffset = new Vector2(0f, 0f); // The offset to apply to the arrow's UI position
    public Vector3 playerStartWorldPos = new Vector3(-181.4f, -51.2f, 96.7f);
    public Vector2 playerStartUIPos = new Vector2(-82f, 75f);
    public bool autoCalibrateOnStart = true; 
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

        // Set the world size of the map
        mapWorldSize = useManualBounds ? manualSize : new Vector2(initialMapRange * 2f, initialMapRange * 2f);

        // Calculate the map scale
        mapScale = mapWorldSize.x / minimapSize;

        // Auto-calibrate if enabled
        if (autoCalibrateOnStart)
        {
            CalibrateForPlayerStart();
        }
        else
        {
            // Default behavior: center the map on the player's starting world position
            mapCenter = new Vector2(player.position.x, player.position.z);
            uiOffset = Vector2.zero;
        }

        Debug.Log($"Minimap Initialized - Center: {mapCenter}, UI Offset: {uiOffset}, Scale: {mapScale:F2}");
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
        // Calculate player's position relative to the map's world center
        float worldX = player.position.x - mapCenter.x;
        float worldZ = player.position.z - mapCenter.y;
        
        // Scale the world position to the minimap's UI coordinates
        float minimapX = worldX / mapScale;
        float minimapZ = worldZ / mapScale;
        
        // Apply the UI offset to correctly position the arrow
        Vector2 finalPosition = new Vector2(minimapX, minimapZ) + uiOffset;

        if (clampArrowToEdge)
        {
            float maxOffset = (minimapSize * 0.5f) - (arrowSize * 0.5f);
            finalPosition.x = Mathf.Clamp(finalPosition.x, -maxOffset, maxOffset);
            finalPosition.y = Mathf.Clamp(finalPosition.y, -maxOffset, maxOffset);
        }
        
        arrow.anchoredPosition = finalPosition;
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
    
    [ContextMenu("4. Calibrate for Player Start Position")]
    public void CalibrateForPlayerStart()
    {
        // Center the map on the player's starting world position.
        mapCenter = new Vector2(playerStartWorldPos.x, playerStartWorldPos.z);

        // The UI offset is simply the desired starting UI position on the minimap.
        uiOffset = playerStartUIPos;

        Debug.Log($"=== CALIBRATION COMPLETE ===");
        Debug.Log($"Map Center set to Player Start: ({mapCenter.x:F1}, {mapCenter.y:F1})");
        Debug.Log($"UI Offset set to: ({uiOffset.x:F1}, {uiOffset.y:F1})");

        // Test the calculation for verification
        float testUIX = (playerStartWorldPos.x - mapCenter.x) / mapScale + uiOffset.x;
        float testUIZ = (playerStartWorldPos.z - mapCenter.y) / mapScale + uiOffset.y;
        Debug.Log($"Verification: Player at start should be at UI ({testUIX:F1}, {testUIZ:F1})");
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