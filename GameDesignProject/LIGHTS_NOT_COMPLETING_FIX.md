# Lights Not Completing - Debug Guide

## The Problem

You see:
```
[PowerBay] Inserting power cell...
[LightsController] Activating lights...
[PowerBay] Power cell inserted successfully!
```

But you DON'T see:
```
[LightsController] Lights activated!
[ObjectiveManager] OnLightsActivated() called!
[LightsController] Activating glowing path...
```

This means the lights activation sequence is stuck or not completing.

---

## 🔍 What to Check Now

### Test Again with New Debug Messages

1. **Press Play**
2. **Open Console** (Ctrl+Shift+C)
3. **Insert power cell**
4. **Watch for NEW messages**:

```
[LightsController] Activating lights...
[LightsController] Found X lights to activate
[LightsController] Activated light 1/X
[LightsController] Activated light 2/X
...
[LightsController] All X lights activated!
[LightsController] Notifying ObjectiveManager...
[LightsController] ObjectiveManager.OnLightsActivated() called!
[LightsController] Lights activated!
[ObjectiveManager] OnLightsActivated() called!
[ObjectiveManager] Invoking onStartGlowingPath event...
[LightsController] Activating glowing path...
```

---

## 🎯 Possible Issues & Solutions

### Issue 1: "Found 0 lights to activate"

**Problem**: The lights array is empty!

**Solution**:
1. Select **LightsController** in Hierarchy
2. In Inspector, find **`lights[]`** array
3. **Is it empty?** → You need to add lights!
4. Click the **+** button for each light in your scene
5. Drag each **Light component** into the array slots

**Quick way to find lights**:
- In Hierarchy, search for "Light"
- Drag each Light GameObject into the lights array

---

### Issue 2: "Found 100 lights to activate"

**Problem**: Too many lights! Taking forever to activate.

**Solution**:
1. Reduce `lightActivationDelay` to 0.05 (faster)
2. Or reduce the number of lights in the array
3. Or skip the sequential activation temporarily

**Temporary fix** - Change this in LightsController:
```csharp
public float lightActivationDelay = 0.05f; // Was 0.2f
```

---

### Issue 3: Lights activate but no "Notifying ObjectiveManager" message

**Problem**: The coroutine is completing but ObjectiveManager is null.

**Solution**:
1. Select **LightsController** in Hierarchy
2. Check **`objectiveManager`** field
3. **Is it assigned?** If not, drag ObjectiveManager GameObject here

---

### Issue 4: "objectiveManager is NULL!"

**Problem**: LightsController can't find ObjectiveManager.

**Solution**:
1. Make sure ObjectiveManager GameObject exists in scene
2. Select **LightsController**
3. Assign **`objectiveManager`** field manually

---

### Issue 5: ObjectiveManager called but no "Invoking onStartGlowingPath"

**Problem**: The UnityEvent is not connected.

**Solution**:
1. Select **ObjectiveManager** in Hierarchy
2. Scroll to **`onStartGlowingPath`** UnityEvent
3. **Is it empty?** → Add the connection:
   - Click **+** button
   - Drag **LightsController** GameObject
   - Select function: **LightsController.ActivateGlowingPath()**

---

## 🚀 Quick Test - Skip Light Animation

If you want to test the rest of the loop without waiting for lights:

1. Select **LightsController** in Hierarchy
2. Set **`lightActivationDelay`** to **0** (instant)
3. Test again

---

## 🎯 Most Likely Solutions

Based on your symptoms, it's probably one of these:

### Solution A: Empty Lights Array (Most Likely)

```
1. Select LightsController
2. Expand lights[] array
3. Click + to add slots
4. Drag Light components into slots
5. Save and test
```

### Solution B: ObjectiveManager Not Assigned

```
1. Select LightsController
2. Find objectiveManager field
3. Drag ObjectiveManager GameObject
4. Save and test
```

### Solution C: UnityEvent Not Connected

```
1. Select ObjectiveManager
2. Find onStartGlowingPath event
3. Click + button
4. Drag LightsController
5. Select ActivateGlowingPath()
6. Save and test
```

---

## 📊 Expected Console Output

After fixing, you should see this complete sequence:

```
[PowerBay] Inserting power cell...
[LightsController] Activating lights...
[LightsController] Found 5 lights to activate
[LightsController] Activated light 1/5
[LightsController] Activated light 2/5
[LightsController] Activated light 3/5
[LightsController] Activated light 4/5
[LightsController] Activated light 5/5
[LightsController] All 5 lights activated!
[LightsController] Notifying ObjectiveManager...
[LightsController] ObjectiveManager.OnLightsActivated() called!
[LightsController] Lights activated!
[PowerBay] Power cell inserted successfully!
[ObjectiveManager] OnLightsActivated() called!
[ObjectiveManager] Invoking onStartGlowingPath event...
[ObjectiveManager] onStartGlowingPath event invoked!
[LightsController] Activating glowing path...
[LightsController] Found 10 path segments
[LightsController] Enabling factory console...
[FactoryConsole] Console enabled for activation
[LightsController] Glowing path activated!
```

---

## 🆘 Emergency Bypass

If you just want to test the console without fixing lights:

Add this to FactoryConsole Start():
```csharp
void Start()
{
    // TEMPORARY: Enable console after 3 seconds
    Invoke("EnableConsole", 3f);
}
```

This will enable the console 3 seconds after game starts, bypassing the whole lights system.

---

Test again and watch the Console window carefully. The new debug messages will tell you exactly where it's getting stuck!
