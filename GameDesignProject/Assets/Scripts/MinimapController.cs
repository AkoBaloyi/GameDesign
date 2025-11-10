using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public RectTransform arrow;
    public RectTransform minimapBackground;
    
    [Header("Multiple Maps Support")]
    public RectTransform[] allArrows; // All player arrows on different maps
    public GameObject[] allMaps; // All map panels

    [Header("Minimap Settings")]
    public float minimapSize = 250f;
    public float arrowSize = 12f;
    public float mapScale = 2f; // This value now directly controls the arrow speed. Higher value = slower arrow.

    [Header("Dynamic Bounds")]
    public bool usePlayerStartAsCenter = true;
    public float initialMapRange = 50f;

    [Header("Manual Override")]
    public bool useManualBounds = true;
    public Vector2 manualCenter = new Vector2(-181.4f, 96.7f); // Player start position (X, Z)
    public Vector2 manualSize = new Vector2(5000f, 5000f); // Drastically increased to slow down arrow movement

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

        // The mapScale is now the primary setting for speed and is NOT calculated from world size.
        // We calculate mapWorldSize from it for debug purposes only.
        mapWorldSize = new Vector2(minimapSize * mapScale, minimapSize * mapScale);

        // Auto-calibrate to set the correct starting position and offset
        if (autoCalibrateOnStart)
        {
            CalibrateForPlayerStart();
        }
        else
        {
            // Fallback if calibration is off: center on player, no offset.
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

        // Update main arrow
        if (arrow != null)
        {
            arrow.anchoredPosition = finalPosition;
        }
        
        // Update all arrows if multiple maps exist
        if (allArrows != null && allArrows.Length > 0)
        {
            foreach (var arr in allArrows)
            {
                if (arr != null && arr.gameObject.activeInHierarchy)
                {
                    arr.anchoredPosition = finalPosition;
                }
            }
        }
    }

    private void UpdateArrowRotation()
    {
        float playerYaw = player.eulerAngles.y;
        
        // Update main arrow
        if (arrow != null)
        {
            arrow.localEulerAngles = new Vector3(0f, 0f, -playerYaw);
        }
        
        // Update all arrows if multiple maps exist
        if (allArrows != null && allArrows.Length > 0)
        {
            foreach (var arr in allArrows)
            {
                if (arr != null && arr.gameObject.activeInHierarchy)
                {
                    arr.localEulerAngles = new Vector3(0f, 0f, -playerYaw);
                }
            }
        }
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
        // Center the map's reference point on the player's starting world position.
        mapCenter = new Vector2(playerStartWorldPos.x, playerStartWorldPos.z);

        // The UI offset is the desired starting UI position on the minimap.
        uiOffset = playerStartUIPos;

        Debug.Log($"=== CALIBRATION COMPLETE ===");
        Debug.Log($"Map Center set to Player Start: ({mapCenter.x:F1}, {mapCenter.y:F1})");
        Debug.Log($"UI Offset set to: ({uiOffset.x:F1}, {uiOffset.y:F1})");
        Debug.Log($"Using Map Scale: {mapScale}");

        // Verification calculation
        float testUIX = ((playerStartWorldPos.x - mapCenter.x) / mapScale) + uiOffset.x;
        float testUIZ = ((playerStartWorldPos.z - mapCenter.y) / mapScale) + uiOffset.y;
        Debug.Log($"Verification: Player at start should map to UI ({testUIX:F1}, {testUIZ:F1}). This should match your desired start UI position.");
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