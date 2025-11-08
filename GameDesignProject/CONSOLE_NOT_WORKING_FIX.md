# Console Not Working - Troubleshooting Guide

## The Problem
You can pick up the power cell and insert it into the power bay, but when you reach the console, pressing F does nothing.

---

## 🎯 Most Likely Cause

**The console is not being enabled by the LightsController.**

The console starts disabled (`canActivate = false`) and must be enabled by the LightsController after the glowing path activates.

---

## ✅ Step-by-Step Fix

### Step 1: Check LightsController Reference (90% of the time this is the issue)

1. **Select LightsController** GameObject in Hierarchy
2. **Look in Inspector** for the FactoryConsole script
3. **Find the field**: `factoryConsole`
4. **Is it assigned?**
   - ❌ If empty/None → **This is your problem!**
   - ✅ If assigned → Go to Step 2

**To fix:**
- Drag the **FactoryConsole** GameObject from Hierarchy into the `factoryConsole` field
- Save the scene (Ctrl+S)
- Test again

---

### Step 2: Check ObjectiveManager UnityEvents

1. **Select ObjectiveManager** in Hierarchy
2. **Scroll to UnityEvents section**
3. **Find**: `onStartGlowingPath`
4. **Should have**:
   - Object: LightsController
   - Function: LightsController.ActivateGlowingPath()
5. **If missing**:
   - Click + button
   - Drag LightsController GameObject
   - Select function: ActivateGlowingPath()

---

### Step 3: Test with Debug Mode

I've added debug logging to help you find the issue.

1. **Press Play**
2. **Open Console** (Ctrl+Shift+C)
3. **Complete the power cell insertion**
4. **Watch for these messages**:

```
[PowerBay] Inserting power cell...
[PowerBay] Power cell inserted successfully!
[LightsController] Activating lights...
[LightsController] Lights activated!
[LightsController] Activating glowing path...
[LightsController] Enabling factory console...
[FactoryConsole] Console enabled for activation
[LightsController] Glowing path activated!
```

**If you see:**
- ❌ `[LightsController] factoryConsole is NULL!`
  - **Fix**: Assign FactoryConsole in LightsController Inspector

- ❌ No "Activating glowing path" message
  - **Fix**: Check ObjectiveManager.onStartGlowingPath UnityEvent

- ❌ No "Console enabled" message
  - **Fix**: LightsController.factoryConsole is not assigned

---

### Step 4: Manual Test (Bypass the System)

I've added a debug key to force-enable the console:

1. **Press Play**
2. **Walk to the console**
3. **Press C key** (not F)
4. **Console should enable immediately**
5. **Now try F key**

**If F works after pressing C:**
- Problem confirmed: Console is not being enabled automatically
- Fix: Check LightsController.factoryConsole assignment

**If F still doesn't work after C:**
- Different problem: Check detection range and player layer

---

### Step 5: Check Detection Settings

If console is enabled but F still doesn't work:

1. **Select FactoryConsole** in Hierarchy
2. **Check these settings**:
   - `detectionRange` → Should be **5** or higher
   - `playerLayer` → Should be **"Player"**
3. **Select Player** in Hierarchy
4. **Check Layer** (top of Inspector) → Should be **"Player"**

---

### Step 6: Check Distance

While in Play mode:

1. **Press F12** to show diagnostic
2. **Walk to console**
3. **Look at "DISTANCES" section**
4. **Distance to Console** should be less than 5

If distance is more than 5:
- Increase `detectionRange` in FactoryConsole
- Or move console closer to path

---

## 🔍 Additional Debug Info

### Check Console Light

When console is enabled, the light should turn yellow:

1. **Select FactoryConsole** in Hierarchy
2. **Find**: `consoleLight` field
3. **Is a Light assigned?**
   - If yes: Light should turn yellow when enabled
   - If no: Add a Light component to console, assign it

### Check Console Prompt UI

When you're near the console and it's enabled, you should see "Press F":

1. **Check**: `promptUI` field in FactoryConsole
2. **Is it assigned?**
   - If no: Create World Space Canvas above console
   - Add TextMeshProUGUI: "Press F to Activate Console"
   - Assign Canvas to promptUI field

---

## 🎯 Quick Checklist

Run through this checklist:

- [ ] LightsController.factoryConsole is assigned
- [ ] ObjectiveManager.onStartGlowingPath → LightsController.ActivateGlowingPath()
- [ ] FactoryConsole.objectiveManager is assigned
- [ ] FactoryConsole.detectionRange = 5 or higher
- [ ] FactoryConsole.playerLayer = "Player"
- [ ] Player GameObject Layer = "Player"
- [ ] Console shows debug message "Console enabled for activation"
- [ ] Console light turns yellow when enabled
- [ ] "Press F" prompt appears when near console

---

## 🧪 Test Sequence

After fixing:

1. **Press Play**
2. **Open Console** (Ctrl+Shift+C) - watch for messages
3. **Pick up power cell** (E)
4. **Insert into power bay** (F)
5. **Watch Console for messages**:
   - Should see lights activating
   - Should see path activating
   - Should see "Console enabled"
6. **Follow glowing path to console**
7. **Press F12** - check distance to console
8. **Walk close to console** (within 5 units)
9. **Look for "Press F" prompt**
10. **Press F** - should activate!

---

## 🆘 Still Not Working?

### Try This Emergency Fix

Add this to FactoryConsole Start() method temporarily:

```csharp
void Start()
{
    // TEMPORARY: Enable console immediately for testing
    Invoke("EnableConsole", 5f); // Enable after 5 seconds
}
```

If this works, the problem is definitely in the LightsController connection.

### Check These Files

Make sure these are set up:
1. **LightsController.cs** - Has factoryConsole field assigned
2. **ObjectiveManager** - Has onStartGlowingPath event connected
3. **FactoryConsole.cs** - Has objectiveManager assigned

---

## 💡 Most Common Solutions

**90% of the time it's one of these:**

1. ✅ **LightsController.factoryConsole not assigned** → Assign it!
2. ✅ **ObjectiveManager.onStartGlowingPath not connected** → Connect it!
3. ✅ **Detection range too small** → Increase to 5+
4. ✅ **Player layer not set** → Set to "Player"

---

After fixing, you should see the complete sequence work perfectly! 🎉
