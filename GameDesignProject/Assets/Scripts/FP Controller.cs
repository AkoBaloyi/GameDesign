using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 0.5f;
    public float verticalLookLimit = 90f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform gunPoint;

    [Header("Crouch settings")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 4f;
    private float originalMoveSpeed;

    [Header("Sprint Settings")]
    public float sprintSpeed = 12f;
    public float sprintAcceleration = 10f;
    public float sprintDeceleration = 8f;
    private bool isSprinting = false;
    private float currentSpeed;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    public PickUpObject heldObject;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float throwUpwardBoost = 1f;

    [Header("Menu & Settings")]
    public GameObject pauseMenuPanel;
    public Image brightnessOverlay;
    public bool isPaused = false;

    [Header("Game State Reference")]
    public GameStateManager gameStateManager;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private bool isCrouching = false;
    private float horizontalRotation = 0f;
    private HighlightableObject currentHighlightedObject;


    private bool inputEnabled = true;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        originalMoveSpeed = moveSpeed;
        currentSpeed = moveSpeed;


        horizontalRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {

        if (!isPaused && inputEnabled)
        {
            HandleMovement();
            HandleLook();
            HandlePickupHighlighting();
        }


        UpdatePauseInput();


        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }
    }


    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
        if (!enabled)
        {

            moveInput = Vector2.zero;
            lookInput = Vector2.zero;
            isSprinting = false;
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (!inputEnabled || isPaused) return;
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!inputEnabled || isPaused) return;
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!inputEnabled || isPaused) return;
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (!inputEnabled || isPaused) return;

        if (context.performed)
        {
            if (!isCrouching && moveInput.magnitude > 0.1f)
            {
                isSprinting = true;
            }
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    public void onShoot(InputAction.CallbackContext context)
    {
        if (!inputEnabled || isPaused) return;
        if (context.performed)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && gunPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(gunPoint.forward * 1000f);
            }
        }
    }

    public void onCrouch(InputAction.CallbackContext context)
    {
        if (!inputEnabled || isPaused) return;

        if (context.performed)
        {
            controller.height = crouchHeight;
            isCrouching = true;
            isSprinting = false;
        }
        else if (context.canceled)
        {
            controller.height = standHeight;
            isCrouching = false;
        }
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!inputEnabled || isPaused) return;
        if (!context.performed) return;

        Debug.Log("Pickup input detected!");

        if (heldObject == null)
        {

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * pickupRange, Color.red, 1f);

            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                Debug.Log($"Raycast hit: {hit.collider.name}");

                PickUpObject pickUp = hit.collider.GetComponent<PickUpObject>();

                if (pickUp != null)
                {
                    Debug.Log($"Picking up: {hit.collider.name}");
                    pickUp.PickUp(holdPoint);
                    heldObject = pickUp;
                }
                else
                {
                    Debug.Log($"Object {hit.collider.name} doesn't have PickUpObject component");
                }
            }
            else
            {
                Debug.Log("Raycast didn't hit anything within pickup range");
            }
        }
        else
        {
            Debug.Log($"Dropping: {heldObject.name}");
            heldObject.Drop();
            heldObject = null;
        }
    }


    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!inputEnabled || isPaused) return;
        if (!context.performed) return;
        if (heldObject == null) return;

        Vector3 dir = cameraTransform.forward;
        Vector3 impulse = dir * throwForce + Vector3.up * throwUpwardBoost;

        heldObject.Throw(impulse);
        heldObject = null;
    }

    private void UpdatePauseInput()
    {

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (gameStateManager != null)
            {
                if (gameStateManager.currentState == GameStateManager.GameState.Playing)
                {
                    gameStateManager.PauseGame();
                }
                else if (gameStateManager.currentState == GameStateManager.GameState.Paused)
                {
                    gameStateManager.ResumeGame();
                }
            }
            else
            {

                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }


    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    public void HandleMovement()
    {
        if (!inputEnabled) return;

        float targetSpeed = GetTargetSpeed();
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, GetSpeedTransitionRate() * Time.deltaTime);

        if (moveInput.magnitude < 0.1f)
        {
            isSprinting = false;
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private float GetTargetSpeed()
    {
        if (isCrouching)
            return crouchSpeed;
        else if (isSprinting && moveInput.magnitude > 0.1f)
            return sprintSpeed;
        else
            return originalMoveSpeed;
    }

    private float GetSpeedTransitionRate()
    {
        if (currentSpeed < GetTargetSpeed())
            return sprintAcceleration;
        else
            return sprintDeceleration;
    }



    public void HandleLook()
    {
        if (!inputEnabled) return;

        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        horizontalRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }

    public void SetSensitivity(float value)
    {
        lookSensitivity = value;
    }

    public void SetBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1f - value;
            brightnessOverlay.color = c;
        }
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }


private void HandlePickupHighlighting()
{
    Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

    // Debug ray - you can see this in Scene view
    Debug.DrawRay(cameraTransform.position, cameraTransform.forward * pickupRange, Color.green, 0.1f);

    if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
    {
        PickUpObject pickupObject = hit.collider.GetComponent<PickUpObject>();

        if (pickupObject != null)
        {
            HighlightableObject highlightable = hit.collider.GetComponent<HighlightableObject>();

            // If we're looking at a new object, unhighlight the old one
            if (highlightable != currentHighlightedObject)
            {
                if (currentHighlightedObject != null)
                {
                    currentHighlightedObject.HighlightOff();
                }

                // Highlight the new object
                currentHighlightedObject = highlightable;
                if (currentHighlightedObject != null)
                {
                    currentHighlightedObject.HighlightOn();
                }
            }
        }
        else
        {
            // Not looking at a pickup object, turn off highlighting
            if (currentHighlightedObject != null)
            {
                currentHighlightedObject.HighlightOff();
                currentHighlightedObject = null;
            }
        }
    }
    else
    {
        // Not looking at anything, turn off highlighting
        if (currentHighlightedObject != null)
        {
            currentHighlightedObject.HighlightOff();
            currentHighlightedObject = null;
        }
    }
}
}