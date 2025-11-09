# 🔧 Fix: Factory Sound Plays at Launch

## The Problem
Factory ambient sound plays immediately instead of waiting for power restoration.

---

## The Fix (30 seconds)

### Step 1: Find the AudioSource
1. **Select FactoryAmbient GameObject** (or whatever you named it)
2. **Look at AudioSource component in Inspector**

### Step 2: Uncheck "Play On Awake"
```
AudioSource component:
├─ AudioClip: [Factory sound] ✓
├─ Play On Awake: ✗ UNCHECK THIS!
├─ Loop: ✓
└─ Volume: 0.3
```

**Just uncheck that one box!**

### Step 3: Test
1. Press Play
2. Game should start **SILENT**
3. Insert power cell
4. Factory sound should start when lights activate

---

## If You Have Multiple AudioSources

Check ALL AudioSources in the scene:
- Find any with factory ambient sound
- Uncheck "Play On Awake" on all of them
- Only the one assigned to LightsController should play (via script)

---

## Visual Check

**Before fix**:
```
AudioSource:
  Play On Awake: ☑ ← WRONG!
```

**After fix**:
```
AudioSource:
  Play On Awake: ☐ ← CORRECT!
```

---

**That's it! Just uncheck one box and it will work!** ✅
