# 🔧 Fix These Issues NOW

## Issue 1: Arrow Still Points Up

### The Problem
Unity might have cached the old version of the script.

### The Fix (1 minute):

**Option A: Force Recompile**
1. In Unity, go to **Assets → Reimport All**
2. Wait for it to finish
3. Remove and re-add ObjectiveWaypoint to power cell
4. Arrow should now point down!

**Option B: Manual Arrow (If still not working)**
1. Don't use ObjectiveWaypoint script
2. Create arrow manually:
   - Create Cylinder above power cell
   - Rotate it to point down
   - Add emissive orange material
   - Make it bob up and down (optional)

**Option C: Skip the Arrow**
- Just make power cell HUGE (scale 5x)
- Add bright orange light
- It will be impossible to miss without arrow!

---

## Issue 2: Shadow Atlas Warnings

### The Problem
Too many lights have shadows enabled.

### The Fix (2 minutes):

1. **Select ALL factory lights**:
   - Hold Ctrl
   - Click each factory light in Hierarchy
   - Or select parent object if they're grouped

2. **In Inspector**:
   - Find "Shadows" dropdown
   - Change to **"No Shadows"**

3. **Done!** Warnings will stop.

**Why this is fine**:
- Factory lights don't need shadows
- Better performance
- Lights still look great
- No visual difference

---

## Quick Test

After fixes:
1. Press Play
2. Check Console - no more shadow warnings
3. Power cell should be easy to find (huge + glowing)
4. Arrow optional - not critical if power cell is big enough

---

## Priority

**Most Important**:
1. ✅ Make power cell HUGE (scale 5x)
2. ✅ Add bright orange light to power cell
3. ✅ Disable shadows on factory lights
4. ⭐ Arrow is nice-to-have but not critical

**If power cell is HUGE and GLOWING, players will find it easily even without arrow!**

---

## Next Steps

After these fixes:
1. Continue with **DO_THIS_NOW.md**
2. Complete the 5 quick wins
3. Test the complete loop
4. Add polish

---

**Don't get stuck on the arrow - a huge glowing power cell is enough!** 🎯
