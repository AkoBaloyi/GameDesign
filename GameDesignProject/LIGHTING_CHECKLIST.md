# ✅ Lighting Checklist - Make Lights Visible

## Do These in Order:

### 1. Darken Directional Light
- [ ] Find "Directional Light" in Hierarchy
- [ ] Set Intensity to **0.2**
- [ ] (Or disable it completely for testing)

### 2. Test ONE Factory Light
- [ ] Select one factory light
- [ ] Check Mode: **Realtime** ✓
- [ ] Set Intensity: **5**
- [ ] Set Range: **30**
- [ ] Set Spot Angle: **60**
- [ ] Rotate to point down (X = 90)

### 3. Press Play
- [ ] Can you see a bright cone on the floor?
- [ ] **YES**: Great! Continue to step 4
- [ ] **NO**: See troubleshooting below

### 4. Apply to All Lights
- [ ] Select all factory lights (Ctrl+Click)
- [ ] Set Intensity: **5**
- [ ] Set Range: **30**

### 5. Add Fog (Optional)
- [ ] Window → Rendering → Lighting
- [ ] Check **Fog** ✓
- [ ] Density: **0.01**

### 6. Test Again
- [ ] Press Play
- [ ] Lights should be VERY visible now!

---

## Troubleshooting

### Can't see test light?

**Try this**:
1. Increase Intensity to **10**
2. Increase Range to **50**
3. Make sure light is **enabled** (checkmark)
4. Make sure Mode is **Realtime**
5. Disable Directional Light completely

### Still nothing?

**Create new test light**:
1. GameObject → Light → Spot Light
2. Position: (0, 5, 0)
3. Rotation: (90, 0, 0)
4. Intensity: 10
5. Range: 50
6. Press Play

If you see this, your lighting works!

---

## Expected Result

After these steps:
- ✅ Dark scene with bright light cones
- ✅ Clear difference between lit and unlit areas
- ✅ Dramatic factory atmosphere
- ✅ Lights turn on/off during gameplay

**No baking needed!** 🎉
