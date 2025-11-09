# 🔧 Fix: Only Directional Light Works

## The Problem

If ONLY the Directional Light works but Spot/Point lights don't, the **URP Renderer** is configured to disable additional lights.

---

## The Fix (2 minutes)

### Step 1: Find URP Renderer Asset

1. **Edit → Project Settings → Graphics**
2. **Find "Scriptable Render Pipeline Settings"**
3. **Click on the asset** (should be "UniversalRenderPipelineAsset" or similar)
4. **This opens it in Inspector**

### Step 2: Find Renderer List

In the URP Asset Inspector:
1. **Look for "Renderer List"** or **"Default Renderer"**
2. **Click on the renderer** (might be called "UniversalRenderer" or "ForwardRenderer")
3. **This opens the Renderer asset**

### Step 3: Enable Additional Lights

In the Renderer asset:
1. **Find "Lighting" section**
2. **Look for "Additional Lights"** or **"Per Object Limit"**
3. **Settings should be**:
   ```
   Additional Lights: Enabled (or Per Pixel)
   Per Object Limit: 8 (or higher)
   ```

4. **If "Additional Lights" is disabled**: Enable it!

---

## Alternative Path (If Above Doesn't Work)

### Find Renderer in Project Window:

1. **Open Project window**
2. **Search**: "Renderer" or "ForwardRenderer" or "UniversalRenderer"
3. **Select the Renderer asset**
4. **In Inspector, check**:
   ```
   Lighting:
   ├─ Additional Lights: Per Pixel (or Enabled)
   └─ Per Object Limit: 8
   ```

---

## Unity 6 Specific Settings

In the **URP Renderer** asset, check:

```
Rendering:
├─ Rendering Path: Forward or Forward+
└─ Depth Priming Mode: Auto

Lighting:
├─ Main Light: Enabled
├─ Additional Lights: Per Pixel ← CRITICAL!
├─ Per Object Limit: 8 ← CRITICAL!
└─ Additional Lights Shadow: Enabled (optional)

Shadows:
├─ Max Distance: 50
└─ Cascade Count: 1 or 2
```

**The key setting**: **Additional Lights must be "Per Pixel" or "Enabled"!**

---

## Step-by-Step Visual Guide

### 1. Open Graphics Settings:
```
Edit → Project Settings → Graphics
```

### 2. Click on URP Asset:
```
Scriptable Render Pipeline Settings: [Click this asset]
```

### 3. In Inspector, find Renderer:
```
Renderer List:
└─ Default: [Click this]
```

### 4. Enable Additional Lights:
```
Lighting:
├─ Additional Lights: Per Pixel ← Change this!
└─ Per Object Limit: 8 ← Set this!
```

---

## Quick Test

After changing settings:

1. **Save** (Ctrl+S)
2. **Press Play**
3. **Your Spot/Point lights should now work!**

---

## If You Can't Find Renderer Asset

### Create New Renderer:

1. **Right-click in Project window**
2. **Create → Rendering → URP Renderer (Forward)**
3. **Name it "ForwardRenderer"**
4. **Select it**
5. **In Inspector**:
   ```
   Lighting:
   ├─ Additional Lights: Per Pixel
   └─ Per Object Limit: 8
   ```

6. **Assign to URP Asset**:
   - Select URP Asset
   - Renderer List → Add this new renderer

---

## Common Unity 6 Settings

### For Indoor Factory Game:

**URP Asset**:
```
Quality:
├─ HDR: Enabled
├─ Anti Aliasing: MSAA or TAA
└─ Render Scale: 1.0

Lighting:
├─ Main Light: Enabled
└─ Additional Lights: Enabled

Shadows:
├─ Max Distance: 50
└─ Soft Shadows: Enabled
```

**Renderer Asset**:
```
Rendering Path: Forward+

Lighting:
├─ Additional Lights: Per Pixel
├─ Per Object Limit: 8
└─ Additional Lights Shadow: Enabled

Post-processing:
└─ Enabled
```

---

## Why This Happens

Unity 6 URP has **"Additional Lights"** disabled by default for performance.

**Directional Light** = Main Light (always works)
**Spot/Point Lights** = Additional Lights (need to be enabled)

---

## After Fix

Once "Additional Lights" is enabled:
- ✅ Spot lights will work
- ✅ Point lights will work
- ✅ Up to 8 lights per object (configurable)
- ✅ Your factory lights will be visible!

---

## Performance Note

**Per Object Limit: 8** means each object can be lit by up to 8 lights.

- **8 lights**: Good for most games
- **4 lights**: Better performance
- **16 lights**: More lights but slower

For your factory: **8 is perfect!**

---

**This is definitely your issue! Enable "Additional Lights" in the Renderer and your lights will work!** 🎯
