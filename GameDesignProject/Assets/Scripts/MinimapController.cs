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
    public Vector2 manualCenter = new Vector2(-181.4f, 96.7f); // Player start position (X, Z)
    public Vector2 manualSize = new Vector2(500f, 500f);
    
    [Header("Calibration Settings")]
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

        if (useManualBounds)
        {
            mapCenter = manualCenter;
            mapWorldSize = manualSize;
            
            // Auto-calibrate if enabled
            if (autoCalibrateOnStart)
            {
                CalibrateForPlayerStart();
            }
            
            Debug.Log($"Using manual bounds - Center: {mapCenter}, Size: {mapWorldSize}");
        }
        else if (usePlayerStartAsCenter)
        {
            mapCenter = new Vector2(player.position.x, player.position.z);
            mapWorldSize = new Vector2(initialMapRange * 2f, initialMapRange * 2f);
            Debug.Log($"Minimap centered on player start: {mapCenter}, Size: {mapWorldSize}");
        }

        // Calculate map scale based on minimap size and world size
        mapScale = mapWorldSize.x / minimapSize;
        
        Debug.Log($"Map scale calculated: {mapScale:F2} (covers {mapWorldSize.x}x{mapWorldSize.y} world units)");
        
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
    
    [ContextMenu("4. Calibrate for Player Start Position")]
    public void CalibrateForPlayerStart()
    {
        // Set the map center to the player start position
        manualCenter = new Vector2(playerStartWorldPos.x, playerStartWorldPos.z);
        
        // Calculate the map scale based on the desired UI position
        // We want player at (-181.4, 96.7) to show at (-82, 75) on the minimap
        
        // Calculate how far the player is from the center (should be 0,0)
        float distanceFromCenterX = playerStartWorldPos.x - manualCenter.x; // Should be 0
        float distanceFromCenterZ = playerStartWorldPos.z - manualCenter.y; // Should be 0
        
        // The UI position should be (0, 0) when player is at center
        // But we want it to be at (-82, 75), so we need to adjust the center
        // Let's calculate what the center should be to achieve this
        
        // If player at (-181.4, 96.7) should show at (-82, 75)
        // Then: UI_X = (World_X - Center_X) / Scale
        // -82 = (-181.4 - Center_X) / Scale
        // -82 * Scale = -181.4 - Center_X
        // Center_X = -181.4 + 82 * Scale
        
        // For now, let's use a simple approach: adjust the center to account for the offset
        float uiOffsetX = playerStartUIPos.x;
        float uiOffsetZ = playerStartUIPos.y;
        
        // Calculate what the center should be to get the desired UI position
        // This is a simplified calculation - you may need to fine-tune
        manualCenter = new Vector2(
            playerStartWorldPos.x - uiOffsetX * (mapWorldSize.x / minimapSize),
            playerStartWorldPos.z - uiOffsetZ * (mapWorldSize.y / minimapSize)
        );
        
        Debug.Log($"Calibrated center: {manualCenter}");
        Debug.Log($"Player start: {playerStartWorldPos}");
        Debug.Log($"Expected UI pos: {playerStartUIPos}");
        
        // Recalculate map scale
        mapScale = mapWorldSize.x / minimapSize;
        
        Debug.Log($"Calibration complete! Map center: {manualCenter}, Scale: {mapScale:F2}");
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