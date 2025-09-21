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
        tutorialPanel.SetActive(true);
        
        // Disable player input initially
        if (playerController != null)
        {
            playerController.SetInputEnabled(false);
        }
        
        // Start with device detection
        currentStep = TutorialStep.Detecting;
        UpdateTutorialText("Welcome! Move your mouse or gamepad to begin...", "");
        
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
        
        // Check for mouse movement
        Vector2 currentMousePos = Mouse.current.position.ReadValue();
        if (Vector2.Distance(currentMousePos, lastMousePosition) > 10f)
        {
            mouseDetected = true;
            lastMousePosition = currentMousePos;
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
        
        // Determine primary input device
        if (mouseDetected && !gamepadDetected)
        {
            detectedDevice = InputDevice.KeyboardMouse;
            NextStep();
        }
        else if (gamepadDetected && !mouseDetected)
        {
            detectedDevice = InputDevice.Gamepad;
            NextStep();
        }
        else if (mouseDetected && gamepadDetected)
        {
            // Both detected, prefer the one used most recently
            detectedDevice = InputDevice.KeyboardMouse; // Default to KB+M
            NextStep();
        }
        
        // Auto-advance after 5 seconds if no input detected
        if (detectionTimer > 5f)
        {
            detectedDevice = InputDevice.KeyboardMouse;
            NextStep();
        }
    }
    
    void UpdateCurrentStep()
    {
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
            case TutorialStep.PickupCube:
                StartPickupCubeStep();
                break;
            case TutorialStep.ThrowCube:
                StartThrowCubeStep();
                break;
                HandlePickupStep();
                break;
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
        // Track mouse/stick movement
        if (detectedDevice == InputDevice.KeyboardMouse)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            lookMovement += mouseDelta.magnitude;
        }
        else
        {
            if (Gamepad.current != null)
            {
                Vector2 stickDelta = Gamepad.current.rightStick.ReadValue();
                lookMovement += stickDelta.magnitude * 100f; // Scale up for comparison
            }
        }
        
        // Complete when enough look movement detected
        if (lookMovement > 500f)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleMovementStep()
    {
        // Track movement input
        if (detectedDevice == InputDevice.KeyboardMouse)
        {
            movementInput = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) movementInput.y += 1;
            if (Keyboard.current.sKey.isPressed) movementInput.y -= 1;
            if (Keyboard.current.aKey.isPressed) movementInput.x -= 1;
            if (Keyboard.current.dKey.isPressed) movementInput.x += 1;
        }
        else
        {
            if (Gamepad.current != null)
            {
                movementInput = Gamepad.current.leftStick.ReadValue();
            }
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
    {
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
        
        switch (currentStep)
        {
            case TutorialStep.LookAround:
                StartLookAroundStep();
                break;
            case TutorialStep.Movement:
                StartMovementStep();
                break;
            case TutorialStep.PickupNailgun:
            case TutorialStep.PickupNailgun:
                StartPickupStep();
                break;
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
    }
    
    void StartMovementStep()
    {
        string mainText = "Perfect! Now let's learn to move.";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Use W, A, S, D keys to move around" : 
            "Use the left stick to move around";
        
        UpdateTutorialText(mainText, inputText);
    }
    
    void StartPickupCubeStep()
    {
        string mainText = "Now let's learn to interact with objects!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Look at the glowing orange cube and press E to pick it up" : 
            "Look at the glowing orange cube and press X to pick it up";
        
        UpdateTutorialText(mainText, inputText);
        
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
    }
    
    void StartPickupStep()
    {
        string mainText = "Time to pick up your first tool!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Look at the glowing Nailgun and press E to pick it up" : 
            "Look at the glowing Nailgun and press X to pick it up";
        
        UpdateTutorialText(mainText, inputText);
        
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
        
        // Highlight the crate
        if (crateHighlight != null)
        {
            crateHighlight.HighlightOn();
        }
    }
    
    void StartShootingStep()
    {
        string mainText = "Now you're ready to fire! Practice shooting.";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Left-click to fire the Nailgun (fire 3 times to continue)" : 
            "Pull the right trigger to fire the Nailgun (fire 3 times to continue)";
        
        UpdateTutorialText(mainText, inputText);
    }
    
    void CompleteTutorial()
    {
        tutorialActive = false;
        
        // Hide tutorial UI
        tutorialPanel.SetActive(false);
        
        // Ensure player has full control
        if (playerController != null)
        {
            playerController.SetInputEnabled(true);
        }
        
        // Turn off all highlights
        if (cubeHighlight != null) cubeHighlight.HighlightOff();
        if (nailgunHighlight != null) nailgunHighlight.HighlightOff();
        if (crateHighlight != null) crateHighlight.HighlightOff();
        
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
    
    // Skip tutorial option
    [ContextMenu("Skip Tutorial")]
    public void SkipTutorial()
    {
        CompleteTutorial();
    }
}