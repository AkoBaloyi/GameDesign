using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Minimap marker for objectives with pulsing animation
/// Attach to UI Image on minimap
/// </summary>
public class MinimapMarker : MonoBehaviour
{
    [Header("Target")]
    public Transform worldTarget; // The object in the world to track
    public Transform player; // Player transform
    
    [Header("Minimap Settings")]
    public RectTransform minimapRect; // The minimap UI element
    public float minimapScale = 1f; // Scale factor for minimap
    
    [Header("Visual Settings")]
    public Image markerImage;
    public Color markerColor = Color.green;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.5f;
    public float minAlpha = 0.5f;
    public float maxAlpha = 1f;
    
    [Header("Marker Type")]
    public MarkerType type = MarkerType.Objective;
    
    public enum MarkerType
    {
        PowerCell,  // Orange
        PowerBay,   // Blue
        Console,    // Green
        Objective   // Custom color
    }
    
    private RectTransform rectTransform;
    private Vector3 initialScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        if (markerImage == null)
        {
            markerImage = GetComponent<Image>();
        }
        
        if (rectTransform != null)
        {
            initialScale = rectTransform.localScale;
        }
        
        // Set color based on type
        SetMarkerColor();
        
        // Find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        UpdatePosition();
        UpdatePulse();
    }

    private void UpdatePosition()
    {
        if (worldTarget == null || player == null || minimapRect == null) return;
        
        // Calculate relative position
        Vector3 relativePos = worldTarget.position - player.position;
        
        // Convert to minimap coordinates (top-down view)
        Vector2 minimapPos = new Vector2(relativePos.x, relativePos.z) * minimapScale;
        
        // Clamp to minimap bounds
        float halfWidth = minimapRect.rect.width * 0.5f;
        float halfHeight = minimapRect.rect.height * 0.5f;
        minimapPos.x = Mathf.Clamp(minimapPos.x, -halfWidth, halfWidth);
        minimapPos.y = Mathf.Clamp(minimapPos.y, -halfHeight, halfHeight);
        
        // Update marker position
        rectTransform.anchoredPosition = minimapPos;
    }

    private void UpdatePulse()
    {
        if (markerImage == null) return;
        
        // Pulse alpha
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, pulse);
        
        Color color = markerImage.color;
        color.a = alpha;
        markerImage.color = color;
        
        // Pulse scale
        if (rectTransform != null)
        {
            float scale = 1f + (pulse * pulseAmount);
            rectTransform.localScale = initialScale * scale;
        }
    }

    private void SetMarkerColor()
    {
        if (markerImage == null) return;
        
        switch (type)
        {
            case MarkerType.PowerCell:
                markerColor = new Color(1f, 0.5f, 0f); // Orange
                break;
            case MarkerType.PowerBay:
                markerColor = new Color(0f, 0.5f, 1f); // Blue
                break;
            case MarkerType.Console:
                markerColor = new Color(0f, 1f, 0f); // Green
                break;
        }
        
        markerImage.color = markerColor;
    }

    /// <summary>
    /// Set the world target to track
    /// </summary>
    public void SetTarget(Transform target)
    {
        worldTarget = target;
    }

    /// <summary>
    /// Show or hide the marker
    /// </summary>
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    /// <summary>
    /// Change marker type and color
    /// </summary>
    public void SetMarkerType(MarkerType newType)
    {
        type = newType;
        SetMarkerColor();
    }
}
