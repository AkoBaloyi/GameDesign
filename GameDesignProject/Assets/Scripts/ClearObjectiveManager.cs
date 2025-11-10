using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// Clear, simple objective manager with room names and map markers
/// No confusing directional arrows - just clear instructions!
/// </summary>
public class ClearObjectiveManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI locationText; // Shows room name
    public TextMeshProUGUI instructionText; // Shows what to do
    public GameObject objectivePanel;
    
    [Header("Map")]
    public GameObject minimapPanel;
    public GameObject fullMapPanel;
    public GameObject consoleMapMarker; // Marker on map for console
    public GameObject powerBayMapMarker; // Marker on map for power bay
    public GameObject workshopMapMarker; // Marker on map for workshop
    
    [Header("References")]
    public Light[] factoryLights;
    public EnemySpawner enemySpawner;
    public InspectableObject consoleInspectable;
    public InspectableObject powerBayInspectable;
    public SpeedrunTimer speedrunTimer;
    
    private bool mapOpen = false;
    
    public enum Step
    {
        Tutorial,
        LightsOut,
        CheckConsole,
        CheckPowerBay,
        GoToWorkshop,
        GetPowerCell,
        ReturnToPowerBay,
        GoToConsole,
        Complete
    }
    
    public Step currentStep = Step.Tutorial;

    private void Start()
    {
        // Hide all map markers initially
        HideAllMapMarkers();
        
        // Hide full map initially
        if (fullMapPanel != null)
        {
            fullMapPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // M key to toggle full map
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            ToggleFullMap();
        }
    }

    private void ToggleFullMap()
    {
        mapOpen = !mapOpen;
        
        if (fullMapPanel != null)
        {
            fullMapPanel.SetActive(mapOpen);
        }
        
        // Hide minimap when full map is open
        if (minimapPanel != null)
        {
            minimapPanel.SetActive(!mapOpen);
        }
        
        Debug.Log($"[ClearObjectiveManager] Map {(mapOpen ? "opened" : "closed")}");
    }

    private void HideAllMapMarkers()
    {
        if (consoleMapMarker != null) consoleMapMarker.SetActive(false);
        if (powerBayMapMarker != null) powerBayMapMarker.SetActive(false);
        if (workshopMapMarker != null) workshopMapMarker.SetActive(false);
    }

    private void ShowMapMarker(GameObject marker)
    {
        HideAllMapMarkers();
        if (marker != null) marker.SetActive(true);
    }

    private void SetObjective(string objective, string location, string instruction)
    {
        if (objectiveText != null)
        {
            objectiveText.text = objective;
        }
        
        if (locationText != null)
        {
            locationText.text = location;
        }
        
        if (instructionText != null)
        {
            instructionText.text = instruction;
        }
        
        Debug.Log($"[ClearObjectiveManager] {objective} | {location} | {instruction}");
    }

    // ===== TUTORIAL =====
    public void OnTutorialComplete()
    {
        currentStep = Step.LightsOut;
        
        // START SPEEDRUN TIMER!
        if (speedrunTimer != null)
        {
            speedrunTimer.StartTimer();
            Debug.Log("[ClearObjectiveManager] Speedrun timer started!");
        }
        
        // Turn off all lights
        foreach (var light in factoryLights)
        {
            if (light != null) light.enabled = false;
        }
        
        SetObjective(
            "POWER FAILURE!",
            "The factory has lost power",
            "Press M to open map"
        );
        
        // Wait a moment then move to next step
        Invoke("StartInvestigation", 3f);
    }

    private void StartInvestigation()
    {
        currentStep = Step.CheckConsole;
        
        SetObjective(
            "Investigate main console (Tablet computer on wall)",
            "Location: Assembly Line Corridor (Blue marked room)",
            "Press E to inspect when you arrive"
        );
        
        ShowMapMarker(consoleMapMarker);
        
        // Enable console inspection
        if (consoleInspectable != null)
        {
            consoleInspectable.canInspect = true;
        }
    }

    // ===== CONSOLE INSPECTION =====
    public void OnConsoleInspected()
    {
        if (currentStep != Step.CheckConsole) return;
        
        currentStep = Step.CheckPowerBay;
        
        SetObjective(
            "Check the power bay",
            "Location: Power Grid Chamber (Blue circle marker)",
            "Press E to inspect when you arrive"
        );
        
        ShowMapMarker(powerBayMapMarker);
        
        // Enable power bay inspection
        if (powerBayInspectable != null)
        {
            powerBayInspectable.canInspect = true;
        }
    }

    // ===== POWER BAY INSPECTION =====
    public void OnPowerBayInspected()
    {
        if (currentStep != Step.CheckPowerBay) return;
        
        currentStep = Step.GoToWorkshop;
        
        SetObjective(
            "⚠️ DANGER: Rogue bots detected!",
            "Location: Workshop (Blue marked room)",
            "Get replacement power cell - DO NOT GET CAUGHT BY THE BOTS!"
        );
        
        ShowMapMarker(workshopMapMarker);
        
        // Start enemy spawning
        if (enemySpawner != null)
        {
            enemySpawner.EnableSpawning();
            Debug.Log("[ClearObjectiveManager] Enemy spawning started!");
        }
    }

    // ===== POWER CELL PICKUP =====
    public void OnPowerCellPickedUp()
    {
        if (currentStep != Step.GoToWorkshop) return;
        
        currentStep = Step.ReturnToPowerBay;
        
        SetObjective(
            "Return to power bay",
            "Location: Power Grid Chamber (Blue circle marker)",
            "Press F to insert power cell - watch for bots!"
        );
        
        ShowMapMarker(powerBayMapMarker);
    }

    // ===== POWER CELL INSERTED =====
    public void OnPowerCellInserted()
    {
        if (currentStep != Step.ReturnToPowerBay) return;
        
        currentStep = Step.GoToConsole;
        
        // Turn lights back on
        foreach (var light in factoryLights)
        {
            if (light != null) light.enabled = true;
        }
        
        // Stop enemy spawning
        if (enemySpawner != null)
        {
            enemySpawner.OnPowerRestored();
            Debug.Log("[ClearObjectiveManager] Enemy spawning stopped!");
        }
        
        SetObjective(
            "✓ Power restored! Return to console",
            "Location: Assembly Line Corridor (Blue marked room)",
            "Press F to activate console"
        );
        
        ShowMapMarker(consoleMapMarker);
    }

    // ===== CONSOLE ACTIVATED =====
    public void OnConsoleActivated()
    {
        if (currentStep != Step.GoToConsole) return;
        
        currentStep = Step.Complete;
        
        // STOP TIMER AND SHOW LEADERBOARD!
        if (speedrunTimer != null)
        {
            speedrunTimer.CompleteRun();
            Debug.Log("[ClearObjectiveManager] Speedrun complete!");
        }
        
        SetObjective(
            "✓ FACTORY SECURED!",
            "Mission Complete",
            "Check your time!"
        );
        
        HideAllMapMarkers();
    }

    // ===== NAILGUN PICKUP (Optional) =====
    public void OnNailgunPickedUp()
    {
        // Just a notification, doesn't change objective
        if (instructionText != null)
        {
            instructionText.text = "Weapon equipped! Left-click to shoot bots";
        }
    }
}
