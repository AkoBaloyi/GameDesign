

# Complete Menu System Setup Guide
## Main Menu + Pause Menu + Settings

---

## 🎯 Overview

This system provides:
- **Main Menu** - Play, Settings, Quit
- **Pause Menu** - Resume, Settings, Main Menu, Quit
- **Settings** - Sensitivity, Brightness, Volume, Graphics
- **Scene Transitions** - Smooth loading screens
- **Save/Load** - Settings persist between sessions

---

## 📦 What's Been Created

### New Scripts:
- ✅ `SettingsManager.cs` - Complete settings system
- ✅ `PauseMenuManager.cs` - Enhanced pause menu
- ✅ `SceneTransitionManager.cs` - Loading screens

### Existing Scripts (You Already Have):
- ✅ `MainMenuManager.cs` - Main menu (already exists)
- ✅ `GameStateManager.cs` - Game state management

---

## 🛠️ Unity Setup

### **PART 1: Main Menu Scene**

#### **Step 1: Create Main Menu Scene**

1. **File → New Scene**
2. **Save as:** `MainMenu`
3. **Add to Build Settings:**
   - File → Build Settings
   - Add Open Scenes
   - Make sure MainMenu is **index 0**

#### **Step 2: Create Main Menu UI**

Create this hierarchy:

```
Canvas (Screen Space - Overlay)
├── MainMenuPanel
│   ├── Background (Image - dark)
│   ├── Title (TextMeshPro - "FACTORY POWER")
│   ├── PlayButton (Button - "PLAY")
│   ├── SettingsButton (Button - "SETTINGS")
│   └── QuitButton (Button - "QUIT")
│
├── SettingsPanel (Initially inactive)
│   ├── Background (Image - dark)
│   ├── Title (TextMeshPro - "SETTINGS")
│   │
│   ├── SensitivitySection
│   │   ├── Label (TextMeshPro - "Mouse Sensitivity")
│   │   ├── SensitivitySlider (Slider - 0.2 to 2.0)
│   │   └── ValueText (TextMeshPro - "1.00")
│   │
│   ├── BrightnessSection
│   │   ├── Label (TextMeshPro - "Brightness")
│   │   ├── BrightnessSlider (Slider - 0 to 1)
│   │   └── ValueText (TextMeshPro - "70%")
│   │
│   ├── VolumeSection
│   │   ├── MasterVolumeSlider (Slider - "Master")
│   │   ├── MusicVolumeSlider (Slider - "Music")
│   │   └── SFXVolumeSlider (Slider - "SFX")
│   │
│   ├── GraphicsSection
│   │   ├── QualityDropdown (Dropdown - Low/Medium/High)
│   │   ├── FullscreenToggle (Toggle - "Fullscreen")
│   │   └── ResolutionDropdown (Dropdown)
│   │
│   ├── BackButton (Button - "BACK")
│   └── ResetButton (Button - "RESET TO DEFAULTS")
│
└── BrightnessOverlay (Image - black, full screen, alpha 0.3)
```

#### **Step 3: Setup MainMenuManager**

1. **Create empty GameObject** named "MenuManager"
2. **Add Component** → `MainMenuManager` (you already have this)
3. **Wire up references:**
   - **Main Menu Panel**: Drag MainMenuPanel
   - **Settings Panel**: Drag SettingsPanel
   - **Brightness Overlay**: Drag BrightnessOverlay

4. **Wire up buttons:**
   - **PlayButton** → OnClick → MenuManager → `StartGame()`
   - **SettingsButton** → OnClick → MenuManager → `ShowSettings()`
   - **QuitButton** → OnClick → MenuManager → `ExitGame()`
   - **BackButton** → OnClick → MenuManager → `ShowMainMenu()`

#### **Step 4: Setup SettingsManager**

1. **Add Component to MenuManager** → `SettingsManager`
2. **Wire up all sliders and UI elements** (see SettingsPanel structure above)
3. **Configure default values:**
   - Min Sensitivity: 0.2
   - Max Sensitivity: 2.0
   - Default Sensitivity: 1.0
   - Default Brightness: 0.7

---

### **PART 2: Pause Menu (In Game Scene)**

