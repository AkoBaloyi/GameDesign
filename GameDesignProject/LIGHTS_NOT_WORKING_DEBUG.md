# 🔍 Lights Not Working - Debug Guide

## If Intensity 1000 Doesn't Work, Something Else Is Wrong

This isn't a settings issue - there's a deeper problem. Let's debug systematically.

---

## Step 1: Check if Lights Are Actually Enabled

1. **Select a factory light**
2. **Look at Inspector**
3. **Is the Light component checked?** ✓
   - If NO checkmark: Click to enable it
   - If grayed out: Component is disabled

4. **Is the GameObject active?**
   - Look at top of Inspector
   - Should have checkmark next to name
   - If grayed out: GameObject is disabled

---

## Step 2: Check Light Settings

**Select a light and verify**:

```
Type: Spot (or Point)
Mode: Realtime (NOT Baked or Mixed)
Range: 30
Intensity: 5
Color: White (R=1, G=1, B=1)
Culling Mask: Everything
Render Mode: Auto
```

**Critical**: If Mode is "Baked", lights won't work in Play mode!

---

## Step 3: Check Rendering Settings

### Check URP Asset:

1. **Edit → Project Settings → Graphics**
2. **Check "Scriptable Render Pipeline Settings"**
3. **Should have a URP asset assigned**
4. **If empty**: You need to assign URP asset

### Check Quality Settings:

1. **Edit → Project Settings → Quality**
2. **Check "Rendering"**
3. **Should have URP asset assigned**

---

## Step 4: Check Camera Settings

1. **Select Main Camera**
2. **Check these settings**:
   ```
   Rendering Path: Use Graphics Settings
   Allow HDR: Checked
   Allow MSAA: Checked (optional)
   ```

3. **Check if camera has "Universal Additional Camera Data" component**
   - If missing: Add Component → Rendering → Universal Additional Camera Data

---

## Step 5: Create Test Light from Scratch

Let's verify lighting works at all:

1. **Delete all existing lights** (or disable them)

2. **Create new Spot Light**:
   - GameObject → Light → Spot Light
   - Name: "TestLight"

3. **Position**:
   - X: 0
   - Y: 10
   - Z: 0

4. **Rotation**:
   - X: 90 (pointing straight down)
   - Y: 0
   - Z: 0

5. **Settings**:
   ```
   Type: Spot
   Mode: Realtime
   Range: 50
   Spot Angle: 60
   Intensity: 10
   Color: White
   Shadows: No Shadows
   ```

6. **Press Play**

**Expected**: You should see a BRIGHT white cone on the floor at origin.

**If you DON'T see this**: Problem is with project settings, not light settings.

---

## Step 6: Check Scene View vs Game View

**Important**: Are you looking in the right view?

1. **Scene View** (editor):
   - Shows lights even when not playing
   - Has its own lighting settings

2. **Game View** (what player sees):
   - Only shows what camera sees
   - This is what matters!

**Make sure you're testing in GAME VIEW (Play mode)!**

---

## Step 7: Check if URP is Actually Active

1. **Window → Rendering → Lighting**
2. **Look at top of window**
3. **Should say "Universal Render Pipeline"**
4. **If it says "Built-in Render Pipeline"**: URP is not active!

### To fix:
1. **Edit → Project Settings → Graphics**
2. **Assign URP asset to "Scriptable Render Pipeline Settings"**
3. **Restart Unity**

---

## Step 8: Check Console for Errors

1. **Open Console** (Ctrl+Shift+C)
2. **Look for RED errors** (not warnings)
3. **Common errors**:
   - "Render Pipeline asset is null"
   - "Camera is not rendering"
   - "URP asset missing"

**If you see errors**: Fix those first!

---

## Step 9: Nuclear Option - Reset Lighting

If nothing works:

1. **Window → Rendering → Lighting**
2. **Click "Generate Lighting"** (bottom right)
3. **Wait for it to complete**
4. **Test again**

---

## Step 10: Check Layer Culling

1. **Select a light**
2. **Check "Culling Mask"**
3. **Should be "Everything"**
4. **If not**: Change to "Everything"

---

## Common Issues & Solutions

### Issue: Lights work in Scene View but not Game View
**Solution**: Camera settings wrong or URP not active

### Issue: Lights don't show at all
**Solution**: Mode is "Baked" instead of "Realtime"

### Issue: Scene is completely black
**Solution**: No lights enabled OR camera not rendering

### Issue: Lights flicker or disappear
**Solution**: Too many lights, disable shadows

### Issue: Can't see light cones
**Solution**: Need fog OR scene too bright

---

## Emergency Test Scene

Create a minimal test:

1. **New Scene** (File → New Scene)
2. **Add Plane** (GameObject → 3D Object → Plane)
3. **Add Spot Light** above plane
4. **Set light to Intensity 10, Range 50**
5. **Press Play**

**If you see light on plane**: Your project works, issue is with your scene.

**If you still don't see light**: Project settings are broken.

---

## What to Check in Your Scene

Since intensity 1000 doesn't work, check:

1. **Are lights actually enabled?** (checkmark)
2. **Is Mode set to Realtime?** (not Baked)
3. **Is camera rendering?** (check Game View)
4. **Is URP active?** (check Graphics settings)
5. **Any errors in Console?** (red errors)

---

## Send Me This Info

If still not working, tell me:

1. **Unity version**: (e.g., 6000.0.53f1)
2. **Render pipeline**: Built-in or URP?
3. **Light Mode**: Realtime, Baked, or Mixed?
4. **Any errors in Console?**: Yes/No
5. **Can you see test light?**: Yes/No
6. **Scene View or Game View?**: Which are you testing in?

---

**Most likely issue**: Lights are set to "Baked" mode instead of "Realtime"!

Check that first! 🔍
