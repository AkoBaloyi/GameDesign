# 🔧 Fix These Issues NOW

## Issue 1: Arrow Points Up ✅ FIXED IN CODE

The ObjectiveWaypoint script is now fixed. The arrow will point DOWN at the object.

**What to do**:
1. If you already added ObjectiveWaypoint to power cell, **remove it**
2. **Add it again** (the new version will be used)
3. Arrow should now point down!

---

## Issue 2: Lights Not Visible 💡

**Good news**: You don't need baked lighting! Realtime is perfect.

### Quick Fix (3 minutes):

1. **Darken the Scene**:
   - Select **Directional Light**
   - Intensity: **0.2** (or disable it completely for testing)
   - **Why**: Scene is too bright, factory lights don't show

2. **Boost ONE Factory Light** (test):
   - Select one factory light
   - Mode: **Realtime** (should be default)
   - Intensity: **5** (make it BRIGHT)
   - Range: **30**
   - Spot Angle: **60**
   - Rotation X: **90** (point down)

3. **Press Play** - You should see BRIGHT cone on floor!

4. **If it works**:
   - Apply same settings to all 27 lights
   - Or just increase all Intensity to 5

5. **Add Fog** (optional but makes beams visible):
   - Window → Rendering → Lighting
   - Check **Fog** ✓
   - Density: **0.01**

---

### If Still Not Visible:

**Test with ONE light**:
1. Create new GameObject → Light → Spot Light
2. Position above floor
3. Rotate to point down (X rotation = 90)
4. Settings:
   - Range: 30
   - Intensity: 5
   - Spot Angle: 60
   - Color: White

5. Press Play - You should see bright cone on floor

**If this works**: Your original lights need these settings
**If this doesn't work**: Check LIGHTING_FIX.md

---

## Quick Settings Reference

### For ALL Factory Lights:
```
Type: Spot
Range: 25
Spot Angle: 60
Intensity: 4
Rotation: Point down (X = 90)
```

### Scene Settings:
```
Directional Light Intensity: 0.3
Fog: Enabled
Fog Density: 0.01
Ambient Intensity: 0.3
```

---

## Test Checklist

After fixes:
- [ ] Arrow points DOWN at power cell
- [ ] Can see light cones on floor
- [ ] Can see light beams in fog
- [ ] Dark areas are DARK
- [ ] Lit areas are BRIGHT
- [ ] Dramatic contrast

---

**Do these fixes, then continue with DO_THIS_NOW.md!** 🚀
