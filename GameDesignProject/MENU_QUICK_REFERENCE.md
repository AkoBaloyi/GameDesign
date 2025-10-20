# Menu System Quick Reference

## 📦 Scripts Created

| Script | Purpose | Location |
|--------|---------|----------|
| `SettingsManager.cs` | Manages all game settings | Main Menu & Pause Menu |
| `PauseMenuManager.cs` | Handles pause menu | Game Scene |
| `SceneTransitionManager.cs` | Loading screens | Persistent (all scenes) |
| `MainMenuManager.cs` | Main menu (existing) | Main Menu Scene |

---

## 🎯 Quick Setup Summary

### **Main Menu (30 min)**
1. Create MainMenu scene
2. Build UI hierarchy (MainMenuPanel + SettingsPanel)
3. Add MainMenuManager + SettingsManager
4. Wire buttons and sliders
5. Test Play/Settings/Quit

### **Pause Menu (20 min)**
1. Create pause UI in game scene
2. Add PauseMenuManager + SettingsManager
3. Wire buttons
4. Test ESC key pause/resume

### **Scene Transitions (15 min)**
1. Create LoadingScreen UI
2. Add SceneTransitionManager (persistent)
3. Wire loading UI elements
4. Test scene loading

---

## 🎮 Player Experience

```
MAIN MENU
  ├── PLAY → Loading Screen → Game
  ├── SETTINGS → Adjust → Save → Back
  └── QUIT → Exit

IN-GAME
  ├── ESC → PAUSE MENU
  │     ├── RESUME → Continue playing
  │     ├── SETTINGS → Adjust → Back
  │     ├── MAIN MENU → Confirm → Load MainMenu
  │     └── QUIT → Confirm → Exit
  └── Playing...
```

---

## ⚙️ Settings Features

### **Sensitivity**
- Range: 0.2 - 2.0
- Default: 1.0
- Updates: FPController.lookSensitivity
- Saved: PlayerPrefs

### **Brightness**
- Range: 0 - 100%
- Default: 70%
- Updates: Overlay alpha
- Saved: PlayerPrefs

### **Volume (3 sliders)**
- Master, Music, SFX
- Range: 0 - 100%
- Updates: AudioMixer
- Saved: PlayerPrefs

### **Graphics**
- Quality: Low/Medium/High
- Fullscreen: Toggle
- Resolution: Dropdown
- Saved: PlayerPrefs

---

## 🔑 Important Methods

### **SettingsManager**
```csharp
OnSensitivityChanged(float value)  // Updates player sensitivity
OnBrightnessChanged(float value)   // Updates screen brightness
OnMasterVolumeChanged(float value) // Updates master volume
ResetToDefaults()                  // Resets all settings
```

### **PauseMenuManager**
```csharp
PauseGame()      // Pauses game, shows menu
ResumeGame()     // Resumes game, hides menu
ShowSettings()   // Opens settings panel
QuitToMainMenu() // Returns to main menu
```

### **SceneTransitionManager**
```csharp
LoadScene(string sceneName)  // Load with loading screen
LoadScene(int sceneIndex)    // Load with loading screen
QuickLoadScene(string name)  // Load instantly (no screen)
```

---

## 🎨 UI Hierarchy Templates

### **Main Menu**
```
Canvas
├── MainMenuPanel
│   ├── Title
│   ├── PlayButton
│   ├── SettingsButton
│   └── QuitButton
└── SettingsPanel
    ├── SensitivitySlider
    ├── BrightnessSlider
    ├── VolumeSliders (x3)
    ├── QualityDropdown
    └── BackButton
```

### **Pause Menu**
```
Canvas
├── PauseMenuPanel
│   ├── ResumeButton
│   ├── SettingsButton
│   ├── MainMenuButton
│   └── QuitButton
├── SettingsPanel (same as main menu)
└── ConfirmQuitPanel
    ├── YesButton
    └── NoButton
```

### **Loading Screen**
```
Canvas
└── LoadingScreen
    ├── Background
    ├── ProgressBar
    ├── LoadingText
    └── TipText
```

---

## 🔧 Common Customizations

### **Change Loading Tips:**
Edit `SceneTransitionManager.loadingTips` array

### **Change Default Sensitivity:**
Edit `SettingsManager.defaultSensitivity`

### **Change Fade Duration:**
Edit `SceneTransitionManager.fadeDuration`

### **Add New Setting:**
1. Add slider/dropdown to UI
2. Add field to SettingsManager
3. Add OnValueChanged method
4. Add to LoadSettings()
5. Add to ApplyAllSettings()

---

## 🐛 Quick Fixes

| Problem | Solution |
|---------|----------|
| ESC doesn't pause | Check PauseMenuManager is in scene |
| Settings don't save | Check PlayerPrefs calls |
| Volume doesn't work | Check AudioMixer is assigned |
| Scene won't load | Check Build Settings |
| Cursor stuck | Check Cursor.lockState |

---

## ✅ Testing Shortcuts

**Main Menu:**
- Play → Should load game
- Settings → Should open settings
- Quit → Should exit

**Pause Menu:**
- ESC → Should pause
- ESC again → Should resume
- Settings → Should work in pause

**Settings:**
- Move sliders → Should update immediately
- Restart game → Settings should persist

---

## 📋 Build Settings Order

Make sure scenes are in this order:
1. **MainMenu** (index 0)
2. **Game** (index 1)
3. **LoadingScreen** (optional, if separate)

---

This is your complete menu system! 🎮
