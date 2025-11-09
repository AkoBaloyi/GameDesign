# ⚡ Realtime Lighting Setup (No Baking Needed!)

## Good News!
You DON'T need to bake lighting for this game. Unity URP uses realtime lighting which is perfect for your needs.

---

## Why You Can't See Lights

Your lights are probably **too weak** or the **scene is too bright**. Here's the fix:

---

## 3-Step Fix (5 minutes)

### Step 1: Make Scene Dark (1 min)

1. **Find Directional Light** in Hierarchy
2. **Set Intensity to 0.2** (very dim)
3. **Or disable it completely** for testing

**Why**: If the scene is already bright from the sun, you won't see your factory lights.

---

### Step 2: Configure ONE Factory Light (2 min)

1. **Select one factory light**
2. **Set these values**:
   ```
   Type: Spot
   Mode: Realtime (should be default)
   Range: 30
   Spot Angle: 60
   Intensity: 5
   Color: White
   ```
3. **Rotate it to point DOWN**:
   - Rotation X: 90
   - (The light cone should point at the floor)

4. **Press Play** - You should see a BRIGHT cone on the floor!

---

### Step 3: Apply to All Lights (2 min)

If Step 2 worked:
1. **Copy the working light's settings**
2. **Apply to all other factory lights**
3. **Or just increase Intensity on all lights to 5**

---

## Quick Test

**In Scene View** (not Play mode):
1. Select a light
2. Look at Scene view
3. You should see a **yellow cone** showing light direction
4. The cone should point at the floor

**If you don't see the cone**:
- Light might be disabled
- Light might be set to "Baked" instead of "Realtime"

---

## Realtime vs Baked

**Realtime** (What you want):
- ✅ Lights can turn on/off during gameplay
- ✅ No baking needed
- ✅ Works immediately
- ✅ Perfect for your game

**Baked** (You DON'T need this):
- ❌ Lights are "frozen" in place
- ❌ Requires baking process
- ❌ Can't turn on/off during gameplay
- ❌ Not needed for your game

---

## Settings for Each Light Type

### Factory Spotlights (27 lights):
```
Type: Spot
Mode: Realtime
Range: 25-30
Spot Angle: 60
Intensity: 4-5
Color: White or Warm White
Shadows: No Shadows (for performance)
```

### Power Cell Light:
```
Type: Point
Mode: Realtime
Range: 15
Intensity: 2
Color: Orange #FF8000
```

### Power Bay Light:
```
Type: Spot
Mode: Realtime
Range: 20
Intensity: 5
Color: Cyan #00FFFF
```

### Console Light:
```
Type: Point
Mode: Realtime
Range: 10
Intensity: 2
Color: Green #00FF00
```

---

## Make Lights More Visible

### Add Fog (Makes light beams visible):
1. Window → Rendering → Lighting
2. Environment tab
3. Check **Fog** ✓
4. Density: 0.01
5. Color: Dark gray

### Darken Ambient:
1. Same Lighting window
2. Environment tab
3. Ambient Mode: Color
4. Ambient Color: Very dark #0A0A0A
5. Ambient Intensity: 0.3

---

## Troubleshooting

### "I still can't see lights!"

**Check 1**: Is the light enabled?
- Component should have checkmark
- If grayed out, click to enable

**Check 2**: Is Mode set to Realtime?
- Select light
- Check "Mode" dropdown
- Should say "Realtime"

**Check 3**: Is Intensity high enough?
- Try setting to 10 for testing
- If you see it now, it was just too dim

**Check 4**: Is Range large enough?
- Try setting to 50 for testing
- If you see it now, range was too small

**Check 5**: Is scene too bright?
- Disable Directional Light
- If you see lights now, scene was too bright

---

## Emergency Test

Create a test light to verify lighting works:

1. **GameObject → Light → Spot Light**
2. **Position**: (0, 5, 0) - above origin
3. **Rotation**: (90, 0, 0) - pointing down
4. **Settings**:
   - Mode: Realtime
   - Range: 50
   - Intensity: 10
   - Spot Angle: 60

5. **Disable Directional Light**
6. **Press Play**

**You should see**: Bright white cone on floor

**If you see this**: Lighting works! Your factory lights just need better settings.

**If you don't see this**: Check Console for errors or lighting might be disabled in project settings.

---

## Performance Note

**Realtime lights are fine for your game!**
- 27 lights is totally manageable
- No shadows = good performance
- URP handles realtime lights well

**Don't worry about baking** - it's not needed and would make your lights unable to turn on/off during gameplay!

---

## Summary

1. ✅ You DON'T need baked lighting
2. ✅ Realtime is perfect for your game
3. ✅ Just increase light Intensity to 5
4. ✅ Darken Directional Light to 0.2
5. ✅ Add fog for visible beams
6. ✅ Done!

**Your lights will work great with these settings!** 💡
