using UnityEngine;
using TMPro;

/// <summary>
/// Shows persistent hints about where to look for inspection results
/// Hints stay visible until next objective
/// </summary>
public class InspectionHintUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI hintText;
    public CanvasGroup hintCanvasGroup;
    
    private int currentHintStep = 0;
    
    void Start()
    {
        if (hintCanvasGroup != null)
        {
            hintCanvasGroup.alpha = 0f; // Hidden until tutorial ends
        }
    }
    
    /// <summary>
    /// Called when tutorial ends - shows first hint
    /// </summary>
    public void OnTutorialComplete()
    {
        currentHintStep = 1;
        ShowHint("💡 Inspection results will appear at the TOP of the Console!");
    }
    
    /// <summary>
    /// Show hint for power bay inspection (2nd hint)
    /// </summary>
    public void ShowPowerBayHint()
    {
        if (currentHintStep == 1)
        {
            currentHintStep = 2;
            ShowHint("💡 Inspection results will appear at the TOP of the Power Bay!");
        }
    }
    
    /// <summary>
    /// Show hint about enemies (3rd hint)
    /// </summary>
    public void ShowEnemyWarningHint()
    {
        if (currentHintStep == 2)
        {
            currentHintStep = 3;
            ShowHint("⚠️ WARNING: If you come in contact with the robots, you LOSE!");
        }
    }
    
    /// <summary>
    /// Show custom hint - stays visible until replaced
    /// </summary>
    public void ShowHint(string message)
    {
        if (hintText != null)
        {
            hintText.text = message;
        }
        
        // Make sure it's visible
        if (hintCanvasGroup != null)
        {
            hintCanvasGroup.alpha = 1f;
        }
    }
    
    /// <summary>
    /// Hide hint completely
    /// </summary>
    public void HideHint()
    {
        if (hintCanvasGroup != null)
        {
            hintCanvasGroup.alpha = 0f;
        }
    }
}
