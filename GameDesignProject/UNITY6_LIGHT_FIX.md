# 💡 Unity 6 Light Fix Guide

## For Unity 6000.0.53f1

Unity 6 has different UI than older versions. Here's the correct way to check everything.

---

## Step 1: Check Light Settings (Unity 6)

**Select a factory light and check**:

### Light Component:
```
Type: Spot (or Point)
Mode: Realtime ← MOST IMPORTANT!
Range: 30
Intensity: 5
Color: White
Inner/Outer Spot Angle: 60 (for Spot lights)
```

**If Mode says "Baked" or "Mixed"**: Change to **Realtime**!

---

## Step 2: Check Camera (Unity 6)

**Select Main Camera**:

### Camera Component:
- Should have **"Camera"** component
- Should have **"Universal Additional Camera Data"** component
  - If missing: Add Component → search "Universal Additional Camera Data"

### In Universal Additional Camera Data:
```
Rendering:
  - Renderer: (should have a renderer assigned)
  - Post Processing: Enabled (optional)
  
Anti-aliasing:
  - Mode: (any option is fine)
```

**Don't worry about HDR/MSAA settings - Unity 6 handles these differently.**

---

## Step 3: Check URP Settings (Unity 6)

### Graphics Settings:
1. **Edit → Project Settings → Graphics**
2. Look for **"Scriptable Render Pipeline Settings"**
3. Should have **"UniversalRenderPipelineAsset"** or similar assigned
4. If empty: 
   - Look in Project window
   - Find "UniversalRenderPipelineAsset" or "URP-*" asset
   - Drag it to this field

### Quality Settings:
1. **Edit → Project Settings → Quality**
2. Look for **"Render Pipeline Asset"**
3. Should have same URP asset assigned

---

## Step 4: Simple Test (Unity 6)

Let's test if lighting works at all:

1. **Create new Spot Light**:
   - GameObject → Light → Spot Light
   - Name: "TestLight"

2. **Position above floor**:
   - Transform: (0, 10, 0)
   - Rotation: (90, 0, 0) ← pointing down

3. **Light settings**:
   ```
   Type: Spot
   Mode: Realtime ← CHECK THIS!
   Range: 50
   Intensity: 10
   Inner Spot Angle: 30
   Outer Spot Angle: 60
   Color: White
   ```

4. **Disable all other lights** (for testing)

5. **Press Play**

**Expected**: Bright white cone on floor

**If you see it**: Lighting works! Your factory lights just need correct settings.

**If you don't**: Continue to Step 5.

---

## Step 5: Check Rendering (Unity 6)

### Window → Rendering → Lighting:

1. **Environment tab**:
   - Skybox Material: (can be None for indoor)
   - Sun Source: (can be None)
   - Environment Lighting: (default is fine)

2. **Don't worry about "Generate Lighting"** - that's for baked lighting which you don't need!

---

## Step 6: Common Unity 6 Issues

### Issue: Lights are there but not visible

**Check**:
1. Light Mode is **Realtime** (not Baked)
2. Light component is **enabled** (checkmark)
3. GameObject is **active** (not grayed out)
4. You're testing in **Game View** (Play mode), not Scene View

### Issue: Scene is completely black

**Check**:
1. Camera has "Universal Additional Camera Data" component
2. At least one light is enabled
3. Directional Light exists (or create one)

### Issue: Lights flicker or look wrong

**Check**:
1. Too many lights with shadows
2. Shadow atlas too small
3. **Fix**: Disable shadows on most lights

---

## Unity 6 Specific: Light Layers

Unity 6 has "Light Layers" feature. Make sure it's not blocking your lights:

**On each light**:
- Look for "Light Layer" or "Culling Mask"
- Should be set to **"Everything"** or **"Default"**
- If it's set to specific layers, change to "Everything"

---

## Quick Checklist for Unity 6

For EACH factory light:
- [ ] Type: Spot or Point
- [ ] Mode: **Realtime** ← CRITICAL!
- [ ] Range: 25-30
- [ ] Intensity: 5-10
- [ ] Component enabled (checkmark)
- [ ] GameObject active (not grayed out)
- [ ] Light Layer: Everything
- [ ] Shadows: No Shadows (for performance)

For Camera:
- [ ] Has "Camera" component
- [ ] Has "Universal Additional Camera Data" component
- [ ] Renderer is assigned in Additional Camera Data

For Project:
- [ ] Edit → Project Settings → Graphics → URP asset assigned
- [ ] Edit → Project Settings → Quality → URP asset assigned

---

## Most Likely Issue in Unity 6

**Lights are set to "Baked" mode instead of "Realtime"**

**Fix**:
1. Select all factory lights
2. Find "Mode" in Light component
3. Change to **Realtime**
4. Press Play
5. Should work!

---

## If Still Not Working

Tell me:
1. **Light Mode**: Realtime, Baked, or Mixed?
2. **Can you see test light?**: Yes/No
3. **Any errors in Console?**: Copy/paste them
4. **Camera has "Universal Additional Camera Data"?**: Yes/No
5. **URP asset assigned in Graphics settings?**: Yes/No

---

**In Unity 6, the #1 issue is still Light Mode being "Baked" instead of "Realtime"!** 🎯
