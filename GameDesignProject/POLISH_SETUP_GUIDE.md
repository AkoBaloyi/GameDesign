# Polish Setup Guide - Visual & Audio Enhancements

## 🎨 New Scripts Created:

1. **PowerBaySparkEffect.cs** - Dramatic sparks when power cell inserted
2. **ConsoleActivationEffect.cs** - Screen glow and effects when console activated
3. **InteractionPrompt.cs** - Enhanced UI prompts with glow and fade
4. **FactoryAtmosphere.cs** - Dust, steam, fog, and emergency lights
5. **MinimapMarker.cs** - Pulsing objective markers on minimap

---

## 📋 Setup Instructions:

### 1. PowerBay Spark Effect (10 min)

**Step 1: Add Script**
- Select PowerBay GameObject
- Add Component → PowerBaySparkEffect

**Step 2: Create Particle System**
- Right-click PowerBay → Effects → Particle System
- Name it "SparkParticles"
- Configure:
  - Duration: 3
  - Looping: OFF
  - Start Lifetime: 0.5-1
  - Start Speed: 2-5
  - Start Size: 0.1-0.3
  - Start Color: Orange/Yellow
  - Emission: 50 particles/sec
  - Shape: Cone, Angle: 30

**Step 3: Create Flash Light**
- Right-click PowerBay → Light → Point Light
- Name it "FlashLight"
- Configure:
  - Color: Orange
  - Intensity: 5
  - Range: 5
  - Disable it (script will enable)

**Step 4: Assign in Inspector**
- PowerBaySparkEffect component:
  - Spark Particles → SparkParticles
  - Flash Light → FlashLight
  - Audio Source → PowerBay's AudioSource
  - Spark Sound → (find spark sound effect)

**Step 5: Connect to PowerBay**
- Open PowerBay.cs script
- In `InsertPowerCell()` method, add:
```csharp
// Play spark effect
PowerBaySparkEffect sparkEffect = GetComponent<PowerBaySparkEffect>();
if (sparkEffect != null)
{
    sparkEffect.PlaySparkEffect();
}
```

---

### 2. Console Activation Effect (10 min)

**Step 1: Add Script**
- Select FactoryConsole GameObject
- Add Component → ConsoleActivationEffect

**Step 2: Find/Create Screen**
- Find the console screen mesh (or create a plane)
- Name it "ConsoleScreen"
- Create new material: "ConsoleScreenMat"
  - Shader: Standard
  - Emission: Enabled
  - Color: Black initially

**Step 3: Create Particle System**
- Right-click Console → Effects → Particle System
- Name it "ActivationParticles"
- Configure:
  - Duration: 2
  - Looping: OFF
  - Start Color: Green
  - Emission: Burst of 20 particles
  - Shape: Sphere

**Step 4: Create Console Light**
- Right-click Console → Light → Point Light
- Name it "ConsoleLight"
- Configure:
  - Color: Green
  - Intensity: 3
  - Range: 5
  - Disable it

**Step 5: Assign in Inspector**
- ConsoleActivationEffect component:
  - Screen Renderer → ConsoleScreen
  - Activation Particles → ActivationParticles
  - Console Light → ConsoleLight
  - Audio Source → Console's AudioSource

**Step 6: Connect to FactoryConsole**
- Open FactoryConsole.cs
- In `ActivateConsole()` method, add:
```csharp
// Play activation effect
ConsoleActivationEffect effect = GetComponent<ConsoleActivationEffect>();
if (effect != null)
{
    effect.ActivateConsole();
}
```

---

### 3. Interaction Prompts (15 min)

**For PowerBay:**

**Step 1: Create World Space Canvas**
- Right-click PowerBay → UI → Canvas
- Name it "PromptCanvas"
- Canvas component:
  - Render Mode: World Space
  - Position: (0, 2, 0) - above power bay
  - Scale: (0.01, 0.01, 0.01)
  - Width: 400, Height: 100

**Step 2: Add Text**
- Right-click PromptCanvas → UI → Text - TextMeshPro
- Name it "PromptText"
- Configure:
  - Text: "Press F to Insert Power Cell"
  - Font Size: 24
  - Alignment: Center
  - Color: White
  - Add Outline (black, size 0.2)

**Step 3: Add Background Panel**
- Right-click PromptCanvas → UI → Panel
- Name it "PromptBackground"
- Configure:
  - Color: Black with 50% alpha
  - Add slight padding around text

**Step 4: Add Script**
- Select PromptCanvas
- Add Component → InteractionPrompt
- Assign:
  - Prompt Text → PromptText
  - Canvas Group → Add CanvasGroup component

**Step 5: Connect to PowerBay**
- In PowerBay.cs, add reference:
```csharp
public InteractionPrompt interactionPrompt;
```
- In `UpdatePrompt()` method:
```csharp
if (interactionPrompt != null)
{
    if (playerInRange && !isActivated && PlayerHasPowerCell())
    {
        interactionPrompt.Show();
    }
    else
    {
        interactionPrompt.Hide();
    }
}
```

**Repeat for FactoryConsole** with message "Press F to Activate Console"

---

### 4. Factory Atmosphere (20 min)

**Step 1: Create Atmosphere Manager**
- Create empty GameObject: "FactoryAtmosphere"
- Add Component → FactoryAtmosphere

**Step 2: Create Dust Particles**
- Right-click FactoryAtmosphere → Effects → Particle System
- Name it "DustParticles"
- Position at center of factory
- Script will configure automatically

