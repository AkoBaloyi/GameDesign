# ⚡ Quick Light Debug - Check These NOW

## Most Common Issues (Check in Order):

### 1. Light Mode is "Baked" ❌
- [ ] Select a light
- [ ] Check "Mode" dropdown
- [ ] **Should be "Realtime"**
- [ ] If it says "Baked" or "Mixed": Change to **Realtime**

**This is the #1 reason lights don't work!**

---

### 2. Light Component is Disabled ❌
- [ ] Select a light
- [ ] Look at Light component in Inspector
- [ ] **Should have checkmark** ✓
- [ ] If no checkmark: Click to enable

---

### 3. GameObject is Disabled ❌
- [ ] Select a light
- [ ] Look at top of Inspector (GameObject name)
- [ ] **Should have checkmark** ✓
- [ ] If grayed out: Click to enable

---

### 4. Testing in Wrong View ❌
- [ ] Are you in **Game View** (Play mode)?
- [ ] NOT Scene View (editor view)
- [ ] Press Play and look at Game tab

---

### 5. URP Not Active ❌
- [ ] Edit → Project Settings → Graphics
- [ ] Check "Scriptable Render Pipeline Settings"
- [ ] **Should have URP asset assigned**
- [ ] If empty: Assign UniversalRenderPipelineAsset

---

## Quick Test

**Create test light**:
1. GameObject → Light → Spot Light
2. Position: (0, 10, 0)
3. Rotation: (90, 0, 0)
4. Mode: **Realtime**
5. Intensity: 10
6. Range: 50
7. Press Play

**Can you see it?**
- **YES**: Your lights work! Original lights have wrong settings.
- **NO**: Project settings issue. Check #5 above.

---

## Most Likely Fix

**Change all lights from "Baked" to "Realtime"**:
1. Select all factory lights
2. Find "Mode" dropdown
3. Change to **Realtime**
4. Press Play
5. Should work now!

---

**Check Mode first - that's usually the problem!** 🎯
