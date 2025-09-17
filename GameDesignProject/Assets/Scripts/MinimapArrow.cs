using UnityEngine;

public class MinimapArrow : MonoBehaviour
{
    [Header("References")]
    public Transform player;         
    public RectTransform arrow;      
    public RectTransform minimapBG;  

    [Header("Map Settings")]
    public Vector2 mapWorldSize = new Vector2(50, 50); 
    public Vector2 mapCenter = Vector2.zero;          
    
    [Header("Clamping")]
    public bool clampToMinimapBounds = true;          
    public float edgePadding = 5f;                    
    
    [Header("Debug")]
    public bool showDebugInfo = false;

    void Update()
    {
        if (player == null || arrow == null || minimapBG == null) return;

        UpdateArrowRotation();
        UpdateArrowPosition();
        
        if (showDebugInfo) LogDebugInfo();
    }

    void UpdateArrowRotation()
    {
        float yRot = player.eulerAngles.y;
        arrow.localEulerAngles = new Vector3(0, 0, -yRot);
    }

    void UpdateArrowPosition()
    {
        Vector3 worldPos = player.position;
        
        Vector2 relativePos = new Vector2(
            worldPos.x - mapCenter.x,
            worldPos.z - mapCenter.y
        );

        Vector2 normalizedPos = new Vector2(
            Mathf.InverseLerp(-mapWorldSize.x/2, mapWorldSize.x/2, relativePos.x),
            Mathf.InverseLerp(-mapWorldSize.y/2, mapWorldSize.y/2, relativePos.y)
        );

        Rect minimapRect = minimapBG.rect;
        float mapWidth = minimapRect.width;
        float mapHeight = minimapRect.height;

        Vector2 minimapPos = new Vector2(
            (normalizedPos.x - 0.5f) * mapWidth,
            (normalizedPos.y - 0.5f) * mapHeight
        );

        if (clampToMinimapBounds)
        {
            float halfWidth = mapWidth / 2f - edgePadding;
            float halfHeight = mapHeight / 2f - edgePadding;
            
            minimapPos.x = Mathf.Clamp(minimapPos.x, -halfWidth, halfWidth);
            minimapPos.y = Mathf.Clamp(minimapPos.y, -halfHeight, halfHeight);
        }

        arrow.anchoredPosition = minimapPos;
    }

    void LogDebugInfo()
    {
        Vector3 worldPos = player.position;
        Debug.Log($"Player World Pos: {worldPos}, Arrow Pos: {arrow.anchoredPosition}, Map Size: {minimapBG.rect.size}");
    }

    [ContextMenu("Set Map Center to Player Position")]
    void SetMapCenterToPlayer()
    {
        if (player != null)
        {
            mapCenter = new Vector2(player.position.x, player.position.z);
            Debug.Log($"Map center set to: {mapCenter}");
        }
    }

    void OnValidate()
    {
        if (mapWorldSize.x <= 0) mapWorldSize.x = 50;
        if (mapWorldSize.y <= 0) mapWorldSize.y = 50;
        if (edgePadding < 0) edgePadding = 0;
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        
        Vector3 center = new Vector3(mapCenter.x, 0, mapCenter.y);
        Vector3 size = new Vector3(mapWorldSize.x, 0.1f, mapWorldSize.y);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, size);
        
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(player.position, 0.5f);
        }
    }
}