**Step 3: Create Steam Vents**
- Find pipes or vents in your factory
- Add Particle System to each
- Name them "SteamVent_1", "SteamVent_2", etc.
- Configure:
  - Start Color: White/Gray
  - Start Speed: 1-3
  - Start Size: 0.5-1
  - Emission: Burst of 10-20
  - Shape: Cone pointing up
  - Looping: OFF (script controls)

**Step 4: Setup Emergency Lights**
- Find or create emergency lights
- Use Point Lights or Spot Lights
- Color: Orange/Red
- Low intensity (0.5-1)

**Step 5: Assign in Inspector**
- FactoryAtmosphere component:
  - Dust Particles → DustParticles
  - Steam Vents[] → Drag all steam vent particle systems
  - Emergency Lights[] → Drag all emergency light objects
  - Enable Fog: Checked
  - Fog Color: Dark gray/blue
  - Fog Density: 0.02

---

### 5. Minimap Markers (15 min)

**Step 1: Find Minimap UI**
- Open your UI Canvas
- Find the minimap panel/image

**Step 2: Create Marker Prefab**
- Right-click Minimap → UI → Image
- Name it "ObjectiveMarker"
- Configure:
  - Size: 20x20
  - Color: Green
  - Sprite: Circle or custom marker sprite

**Step 3: Add Script**
- Select ObjectiveMarker
- Add Component → MinimapMarker
- Assign:
  - Minimap Rect → Your minimap RectTransform
  - Marker Type → Console (or appropriate type)

**Step 4: Create Markers for Each Objective**
- Duplicate ObjectiveMarker
- Rename: "PowerCellMarker", "PowerBayMarker", "ConsoleMarker"
- Set marker types:
  - PowerCellMarker → MarkerType.PowerCell (Orange)
  - PowerBayMarker → MarkerType.PowerBay (Blue)
  - ConsoleMarker → MarkerType.Console (Green)

**Step 5: Assign World Targets**
- PowerCellMarker:
  - World Target → PowerCell GameObject
- PowerBayMarker:
  - World Target → PowerBay GameObject
- ConsoleMarker:
  - World Target → FactoryConsole GameObject

**Step 6: Control Visibility**
- In ObjectiveManager, add references:
```csharp
public MinimapMarker powerCellMarker;
public MinimapMarker powerBayMarker;
public MinimapMarker consoleMarker;
```
- Show/hide based on current objective

---

## 🎵 Audio Setup (20 min)

### Find Free Sound Effects:

**Recommended Sites:**
- Freesound.org
- Zapsplat.com
- Mixkit.co

**Sounds Needed:**
1. **Power Cell Pickup** - Electronic beep, item pickup
2. **Power Bay Insertion** - Mechanical clunk, electrical surge
3. **Spark Effect** - Electrical sparks, zapping
4. **Console Activation** - Computer bootup, beep sequence
5. **Light Activation** - Power surge, electrical hum
6. **Path Activation** - Energy pulse, whoosh

### Import and Assign:

1. **Download sounds** (MP3 or WAV)
2. **Import to Unity:**
   - Drag into Assets/Audio folder
   - Select each → Inspector → Force to Mono (for 3D sounds)
3. **Assign to scripts:**
   - PowerCell → pickupSound
   - PowerBay → insertSound (add to PowerBay script)
   - PowerBaySparkEffect → sparkSound
   - ConsoleActivationEffect → activationSound, bootupSound
   - LightsController → powerOnSfx, pathActivationSfx

---

## ✅ Testing Checklist:

After setup, test each feature:

### Visual Effects:
- [ ] Sparks appear when power cell inserted
- [ ] Console screen glows green when activated
- [ ] Interaction prompts fade in/out smoothly
- [ ] Dust particles visible in factory
- [ ] Steam vents activate periodically
- [ ] Emergency lights flicker
- [ ] Fog creates atmosphere
- [ ] Minimap markers pulse and track objectives

### Audio:
- [ ] Power cell makes sound when picked up
- [ ] Power bay makes sound when cell inserted
- [ ] Sparks have electrical sound
- [ ] Console makes bootup sound
- [ ] Lights make power surge sound
- [ ] Path makes activation sound
- [ ] All sounds at appropriate volume

### Polish:
- [ ] Everything feels responsive
- [ ] Visual feedback is clear
- [ ] Audio enhances experience
- [ ] Atmosphere feels immersive
- [ ] No performance issues

---

## 🚀 Quick Wins:

If short on time, prioritize:

1. **PowerBay spark effect** - Most dramatic visual
2. **Console activation effect** - Satisfying completion
3. **Interaction prompts** - Improves clarity
4. **Dust particles** - Easy atmosphere boost
5. **Minimap markers** - Helps navigation

---

## 💡 Pro Tips:

1. **Test incrementally** - Add one feature, test, then move on
2. **Adjust values in Play mode** - See changes in real-time
3. **Use particle system presets** - Modify existing effects
4. **Keep sounds short** - 0.5-2 seconds ideal
5. **Balance audio levels** - No sound should overpower others
6. **Use color coding** - Orange=power, Blue=tech, Green=success
7. **Save often** - Ctrl+S after each major change

---

## 🎯 Expected Result:

After completing this setup:
- Factory feels alive and atmospheric
- Every action has clear visual feedback
- Audio enhances immersion
- Players always know where to go
- Game feels polished and professional

---

**Time Estimate: 1.5-2 hours total**

Take your time, test each feature, and enjoy watching your game come to life! 🎮✨
