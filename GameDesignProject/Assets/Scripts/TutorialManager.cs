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
    public GameObject tutorialArrow;
    
    [Header("Game HUD References")]
    public GameObject gameHUDCanvas;
    public GameObject ammoDisplay;
    public GameObject crosshair;
    
    [Header("Player References")]
    public FPController playerController;
    public Transform player;
    public Transform cameraTransform;
    
    [Header("Tutorial Objects")]
    public GameObject orangeCube; // Practice pickup object
    public GameObject nailgun;
    public GameObject nailCrate;
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
    public float stepDelay = 0.5f; // Reduced from 2f to 0.5f
    public float textDisplayDuration = 1.5f; // Reduced from 4f to 1.5f  
    public bool skipTutorial = false;
    
    // Tutorial state
    public enum TutorialStep
    {
        Detecting,
        LookAround,
        Movement,
        PickupObject,
        DropThrow,
        Sprint,
        Crouch,
        Jump,
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
    private Vector2 movementInput;
    private bool hasPickedUpObject = false;
    private bool hasDroppedOrThrown = false;
    private bool hasSprinted = false;
    private bool hasCrouched = false;
    private bool hasJumped = false;
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
        if (!tutorialActive || skipTutorial) return;
        
        DetectInputDevice();
        UpdateCurrentStep();
    }
    
    void InitializeTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
        
        HideGameHUD();
        
        if (playerController != null)
        {
            playerController.SetInputEnabled(false);
        }
        
        currentStep = TutorialStep.Detecting;
        UpdateTutorialText("Welcome to the Factory Tutorial!", "Move your mouse or gamepad to begin...");
        textDisplayed = true;
        
        // Highlight the orange cube for later pickup tutorial
        if (cubeHighlight != null)
        {
            cubeHighlight.HighlightOn();
        }
    }
    
    void DetectInputDevice()
    {
        if (currentStep != TutorialStep.Detecting || stepCompleted) return;

        detectionTimer += Time.deltaTime;
        if (detectionTimer < textDisplayDuration) return;

        // Check for mouse movement
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            if (mouseDelta.magnitude > 5f)
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

        if (mouseDetected || gamepadDetected)
        {
            detectedDevice = (gamepadDetected && !mouseDetected) ? InputDevice.Gamepad : InputDevice.KeyboardMouse;
            UpdateTutorialText("Input Detected!", $"Using {detectedDevice} controls.");
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
        else if (detectionTimer > 12f)
        {
            detectedDevice = InputDevice.KeyboardMouse;
            UpdateTutorialText("Starting Tutorial...", "Using default keyboard & mouse controls.");
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void UpdateCurrentStep()
    {
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
            case TutorialStep.PickupObject:
                HandlePickupObjectStep();
                break;
            case TutorialStep.DropThrow:
                HandleDropThrowStep();
                break;
            case TutorialStep.Sprint:
                HandleSprintStep();
                break;
            case TutorialStep.Crouch:
                HandleCrouchStep();
                break;
            case TutorialStep.Jump:
                HandleJumpStep();
                break;
            case TutorialStep.PickupNailgun:
                HandlePickupNailgunStep();
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
        if (stepCompleted) return;
        if (textDisplayTimer < textDisplayDuration) return;
        
        if (detectedDevice == InputDevice.KeyboardMouse && Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            lookMovement += mouseDelta.magnitude;
        }
        else if (Gamepad.current != null)
        {
            Vector2 stickDelta = Gamepad.current.rightStick.ReadValue();
            lookMovement += stickDelta.magnitude * 100f;
        }
        
        if (lookMovement > 1000f)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleMovementStep()
    {
        if (stepCompleted) return;
        if (textDisplayTimer < textDisplayDuration) return;
        
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
        
        if (movementInput.magnitude > 0.5f)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleJumpStep()
    {
        if (stepCompleted) return;
        
        if (detectedDevice == InputDevice.KeyboardMouse && Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                hasJumped = true;
                Debug.Log("Jump detected via direct input!");
            }
        }
        else if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                hasJumped = true;
                Debug.Log("Jump detected via direct gamepad input!");
            }
        }
        
        if (hasJumped)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleSprintStep()
    {
        if (stepCompleted) return;
        
        bool sprintPressed = false;
        
        if (detectedDevice == InputDevice.KeyboardMouse && Keyboard.current != null)
        {
            if (Keyboard.current.leftShiftKey.isPressed)
            {
                sprintPressed = true;
            }
        }
        else if (Gamepad.current != null)
        {
            // FIXED: Changed 'gamepad' to 'Gamepad.current'
            if (Gamepad.current.leftStickButton.wasPressedThisFrame)
            {
                sprintPressed = true;
            }
        }
        
        if (sprintPressed && movementInput.magnitude > 0.1f)
        {
            hasSprinted = true;
            Debug.Log("Sprint detected!");
        }
        
        if (hasSprinted)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleCrouchStep()
    {
        if (stepCompleted) return;
        
        bool crouchPressed = false;
        
        if (detectedDevice == InputDevice.KeyboardMouse && Keyboard.current != null)
        {
            if (Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.ctrlKey.isPressed)
            {
                crouchPressed = true;
            }
        }
        else if (Gamepad.current != null)
        {
            if (Gamepad.current.rightShoulder.isPressed)
            {
                crouchPressed = true;
            }
        }
        
        if (crouchPressed)
        {
            hasCrouched = true;
            Debug.Log("Crouch detected!");
        }
        
        if (hasCrouched)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandlePickupObjectStep()
    {
        if (hasPickedUpObject)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleDropThrowStep()
    {
        if (hasDroppedOrThrown)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandlePickupNailgunStep()
    {
        if (hasPickedUpNailgun)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleLoadingStep()
    {
        if (hasLoadedNails)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void HandleShootingStep()
    {
        if (shotsCount >= 3)
        {
            stepCompleted = true;
            StartCoroutine(DelayedNextStep());
        }
    }
    
    void NextStep()
    {
        if (currentStep >= TutorialStep.Complete) return;
        
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
            case TutorialStep.PickupObject:
                StartPickupObjectStep();
                break;
            case TutorialStep.DropThrow:
                StartDropThrowStep();
                break;
            case TutorialStep.Sprint:
                StartSprintStep();
                break;
            case TutorialStep.Crouch:
                StartCrouchStep();
                break;
            case TutorialStep.Jump:
                StartJumpStep();
                break;
            case TutorialStep.PickupNailgun:
                StartPickupNailgunStep();
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
        if (playerController != null)
        {
            playerController.SetInputEnabled(true);
        }
        
        string mainText = "Let's learn the controls! First, looking around.";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Move your mouse to look around" : 
            "Use the right stick to look around";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }
    
    void StartMovementStep()
    {
        string mainText = "Great! Now let's learn to move.";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Use W, A, S, D keys to move around" : 
            "Use the left stick to move around";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }
    
    void StartCrouchStep()
    {
        string mainText = "Time to learn crouching!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ?
            "Press LEFT CTRL to crouch" :
            "Press Right Bumper (RB) to crouch";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }
    
    void StartJumpStep()
    {
        string mainText = "Time to learn jumping!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Press SPACE to jump" : 
            "Press A button to jump";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }
    
    void StartSprintStep()
    {
        string mainText = "Now let's learn to run!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Hold LEFT SHIFT while moving to sprint" : 
            "Press and hold Left Stick (L3) while moving to sprint";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }
    
    void StartPickupObjectStep()
    {
        string mainText = "Let's learn object interaction!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Look at the orange cube and press E to pick it up" : 
            "Look at the orange cube and press X to pick it up";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
        
        if (nailgunHighlight != null)
        {
            nailgunHighlight.HighlightOff();
        }
        
        if (cubeHighlight != null)
        {
            cubeHighlight.HighlightOn();
        }
    }
    
    void StartDropThrowStep()
    {
        string mainText = "Now you can drop or throw objects!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Press E to drop the object, or G to throw it" : 
            "Press X to drop the object, or Y to throw it";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }
    
    void StartPickupNailgunStep()
    {
        string mainText = "Time for your first tool!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Look at the glowing Nailgun and press E to pick it up" : 
            "Look at the glowing Nailgun and press X to pick it up";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
        
        if (nailgunHighlight != null)
        {
            nailgunHighlight.HighlightOn();
        }
    }
    
    void StartLoadingStep()
    {
        string mainText = "Your Nailgun needs ammunition!";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Look at the crate and press E to load nails (50 per magazine)" : 
            "Look at the crate and press X to load nails (50 per magazine)";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
        
        if (crateHighlight != null)
        {
            crateHighlight.HighlightOn();
        }
    }
    
    void StartShootingStep()
    {
        string mainText = "Ready to fire! Practice shooting.";
        string inputText = detectedDevice == InputDevice.KeyboardMouse ? 
            "Left-click to fire the Nailgun (fire 3 times to complete tutorial)" : 
            "Pull Right Trigger to fire the Nailgun (fire 3 times to complete tutorial)";
        
        UpdateTutorialText(mainText, inputText);
        textDisplayed = true;
    }
    
    void CompleteTutorial()
    {
        tutorialActive = false;
        
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        
        ShowGameHUD();
        
        if (cubeHighlight != null) cubeHighlight.HighlightOff();
        if (nailgunHighlight != null) nailgunHighlight.HighlightOff();
        if (crateHighlight != null) crateHighlight.HighlightOff();
        
        if (playerController != null)
        {
            playerController.SetInputEnabled(true);
        }
        
        Debug.Log("Tutorial Complete! You're ready for the factory.");
        PlayerPrefs.SetInt("TutorialCompleted", 1);

        // Notify ObjectiveManager to start core objectives
        ObjectiveManager objectiveManager = FindObjectOfType<ObjectiveManager>();
        if (objectiveManager != null)
        {
            objectiveManager.OnTutorialCompleted();
        }
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
    
    // Public callback methods
    public void OnJump()
    {
        hasJumped = true;
        Debug.Log("Jump detected!");
    }
    
    public void OnSprint()
    {
        hasSprinted = true;
        Debug.Log("Sprint detected!");
    }
    
    public void OnCrouch()
    {
        hasCrouched = true;
        Debug.Log("Crouch detected!");
    }
    
   public void OnObjectPickedUp()
{
    hasPickedUpObject = true;
    PlayPickupEffect();
    Debug.Log("Object picked up!");

    if (currentStep == TutorialStep.PickupObject && !stepCompleted)
    {
        stepCompleted = true;
        StartCoroutine(DelayedNextStep());
    }
}

    
    public void OnObjectDroppedOrThrown()
    {
        hasDroppedOrThrown = true;
        Debug.Log("Object dropped/thrown!");
    }
    
    public void OnNailgunPickedUp()
    {
        hasPickedUpNailgun = true;
        PlayPickupEffect();
        ShowGameHUD();
        Debug.Log("Nailgun equipped!");
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
    
    public void ShowGameHUD()
    {
        if (gameHUDCanvas != null) gameHUDCanvas.SetActive(true);
        if (ammoDisplay != null) ammoDisplay.SetActive(true);
        if (crosshair != null) crosshair.SetActive(true);
    }
    
    public void HideGameHUD()
    {
        if (gameHUDCanvas != null) gameHUDCanvas.SetActive(false);
        if (ammoDisplay != null) ammoDisplay.SetActive(false);
        if (crosshair != null) crosshair.SetActive(false);
    }
}