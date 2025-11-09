# 🔊 Audio Setup Guide

## Your Sound Files

You have 4 perfect sounds! Here's how to use them:

---

## 1. Factory Ambient (34 seconds)
**File**: Industrial/Factory/Fans Soundscape
**Use**: Starts when power is restored (MUCH BETTER!)

### Setup:
1. **Import to Unity**: Drag into Assets/Audio folder
2. **Create GameObject**: "FactoryAmbient"
3. **Add AudioSource component**
4. **Settings**:
   ```
   AudioClip: [Your factory ambient file]
   Play On Awake: UNCHECKED ✗ (starts silent!)
   Loop: Checked ✓
   Volume: 0.3
   Spatial Blend: 0 (2D sound)
   Priority: 128
   ```

5. **Assign to LightsController**:
   - Select LightsController
   - Add new field reference (see code below)
   - Drag FactoryAmbient GameObject to field

**Result**: Silent factory → Power restored → Factory comes ALIVE! 🔥

---

## 2. Generator Power Up (8:39 - LONG!)
**File**: Hobart diesel generator
**Use**: When lights activate (power restored)

### Setup:
1. **Import to Unity**
2. **Trim in audio editor** (optional):
   - Keep just the "revs up" part (10-20 seconds)
   - Or use full sound for dramatic effect
3. **Assign to LightsController**:
   - Select LightsController
   - Find `powerOnSfx` field
   - Drag sound file here

### In Code (already set up):
```csharp
// LightsController plays this when lights activate
if (audioSource != null && powerOnSfx != null)
{
    audioSource.PlayOneShot(powerOnSfx);
}
```

**Result**: DRAMATIC power-up sound when cell inserted!

---

## 3. Electrical Transformer (Duration?)
**File**: Electrical vibration/transformer
**Use**: Power Bay sparks sound OR power cell hum

### Option A: Power Bay Sparks
1. **Import to Unity**
2. **Assign to PowerBay**:
   - Select PowerBay GameObject
   - Add AudioSource (if not there)
   - AudioClip: [Transformer sound]
   - Play On Awake: Checked ✓
   - Loop: Checked ✓
   - Volume: 0.5
   - Spatial Blend: 1.0 (3D sound)

3. **Stop when power restored**:
   - In PowerBay script, add:
   ```csharp
   AudioSource sparkAudio = GetComponent<AudioSource>();
   if (sparkAudio != null) sparkAudio.Stop();
   ```

### Option B: Power Cell Hum
1. **Assign to PowerCell**
2. **Loop while not picked up**
3. **Stop when picked up**

**Result**: Electrical buzzing at power bay or power cell!

---

## 4. PlayStation Startup (18 seconds)
**File**: PS getting switched on
**Use**: Console activation sound

### Setup:
1. **Import to Unity**
2. **Assign to FactoryConsole**:
   - Select FactoryConsole
   - Find `activationSound` field
   - Drag sound file here

### In Code (already set up):
```csharp
// FactoryConsole plays this when activated
if (audioSource != null && activationSound != null)
{
    audioSource.PlayOneShot(activationSound);
}
```

**Result**: Satisfying console boot-up sound!

---

## Quick Setup Checklist

### Step 1: Import All Sounds (2 min)
- [ ] Create folder: Assets/Audio
- [ ] Drag all 4 sound files into folder
- [ ] Wait for Unity to import

### Step 2: Ambient Sound (2 min)
- [ ] Create GameObject: "AmbientAudio"
- [ ] Add AudioSource
- [ ] Assign factory ambient sound
- [ ] Loop: ✓, Volume: 0.3, 2D

### Step 3: Power Bay (3 min)
- [ ] Select PowerBay
- [ ] Add AudioSource (if needed)
- [ ] Assign transformer sound
- [ ] Loop: ✓, Volume: 0.5, 3D
- [ ] Assign to `audioSource` field in PowerBay script

### Step 4: LightsController (2 min)
- [ ] Select LightsController
- [ ] Add AudioSource (if needed)
- [ ] Assign generator sound to `powerOnSfx` field

### Step 5: Console (2 min)
- [ ] Select FactoryConsole
- [ ] Add AudioSource (if needed)
- [ ] Assign PlayStation sound to `activationSound` field

---

## Audio Settings Reference

### 2D Sounds (Ambient):
```
Spatial Blend: 0
Volume: 0.2-0.4
Loop: Yes
Play On Awake: Yes
```

### 3D Sounds (Power Bay, Console):
```
Spatial Blend: 1.0
Volume: 0.5-0.8
Loop: No (for one-shots)
Min Distance: 1
Max Distance: 20
```

### One-Shot Sounds (Effects):
```
Play via script: audioSource.PlayOneShot(clip)
Volume: 0.5-1.0
Don't loop
```

---

## Sound Timing

### Game Flow with Audio:

```
START
  ↓
[Ambient plays continuously]
  ↓
Tutorial complete
  ↓
Find power cell (transformer hum if assigned)
  ↓
Pick up cell (hum stops)
  ↓
Insert into power bay
  ↓
[GENERATOR SOUND PLAYS] ← Dramatic!
  ↓
Lights activate
  ↓
Go to console
  ↓
Activate console
  ↓
[PLAYSTATION SOUND PLAYS] ← Satisfying!
  ↓
Win screen
```

---

## Volume Mixing

**Recommended volumes**:
- Ambient: 0.3 (background)
- Transformer: 0.5 (noticeable but not loud)
- Generator: 0.7 (dramatic moment!)
- PlayStation: 0.8 (victory moment!)

**Adjust based on your preference!**

---

## Optional: Trim Long Sounds

The generator sound is 8+ minutes. You probably want just a portion:

### Option 1: Use Full Sound
- Dramatic and realistic
- Plays entire startup sequence
- Might be too long

### Option 2: Trim in Audacity (Free)
1. Download Audacity (free audio editor)
2. Open generator sound
3. Find the "rev up" part (probably 10-30 seconds)
4. Select and export just that part
5. Import trimmed version to Unity

### Option 3: Use as-is
- Unity will play it
- You can fade it out in code if needed
- Might add to atmosphere!

---

## Testing Audio

### Test Checklist:
1. [ ] Press Play
2. [ ] Hear ambient factory sound immediately
3. [ ] Hear transformer buzz at power bay
4. [ ] Insert power cell
5. [ ] Hear generator power-up (dramatic!)
6. [ ] Transformer buzz stops
7. [ ] Go to console
8. [ ] Activate console
9. [ ] Hear PlayStation startup (satisfying!)

---

## Pro Tips

1. **Volume Balance**: Ambient should be quietest, effects loudest
2. **3D Sound**: Use for localized sounds (power bay)
3. **2D Sound**: Use for ambient and UI sounds
4. **Fade Out**: If generator sound is too long, fade it after 10 seconds
5. **Test with Headphones**: Hear the full effect!

---

**Your sound choices are PERFECT for a factory game! This will add SO much atmosphere!** 🔊🎵
