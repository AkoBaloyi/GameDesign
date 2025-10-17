# Complete Gameplay Loop Setup Guide
## Power Restoration Mission

---

## 🎯 Overview

This system creates a complete gameplay loop:
1. **Tutorial** → Learn controls
2. **Objective** → Restore power to factory
3. **Task 1** → Find and pickup orange Power Cell (E key)
4. **Task 2** → Insert Power Cell into Power Bay (F key)
5. **Visual Feedback** → Lights activate, glowing path appears
6. **Task 3** → Follow path to Console
7. **Task 4** → Activate Console (F key)
8. **Win State** → Mission Complete screen

---

## 📦 What's Been Created

### Core Scripts:
- ✅ `ObjectiveManager.cs` - Enhanced with banner UI and tracker
- ✅ `PowerCell.cs` - Enhanced with visual feedback
- ✅ `PowerBay.cs` - Enhanced with F key interaction
- ✅ `FactoryConsole.cs` - NEW! Console activation
- ✅ `WinStateManager.cs` - NEW! Win screen
- ✅ `LightsController.cs` - Enhanced with glowing path

### Helper Methods Added:
- ✅ `FPController.GetHeldObject()` - Check what player is holding
- ✅ `FPController.DropHeldObject()` - Drop held object

---

## 🛠️ Unity Setup (Step-by-Step)

### **STEP 1: Setup Objective Manager**

1. **Create empty GameObject** named "GameManager"
2. **Add Component** → `ObjectiveManager`
3. **Create UI Elements:**

#### Objective Banner (Big popup):
```
Canvas → ObjectiveBanner (Panel)
  ├── Background (Image - dark semi-transparent)
  ├── ObjectiveText (TextMeshPro - large, bold)
  └── CanvasGroup component
```

#### Objective Tracker (Top corner HUD):
```
Canvas → TrackerPanel (Panel)
  └── TrackerText (TextMeshPro - "Power Cells: 0/1")
```

4. **Wire up ObjectiveManager:**
   - **Objective Text**: Drag `ObjectiveText` here
   - **Objective Banner**: Drag `ObjectiveBanner` panel here
   - **Banner Canvas Group**: Drag `CanvasGroup` component here
   - **Tracker Text**: Drag `TrackerText` here
   - **Tracker Panel**: Drag `TrackerPanel` here

---

### **STEP 2: Setup Power Cell**

1. **Find your orange cube** (or create one)
2. **Add Components:**
   - `PowerCell` script
   - `HighlightableObject` script
   - `PickUpObject` script (if not already there)
   - `Rigidbody` (if not already there)
   - `Collider` (if not already there)

3. **Configure PowerCell:**
   - **Objective Manager**: Drag GameManager here
   - **Cell Color**: Orange (1, 0.5, 0)
   - **Rotation Speed**: 30
   - **Bob Speed**: 1
   - **Bob Height**: 0.2

4. **Make it orange and glowing:**
   - Select the cube's Material
   - Set Albedo color to orange
   - Enable Emission
   - Set Emission color to orange

---

### **STEP 3: Setup Power Bay**

1. **Create Power Bay GameObject:**
```
PowerBay (Empty GameObject)
  ├── BayModel (3D model/cube - the station)
  ├── SocketPoint (Empty GameObject - where cell goes)
  └── PromptUI (World Space Canvas with text)
```

2. **Add Component** → `PowerBay` to parent
3. **Configure PowerBay:**
   - **Objective Manager**: Drag GameManager
   - **Socket Point**: Drag the SocketPoint child
   - **Prompt UI**: Drag the PromptUI canvas
   - **Prompt Text**: Drag the TextMeshPro from PromptUI
   - **Detection Range**: 3
   - **Player Layer**: Select "Player" layer

4. **Setup Player Input for Power Bay:**
   - Select **Player** GameObject
   - Find **Player Input** component
   - **Events** → **Player** → **Interact**
   - Click **+** button
   - Drag **PowerBay** GameObject into field
   - Select: `PowerBay` → `OnInteract`

---

### **STEP 4: Setup Lights Controller**

1. **Create empty GameObject** named "LightsController"
2. **Add Component** → `LightsController`
3. **Create factory lights:**
   - Add several `Light` components in your scene
   - Drag them all into the **Lights** array

4. **Create glowing path** (optional but cool):
   - Create plane/cube objects as path segments
   - Arrange them from Power Bay to Console
   - Drag them into **Path Segments** array
   - Create a glowing material (emissive) and assign to **Glowing Material**

5. **Configure LightsController:**
   - **Objective Manager**: Drag GameManager
   - **Factory Console**: Will assign in next step
   - **Light Activation Delay**: 0.2
   - **Path Activation Delay**: 0.3

---

### **STEP 5: Setup Factory Console**

1. **Create Console GameObject:**
```
FactoryConsole (Empty GameObject)
  ├── ConsoleModel (3D model/cube)
  ├── Screen (Plane with material)
  ├── ConsoleLight (Light component)
  └── PromptUI (World Space Canvas)
```

2. **Add Component** → `FactoryConsole` to parent
3. **Configure FactoryConsole:**
   - **Objective Manager**: Drag GameManager
   - **Prompt UI**: Drag PromptUI canvas
   - **Prompt Text**: Drag TextMeshPro
   - **Console Renderer**: Drag Screen renderer
   - **Console Light**: Drag Light component
   - **Detection Range**: 3
   - **Player Layer**: Select "Player"

4. **Setup Player Input for Console:**
   - Select **Player** GameObject
   - **Player Input** → **Events** → **Player** → **Interact**
   - Click **+** button (add another listener)
   - Drag **FactoryConsole** into field
   - Select: `FactoryConsole` → `OnInteract`

