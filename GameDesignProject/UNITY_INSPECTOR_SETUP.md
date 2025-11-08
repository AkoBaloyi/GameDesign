# Unity Inspector Setup Guide

## Step-by-Step Setup for All Components

Follow these instructions exactly to set up your game scene.

---

## 1. ObjectiveManager Setup

### Find ObjectiveManager
1. In **Hierarchy**, find GameObject with **ObjectiveManager** script
   - If it doesn't exist, create one: Right-click → Create Empty → Name it "ObjectiveManager"
   - Add Component → ObjectiveManager script

### Assign References in Inspector

**UI - Objective Display:**
- `objectiveText` → Drag the TextMeshProUGUI that shows objective text
- `objectiveBanner` → Drag the GameObject that contains the banner panel
- `bannerCanvasGroup` → Drag the CanvasGroup component on the banner
- `bannerDisplayDuration` → Set to 3

**UI - Tracker:**
- `trackerText` → Drag the TextMeshProUGUI that shows progress (e.g., "Power Cells: 0/1")
- `trackerPanel` → Drag the GameObject that contains the tracker UI

**Audio:**
- `audioSource` → Drag AudioSource component (or add one to ObjectiveManager)
- `advanceObjectiveSfx` → Drag an audio clip for objective completion sound

**Signals / Hooks (UnityEvents):**
These connect to other systems. Click the + button for each:

- `onPowerCellInserted`:
  - Click + button
  - Drag **LightsController** GameObject into the object field
  - Select function: `LightsController.ActivateLights()`

- `onStartGlowingPath`:
  - Click + button
  - Drag **LightsController** GameObject into the object field
  - Select function: `LightsController.ActivateGlowingPath()`

- `onWin`:
  - Click + button
  - Drag **WinStateManager** GameObject into the object field
  - Select function: `WinStateManager.ShowWinScreen()`

---

## 2. Power Cell Setup

### Find Power Cell
1. In **Hierarchy**, find your Power Cell GameObject
   - Should be a visible object in the scene (orange glowing cube)

### Required Components
Make sure it has these components (Add Component if missing):
- ✅ **PowerCell** script
- ✅ **PickUpObject** script
- ✅ **HighlightableObject** script (optional but recommended)
- ✅ **Rigidbody** (NOT kinematic, Use Gravity = true)
- ✅ **Collider** (Box, Sphere, or Capsule)

### Assign References in PowerCell Script

**References:**
- `objectiveManager` → Drag the **ObjectiveManager** GameObject from Hierarchy
- `playerHand` → Leave empty (will be set by player when picked up)

**Visual Feedback:**
- `glowEffect` → Drag any particle system child object (optional)
- `cellColor` → Set to Orange (RGB: 255, 128, 0)
- `rotationSpeed` → 30
- `bobSpeed` → 1
- `bobHeight` → 0.2

**Audio:**
- `audioSource` → Add AudioSource component to Power Cell, then drag it here
- `pickupSound` → Drag an audio clip for pickup sound

---

## 3. Power Bay Setup

### Find Power Bay
1. In **Hierarchy**, find your Power Bay GameObject
   - This is where the player inserts the power cell

### Create Socket Point
1. Right-click on Power Bay → Create Empty
2. Name it "SocketPoint"
3. Position it where you want the power cell to snap to
4. This should be inside/on the power bay model

### Required Components
Make sure Power Bay has:
- ✅ **PowerBay** script
- ✅ **Collider** (for detection range)

### Assign References in PowerBay Script

**References:**
- `objectiveManager` → Drag **ObjectiveManager** GameObject from Hierarchy
- `socketPoint` → Drag the **SocketPoint** child object you just created

**UI:**
- `promptUI` → Create a World Space Canvas above Power Bay:
  1. Right-click Power Bay → UI → Canvas
  2. Set Canvas to "World Space"
  3. Scale it down (try 0.01, 0.01, 0.01)
  4. Position it above the bay (Y + 2)
  5. Add TextMeshProUGUI as child
  6. Set text to "Press F to Insert Power Cell"
  7. Make text large and bright (cyan or yellow)
  8. Drag the Canvas GameObject to `promptUI` field
