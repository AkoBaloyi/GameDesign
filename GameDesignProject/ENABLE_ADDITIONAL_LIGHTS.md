# ⚡ Enable Additional Lights - Quick Fix

## The Problem
Only Directional Light works = Additional Lights are disabled in URP Renderer

---

## The Fix (3 steps, 2 minutes)

### Step 1: Open Graphics Settings
```
Edit → Project Settings → Graphics
```

### Step 2: Open URP Renderer
```
1. Find "Scriptable Render Pipeline Settings"
2. Click on the asset (UniversalRenderPipelineAsset)
3. Look for "Renderer List" or "Default Renderer"
4. Click on the renderer asset
```

### Step 3: Enable Additional Lights
```
In the Renderer Inspector:

Lighting section:
├─ Additional Lights: Change to "Per Pixel"
└─ Per Object Limit: Set to 8
```

**Save and Press Play - Lights should work now!**

---

## Can't Find It? Alternative Method

### Search in Project Window:

1. **Project window → Search**: "Renderer"
2. **Find**: "ForwardRenderer" or "UniversalRenderer"
3. **Select it**
4. **In Inspector**:
   ```
   Lighting:
   ├─ Additional Lights: Per Pixel
   └─ Per Object Limit: 8
   ```

---

## Visual Path

```
Edit → Project Settings → Graphics
    ↓
Click on [UniversalRenderPipelineAsset]
    ↓
Find "Renderer List" or "Default Renderer"
    ↓
Click on [ForwardRenderer] or [UniversalRenderer]
    ↓
Lighting section:
    ↓
Additional Lights: Per Pixel ← CHANGE THIS!
Per Object Limit: 8 ← SET THIS!
    ↓
Save (Ctrl+S)
    ↓
Press Play
    ↓
Lights work! ✅
```

---

## What to Set

```
Additional Lights: Per Pixel (NOT Disabled!)
Per Object Limit: 8
```

**That's it!**

---

## After This Fix

- ✅ Spot lights will work
- ✅ Point lights will work
- ✅ All 27 factory lights will be visible
- ✅ Power cell light will work
- ✅ Console light will work

---

**This is 100% your issue! Enable Additional Lights and everything will work!** 💡
