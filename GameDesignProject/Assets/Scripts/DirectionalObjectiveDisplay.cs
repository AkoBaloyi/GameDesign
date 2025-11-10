using UnityEngine;
using TMPro;




public class DirectionalObjectiveDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI directionText;
    public GameObject objectivePanel;
    
    [Header("Current Target")]
    public Transform currentTarget;
    
    [Header("Settings")]
    public bool showDirection = true;
    public bool showDistance = true;
    public float updateInterval = 0.1f;
    
    private Transform player;
    private float updateTimer = 0f;

    void Start()
    {

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        if (objectivePanel != null)
        {
            objectivePanel.SetActive(true);
        }
    }

    void Update()
    {
        updateTimer += Time.deltaTime;
        
        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;
            UpdateDirectionDisplay();
        }
    }

    void UpdateDirectionDisplay()
    {
        if (!showDirection || currentTarget == null || player == null || directionText == null)
        {
            if (directionText != null) directionText.text = "";
            return;
        }

        Vector3 toTarget = currentTarget.position - player.position;
        float distance = toTarget.magnitude;

        string arrow = GetDirectionArrow(toTarget.normalized);

        string displayText = arrow;
        
        if (showDistance)
        {
            displayText += $" {distance:F0}m";
        }
        
        directionText.text = displayText;
    }

    string GetDirectionArrow(Vector3 direction)
    {
        if (player == null) return "↑";

        Vector3 playerForward = player.forward;
        float angle = Vector3.SignedAngle(playerForward, direction, Vector3.up);

        if (angle >= -22.5f && angle < 22.5f) return "↑";
        if (angle >= 22.5f && angle < 67.5f) return "↗";
        if (angle >= 67.5f && angle < 112.5f) return "→";
        if (angle >= 112.5f && angle < 157.5f) return "↘";
        if (angle >= 157.5f || angle < -157.5f) return "↓";
        if (angle >= -157.5f && angle < -112.5f) return "↙";
        if (angle >= -112.5f && angle < -67.5f) return "←";
        if (angle >= -67.5f && angle < -22.5f) return "↖";
        
        return "↑";
    }

    public void SetObjective(string text, Transform target = null)
    {
        if (objectiveText != null)
        {
            objectiveText.text = text;
        }
        
        currentTarget = target;
    }

    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    public void ClearTarget()
    {
        currentTarget = null;
        if (directionText != null)
        {
            directionText.text = "";
        }
    }

    public void Show()
    {
        if (objectivePanel != null)
        {
            objectivePanel.SetActive(true);
        }
    }

    public void Hide()
    {
        if (objectivePanel != null)
        {
            objectivePanel.SetActive(false);
        }
    }
}
