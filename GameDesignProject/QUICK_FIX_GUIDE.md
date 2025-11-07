# Quick Fix Guide - Get the Game Working NOW

## ✅ Code Fixes Applied

I've already fixed the critical code issues:

1. **PowerBay.cs** - Added direct F-key detection in Update()
2. **FactoryConsole.cs** - Added direct F-key detection in Update()
3. **PlayerInteractionHandler.cs** - Now broadcasts to PowerBay and FactoryConsole

These changes ensure the F-key will work even if the Input System isn't properly wired up.

---

## 🔧 Unity Scene Setup Checklist

### Step 1: Verify ObjectiveManager Setup
1. Open the **Game** scene
2. Find the **ObjectiveManager** GameObject
3. In Inspector, verify these UnityEvents are connected:
   - `onPowerCellInserted` → `LightsController.ActivateLights()`
   - `onStartGlowingPath` → `LightsController.ActivateGlowingPath()`
   - `onWin` → `WinStateManager.ShowWinScreen()`

### Step 2: Verify Power Cell Setup
1. Find the **Power Cell** GameObject in scene
2. Ensure it has these components:
   - `PowerCell` script
   - `PickUpObject` script
   - `HighlightableObject` script (optional but recommended)
   - `Rigidbody` (not kinematic)
   - `Collider`
3. In PowerCell script, assign:
   - `objectiveManager` → ObjectiveManager GameObject
4. **Make it VISIBLE:**
   - Scale it up (try 2x or 3x larger)
   - Add a bright orange material with emission
   - Add a Point Light (orange, range 10, intensity 2)
   - Add a particle system (glowing particles)

### Step 3: Verify Power Bay Setup
1. Find the **Power Bay** GameObject
2. Ensure it has:
   - `PowerBay` script
   - `Collider` (trigger or regular)
3. In PowerBay script, assign:
   - `objectiveManager` → ObjectiveManager
   - `socketPoint` → Empty child GameObject where cell will snap
   - `promptUI` → Canvas with "Press F" text
   - `playerLayer` → Set to layer containing Player
4. Set `detectionRange` to 5 (make it generous)
5. **Add Visual Prompt:**
   - Create a World Space Canvas above Power Bay
   - Add large TextMeshPro text: "PRESS F TO INSERT POWER CELL"
   - Make text size 0.5, color cyan/yellow
   - Assign to `promptUI` field

### Step 4: Verify Factory Console Setup
1. Find the **Factory Console** GameObject
2. Ensure it has:
   - `FactoryConsole` script
   - `Collider`
3. In FactoryConsole script, assign:
   - `objectiveManager` → ObjectiveManager
   - `promptUI` → Canvas with "Press F" text
   - `consoleLight` → Light component (starts disabled)
   - `playerLayer` → Set to layer containing Player
4. Set `detectionRange` to 5
5. **Add Visual Prompt:**
   - Create World Space Canvas above console
   - Add text: "PRESS F TO ACTIVATE CONSOLE"
   - Assign to `promptUI` field

### Step 5: Verify Lights Controller Setup
1. Find **LightsController** GameObject
2. In Inspector, assign:
   - `lights[]` → Drag all factory lights into array
   - `pathSegments[]` → Drag all glowing path floor tiles
   - `objectiveManager` → ObjectiveManager
   - `factoryConsole` → FactoryConsole GameObject
3. Ensure lights are disabled at start
4. Ensure path segments are disabled at start

### Step 6: Verify Player Setup
1. Find **Player** GameObject
2. Ensure it has:
   - `FPController` script
   - `PlayerInteractionHandler` script
   - `CharacterController` component
3. Set Player's layer to "Player"
4. In FPController, assign:
   - `holdPoint` → Empty child GameObject in front of camera
   - `cameraTransform` → Camera child object
5. In PlayerInteractionHandler:
   - Set `interactionRange` to 5

### Step 7: Test the Complete Loop
1. Press Play
2. Complete tutorial (or skip it)
3. **Find Power Cell** - Should be glowing orange
4. **Look at Power Cell** - Should highlight
5. **Press E** - Should pick up, objective updates
6. **Find Power Bay** - Look for blue marker on minimap
7. **Walk to Power Bay** - "Press F" prompt should appear
8. **Press F** - Power cell should insert, lights activate
9. **Wait for lights** - Sequential activation
10. **Follow glowing path** - Floor tiles light up
11. **Reach Console** - "Press F" prompt appears
12. **Press F** - Console activates
13. **Win Screen** - Should appear after 2 seconds

---

## 🚨 If It Still Doesn't Work

### Debug Power Cell Pickup
1. Select Power Cell in Hierarchy
2. Press Play
3. Try to pick it up with E
4. Open Console (Ctrl+Shift+C)
5. Look for: `[PowerCell] Picked up!`
6. If not appearing:
   - Check PickUpObject component is attached
   - Check PowerCell.OnPickedUp() is being called
   - Check FPController.pickupRange is large enough (try 5)

