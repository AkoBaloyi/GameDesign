using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI inputPromptText;
    public Image tutorialBackground;
    public GameObject tutorialArrow; // Optional pointing arrow
    
    [Header("Game HUD References")]
    public GameObject gameHUDCanvas; // Main game HUD canvas
    public GameObject ammoDisplay; // Ammo counter display
    public GameObject crosshair; // Crosshair display

    [Header("Player References")]
    public FPController playerController;
    public Transform player;
    public Transform cameraTransform;
    
    [Header("Tutorial Objects")]
    public GameObject orangeCube; // The orange cube to pick up
    public GameObject nailgun; // The nailgun to pick up
    public GameObject nailCrate; // The crate with nails
    public HighlightableObject cubeHighlight;
    public HighlightableObject nailgunHighlight;
    public HighlightableObject crateHighlight;
    
    [Header("Visual Effects")]
    public ParticleSystem pickupEffect;
    public ParticleSystem loadingEffect;
    public AudioSource tutorialAudio;
    public AudioClip pickupSound;
    public AudioClip loadingSound;
    
    [Header("Tutorial Settings")]
    public float textFadeSpeed = 2f;
    public float stepDelay = 1f;
    public float textDisplayDuration = 3f; // How long to show tutorial text
    public bool skipTutorial = false;
    
    // Tutorial state
    public enum TutorialStep
    {
        Detecting,
        LookAround,
        Movement,
        PickupCube,
        ThrowCube,
        PickupNailgun,
        LoadNails,
        Shooting,
        Complete
    }
    
    public enum InputDevice
    {
        KeyboardMouse,
        Gamepad
    }
    
    [Header("Debug")]
    public TutorialStep currentStep = TutorialStep.Detecting;
    public InputDevice detectedDevice = InputDevice.KeyboardMouse;
    
    // Private variables
    private bool tutorialActive = true;
    private bool stepCompleted = false;
    private float lookMovement = 0f;
    private Vector2 lastMousePosition;
    private Vector2 movementInput;
    private bool hasPickedUpCube = false;
    private bool hasThrownCube = false;
    private bool hasPickedUpNailgun = false;
    private bool hasLoadedNails = false;
    private int shotsCount = 0;
    
    // Input detection
    private bool mouseDetected = false;
    private bool gamepadDetected = false;
    private float detectionTimer = 0f;
    private float textDisplayTimer = 0f;
    private bool textDisplayed = false;
    
    void Start()
    {
        if (skipTutorial)
        {
            CompleteTutorial();
            return;
        }
        
        InitializeTutorial();
    }
    
    void Update()
    {
        if (!tutorialActive) return;
        
        DetectInputDevice();
        UpdateCurrentStep();
    }
    
    void InitializeTutorial()
    {
        // Show tutorial UI
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
        
        // Hide game HUD initially
        HideGameHUD();
        
        // Disable player input initially
        if (playerController != null)
        {
            playerController.SetInputEnabled(false);
        }
        
        // Start with device detection
        currentStep = TutorialStep.Detecting;
        UpdateTutorialText("Welcome! Move your mouse or gamepad to begin...", "");
        textDisplayed = true;
        
        // Highlight objects that will be needed later
        if (cubeHighlight != null)
        {
            cubeHighlight.HighlightOn();
        }
    }
    
    void DetectInputDevice()
    {
        if (currentStep != TutorialStep.Detecting) return;
        
        detectionTimer += Time.deltaTime;
        
        // Check for mouse movement (only if mouse delta is significant)
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            if (mouseDelta.magnitude > 5f) // Increased threshold to avoid accidental detection
            {
                mouseDetected = true;
            }
        }
        
        // Check for gamepad input
        if (Gamepad.current != null)
        {
            Vector2 rightStick = Gamepad.current.rightStick.ReadValue();
            Vector2 leftStick = Gamepad.current.leftStick.ReadValue();
            
            if (rightStick.magnitude > 0.3f || leftStick.magnitude > 0.3f)
            {
                gamepadDetected = true;
            }
        }

        // Determine primary input device and advance
        if (mouseDetected || gamepadDetected)
        {
            detectedDevice = (gamepadDetected && !mouseDetected) ? InputDevice.Gamepad : InputDevice.KeyboardMouse;
            UpdateTutorialText("Input Detected!", "Get ready to start.");
            stepCompleted = true; // Mark as completed to prevent re-triggering
            StartCoroutine(DelayedNextStep());
        }
        // Auto-advance after 10 seconds if no input is detected
        else if (detectionTimer > 10f)
        {
            detectedDevice = InputDevice.KeyboardMouse;
            UpdateTutorialText("Starting Tutorial...", "");
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void UpdateCurrentStep()
    {
        // Handle text display timing
        if (textDisplayed)
        {
            textDisplayTimer += Time.deltaTime;
        }
        
        switch (currentStep)
        {
            case TutorialStep.LookAround:
                HandleLookAroundStep();
                break;
            case TutorialStep.Movement:
                HandleMovementStep();
                break;
            case TutorialStep.PickupCube:
                HandlePickupCubeStep();
                break;
            case TutorialStep.ThrowCube:
                HandleThrowCubeStep();
                break;
            case TutorialStep.PickupNailgun:
                HandlePickupStep();
                break;
            case TutorialStep.LoadNails:
                HandleLoadingStep();
                break;
            case TutorialStep.Shooting:
                HandleShootingStep();
                break;
        }
    }
    
    void HandleLookAroundStep()
    {
        // Prevent re-completion
        if (stepCompleted) return;

        // Only start tracking after text has been displayed for minimum time
        if (textDisplayTimer < textDisplayDuration) return;
        
        // Track mouse/stick movement
        if (detectedDevice == InputDevice.KeyboardMouse && Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            lookMovement += mouseDelta.magnitude;
        }
        else if (Gamepad.current != null)
        {
            Vector2 stickDelta = Gamepad.current.rightStick.ReadValue();
            lookMovement += stickDelta.magnitude * 100f; // Scale up for comparison
        }
        
        // Complete when enough look movement is detected over a short period
        if (lookMovement > 800f) // Increased threshold from 500f
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleMovementStep()
    {
        if (stepCompleted) return;
        
        // Only start tracking after text has been displayed for minimum time
        if (textDisplayTimer < textDisplayDuration) return;
        
        // Track movement input
        if (detectedDevice == InputDevice.KeyboardMouse && Keyboard.current != null)
        {
            movementInput = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) movementInput.y += 1;
            if (Keyboard.current.sKey.isPressed) movementInput.y -= 1;
            if (Keyboard.current.aKey.isPressed) movementInput.x -= 1;
            if (Keyboard.current.dKey.isPressed) movementInput.x += 1;
        }
        else if (Gamepad.current != null)
        {
            movementInput = Gamepad.current.leftStick.ReadValue();
        }
        
        // Complete when movement detected
        if (movementInput.magnitude > 0.5f)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }

    void HandlePickupCubeStep()
    {
        // This will be completed by the pickup system
        if (hasPickedUpCube)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleThrowCubeStep()
    {
        // This will be completed when cube is thrown or dropped
        if (hasThrownCube)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandlePickupStep()
    {
        // This will be completed by the pickup system
        if (hasPickedUpNailgun)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }

    void HandleLoadingStep()
    {
        // This will be completed by the loading system
        if (hasLoadedNails)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleShootingStep()
    {
        // Complete after firing a few shots
        if (shotsCount >= 3)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void NextStep()
    {
        currentStep++;
        stepCompleted = false;
        lookMovement = 0f;
        textDisplayTimer = 0f;
        textDisplayed = false;
        
        switch (currentStep)
        {
            case TutorialStep.LookAround:
                StartLookAroundStep();
                break;
            case TutorialStep.Movement:
                StartMovementStep();
                break;
            case TutorialStep.PickupCube:
                StartPickupCubeStep();
                break;
            case TutorialStep.ThrowCube:
                StartThrowCubeStep();
                break;
            case TutorialStep.PickupNailgun:
                StartPickupStep();
                break;
            case TutorialStep.LoadNails:
                StartLoadingStep();
                break;
            case TutorialStep.Shooting:
                StartShootingStep();
                break;
            case TutorialStep.Complete:
                CompleteTutorial();
                break;
        }
    }
    
    void StartLookAroundStep()
    {
        // Enable look controls
        if (playerController != null)
        {
            playerController.SetInputEnabled(true);
        }
        
        string mainText = "Great! Now let's learn to look around.";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Move your mouse to look around" : 
            "Use the right stick to look around";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }
    
    void StartMovementStep()
    {
        string mainText = "Perfect! Now let's learn to move.";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Use W, A, S, D keys to move around" : 
            "Use the left stick to move around";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }

    void StartPickupCubeStep()
    {
        string mainText = "Now let's learn to interact with objects!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Look at the glowing orange cube and press E to pick it up" : 
            "Look at the glowing orange cube and press X to pick it up";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
        
        // Make sure cube is highlighted
        if (cubeHighlight != null)
        {
            cubeHighlight.HighlightOn();
        }
    }

    void StartThrowCubeStep()
    {
        string mainText = "Great! Now you can drop or throw objects.";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Press E to drop the cube, or G to throw it" : 
            "Press X to drop the cube, or Y to throw it";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }
    
    void StartPickupStep()
    {
        string mainText = "Time to pick up your first tool!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Look at the glowing Nailgun and press E to pick it up" : 
            "Look at the glowing Nailgun and press X to pick it up";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
        
        // Make sure nailgun is highlighted
        if (nailgunHighlight != null)
        {
            nailgunHighlight.HighlightOn();
        }
    }
    
    void StartLoadingStep()
    {
        string mainText = "Your Nailgun needs ammo! Let's load some nails.";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Look at the crate and press E to load nails (50 nails per magazine)" : 
            "Look at the crate and press X to load nails (50 nails per magazine)";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
        
        // Highlight the crate
        if (crateHighlight != null)
        {
            crateHighlight.HighlightOn();
        }
        
        // Show game HUD when nailgun is equipped
        ShowGameHUD();
    }
    
    void StartShootingStep()
    {
        string mainText = "Now you're ready to fire! Practice shooting.";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Left-click to fire the Nailgun (fire 3 times to continue)" : 
            "Pull the right trigger to fire the Nailgun (fire 3 times to continue)";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }
    
    void CompleteTutorial()
    {
        tutorialActive = false;
        
        // Hide tutorial UI
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        
        // Show game HUD
        ShowGameHUD();
        
        // Turn off all highlights
        if (cubeHighlight != null) cubeHighlight.HighlightOff();
        if (nailgunHighlight != null) nailgunHighlight.HighlightOff();
        if (crateHighlight != null) crateHighlight.HighlightOff();
        
        // Ensure player has full control
        if (playerController != null)
        {
            playerController.SetInputEnabled(true);
        }
        
        Debug.Log("Tutorial completed!");
        
        // Optional: Save tutorial completion
        PlayerPrefs.SetInt("TutorialCompleted", 1);
    }
    
    void UpdateTutorialText(string main, string input)
    {
        if (tutorialText != null) tutorialText.text = main;
        if (inputPromptText != null) inputPromptText.text = input;
    }
    
    IEnumerator DelayedNextStep()
    {
        yield return new WaitForSeconds(stepDelay);
        NextStep();
    }
    
    // Public methods for other systems to call
    public void OnCubePickedUp()
    {
        hasPickedUpCube = true;
        PlayPickupEffect();
    }
    
    public void OnCubeDroppedOrThrown()
    {
        hasThrownCube = true;
    }
    
    public void OnNailgunPickedUp()
    {
        hasPickedUpNailgun = true;
        PlayPickupEffect();
    }
    
    public void OnNailsLoaded()
    {
        hasLoadedNails = true;
        PlayLoadingEffect();
    }
    
    public void OnNailgunFired()
    {
        shotsCount++;
    }
    
    void PlayPickupEffect()
    {
        if (pickupEffect != null) pickupEffect.Play();
        if (tutorialAudio != null && pickupSound != null)
        {
            tutorialAudio.PlayOneShot(pickupSound);
        }
    }
    
    void PlayLoadingEffect()
    {
        if (loadingEffect != null) loadingEffect.Play();
        if (tutorialAudio != null && loadingSound != null)
        {
            tutorialAudio.PlayOneShot(loadingSound);
        }
    }

    // Show Game HUD method
    void ShowGameHUD()
    {
        if (gameHUDCanvas != null)
        {
            gameHUDCanvas.SetActive(true);
        }
        
        if (ammoDisplay != null)
        {
            ammoDisplay.SetActive(true);
        }
        
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }
    }
    
    // Hide Game HUD method
    void HideGameHUD()
    {
        if (gameHUDCanvas != null)
        {
            gameHUDCanvas.SetActive(false);
        }
        
        if (ammoDisplay != null)
        {
            ammoDisplay.SetActive(false);
        }
        
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }
    
    // Skip tutorial option
    [ContextMenu("Skip Tutorial")]
    public void SkipTutorial()
    {
        CompleteTutorial();
    }
}