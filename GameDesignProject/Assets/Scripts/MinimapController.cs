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
    public float mapScale = 2f; // LOCKED: How much world space per minimap pixel

    [Header("Dynamic Bounds")]
    public bool usePlayerStartAsCenter = true;
    public float initialMapRange = 50f; // Initial range around player start position
    
    [Header("Manual Override")]
    public bool useManualBounds = true;
    public Vector2 manualCenter = new Vector2(-181.4f, 96.7f); // Player's X and Z position
    public Vector2 manualSize = new Vector2(500f, 500f); // Locked scale 2 = 250*2 = 500

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
            // Use manual settings
            mapCenter = manualCenter;
            mapWorldSize = manualSize;
            Debug.Log($"Using manual bounds - Center: {mapCenter}, Size: {mapWorldSize}");
        }
        else if (usePlayerStartAsCenter)
        {
            // Use player's starting position as center
            mapCenter = new Vector2(player.position.x, player.position.z);
            mapWorldSize = new Vector2(initialMapRange * 2f, initialMapRange * 2f);
            Debug.Log($"Minimap centered on player start: {mapCenter}, Size: {mapWorldSize}");
        }

        // FORCE map scale to always be 2 (since that's accurate for your setup)
        mapScale = 2f;
        
        // Calculate what world size this scale represents
        mapWorldSize = new Vector2(minimapSize * mapScale, minimapSize * mapScale);
        
        Debug.Log($"Map scale LOCKED to: {mapScale} (covers {mapWorldSize.x}x{mapWorldSize.y} world units)");
        
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
        // Get player position relative to map center
        float worldX = player.position.x - mapCenter.x;
        float worldZ = player.position.z - mapCenter.y;
        
        // Convert to minimap coordinates
        float minimapX = worldX / mapScale;
        float minimapZ = worldZ / mapScale;
        
        // Clamp to minimap bounds
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

    // Context menu options for easy setup
    [ContextMenu("QUICK SETUP: Set Map Center to World Center (Not Player)")]
    public void SetMapCenterToWorldCenter()
    {
        // We need to find the CENTER of your actual game world
        // Since player starts at (-181.4, 96.7) and is at the edge,
        // we need to determine where the center of your world actually is
        
        // For now, let's use a reasonable guess - you can adjust these values
        mapCenter = new Vector2(0f, 0f); // Try world center at origin first
        mapScale = 2f; // Keep locked to 2 for accuracy
        mapWorldSize = new Vector2(500f, 500f); // Covers 500x500 world units
        
        // Update manual bounds
        useManualBounds = true;
        manualCenter = mapCenter;
        manualSize = mapWorldSize;
        
        Debug.Log($"Map center set to world center: {mapCenter}");
        Debug.Log($"If player appears at wrong position, adjust Manual Center in inspector");
        Debug.Log($"Player at (-181.4, 96.7) should appear at minimap position: ({(-181.4f - mapCenter.x) / mapScale:F1}, {(96.7f - mapCenter.y) / mapScale:F1})");
        
        initialized = true;
    }

    [ContextMenu("2. Auto-Size Map for Current Area")]
    public void AutoSizeForCurrentArea()
    {
        // ALWAYS use scale of 2 (since that's accurate for your setup)
        mapScale = 2f;
        
        // Calculate world size based on locked scale
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
        
        // Calculate distance from map center
        float distanceFromCenter = Vector2.Distance(
            new Vector2(playerPos.x, playerPos.z), 
            mapCenter
        );
        
        Debug.Log($"Player: ({playerPos.x:F1}, {playerPos.z:F1}) | " +
                  $"Arrow: ({arrowPos.x:F1}, {arrowPos.y:F1}) | " +
                  $"Distance from center: {distanceFromCenter:F1} | " +
                  $"Map scale: {mapScale:F2}");
    }

    // Gizmos to visualize the minimap area in scene view
    private void OnDrawGizmosSelected()
    {
        if (!initialized && Application.isPlaying) return;
        
        Vector2 centerToUse = useManualBounds ? manualCenter : mapCenter;
        Vector2 sizeToUse = useManualBounds ? manualSize : mapWorldSize;
        
        // Draw map bounds
        Vector3 center3D = new Vector3(centerToUse.x, 0f, centerToUse.y);
        Vector3 size3D = new Vector3(sizeToUse.x, 0.1f, sizeToUse.y);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(center3D, size3D);
        
        // Draw map center
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(center3D, 2f);
        
        // Draw player
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(player.position, 3f);
            
            // Draw line from center to player
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(center3D, player.position);
        }
    }
}