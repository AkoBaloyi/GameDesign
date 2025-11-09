# ✅ Unity 6 Light Checklist

## Check These in Inspector:

### On Each Light:
```
Light Component:
├─ Mode: Realtime ← MUST BE THIS!
├─ Type: Spot (or Point)
├─ Range: 25-30
├─ Intensity: 5-10
├─ Color: White
├─ Outer Spot Angle: 60 (if Spot)
├─ Shadows: No Shadows
└─ Component enabled: ✓
```

### On Main Camera:
```
Components:
├─ Camera ✓
└─ Universal Additional Camera Data ✓
```

### In Project Settings:
```
Edit → Project Settings → Graphics:
└─ Scriptable Render Pipeline Settings: [URP Asset] ✓

Edit → Project Settings → Quality:
└─ Render Pipeline Asset: [URP Asset] ✓
```

---

## Test Procedure:

1. [ ] Create test Spot Light
2. [ ] Set Mode to Realtime
3. [ ] Set Intensity to 10
4. [ ] Position at (0, 10, 0)
5. [ ] Rotate to (90, 0, 0)
6. [ ] Press Play
7. [ ] See bright cone? → Lighting works!

---

## Common Mistakes:

❌ Mode is "Baked" → Change to "Realtime"
❌ Light component disabled → Enable it
❌ GameObject inactive → Activate it
❌ Testing in Scene View → Test in Game View (Play mode)
❌ Camera missing component → Add "Universal Additional Camera Data"

---

## If Test Light Works:

Your lighting system is fine! Just apply same settings to factory lights:
- Mode: Realtime
- Intensity: 5-10
- Range: 25-30

---

## If Test Light Doesn't Work:

Check:
1. Camera has "Universal Additional Camera Data"?
2. URP asset assigned in Graphics settings?
3. Any errors in Console?

---

**The #1 fix: Change Light Mode to "Realtime"!** 💡
