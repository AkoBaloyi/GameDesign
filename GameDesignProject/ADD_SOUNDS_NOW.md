# 🔊 Add Sounds NOW - 10 Minute Guide

## Your 4 Sounds:
1. ✅ Factory ambient (34s) - Background
2. ✅ Generator (8min) - Power up
3. ✅ Transformer - Electrical buzz
4. ✅ PlayStation - Console boot

---

## Quick Setup (10 minutes):

### 1. Import Sounds (2 min)
```
1. Create folder: Assets/Audio
2. Drag all 4 files into folder
3. Wait for import
```

### 2. Factory Ambient (3 min)
```
1. Create Empty GameObject: "FactoryAmbient"
2. Add Component → Audio Source
3. Assign: Factory ambient sound
4. UNCHECK: Play On Awake ✗ (starts silent!)
5. Check: Loop ✓
6. Volume: 0.3
7. Spatial Blend: 0

8. Select LightsController
9. In Inspector, find: factoryAmbientSource field
10. Drag FactoryAmbient GameObject to this field
```

**Result**: Silent factory → Power restored → Factory roars to life! 🔥

### 3. Power Bay Sound (2 min)
```
1. Select PowerBay GameObject
2. Add Component → Audio Source (if needed)
3. Assign: Transformer sound
4. Check: Play On Awake ✓
5. Check: Loop ✓
6. Volume: 0.5
7. Spatial Blend: 1.0
8. In PowerBay script: Assign to audioSource field
```

### 4. Lights Sound (2 min)
```
1. Select LightsController GameObject
2. Add Component → Audio Source (if needed)
3. In LightsController script:
   - Assign AudioSource to audioSource field
   - Assign Generator sound to powerOnSfx field
```

### 5. Console Sound (2 min)
```
1. Select FactoryConsole GameObject
2. Add Component → Audio Source (if needed)
3. In FactoryConsole script:
   - Assign AudioSource to audioSource field
   - Assign PlayStation sound to activationSound field
```

---

## Test (2 min)

Press Play and check:
- [ ] Game starts SILENT (eerie!)
- [ ] Hear transformer buzz at power bay
- [ ] Insert power cell → Hear generator power up!
- [ ] Factory ambient starts! (factory comes alive!)
- [ ] Activate console → Hear PlayStation boot!

**Perfect audio storytelling!** 🎵

---

## If Sounds Don't Play

**Check**:
1. AudioSource component exists
2. AudioClip is assigned
3. Volume is not 0
4. Mute is not checked
5. Audio Listener exists on camera

---

## Volume Recommendations

```
Ambient: 0.3 (quiet background)
Transformer: 0.5 (noticeable)
Generator: 0.7 (dramatic!)
PlayStation: 0.8 (victory!)
```

Adjust to taste!

---

**10 minutes and your game will sound AMAZING!** 🎵
