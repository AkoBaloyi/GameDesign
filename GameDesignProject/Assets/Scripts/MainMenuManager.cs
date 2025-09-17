using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    
    [Header("Game State Reference")]
    public GameStateManager gameStateManager;

    [Header("Sensitivity Settings")]
    public RawImage[] sensitivitySteps; 
    [Range(1, 9)] public int currentSensitivityLevel = 5; 
    public FPController playerController;

    [Header("Brightness Settings")]
    public Scrollbar brightnessScrollbar;
    public Image brightnessOverlay; 
    private void Start()
    {
        
        ShowMainMenu();

       
        UpdateSensitivityVisuals();
        if (playerController != null)
        {
            playerController.lookSensitivity = currentSensitivityLevel * 0.2f; 
        }

        
        if (brightnessScrollbar != null)
        {
            brightnessScrollbar.onValueChanged.AddListener(SetBrightness);
            
            brightnessScrollbar.value = 0.7f;
            SetBrightness(0.7f);
        }
    }

       public void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void StartGame()
    {
        if (gameStateManager != null)
        {
           
            gameStateManager.StartGame();
        }
        else
        {
            
            Debug.LogWarning("GameStateManager not assigned. Loading scene instead.");
            SceneManager.LoadScene("Game");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited"); 
    }

   
    public void ResumeGame()
    {
        if (gameStateManager != null)
        {
            gameStateManager.ResumeGame();
        }
    }
    
    public void ReturnToMainMenu()
    {
        if (gameStateManager != null)
        {
            gameStateManager.ReturnToMainMenu();
        }
    }

   
    public void IncreaseSensitivity()
    {
        if (currentSensitivityLevel < 9)
        {
            currentSensitivityLevel++;
            ApplySensitivity();
        }
    }

    public void DecreaseSensitivity()
    {
        if (currentSensitivityLevel > 1)
        {
            currentSensitivityLevel--;
            ApplySensitivity();
        }
    }

    private void ApplySensitivity()
    {
        if (playerController != null)
        {
            
            playerController.SetSensitivity(currentSensitivityLevel * 0.2f);
        }
        UpdateSensitivityVisuals();
    }

    private void UpdateSensitivityVisuals()
    {
        if (sensitivitySteps != null)
        {
            for (int i = 0; i < sensitivitySteps.Length && i < 9; i++)
            {
                if (sensitivitySteps[i] != null)
                {
                    sensitivitySteps[i].color = (i < currentSensitivityLevel) ? Color.green : Color.gray;
                }
            }
        }
    }

    
    public void SetBrightness(float value)
    {
        
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1f - value; 
            brightnessOverlay.color = c;
        }
        
        
        if (playerController != null)
        {
            playerController.SetBrightness(value);
        }
    }
    
   
    private void OnValidate()
    {
        
        currentSensitivityLevel = Mathf.Clamp(currentSensitivityLevel, 1, 9);
    }
}