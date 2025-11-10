using UnityEngine;
using TMPro;





public class GameplayLoopDiagnostic : MonoBehaviour
{
    [Header("UI")]
    public bool showDiagnosticUI = true;
    public KeyCode toggleKey = KeyCode.F12;
    
    private ObjectiveManager objectiveManager;
    private PowerCell powerCell;
    private PowerBay powerBay;
    private FactoryConsole factoryConsole;
    private LightsController lightsController;
    private FPController player;
    
    private GUIStyle headerStyle;
    private GUIStyle normalStyle;
    private GUIStyle errorStyle;
    private GUIStyle successStyle;
    
    private bool initialized = false;
    private bool uiVisible = true;

    void Start()
    {
        FindComponents();
        InitializeStyles();
        initialized = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            uiVisible = !uiVisible;
        }
    }

    void FindComponents()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        powerCell = FindObjectOfType<PowerCell>();
        powerBay = FindObjectOfType<PowerBay>();
        factoryConsole = FindObjectOfType<FactoryConsole>();
        lightsController = FindObjectOfType<LightsController>();
        player = FindObjectOfType<FPController>();
    }

    void InitializeStyles()
    {
        headerStyle = new GUIStyle();
        headerStyle.fontSize = 16;
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.normal.textColor = Color.yellow;
        
        normalStyle = new GUIStyle();
        normalStyle.fontSize = 12;
        normalStyle.normal.textColor = Color.white;
        
        errorStyle = new GUIStyle();
        errorStyle.fontSize = 12;
        errorStyle.normal.textColor = Color.red;
        errorStyle.fontStyle = FontStyle.Bold;
        
        successStyle = new GUIStyle();
        successStyle.fontSize = 12;
        successStyle.normal.textColor = Color.green;
    }

    void OnGUI()
    {
        if (!showDiagnosticUI || !uiVisible || !initialized) return;

        GUI.Box(new Rect(10, 10, 400, 600), "");
        
        float y = 20;
        float lineHeight = 20;

        GUI.Label(new Rect(20, y, 380, 30), "GAMEPLAY LOOP DIAGNOSTIC", headerStyle);
        y += 35;
        
        GUI.Label(new Rect(20, y, 380, 20), $"Press {toggleKey} to toggle this panel", normalStyle);
        y += 25;

        GUI.Label(new Rect(20, y, 380, 20), "=== COMPONENT STATUS ===", headerStyle);
        y += lineHeight + 5;
        
        DrawComponentStatus("ObjectiveManager", objectiveManager != null, ref y);
        DrawComponentStatus("PowerCell", powerCell != null, ref y);
        DrawComponentStatus("PowerBay", powerBay != null, ref y);
        DrawComponentStatus("FactoryConsole", factoryConsole != null, ref y);
        DrawComponentStatus("LightsController", lightsController != null, ref y);
        DrawComponentStatus("Player (FPController)", player != null, ref y);
        
        y += 10;

        GUI.Label(new Rect(20, y, 380, 20), "=== CURRENT STATE ===", headerStyle);
        y += lineHeight + 5;
        
        if (objectiveManager != null)
        {
            GUI.Label(new Rect(20, y, 380, 20), 
                $"Current Step: {objectiveManager.currentStep}", normalStyle);
            y += lineHeight;
        }
        
        if (powerCell != null)
        {
            bool pickedUp = powerCell.IsPickedUp();
            GUI.Label(new Rect(20, y, 380, 20), 
                $"Power Cell Picked Up: {pickedUp}", 
                pickedUp ? successStyle : normalStyle);
            y += lineHeight;
        }
        
        if (player != null)
        {
            GameObject heldObject = player.GetHeldObject();
            GUI.Label(new Rect(20, y, 380, 20), 
                $"Player Holding: {(heldObject != null ? heldObject.name : "Nothing")}", 
                normalStyle);
            y += lineHeight;
        }
        
        y += 10;

        GUI.Label(new Rect(20, y, 380, 20), "=== SETUP VERIFICATION ===", headerStyle);
        y += lineHeight + 5;

        if (powerCell != null)
        {
            bool hasPickUpObject = powerCell.GetComponent<PickUpObject>() != null;
            bool hasRigidbody = powerCell.GetComponent<Rigidbody>() != null;
            bool hasCollider = powerCell.GetComponent<Collider>() != null;
            
            DrawSetupCheck("PowerCell has PickUpObject", hasPickUpObject, ref y);
            DrawSetupCheck("PowerCell has Rigidbody", hasRigidbody, ref y);
            DrawSetupCheck("PowerCell has Collider", hasCollider, ref y);
        }

        if (powerBay != null)
        {
            bool hasObjectiveManager = GetPrivateField<ObjectiveManager>(powerBay, "objectiveManager") != null;
            bool hasSocketPoint = GetPrivateField<Transform>(powerBay, "socketPoint") != null;
            
            DrawSetupCheck("PowerBay has ObjectiveManager ref", hasObjectiveManager, ref y);
            DrawSetupCheck("PowerBay has SocketPoint", hasSocketPoint, ref y);
        }

        if (factoryConsole != null)
        {
            bool hasObjectiveManager = GetPrivateField<ObjectiveManager>(factoryConsole, "objectiveManager") != null;
            
            DrawSetupCheck("Console has ObjectiveManager ref", hasObjectiveManager, ref y);
        }
        
        y += 10;

        if (player != null)
        {
            GUI.Label(new Rect(20, y, 380, 20), "=== DISTANCES ===", headerStyle);
            y += lineHeight + 5;
            
            if (powerCell != null)
            {
                float dist = Vector3.Distance(player.transform.position, powerCell.transform.position);
                GUI.Label(new Rect(20, y, 380, 20), 
                    $"Distance to Power Cell: {dist:F1}m", normalStyle);
                y += lineHeight;
            }
            
            if (powerBay != null)
            {
                float dist = Vector3.Distance(player.transform.position, powerBay.transform.position);
                GUI.Label(new Rect(20, y, 380, 20), 
                    $"Distance to Power Bay: {dist:F1}m", normalStyle);
                y += lineHeight;
            }
            
            if (factoryConsole != null)
            {
                float dist = Vector3.Distance(player.transform.position, factoryConsole.transform.position);
                GUI.Label(new Rect(20, y, 380, 20), 
                    $"Distance to Console: {dist:F1}m", normalStyle);
                y += lineHeight;
            }
        }
        
        y += 10;

        GUI.Label(new Rect(20, y, 380, 20), "=== QUICK TESTS ===", headerStyle);
        y += lineHeight + 5;
        
        GUI.Label(new Rect(20, y, 380, 20), "1. Walk to Power Cell and press E", normalStyle);
        y += lineHeight;
        GUI.Label(new Rect(20, y, 380, 20), "2. Walk to Power Bay and press F", normalStyle);
        y += lineHeight;
        GUI.Label(new Rect(20, y, 380, 20), "3. Wait for lights to activate", normalStyle);
        y += lineHeight;
        GUI.Label(new Rect(20, y, 380, 20), "4. Follow glowing path to Console", normalStyle);
        y += lineHeight;
        GUI.Label(new Rect(20, y, 380, 20), "5. Press F at Console", normalStyle);
        y += lineHeight;
        
        y += 10;

        if (objectiveManager == null || powerCell == null || powerBay == null || 
            factoryConsole == null || player == null)
        {
            GUI.Label(new Rect(20, y, 380, 20), "⚠ MISSING COMPONENTS!", errorStyle);
            y += lineHeight;
            GUI.Label(new Rect(20, y, 380, 20), "Check scene setup!", errorStyle);
        }
    }

    void DrawComponentStatus(string name, bool exists, ref float y)
    {
        string status = exists ? "✓ Found" : "✗ MISSING";
        GUIStyle style = exists ? successStyle : errorStyle;
        GUI.Label(new Rect(20, y, 380, 20), $"{name}: {status}", style);
        y += 20;
    }

    void DrawSetupCheck(string name, bool isValid, ref float y)
    {
        string status = isValid ? "✓" : "✗";
        GUIStyle style = isValid ? successStyle : errorStyle;
        GUI.Label(new Rect(20, y, 380, 20), $"{status} {name}", style);
        y += 20;
    }

    T GetPrivateField<T>(object obj, string fieldName) where T : class
    {
        var field = obj.GetType().GetField(fieldName, 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            return field.GetValue(obj) as T;
        }
        return null;
    }

    [ContextMenu("Log Full Diagnostic")]
    public void LogFullDiagnostic()
    {
        Debug.Log("=== GAMEPLAY LOOP DIAGNOSTIC ===");
        
        Debug.Log($"ObjectiveManager: {(objectiveManager != null ? "Found" : "MISSING")}");
        Debug.Log($"PowerCell: {(powerCell != null ? "Found" : "MISSING")}");
        Debug.Log($"PowerBay: {(powerBay != null ? "Found" : "MISSING")}");
        Debug.Log($"FactoryConsole: {(factoryConsole != null ? "Found" : "MISSING")}");
        Debug.Log($"LightsController: {(lightsController != null ? "Found" : "MISSING")}");
        Debug.Log($"Player: {(player != null ? "Found" : "MISSING")}");
        
        if (objectiveManager != null)
        {
            Debug.Log($"Current Step: {objectiveManager.currentStep}");
        }
        
        if (powerCell != null)
        {
            Debug.Log($"Power Cell Picked Up: {powerCell.IsPickedUp()}");
        }
        
        Debug.Log("=== END DIAGNOSTIC ===");
    }
}