5. **Go back to LightsController:**
   - **Factory Console**: Drag the FactoryConsole GameObject here

---

### **STEP 6: Setup Win State Manager**

1. **Create Win Screen UI:**
```
Canvas → WinPanel (Panel - full screen)
  ├── Background (Image - dark)
  ├── TitleText (TextMeshPro - "MISSION COMPLETE")
  ├── MessageText (TextMeshPro - "Factory Power Restored!")
  ├── RestartButton (Button - "Restart")
  ├── QuitButton (Button - "Quit")
  └── CanvasGroup component
```

2. **Create empty GameObject** named "WinStateManager"
3. **Add Component** → `WinStateManager`
4. **Configure WinStateManager:**
   - **Win Panel**: Drag WinPanel
   - **Title Text**: Drag TitleText
   - **Message Text**: Drag MessageText
   - **Restart Button**: Drag RestartButton
   - **Quit Button**: Drag QuitButton
   - **Panel Canvas Group**: Drag CanvasGroup

---

### **STEP 7: Wire Everything Together**

Now connect all the UnityEvents in ObjectiveManager:

1. **Select GameManager** (ObjectiveManager)
2. **Find "Signals / Hooks" section**

#### onPowerCellInserted:
- Click **+**
- Drag **LightsController** into field
- Select: `LightsController` → `ActivateLights()`

#### onStartGlowingPath:
- Click **+**
- Drag **LightsController** into field
- Select: `LightsController` → `ActivateGlowingPath()`

#### onWin:
- Click **+**
- Drag **WinStateManager** into field
- Select: `WinStateManager` → `ShowWinScreen()`

---

### **STEP 8: Connect Tutorial to Objectives**

1. **Find your TutorialManager** GameObject
2. **At the end of tutorial**, call `ObjectiveManager.OnTutorialCompleted()`
   - You can do this via UnityEvent or code

---

## 🎮 Testing Checklist

### Test 1: Tutorial
- [ ] Tutorial plays normally
- [ ] At end, see banner: "OBJECTIVE: Restore Power to the Factory"
- [ ] Tracker shows: "Power Cells: 0/1"

### Test 2: Power Cell
- [ ] Orange cube is rotating and bobbing
- [ ] Highlights when you look at it
- [ ] Press E to pick up
- [ ] Tracker updates: "Power Cells: 1/1 - Find Power Bay"

### Test 3: Power Bay
- [ ] Walk to Power Bay with power cell
- [ ] See prompt: "Press F to Insert Power Cell"
- [ ] Press F
- [ ] Power cell snaps into socket
- [ ] Banner shows: "Power Cell Inserted!"
- [ ] Lights turn on one by one
- [ ] Tracker shows: "Power Cell Inserted ✓"

### Test 4: Glowing Path
- [ ] After lights activate, glowing path appears
- [ ] Path leads to console
- [ ] Tracker shows: "Follow the path"

### Test 5: Console
- [ ] Walk to console
- [ ] Console light is yellow (ready)
- [ ] See prompt: "Press F to Activate Console"
- [ ] Press F
- [ ] Console activates (light turns green)
- [ ] After 2 seconds, win screen appears

### Test 6: Win Screen
- [ ] See "MISSION COMPLETE"
- [ ] See "Factory Power Restored!"
- [ ] Restart button works
- [ ] Quit button works

---

## 🎨 Visual Enhancements (Optional)

### Make it look better:
1. **Power Cell**: Add particle system (orange glow)
2. **Power Bay**: Add blue circle decal on floor
3. **Lights**: Use different colored lights
4. **Path**: Use emissive materials with bloom
5. **Console**: Add screen with animated texture
6. **Win Screen**: Add particle effects, animations

---

## 🔊 Audio Enhancements (Optional)

Add AudioSource components and assign clips:
- **PowerCell**: Pickup sound
- **PowerBay**: Insert sound, activation sound
- **LightsController**: Power on sound, path activation sound
- **FactoryConsole**: Activation sound, success sound
- **WinStateManager**: Win music, win sound effect

---

## 🗺️ Minimap Integration

To show objectives on minimap:

1. **Add MinimapArrow script** to UI
2. **Set targets dynamically:**
   - When tutorial ends → Point to Power Cell
   - When cell picked up → Point to Power Bay
   - When lights activate → Point to Console

You can do this in ObjectiveManager's methods by calling:
```csharp
minimapArrow.SetTarget(powerCellTransform);
```

---

## 🐛 Troubleshooting

### Power Cell doesn't highlight:
- Check it has `HighlightableObject` component
- Check it has `PickUpObject` component

### Power Bay doesn't show prompt:
- Check Player has "Player" layer assigned
- Check PowerBay's Player Layer mask includes "Player"
- Check PowerBay has OnInteract wired in Player Input

### Lights don't activate:
- Check ObjectiveManager's onPowerCellInserted event is wired
- Check LightsController has lights in array
- Check lights are initially disabled

### Console doesn't activate:
- Check it has OnInteract wired in Player Input
- Check LightsController calls EnableConsole()
- Check console's canActivate is true

### Win screen doesn't show:
- Check ObjectiveManager's onWin event is wired
- Check WinStateManager has all UI references assigned

---

## ✅ Final Steps

1. Test the entire loop start to finish
2. Adjust timings (banner duration, light delays, etc.)
3. Add audio and visual polish
4. Build and celebrate! 🎉

---

## 📝 Summary

You now have a complete gameplay loop with:
- ✅ Clear objectives
- ✅ Visual feedback
- ✅ Player progression
- ✅ Win state
- ✅ Professional game feel

The player knows what to do, where to go, and gets satisfying feedback at every step!
