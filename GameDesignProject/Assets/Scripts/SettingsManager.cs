using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;




public class SettingsManager : MonoBehaviour
{
    [Header("Player Reference")]
    public FPController playerController;
    
    [Header("Sensitivity Settings")]
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityValueText;
    public float minSensitivity = 0.2f;
    public float maxSensitivity = 2.0f;
    public float defaultSensitivity = 1.0f;
    
    [Header("Brightness Settings")]
    public Slider brightnessSlider;
    public TextMeshProUGUI brightnessValueText;
    public Image brightnessOverlay;
    public float defaultBrightness = 0.7f;
    
    [Header("Volume Settings")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public TextMeshProUGUI masterVolumeText;
    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI sfxVolumeText;
    public AudioMixer audioMixer;
    
    [Header("Graphics Settings")]
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;
    
    private Resolution[] resolutions;
    
    private void Start()
    {

        if (playerController == null)
        {
            playerController = FindObjectOfType<FPController>();
        }

        LoadSettings();

        SetupListeners();

        SetupResolutions();

        ApplyAllSettings();
    }
    
    private void SetupListeners()
    {
        if (sensitivitySlider != null)
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
            
        if (brightnessSlider != null)
            brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
            
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            
        if (qualityDropdown != null)
            qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
            
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
            
        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }
    
    private void SetupResolutions()
    {
        if (resolutionDropdown == null) return;
        
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        
        System.Collections.Generic.List<string> options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void OnSensitivityChanged(float value)
    {
        if (playerController != null)
        {
            playerController.SetSensitivity(value);
        }
        
        if (sensitivityValueText != null)
        {
            sensitivityValueText.text = value.ToString("F2");
        }
        
        PlayerPrefs.SetFloat("Sensitivity", value);
    }

    public void OnBrightnessChanged(float value)
    {
        if (playerController != null)
        {
            playerController.SetBrightness(value);
        }
        
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1f - value;
            brightnessOverlay.color = c;
        }
        
        if (brightnessValueText != null)
        {
            brightnessValueText.text = Mathf.RoundToInt(value * 100) + "%";
        }
        
        PlayerPrefs.SetFloat("Brightness", value);
    }

    public void OnMasterVolumeChanged(float value)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        }
        
        if (masterVolumeText != null)
        {
            masterVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
        }
        
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    
    public void OnMusicVolumeChanged(float value)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        }
        
        if (musicVolumeText != null)
        {
            musicVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
        }
        
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    
    public void OnSFXVolumeChanged(float value)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        }
        
        if (sfxVolumeText != null)
        {
            sfxVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
        }
        
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void OnQualityChanged(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }
    
    public void OnFullscreenChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }
    
    public void OnResolutionChanged(int resolutionIndex)
    {
        if (resolutions != null && resolutionIndex < resolutions.Length)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        }
    }

    private void LoadSettings()
    {

        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", defaultSensitivity);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = sensitivity;
        }

        float brightness = PlayerPrefs.GetFloat("Brightness", defaultBrightness);
        if (brightnessSlider != null)
        {
            brightnessSlider.value = brightness;
        }

        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = masterVolume;
        }
        
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = musicVolume;
        }
        
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = sfxVolume;
        }

        int qualityLevel = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        if (qualityDropdown != null)
        {
            qualityDropdown.value = qualityLevel;
        }
        
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = fullscreen;
        }
        
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        if (resolutionDropdown != null && resolutions != null && resolutionIndex < resolutions.Length)
        {
            resolutionDropdown.value = resolutionIndex;
        }
    }
    
    private void ApplyAllSettings()
    {

        if (sensitivitySlider != null)
        {
            OnSensitivityChanged(sensitivitySlider.value);
        }

        if (brightnessSlider != null)
        {
            OnBrightnessChanged(brightnessSlider.value);
        }

        if (masterVolumeSlider != null)
        {
            OnMasterVolumeChanged(masterVolumeSlider.value);
        }
        if (musicVolumeSlider != null)
        {
            OnMusicVolumeChanged(musicVolumeSlider.value);
        }
        if (sfxVolumeSlider != null)
        {
            OnSFXVolumeChanged(sfxVolumeSlider.value);
        }
    }
    
    public void ResetToDefaults()
    {
        if (sensitivitySlider != null)
            sensitivitySlider.value = defaultSensitivity;
            
        if (brightnessSlider != null)
            brightnessSlider.value = defaultBrightness;
            
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = 0.8f;
            
        if (musicVolumeSlider != null)
            musicVolumeSlider.value = 0.7f;
            
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = 0.8f;
            
        if (qualityDropdown != null)
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = true;
    }
}
