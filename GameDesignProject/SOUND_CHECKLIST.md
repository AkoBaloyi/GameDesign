# ✅ Sound Setup Checklist

## For Each Sound, Check These:

---

## PowerBay (Transformer Buzz)

**GameObject**: PowerBay

### AudioSource Component:
- [ ] Component exists
- [ ] AudioClip: Transformer sound
- [ ] Play On Awake: ☑
- [ ] Loop: ☑
- [ ] Volume: 0.5
- [ ] Mute: ☐

### PowerBay Script:
- [ ] `audioSource` field → AudioSource component

**Test**: Should buzz constantly at power bay

---

## LightsController (Generator Roar)

**GameObject**: LightsController

### AudioSource Component:
- [ ] Component exists
- [ ] (AudioClip can be empty, uses PlayOneShot)

### LightsController Script:
- [ ] `audioSource` field → AudioSource component
- [ ] `powerOnSfx` field → Generator sound file

**Test**: Should roar when power cell inserted

---

## FactoryConsole (PlayStation Boot)

**GameObject**: FactoryConsole

### AudioSource Component:
- [ ] Component exists
- [ ] (AudioClip can be empty, uses PlayOneShot)

### FactoryConsole Script:
- [ ] `audioSource` field → AudioSource component
- [ ] `activationSound` field → PlayStation sound file

**Test**: Should boot when console activated

---

## FactoryAmbient (Factory Sound)

**GameObject**: FactoryAmbient

### AudioSource Component:
- [ ] Component exists
- [ ] AudioClip: Factory ambient sound
- [ ] Play On Awake: ☐ UNCHECKED!
- [ ] Loop: ☑
- [ ] Volume: 0.3

### LightsController Script:
- [ ] `factoryAmbientSource` field → FactoryAmbient AudioSource

**Test**: Should start when lights activate

---

## Quick Visual Guide

```
PowerBay
├─ AudioSource ✓
│  ├─ Clip: Transformer
│  ├─ Play On Awake: ☑
│  └─ Loop: ☑
└─ PowerBay Script
   └─ audioSource: [AudioSource]

LightsController
├─ AudioSource ✓
└─ LightsController Script
   ├─ audioSource: [AudioSource]
   ├─ powerOnSfx: [Generator file]
   └─ factoryAmbientSource: [FactoryAmbient AudioSource]

FactoryConsole
├─ AudioSource ✓
└─ FactoryConsole Script
   ├─ audioSource: [AudioSource]
   └─ activationSound: [PlayStation file]

FactoryAmbient
└─ AudioSource ✓
   ├─ Clip: Factory ambient
   ├─ Play On Awake: ☐
   └─ Loop: ☑
```

---

## Common Mistakes

❌ AudioSource component missing → Add it!
❌ Script field empty → Assign AudioSource
❌ Sound file not assigned → Drag sound file
❌ Volume is 0 → Increase volume
❌ Mute is checked → Uncheck it

---

**Check each one systematically and tell me which are still not working!** 🔊
