using UnityEngine;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Enhanced objective manager for new narrative flow
/// Handles: Tutorial → Lights Off → Console Check → Power Bay Check → Workshop → Combat → Power Restore
/// </summary>
public class EnhancedObjectiveManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI directionText; // Shows direction to objective
    public GameObject objectivePanel;
    
    [Header("Objective Targets")]
    public Transform consoleTransform;
    public Transform powerBayTransform;
    public Transform workshopTransform;
    
    [Header("References")]
    public Light[] factoryLights;
    public EnemySpawner enemySpawner;
    public NailgunWeapon nailgun;
    
    [Header("Events")]
    public UnityEvent onLightsGoOut;
    public UnityEvent onConsoleInspected;
    public UnityEvent onPowerBayInspected;
    public UnityEvent onNailgunPickedUp;
    public UnityEvent onPowerCellPickedUp;
    public UnityEvent onPowerRestored;
    public UnityEvent onGameComplete;
    
    public enum ObjectiveStep
    {
        Tutorial,
        LightsOut,
        CheckConsole,
        CheckPowerBay,
        GoToWorkshop,
        GetNailgun,
        GetPowerCell,
        ReturnToPowerBay,
        InsertPowerCell,
        GoToConsole,
        ActivateConsole,
        Complete
    }
    
    public ObjectiveStep currentStep = ObjectiveStep.Tutorial;
    private Transform player;

    void Start()
    {
        // Find player
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
        UpdateDirectionIndicator();
    }

    void UpdateDirectionIndicator()
    {
        if (player == null || directionText == null) return;

        Transform target = GetCurrentObjectiveTarget();
        if (target == null) return;

        float distance = Vector3.Distance(player.position, target.position);
        Vector3 direction = (target.position - player.position).normalized;
        
        // Simple direction indicator
        string directionStr = GetDirectionString(direction);
        directionText.text = $"{directionStr} {distance:F0}m";
    }

    string GetDirectionString(Vector3 direction)
    {
        if (player == null) return "";
        
        Vector3 playerForward = player.forward;
        float angle = Vector3.SignedAngle(playerForward, direction, Vector3.up);
        
        if (angle > -22.5f && angle <= 22.5f) return "↑";
        if (angle > 22.5f && angle <= 67.5f) return "↗";
        if (angle > 67.5f && angle <= 112.5f) return "→";
        if (angle > 112.5f && angle <= 157.5f) return "↘";
        if (angle > 157.5f || angle <= -157.5f) return "↓";
        if (angle > -157.5f && angle <= -112.5f) return "↙";
        if (angle > -112.5f && angle <= -67.5f) return "←";
        if (angle > -67.5f && angle <= -22.5f) return "↖";
        
        return "↑";
    }

    Transform GetCurrentObjectiveTarget()
    {
        switch (currentStep)
        {
            case ObjectiveStep.CheckConsole:
                return consoleTransform;
            case ObjectiveStep.CheckPowerBay:
                return powerBayTransform;
            case ObjectiveStep.GoToWorkshop:
            case ObjectiveStep.GetNailgun:
            case ObjectiveStep.GetPowerCell:
                return workshopTransform;
            case ObjectiveStep.ReturnToPowerBay:
            case ObjectiveStep.InsertPowerCell:
                return powerBayTransform;
            case ObjectiveStep.GoToConsole:
            case ObjectiveStep.ActivateConsole:
                return consoleTransform;
            default:
                return null;
        }
    }

    public void SetObjective(ObjectiveStep step)
    {
        currentStep = step;
        UpdateObjectiveText();
    }

    void UpdateObjectiveText()
    {
        if (objectiveText == null) return;

        string text = "";
        
        switch (currentStep)
        {
            case ObjectiveStep.Tutorial:
                text = "Complete the tutorial";
                break;
            case ObjectiveStep.LightsOut:
                text = "The lights went out! What happened?";
                break;
            case ObjectiveStep.CheckConsole:
                text = "Check the main console";
                break;
            case ObjectiveStep.CheckPowerBay:
                text = "Check the power bay";
                break;
            case ObjectiveStep.GoToWorkshop:
                text = "Get a replacement power cell from the workshop";
                break;
            case ObjectiveStep.GetNailgun:
                text = "Grab the nailgun - bots are going rogue!";
                break;
            case ObjectiveStep.GetPowerCell:
                text = "Get the replacement power cell";
                break;
            case ObjectiveStep.ReturnToPowerBay:
                text = "Return to the power bay";
                break;
            case ObjectiveStep.InsertPowerCell:
                text = "Insert the power cell";
                break;
            case ObjectiveStep.GoToConsole:
                text = "Return to the main console";
                break;
            case ObjectiveStep.ActivateConsole:
                text = "Activate the console";
                break;
            case ObjectiveStep.Complete:
                text = "FACTORY SECURED!";
                break;
        }
        
        objectiveText.text = text;
        Debug.Log($"[EnhancedObjectiveManager] Objective: {text}");
    }

    // Called by tutorial manager
    public void OnTutorialComplete()
    {
        Debug.Log("[EnhancedObjectiveManager] Tutorial complete - turning off lights!");
        TurnOffLights();
        SetObjective(ObjectiveStep.LightsOut);
        
        // Wait a moment then tell player to check console
        Invoke(nameof(PromptCheckConsole), 2f);
    }

    void PromptCheckConsole()
    {
        SetObjective(ObjectiveStep.CheckConsole);
    }

    void TurnOffLights()
    {
        if (factoryLights != null)
        {
            foreach (var light in factoryLights)
            {
                if (light != null) light.enabled = false;
            }
        }
        
        onLightsGoOut?.Invoke();
    }

    // Called by console inspection
    public void OnConsoleInspected()
    {
        Debug.Log("[EnhancedObjectiveManager] Console inspected - directing to power bay");
        SetObjective(ObjectiveStep.CheckPowerBay);
        onConsoleInspected?.Invoke();
    }

    // Called by power bay inspection
    public void OnPowerBayInspected()
    {
        Debug.Log("[EnhancedObjectiveManager] Power bay inspected - directing to workshop");
        SetObjective(ObjectiveStep.GoToWorkshop);
        onPowerBayInspected?.Invoke();
        
        // Enable enemy spawning
        if (enemySpawner != null)
        {
            enemySpawner.EnableSpawning();
        }
    }

    // Called when player picks up nailgun
    public void OnNailgunPickedUp()
    {
        Debug.Log("[EnhancedObjectiveManager] Nailgun picked up");
        SetObjective(ObjectiveStep.GetPowerCell);
        onNailgunPickedUp?.Invoke();
    }

    // Called when player picks up power cell
    public void OnPowerCellPickedUp()
    {
        Debug.Log("[EnhancedObjectiveManager] Power cell picked up - return to power bay");
        SetObjective(ObjectiveStep.ReturnToPowerBay);
        onPowerCellPickedUp?.Invoke();
    }

    // Called when power cell is inserted
    public void OnPowerCellInserted()
    {
        Debug.Log("[EnhancedObjectiveManager] Power cell inserted - restoring power");
        
        // Turn lights back on
        if (factoryLights != null)
        {
            foreach (var light in factoryLights)
            {
                if (light != null) light.enabled = true;
            }
        }
        
        // Stop enemy spawning
        if (enemySpawner != null)
        {
            enemySpawner.OnPowerRestored();
        }
        
        SetObjective(ObjectiveStep.GoToConsole);
        onPowerRestored?.Invoke();
    }

    // Called when console is activated
    public void OnConsoleActivated()
    {
        Debug.Log("[EnhancedObjectiveManager] Console activated - game complete!");
        SetObjective(ObjectiveStep.Complete);
        onGameComplete?.Invoke();
    }
}
