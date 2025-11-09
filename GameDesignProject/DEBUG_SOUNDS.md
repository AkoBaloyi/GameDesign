# 🔍 Debug: Other Sounds Not Playing

## Which sounds aren't working?

Let's check each one:

---

## 1. Transformer Buzz (Power Bay)

**Should**: Play constantly at power bay (before power restored)

### Check:
1. **Select PowerBay GameObject**
2. **AudioSource component**:
   ```
   AudioClip: Transformer sound ✓
   Play On Awake: ☑ Checked
   Loop: ☑ Checked
   Volume: 0.5 (not 0!)
   Mute: ☐ Unchecked
   ```

3. **PowerBay script**:
   - Find `audioSource` field
   - Should have AudioSource assigned

**Test**: Stand near power bay, should hear buzzing

---

## 2. Generator Sound (Power Restoration)

**Should**: Play when power cell is inserted

### Check:
1. **Select LightsController GameObject**
2. **AudioSource component** (add if missing):
   - Should exist on GameObject

3. **LightsController script**:
   ```
   audioSource: AudioSource component ✓
   powerOnSfx: Generator sound file ✓
   ```

4. **Check Console** when inserting power cell:
   - Look for: `[LightsController] Activating lights...`
   - Should play sound during this

**Test**: Insert power cell, should hear generator roar

---

## 3. PlayStation Sound (Console Activation)

**Should**: Play when activating console

### Check:
1. **Select FactoryConsole GameObject**
2. **AudioSource component** (add if missing):
   - Should exist on GameObject

3. **FactoryConsole script**:
   ```
   audioSource: AudioSource component ✓
   activationSound: PlayStation sound file ✓
   ```

4. **Check Console** when activating:
   - Look for: `[FactoryConsole] Activating console...`
   - Should play sound during this

**Test**: Activate console, should hear PlayStation boot

---

## Quick Checklist

For EACH sound that's not working:

### Step 1: AudioSource Exists?
- [ ] GameObject has AudioSource component
- [ ] Component is enabled (checkmark)

### Step 2: Sound File Assigned?
- [ ] AudioClip field has sound file
- [ ] File is not None/empty

### Step 3: Volume Not Zero?
- [ ] Volume slider is above 0
- [ ] Mute is NOT checked

### Step 4: Script Reference?
- [ ] Script has audioSource field assigned
- [ ] Script has sound clip field assigned

---

## Common Issues

### Issue: No AudioSource component
**Fix**: Add Component → Audio Source

### Issue: AudioClip is None
**Fix**: Drag sound file to AudioClip field

### Issue: Volume is 0
**Fix**: Set Volume to 0.5-1.0

### Issue: Script fields empty
**Fix**: 
- Drag AudioSource component to audioSource field
- Drag sound file to sound clip field (powerOnSfx, activationSound, etc.)

---

## Test Each Sound

### Test 1: Transformer (Power Bay)
```
1. Press Play
2. Walk to power bay
3. Should hear buzzing
```

### Test 2: Generator (Lights)
```
1. Pick up power cell
2. Insert into power bay
3. Should hear generator roar
4. Factory ambient should start
```

### Test 3: PlayStation (Console)
```
1. Go to console
2. Press F to activate
3. Should hear PlayStation boot
```

---

## Which sounds are you NOT hearing?

Tell me:
1. Transformer buzz? YES/NO
2. Generator roar? YES/NO
3. PlayStation boot? YES/NO
4. Factory ambient (after power)? YES/NO

I'll help fix the specific ones not working!

---

## Quick Fix Template

For any sound not working:

1. **Select the GameObject** (PowerBay, LightsController, or FactoryConsole)
2. **Add AudioSource** if missing
3. **In the script**:
   - Assign AudioSource to `audioSource` field
   - Assign sound file to sound clip field
4. **Test again**

---

**Tell me which specific sounds aren't working and I'll help fix them!** 🔊