#### **Step 1: Create Pause Menu UI**

In your game scene, create:

```
Canvas (Screen Space - Overlay)
├── PauseMenuPanel (Initially inactive)
│   ├── Background (Image - dark semi-transparent)
│   ├── Title (TextMeshPro - "PAUSED")
│   ├── ResumeButton (Button - "RESUME")
│   ├── SettingsButton (Button - "SETTINGS")
│   ├── MainMenuButton (Button - "MAIN MENU")
│   └── QuitButton (Button - "QUIT GAME")
│
├── SettingsPanel (Initially inactive)
│   └── [Same structure as Main Menu Settings]
│
├── ConfirmQuitPanel (Initially inactive)
│   ├── Background (Image - dark)
│   ├── Message (TextMeshPro - "Return to Main Menu?")
│   ├── YesButton (Button - "YES")
│   └── NoButton (Button - "NO")
│
└── BrightnessOverlay (Image - black, full screen)
```

#### **Step 2: Setup PauseMenuManager**

1. **Create empty GameObject** named "PauseMenuManager"
2. **Add Component** → `PauseMenuManager`
3. **Wire up references:**
   - **Pause Menu Panel**: Drag PauseMenuPanel
   - **Settings Panel**: Drag SettingsPanel
   - **Confirm Quit Panel**: Drag ConfirmQuitPanel
   - **Player Controller**: Drag your Player GameObject

4. **Add SettingsManager** to same GameObject
5. **Wire SettingsManager reference** in PauseMenuManager

6. **Wire up buttons:**
   - **ResumeButton** → PauseMenuManager → `ResumeGame()`
   - **SettingsButton** → PauseMenuManager → `ShowSettings()`
   - **MainMenuButton** → PauseMenuManager → `ShowConfirmQuit()`
   - **QuitButton** → PauseMenuManager → `ShowConfirmQuit()`
   - **YesButton** → PauseMenuManager → `QuitToMainMenu()`
   - **NoButton** → PauseMenuManager → `ShowPauseMenu()`

---

### **PART 3: Scene Transition System**

#### **Step 1: Create Loading Screen**

Create a **new scene** named "LoadingScreen" or add to MainMenu:

```
Canvas (Screen Space - Overlay)
└── LoadingScreen (Panel - full screen, initially inactive)
    ├── Background (Image - black)
    ├── ProgressBar (Slider)
    ├── LoadingText (TextMeshPro - "Loading...")
    └── TipText (TextMeshPro - "Tip: Press E to pick up objects")
```

#### **Step 2: Setup SceneTransitionManager**

1. **Create empty GameObject** named "SceneTransitionManager"
2. **Add Component** → `SceneTransitionManager`
3. **Wire up references:**
   - **Loading Screen**: Drag LoadingScreen panel
   - **Progress Bar**: Drag ProgressBar slider
   - **Loading Text**: Drag TipText
   - **Loading Canvas Group**: Add CanvasGroup to LoadingScreen, drag here

4. **This GameObject persists between scenes** (DontDestroyOnLoad)

---

### **PART 4: Audio Mixer (Optional but Recommended)**

#### **Step 1: Create Audio Mixer**

1. **Right-click in Project** → Create → Audio Mixer
2. **Name it:** "MainAudioMixer"
3. **Open Audio Mixer window** (Window → Audio → Audio Mixer)

#### **Step 2: Create Groups**

Create these groups:
- **Master**
  - Music
  - SFX

#### **Step 3: Expose Parameters**

1. **Right-click Master volume** → "Expose 'Volume' to script"
2. **Rename exposed parameter** to "MasterVolume"
3. **Repeat for Music** → "MusicVolume"
4. **Repeat for SFX** → "SFXVolume"

#### **Step 4: Wire to SettingsManager**

- **Audio Mixer**: Drag MainAudioMixer into SettingsManager

---

## 🎮 How It Works

### **Main Menu Flow:**
```
Game Starts
  ↓
Main Menu Scene Loads
  ↓
Player clicks PLAY
  ↓
SceneTransitionManager loads Game scene
  ↓
Loading screen shows with progress bar
  ↓
Game scene loads
  ↓
Player plays game
```

