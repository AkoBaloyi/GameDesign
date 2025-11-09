# 💡 Lighting Fix - Make Lights Visible

## Problem
Lights are "on" but you can't see light rays or illumination on surfaces.

---

## Quick Fixes

### Fix 1: Check Light Type & Settings

1. **Select a factory light** in Hierarchy
2. **Check Light component**:
   - Type: Should be **Spot** or **Point**
   - Range: **20-30** (increase if too small)
   - Intensity: **2-5** (increase if too dim)
   - Spot Angle (if Spot): **60-90** degrees

3. **Check if light is enabled**:
   - Component should have checkmark
   - If grayed out, click to enable

---

### Fix 2: Add Visible Light Cones (Volumetric)

Unity URP doesn't show light rays by default. Here's how to make them visible:

**Option A: Add Fog (Easiest)**
1. Window → Rendering → Lighting
2. Environment tab
3. Check **Fog** checkbox
4. Settings:
   - Mode: Exponential
   - Color: Dark blue-gray #1E2838
   - Density: **0.01** (adjust to taste)

**Result**: Lights will now have visible beams through fog!

---

**Option B: Use Spot Lights with Cookie**
1. Select light
2. Type: **Spot**
3. Range: 20
4. Spot Angle: 60
5. Intensity: 3
6. Cookie: (optional - adds pattern to light)

---

### Fix 3: Increase Light Intensity

Your lights might be too dim:

1. **Select all factory lights**:
   - Hold Ctrl and click each light
   - Or select parent object

2. **In Inspector**:
   - Intensity: Set to **3-5**
   - Range: Set to **20-30**

3. **Test in Play mode**

---

### Fix 4: Check Scene Lighting

1. **Directional Light** (sun):
   - Should be **dim** (0.3 intensity)
   - If too bright, factory lights won't be visible

2. **Ambient Light**:
   - Window → Rendering → Lighting
   - Environment tab
   - Ambient Mode: Color
   - Ambient Color: Very dark gray #0A0A0A
   - Ambient Intensity: 0.3

---

### Fix 5: Add Light Shafts (Advanced)

For dramatic light beams:

1. **Create Light Shaft Mesh**:
   - GameObject → 3D Object → Cylinder
   - Scale: (0.5, 5, 0.5) - tall and thin
   - Rotate: 90 degrees on X axis
   - Position under light

2. **Create Light Shaft Material**:
   - Create → Material
   - Name: "LightShaft"
   - Shader: URP/Lit
   - Surface Type: Transparent
   - Rendering Mode: Fade
   - Base Color: White with Alpha 0.1
   - Enable Emission
   - Emission: White, Intensity 0.5

3. **Apply to cylinder**

4. **Duplicate for each light**

---

## Quick Test Setup

### Test Light Configuration:
```
Type: Spot
Range: 25
Spot Angle: 60
Intensity: 4
Color: White #FFFFFF
Shadows: Soft Shadows (optional)
```

### Test Scene Settings:
```
Directional Light Intensity: 0.3
Fog: Enabled
Fog Density: 0.01
Ambient Intensity: 0.3
```

---

## Recommended Settings for Your Game

### Factory Spotlights:
```
Type: Spot
Range: 25
Spot Angle: 60
Intensity: 3
Color: Warm White #FFFFCC (R=1.0, G=1.0, B=0.8)
```

### Power Bay Light:
```
Type: Spot
Range: 20
Spot Angle: 45
Intensity: 5
Color: Cyan #00FFFF
```

### Console Light:
```
Type: Point
Range: 10
Intensity: 2
Color: Green #00FF00 (when active)
```

### Power Cell Light:
```
Type: Point
Range: 15
Intensity: 2
Color: Orange #FF8000
```

---

## Why Lights Might Not Be Visible

### Common Issues:

1. **Too Bright Ambient**:
   - If scene is already bright, lights don't stand out
   - Solution: Darken ambient and directional light

2. **Range Too Small**:
   - Light doesn't reach far enough
   - Solution: Increase Range to 20-30

3. **Intensity Too Low**:
   - Light is too weak
   - Solution: Increase Intensity to 3-5

4. **No Fog**:
   - Can't see light beams in air
   - Solution: Add fog with low density

5. **Wrong Light Type**:
   - Directional lights don't have range
   - Solution: Use Spot or Point lights

6. **Lights Disabled**:
   - Component is unchecked
   - Solution: Enable the component

---

## Quick Visual Test

1. **Make scene DARK**:
   - Directional Light: 0.1 intensity
   - Ambient: Very dark

2. **Add ONE test light**:
   - Type: Spot
   - Range: 30
   - Intensity: 5
   - Point at floor

3. **Add fog**:
   - Density: 0.02 (higher for testing)

4. **Press Play**

**You should see**:
- Bright cone of light on floor
- Light beam visible in fog
- Clear difference between lit and unlit areas

If you see this, your lights work! Now adjust settings for all lights.

---

## Pro Tips

1. **Use Spot lights for factory** - More dramatic than Point
2. **Point lights down** - Rotate to aim at floor/objects
3. **Add fog** - Makes lights visible in air
4. **Vary intensity** - Some bright, some dim for interest
5. **Use colored lights** - Cyan for tech, orange for warm areas
6. **Add shadows** - Makes lights more realistic (but costs performance)

---

## Emergency Fix

If nothing works:

1. **Delete all lights**
2. **Create ONE new Spot Light**:
   - Position: Above factory floor
   - Rotation: Point down (90 degrees on X)
   - Range: 30
   - Intensity: 5
   - Spot Angle: 60

3. **Test** - You should see clear light cone

4. **If this works**: Your original lights had wrong settings
5. **Duplicate this light** for all positions

---

**After fixing, your factory should have dramatic lighting with visible light cones!** 💡