- `promptText` → Drag the TextMeshProUGUI component
- `promptMessage` → "Press F to Insert Power Cell"

**Visual Feedback:**
- `slotIndicator` → Optional: A visual marker showing where to insert
- `inactiveMaterial` → Material when not activated (gray/dark)
- `activeMaterial` → Material when activated (bright/glowing)
- `bayRenderer` → Drag the Renderer component from Power Bay or its child
- `insertEffect` → Optional: Particle system for insertion effect

**Audio:**
- `audioSource` → Add AudioSource to Power Bay, drag it here
- `insertSound` → Audio clip for insertion
- `activationSound` → Audio clip for activation

**Detection:**
- `detectionRange` → Set to **5** (or higher if needed)
- `playerLayer` → Click dropdown, select "Player" layer
  - If "Player" doesn't exist:
    1. Edit → Project Settings → Tags and Layers
    2. Add "Player" to User Layer 6
    3. Select your Player GameObject
    4. Set Layer to "Player"

---

## 4. Factory Console Setup

### Find Factory Console
1. In **Hierarchy**, find your Factory Console GameObject
   - This is the final objective

### Required Components
- ✅ **FactoryConsole** script
- ✅ **Collider** (for detection)

### Assign References in FactoryConsole Script

**References:**
- `objectiveManager` → Drag **ObjectiveManager** GameObject from Hierarchy

**UI:**
- `promptUI` → Create World Space Canvas above Console (same as Power Bay):
  1. Right-click Console → UI → Canvas
  2. Set to World Space
  3. Scale: 0.01, 0.01, 0.01
  4. Position above console
  5. Add TextMeshProUGUI: "Press F to Activate Console"
  6. Drag Canvas to this field
- `promptText` → Drag the TextMeshProUGUI
- `promptMessage` → "Press F to Activate Console"

**Visual Feedback:**
- `screenDisplay` → Optional: Screen object that lights up
- `inactiveMaterial` → Material when inactive
- `activeMaterial` → Material when active
- `consoleRenderer` → Drag Renderer component
- `consoleLight` → Add a Light component to Console, drag it here
  - Set light to disabled initially
  - Color: Yellow (when ready), Green (when activated)
- `activationEffect` → Optional: Particle system

**Audio:**
- `audioSource` → Add AudioSource, drag here
- `activationSound` → Audio clip for activation
- `successSound` → Audio clip for success

**Detection:**
- `detectionRange` → Set to **5**
- `playerLayer` → Select "Player" layer

**Activation:**
- `activationDuration` → 2 (seconds)
- `requiresPowerCellFirst` → ✅ Checked

---

## 5. Lights Controller Setup

### Find Lights Controller
1. In **Hierarchy**, find **LightsController** GameObject
   - If it doesn't exist, create empty GameObject and add script

### Assign References in LightsController Script

**Lights:**
- `lights[]` → Click + to add slots, drag ALL factory lights into array
  - Find all Light components in your scene
  - Drag each one into a slot
  - These should be disabled at start

**Glowing Path:**
- `pathSegments[]` → Click + to add slots, drag all path floor tiles
  - These are the tiles that light up to show the path
  - Should be disabled at start
- `glowingMaterial` → Create an emissive material (bright cyan)
- `pathActivationDelay` → 0.3

**Effects:**
- `activationVfx[]` → Optional: Particle systems
- `audioSource` → Add AudioSource, drag here
- `powerOnSfx` → Audio clip for power on
- `pathActivationSfx` → Audio clip for path

**References:**
- `objectiveManager` → Drag **ObjectiveManager**
- `factoryConsole` → Drag **FactoryConsole** GameObject

---

## 6. Player Setup

