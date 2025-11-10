using UnityEngine;
using TMPro;




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



    public void OnTutorialComplete()
    {
        currentHintStep = 1;
        ShowHint("💡 Inspection results will appear at the TOP of the Console!");
    }



    public void ShowPowerBayHint()
    {
        if (currentHintStep == 1)
        {
            currentHintStep = 2;
            ShowHint("💡 Inspection results will appear at the TOP of the Power Bay!");
        }
    }



    public void ShowEnemyWarningHint()
    {
        if (currentHintStep == 2)
        {
            currentHintStep = 3;
            ShowHint("⚠️ WARNING: If you come in contact with the robots, you LOSE!");
        }
    }



    public void ShowHint(string message)
    {
        if (hintText != null)
        {
            hintText.text = message;
        }

        if (hintCanvasGroup != null)
        {
            hintCanvasGroup.alpha = 1f;
        }
    }



    public void HideHint()
    {
        if (hintCanvasGroup != null)
        {
            hintCanvasGroup.alpha = 0f;
        }
    }
}
