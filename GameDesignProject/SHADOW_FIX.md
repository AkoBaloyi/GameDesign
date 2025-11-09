# 🔧 Shadow Atlas Warning Fix

## The Warning

```
Reduced additional punctual light shadows resolution by 2 to make 5 shadow maps fit in the 2048x2048 shadow atlas
```

## What It Means

You have too many lights with shadows enabled. Unity can't fit them all in memory.

---

## Quick Fix (2 minutes)

### Option 1: Disable Shadows on Factory Lights (Recommended)

1. **Select all factory lights** (Ctrl+Click each one)
2. **In Inspector, find "Shadows"**
3. **Set to "No Shadows"**
4. **Done!**

**Why**: Factory lights don't need shadows. Saves performance and fixes warning.

---

### Option 2: Keep Shadows on Just a Few Lights

1. **Disable shadows on most lights** (set to "No Shadows")
2. **Keep shadows on 2-3 key lights** (like main spotlight)
3. **Done!**

---

### Option 3: Increase Shadow Atlas Size

1. **Edit → Project Settings → Quality**
2. **Find "Shadow Resolution"**
3. **Change from "Medium" to "High"**
4. **Or find "Shadow Atlas Size"**
5. **Increase to 4096**

**Note**: This uses more memory. Option 1 is better.

---

## Recommended Settings

### For ALL Factory Lights:
```
Shadows: No Shadows
```

**Why**:
- ✅ Better performance
- ✅ No warnings
- ✅ Lights still look great
- ✅ Shadows not needed for factory atmosphere

### If You Want Some Shadows:

**Keep shadows on**:
- 1-2 main spotlights
- Directional light (sun)

**Disable shadows on**:
- All other factory lights
- Power cell light
- Power bay light
- Console light

---

## After Fix

- ✅ No more warnings
- ✅ Better performance
- ✅ Game runs smoother
- ✅ Lights still look great

---

**Just disable shadows on your factory lights and the warning will go away!** 🎉
