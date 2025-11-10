using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;




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

        HideAllMapMarkers();

        if (fullMapPanel != null)
        {
            fullMapPanel.SetActive(false);
        }
    }

    private void Update()
    {

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

    public void OnTutorialComplete()
    {
        currentStep = Step.LightsOut;

        if (speedrunTimer != null)
        {
            speedrunTimer.StartTimer();
            Debug.Log("[ClearObjectiveManager] Speedrun timer started!");
        }

        foreach (var light in factoryLights)
        {
            if (light != null) light.enabled = false;
        }
        
        SetObjective(
            "POWER FAILURE!",
            "The factory has lost power",
            "Press M to open map"
        );

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

        if (consoleInspectable != null)
        {
            consoleInspectable.canInspect = true;
        }
    }

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

        if (powerBayInspectable != null)
        {
            powerBayInspectable.canInspect = true;
        }
    }

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

        if (enemySpawner != null)
        {
            enemySpawner.EnableSpawning();
            Debug.Log("[ClearObjectiveManager] Enemy spawning started!");
        }
    }

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

    public void OnPowerCellInserted()
    {
        if (currentStep != Step.ReturnToPowerBay) return;
        
        currentStep = Step.GoToConsole;

        foreach (var light in factoryLights)
        {
            if (light != null) light.enabled = true;
        }

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

    public void OnConsoleActivated()
    {
        if (currentStep != Step.GoToConsole) return;
        
        currentStep = Step.Complete;

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

    public void OnNailgunPickedUp()
    {

        if (instructionText != null)
        {
            instructionText.text = "Weapon equipped! Left-click to shoot bots";
        }
    }
}
