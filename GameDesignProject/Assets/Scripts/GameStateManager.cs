using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager : MonoBehaviour
{
    [Header("Game State References")]
    public GameObject player;
    public GameObject environment;     public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject gameUI; 
    
    [Header("Player References")]
    public FPController fpController;
    public PlayerInput playerInput;
    
    [Header("Lighting References")]
    public Light directionalLight; 
    public GameObject[] additionalLights; 
    
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused
    }
    
    public GameState currentState = GameState.MainMenu;
    
    private void Awake()
    {
        
        SetGameState(GameState.MainMenu);
    }
    
    private void Start()
    {
        
        SetupProperLighting();
    }
    
    public void SetGameState(GameState newState)
    {
        currentState = newState;
        
        switch (currentState)
        {
            case GameState.MainMenu:
                SetMainMenuState();
                break;
            case GameState.Playing:
                SetPlayingState();
                break;
            case GameState.Paused:
                SetPausedState();
                break;
        }
    }
    
    private void SetMainMenuState()
    {
        
        if (player != null) player.SetActive(false);
        if (environment != null) environment.SetActive(false);
        
        
        if (playerInput != null) playerInput.enabled = false;
        if (fpController != null) fpController.enabled = false;
        
        
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (gameUI != null) gameUI.SetActive(false);
        
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        
        Debug.Log("Game State: Main Menu");
    }
    
    private void SetPlayingState()
    {
        if (player != null) player.SetActive(true);
        if (environment != null) environment.SetActive(true);
        
        if (playerInput != null) playerInput.enabled = true;
        if (fpController != null) 
        {
            fpController.enabled = true;
            fpController.isPaused = false;
        }
        
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (gameUI != null) gameUI.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        
        Debug.Log("Game State: Playing");
    }
    
    private void SetPausedState()
    {
        if (player != null) player.SetActive(true);
        if (environment != null) environment.SetActive(true);
        
        if (fpController != null) fpController.isPaused = true;
        
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        if (gameUI != null) gameUI.SetActive(true); 
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        
        Debug.Log("Game State: Paused");
    }
    
    public void StartGame()
    {
        SetGameState(GameState.Playing);
    }
    
    public void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            SetGameState(GameState.Paused);
        }
    }
    
    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            SetGameState(GameState.Playing);
        }
    }
    
    public void ReturnToMainMenu()
    {
        SetGameState(GameState.MainMenu);
    }
    
    private void SetupProperLighting()
    {
        if (directionalLight == null)
        {
            directionalLight = FindObjectOfType<Light>();
        }
        
        if (directionalLight != null)
        {
            directionalLight.type = LightType.Directional;
            directionalLight.color = Color.white;
            directionalLight.intensity = 1.2f; 
            directionalLight.shadows = LightShadows.Soft;
            
            directionalLight.transform.rotation = Quaternion.Euler(45f, 30f, 0f);
        }
        
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.5f, 0.7f, 1f, 1f); 
        RenderSettings.ambientEquatorColor = new Color(0.4f, 0.4f, 0.4f, 1f);
        RenderSettings.ambientGroundColor = new Color(0.2f, 0.2f, 0.2f, 1f); 
        RenderSettings.ambientIntensity = 0.3f;
        
      
        
      
    }
    
  
    private void ToggleAdditionalLights(bool enable)
    {
        if (additionalLights != null)
        {
            foreach (GameObject lightObj in additionalLights)
            {
                if (lightObj != null)
                {
                    lightObj.SetActive(enable);
                }
            }
        }
    }
}