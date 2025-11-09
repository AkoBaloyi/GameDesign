# ⚡ Unity 6 - Quick Light Fix

## Do These Steps in Order:

### Step 1: Check Light Mode (30 seconds)

1. **Select a factory light**
2. **Look at Light component in Inspector**
3. **Find "Mode" dropdown**
4. **Is it "Realtime"?**
   - ✅ YES: Good! Go to Step 2
   - ❌ NO (Baked/Mixed): Change to **Realtime**

5. **Apply to all lights**:
   - Select all factory lights (Ctrl+Click)
   - Change Mode to **Realtime**

---

### Step 2: Test with One Light (1 minute)

1. **Disable all factory lights** (uncheck them)
2. **Create test light**:
   - GameObject → Light → Spot Light
3. **Settings**:
   - Position: (0, 10, 0)
   - Rotation: (90, 0, 0)
   - Mode: **Realtime**
   - Intensity: 10
   - Range: 50
4. **Press Play**

**Can you see a bright cone on the floor?**
- ✅ YES: Lighting works! Your factory lights need same settings.
- ❌ NO: Go to Step 3

---

### Step 3: Check Camera (1 minute)

1. **Select Main Camera**
2. **Check components**:
   - Should have "Camera"
   - Should have "Universal Additional Camera Data"
3. **If missing "Universal Additional Camera Data"**:
   - Add Component
   - Search: "Universal Additional Camera Data"
   - Add it

---

### Step 4: Check URP Asset (1 minute)

1. **Edit → Project Settings → Graphics**
2. **Find "Scriptable Render Pipeline Settings"**
3. **Is something assigned there?**
   - ✅ YES: Good!
   - ❌ NO: Need to assign URP asset

**To find URP asset**:
- Look in Project window
- Search: "UniversalRenderPipelineAsset"
- Drag it to Graphics settings

---

### Step 5: Apply Working Settings (2 minutes)

If test light worked:

1. **Copy test light settings**
2. **Apply to factory lights**:
   - Mode: Realtime
   - Intensity: 5-10
   - Range: 25-30
   - Shadows: No Shadows

3. **Enable factory lights**
4. **Delete test light**
5. **Press Play**

---

## Quick Reference

### Working Light Settings:
```
Mode: Realtime
Intensity: 5-10
Range: 25-30
Type: Spot
Outer Spot Angle: 60
Shadows: No Shadows
```

### Camera Must Have:
```
- Camera component
- Universal Additional Camera Data component
```

### Project Must Have:
```
- URP asset in Graphics settings
- URP asset in Quality settings
```

---

## Still Not Working?

**Tell me**:
1. Does test light work? (Step 2)
2. What's the Light Mode? (Realtime/Baked/Mixed)
3. Any red errors in Console?

---

**Most common fix: Change Mode from "Baked" to "Realtime"!** 🎯