### **Pause Menu Flow:**
```
Player presses ESC
  ↓
PauseMenuManager pauses game
  ↓
Time.timeScale = 0
  ↓
Cursor unlocked
  ↓
Player can:
  - Resume (ESC or Resume button)
  - Adjust Settings
  - Return to Main Menu
  - Quit Game
```

### **Settings Flow:**
```
Player adjusts slider
  ↓
SettingsManager updates game
  ↓
PlayerPrefs saves value
  ↓
Setting persists between sessions
```

---

## ⚙️ Settings Available

| Setting | Range | Default | Saved |
|---------|-------|---------|-------|
| **Mouse Sensitivity** | 0.2 - 2.0 | 1.0 | ✅ |
| **Brightness** | 0 - 100% | 70% | ✅ |
| **Master Volume** | 0 - 100% | 80% | ✅ |
| **Music Volume** | 0 - 100% | 70% | ✅ |
| **SFX Volume** | 0 - 100% | 80% | ✅ |
| **Graphics Quality** | Low/Med/High | Medium | ✅ |
| **Fullscreen** | On/Off | On | ✅ |
| **Resolution** | Various | Native | ✅ |

---

## 🔑 Key Bindings

| Key | Action |
|-----|--------|
| **ESC** | Pause/Unpause game |
| **ESC** (in settings) | Back to pause menu |
| **ESC** (in confirm) | Cancel and return |

---

## 🎨 Visual Customization

### **Main Menu Styling:**
- Use dark background with factory theme
- Add factory logo/title
- Use industrial fonts
- Add subtle animations (button hover effects)

### **Pause Menu Styling:**
- Semi-transparent background (blur effect if possible)
- Keep UI minimal and clean
- Match game's color scheme

### **Loading Screen:**
- Full black background
- Progress bar with factory theme
- Rotating loading icon (optional)
- Random helpful tips

---

## 🐛 Troubleshooting

### Settings don't save:
- Check PlayerPrefs are being called
- Check platform supports PlayerPrefs
- Try PlayerPrefs.Save() after each change

### Pause menu doesn't show:
- Check ESC key isn't being captured elsewhere
- Check PauseMenuManager is in scene
- Check panels are assigned

### Scene transition doesn't work:
- Check scenes are in Build Settings
- Check SceneTransitionManager persists (DontDestroyOnLoad)
- Check scene names/indices are correct

### Volume sliders don't work:
- Check Audio Mixer is assigned
- Check exposed parameters match names exactly
- Check AudioSource components use mixer groups

---

## 📋 Testing Checklist

### Main Menu:
- [ ] Play button loads game
- [ ] Settings button shows settings
- [ ] Quit button exits game
- [ ] Settings persist after restart

### Pause Menu:
- [ ] ESC pauses game
- [ ] Time stops when paused
- [ ] Cursor appears when paused
- [ ] Resume works (ESC or button)
- [ ] Settings accessible from pause
- [ ] Can return to main menu
- [ ] Confirm quit dialog works

### Settings:
- [ ] Sensitivity slider works
- [ ] Brightness slider works
- [ ] Volume sliders work
- [ ] Quality dropdown works
- [ ] Fullscreen toggle works
- [ ] Resolution dropdown works
- [ ] Reset to defaults works
- [ ] Settings save between sessions

### Scene Transitions:
- [ ] Loading screen appears
- [ ] Progress bar fills
- [ ] Loading tips show
- [ ] Smooth fade in/out
- [ ] No errors in console

---

## ✅ Final Steps

1. **Test main menu** → Play → Game loads
2. **Test pause menu** → ESC → Pause works
3. **Test settings** → Adjust → Changes apply
4. **Test persistence** → Restart → Settings saved
5. **Test transitions** → Smooth loading
6. **Build and test** → Everything works in build

---

## 🚀 You're Done!

You now have a complete professional menu system with:
- ✅ Main menu
- ✅ Pause menu
- ✅ Full settings (sensitivity, brightness, volume, graphics)
- ✅ Smooth scene transitions
- ✅ Settings persistence
- ✅ Confirm dialogs

**Now you can continue with the gameplay loop!** 🎮