### Find Player GameObject
1. In **Hierarchy**, find your Player
   - Should have FPController script

### Set Player Layer
1. Select Player GameObject
2. In Inspector, top-right: Layer → "Player"
3. If asked "Change children too?" → Yes

### Verify FPController References

**Tutorial References:**
- `tutorialManager` → Drag TutorialManager if you have one

**Movement Settings:**
- Already configured, don't change unless needed

**Look Settings:**
- `cameraTransform` → Drag the Camera child object
- `lookSensitivity` → 0.5

**Pickup Settings:**
- `pickupRange` → Set to **5** (important!)
- `holdPoint` → Create empty child object:
  1. Right-click Player → Create Empty
  2. Name it "HoldPoint"
  3. Position it in front of camera (Z: 1.5, Y: -0.5)
  4. Drag it to this field
- `heldObject` → Leave empty (runtime)

**Menu & Settings:**
- `pauseMenuPanel` → Drag pause menu UI
- `brightnessOverlay` → Drag brightness overlay image
- `isPaused` → Unchecked

**Game State Reference:**
- `gameStateManager` → Drag GameStateManager if you have one

---

## 7. Quick Verification Checklist

After setup, verify these in Inspector:

### ObjectiveManager
- [ ] objectiveText assigned
- [ ] trackerText assigned
- [ ] onPowerCellInserted → LightsController.ActivateLights()
- [ ] onStartGlowingPath → LightsController.ActivateGlowingPath()

### Power Cell
- [ ] objectiveManager assigned
- [ ] Has Rigidbody (not kinematic)
- [ ] Has Collider

### Power Bay
- [ ] objectiveManager assigned
- [ ] socketPoint assigned (child object)
- [ ] promptUI assigned (World Space Canvas)
- [ ] detectionRange = 5 or higher
- [ ] playerLayer = "Player"

### Factory Console
- [ ] objectiveManager assigned
- [ ] promptUI assigned
- [ ] detectionRange = 5 or higher
- [ ] playerLayer = "Player"

### Lights Controller
- [ ] lights[] array filled with all lights
- [ ] pathSegments[] array filled with path tiles
- [ ] objectiveManager assigned
- [ ] factoryConsole assigned

### Player
- [ ] Layer set to "Player"
- [ ] holdPoint assigned (child object)
- [ ] pickupRange = 5
- [ ] cameraTransform assigned

---

## 8. Test After Setup

1. Press **Play**
2. Press **F12** to show diagnostic
3. Check that all components show "✓ Found"
4. Check that all setup checks show "✓"
5. If any show "✗", go back and assign that reference

---

## Common Issues

### "No ObjectiveManager ref" in diagnostic
**Solution:** You assigned it in Inspector, but diagnostic checks the private field. This is normal - as long as you assigned the public field, it will work at runtime.

### "No SocketPoint" in diagnostic
**Solution:** Make sure you created the SocketPoint as a child object and dragged it to the socketPoint field.

### Prompt UI doesn't show
**Solution:** 
- Make sure Canvas is set to World Space
- Make sure it's positioned above the object
- Make sure it's not too small (scale 0.01)
- Make sure promptUI field is assigned

### F key doesn't work
**Solution:**
- Check detectionRange is large enough (5+)
- Check playerLayer is set to "Player"
- Check Player GameObject has Layer "Player"
- Check Console for debug messages

---

## Visual Setup Tips

### Make Power Cell Obvious
1. Scale: (3, 3, 3)
2. Add Light component: Point, Orange, Range 15, Intensity 3
3. Material: Enable Emission, bright orange

### Make Power Bay Obvious
1. Add Light component: Spot, Cyan, Range 20, Intensity 5
2. Point light downward at bay
3. Add bright material or decal

### Make Console Obvious
1. Add Light component (assigned to consoleLight field)
2. Starts disabled, turns yellow when ready, green when activated

---

You're all set! After following this guide, run the diagnostic again (F12) and everything should show green checkmarks.