### Debug Power Bay Interaction
1. Pick up Power Cell
2. Walk to Power Bay
3. Open Console
4. Look for: `[PowerBay] F key pressed!`
5. If not appearing:
   - Check you're within detectionRange (try increasing to 10)
   - Check playerLayer is set correctly
   - Check promptUI is showing (should see "Press F" text)
6. If appearing but not inserting:
   - Check FPController.GetHeldObject() returns the power cell
   - Check PowerCell.IsPickedUp() returns true

### Debug Console Activation
1. Complete power cell insertion
2. Wait for lights to activate
3. Follow glowing path to console
4. Open Console
5. Look for: `[FactoryConsole] Console enabled for activation`
6. If not appearing:
   - Check LightsController.factoryConsole is assigned
   - Check ActivateGlowingPath() is being called
7. Press F at console
8. Look for: `[FactoryConsole] F key pressed!`
9. If not appearing:
   - Check canActivate is true
   - Check playerInRange is true
   - Increase detectionRange

---

## 🎨 Quick Visual Improvements (5 Minutes Each)

### Make Power Cell Impossible to Miss
```
1. Select Power Cell
2. Scale: (3, 3, 3)
3. Add Component → Light
   - Type: Point
   - Color: Orange (255, 128, 0)
   - Range: 15
   - Intensity: 3
4. Material: Set Emission to bright orange
```

### Make Power Bay Obvious
```
1. Select Power Bay
2. Add Component → Light
   - Type: Spot
   - Color: Cyan (0, 255, 255)
   - Range: 20
   - Intensity: 5
   - Angle: 60
3. Rotate light to point down at bay
```

### Make Glowing Path BRIGHT
```
1. Select all path segments
2. Create new Material: "GlowingPath"
3. Set Shader: URP/Lit
4. Base Color: Bright cyan (0, 255, 255)
5. Emission: Enabled, same cyan color
6. Emission Intensity: 2
7. Apply to all path segments
```

### Add Dramatic Lighting
```
1. Select Directional Light
2. Intensity: 0.3 (very dim)
3. Color: Slight blue tint
4. Add fog: Window → Rendering → Lighting
   - Fog: Enabled
   - Color: Dark blue-gray
   - Density: 0.01
```

---

## 🎵 Quick Audio Improvements (10 Minutes)

### Add Ambient Sound
```
1. Create Empty GameObject: "AmbientAudio"
2. Add Component → Audio Source
3. Find free factory ambient sound online
4. Assign to Audio Clip
5. Loop: Enabled
6. Volume: 0.3
7. Spatial Blend: 0 (2D sound)
```

### Add Interaction Sounds
Already set up in scripts, just need to assign:
1. PowerBay: insertSound, activationSound
2. FactoryConsole: activationSound, successSound
3. PowerCell: pickupSound

Find free sound effects at:
- freesound.org
- zapsplat.com
- sonniss.com (free GDC packs)

---

## ✅ Success Checklist

After fixes, you should have:
- [x] Power cell is easy to find (bright, glowing, large)
- [x] Power cell can be picked up with E
- [x] Power bay shows clear "Press F" prompt
- [x] F key inserts power cell into bay
- [x] Lights activate sequentially
- [x] Glowing path appears and is easy to follow
- [x] Console shows "Press F" prompt
- [x] F key activates console
- [x] Win screen appears
- [x] Factory has atmospheric lighting
- [x] Ambient sounds play

---

## 📝 Next Steps (After Core Loop Works)

1. **Add waypoint arrows** pointing to objectives
2. **Add minimap markers** for power cell, bay, console
3. **Add more particle effects** (sparks, steam, dust)
4. **Apply textures** to walls and floors
5. **Add background animations** (rotating fans, moving pistons)
6. **Add voice-over hints** or text prompts
7. **Polish UI** with industrial theme
8. **Add music** that builds tension

---

## 🆘 Still Stuck?

Check these common issues:

1. **Input System not set up:**
   - Window → Package Manager
   - Install "Input System"
   - Edit → Project Settings → Player
   - Active Input Handling: Both or Input System Package

2. **Layers not configured:**
   - Edit → Project Settings → Tags and Layers
   - Add "Player" layer
   - Assign Player GameObject to Player layer

3. **Scripts not compiling:**
   - Check Console for errors
   - Fix any red errors before testing

4. **Objects not in scene:**
   - Verify Power Cell, Power Bay, Console are all in Game scene
   - Check they're not disabled in Hierarchy

5. **References not assigned:**
   - Every public field in Inspector should be assigned
   - Use Debug.Log to verify references aren't null
