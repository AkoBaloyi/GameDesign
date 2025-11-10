using UnityEngine;
using TMPro;

/// <summary>
/// Enhanced interaction prompt with smooth fade and glow effect
/// Attach to any interactable object
/// </summary>
public class InteractionPrompt : MonoBehaviour
{
    [Header("UI References")]
    public Canvas promptCanvas;
    public TextMeshProUGUI promptText;
    public CanvasGroup canvasGroup;
    
    [Header("Settings")]
    public string promptMessage = "Press F to Interact";
    public float fadeSpeed = 5f;
    public float hoverHeight = 1.5f;
    
    [Header("Visual Style")]
    public Color textColor = Color.white;
    public Color glowColor = new Color(0f, 1f, 1f); // Cyan
    public float glowIntensity = 1f;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.3f;
    
    private bool isVisible = false;
    private float targetAlpha = 0f;
    private Transform playerTransform;

    private void Awake()
    {
        // Setup canvas if not assigned
        if (promptCanvas == null)
        {
            promptCanvas = GetComponentInChildren<Canvas>();
        }
        
        if (promptCanvas != null)
        {
            promptCanvas.renderMode = RenderMode.WorldSpace;
            promptCanvas.transform.localPosition = Vector3.up * hoverHeight;
        }
        
        // Setup canvas group
        if (canvasGroup == null && promptCanvas != null)
        {
            canvasGroup = promptCanvas.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = promptCanvas.gameObject.AddComponent<CanvasGroup>();
            }
        }
        
        // Setup text
        if (promptText == null && promptCanvas != null)
        {
            promptText = promptCanvas.GetComponentInChildren<TextMeshProUGUI>();
        }
        
        if (promptText != null)
        {
            promptText.text = promptMessage;
            promptText.color = textColor;
            promptText.fontSize = 24;
            promptText.alignment = TextAlignmentOptions.Center;
        }
        
        // Start hidden
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
        
        // Find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        // Fade in/out
        if (canvasGroup != null)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
        }
        
        // Pulse effect when visible
        if (isVisible && promptText != null)
        {
            float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            float currentGlow = glowIntensity * (1f + pulse);
            
            // Apply glow to text
            if (promptText.fontMaterial.HasProperty("_GlowColor"))
            {
                promptText.fontMaterial.SetColor("_GlowColor", glowColor * currentGlow);
            }
        }
        
        // Face player
        if (playerTransform != null && promptCanvas != null)
        {
            promptCanvas.transform.LookAt(playerTransform);
            promptCanvas.transform.Rotate(0, 180, 0); // Flip to face player
        }
    }

    /// <summary>
    /// Show the prompt
    /// </summary>
    public void Show()
    {
        isVisible = true;
        targetAlpha = 1f;
    }

    /// <summary>
    /// Hide the prompt
    /// </summary>
    public void Hide()
    {
        isVisible = false;
        targetAlpha = 0f;
    }

    /// <summary>
    /// Set custom prompt message
    /// </summary>
    public void SetMessage(string message)
    {
        promptMessage = message;
        if (promptText != null)
        {
            promptText.text = message;
        }
    }

    /// <summary>
    /// Flash the prompt for attention
    /// </summary>
    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private System.Collections.IEnumerator FlashCoroutine()
    {
        float flashDuration = 0.3f;
        float elapsed = 0f;
        
        while (elapsed < flashDuration)
        {
            float t = Mathf.Sin(elapsed / flashDuration * Mathf.PI);
            if (promptText != null && promptText.fontMaterial.HasProperty("_GlowColor"))
            {
                promptText.fontMaterial.SetColor("_GlowColor", glowColor * (glowIntensity * 3f * t));
            }
            
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
