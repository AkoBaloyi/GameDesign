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

    public enum TutorialStep
    {
        Detecting,
        LookAround,
        Movement,
        Jump,
        Sprint,
        PickupObject,
        DropThrow,
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

    private bool tutorialActive = true;
    private bool stepCompleted = false;
    private float lookMovement = 0f;
    private Vector2 movementInput;
    private bool hasPickedUpObject = false;
    private bool hasDroppedOrThrown = false;
    private bool hasSprinted = false;
    private bool hasJumped = false;

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

        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            if (mouseDelta.magnitude > 5f)
            {
                mouseDetected = true;
            }
        }

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
            case TutorialStep.Jump:
                HandleJumpStep();
                break;
            case TutorialStep.Sprint:
                HandleSprintStep();
                break;
            case TutorialStep.PickupObject:
                HandlePickupObjectStep();
                break;
            case TutorialStep.DropThrow:
                HandleDropThrowStep();
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
            case TutorialStep.Jump:
                StartJumpStep();
                break;
            case TutorialStep.Sprint:
                StartSprintStep();
                break;
            case TutorialStep.PickupObject:
                StartPickupObjectStep();
                break;
            case TutorialStep.DropThrow:
                StartDropThrowStep();
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

        ObjectiveManager objectiveManager = FindObjectOfType<ObjectiveManager>();
        if (objectiveManager != null)
        {
            objectiveManager.OnTutorialCompleted();
        }

        ClearObjectiveManager clearManager = FindObjectOfType<ClearObjectiveManager>();
        if (clearManager != null)
        {
            clearManager.OnTutorialComplete();
            Debug.Log("[TutorialManager] Notified ClearObjectiveManager!");
        }

        InspectionHintUI hintUI = FindObjectOfType<InspectionHintUI>();
        if (hintUI != null)
        {
            hintUI.OnTutorialComplete();
            Debug.Log("[TutorialManager] Started hint system!");
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

    public void OnNailgunPickedUp() { /* Tutorial no longer uses nailgun */ }
    public void OnNailsLoaded() { /* Tutorial no longer uses nails */ }
    public void OnNailgunFired() { /* Tutorial no longer uses shooting */ }
    
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