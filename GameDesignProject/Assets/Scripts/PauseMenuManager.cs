using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;




public class PauseMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    public GameObject confirmQuitPanel;
    
    [Header("References")]
    public FPController playerController;
    public SettingsManager settingsManager;
    
    [Header("Buttons")]
    public Button resumeButton;
    public Button settingsButton;
    public Button mainMenuButton;
    public Button quitButton;
    public Button settingsBackButton;
    public Button confirmQuitYesButton;
    public Button confirmQuitNoButton;
    
    private bool isPaused = false;
    
    private void Start()
    {

        if (playerController == null)
        {
            playerController = FindObjectOfType<FPController>();
        }

        SetupButtons();

        HideAllPanels();
    }
    
    private void SetupButtons()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
            
        if (settingsButton != null)
            settingsButton.onClick.AddListener(ShowSettings);
            
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ShowConfirmQuit);
            
        if (quitButton != null)
            quitButton.onClick.AddListener(ShowConfirmQuit);
            
        if (settingsBackButton != null)
            settingsBackButton.onClick.AddListener(ShowPauseMenu);
            
        if (confirmQuitYesButton != null)
            confirmQuitYesButton.onClick.AddListener(QuitToMainMenu);
            
        if (confirmQuitNoButton != null)
            confirmQuitNoButton.onClick.AddListener(ShowPauseMenu);
    }
    
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {

                if (settingsPanel != null && settingsPanel.activeSelf)
                {
                    ShowPauseMenu();
                }

                else if (confirmQuitPanel != null && confirmQuitPanel.activeSelf)
                {
                    ShowPauseMenu();
                }

                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        ShowPauseMenu();

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null)
        {
            playerController.SetInputEnabled(false);
        }
        
        Debug.Log("[PauseMenu] Game paused");
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        HideAllPanels();

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerController != null)
        {
            playerController.SetInputEnabled(true);
        }
        
        Debug.Log("[PauseMenu] Game resumed");
    }
    
    public void ShowPauseMenu()
    {
        HideAllPanels();
        
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
    }
    
    public void ShowSettings()
    {
        HideAllPanels();
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }

        if (settingsManager != null && playerController != null)
        {
            settingsManager.playerController = playerController;
        }
    }
    
    public void ShowConfirmQuit()
    {
        HideAllPanels();
        
        if (confirmQuitPanel != null)
        {
            confirmQuitPanel.SetActive(true);
        }
    }
    
    public void QuitToMainMenu()
    {
        Debug.Log("[PauseMenu] Returning to main menu...");

        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
    
    public void QuitGame()
    {
        Debug.Log("[PauseMenu] Quitting game...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    private void HideAllPanels()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
            
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
            
        if (confirmQuitPanel != null)
            confirmQuitPanel.SetActive(false);
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
