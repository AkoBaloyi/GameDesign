# 🎨 Color Reference Guide

## Unity Color System

Unity uses **0-1 range** for RGB values in the Inspector.
- Red = 1.0 means 255 in standard RGB
- Red = 0.5 means 128 in standard RGB
- Red = 0.0 means 0 in standard RGB

**To convert**: Divide RGB value by 255
- Example: RGB(255, 128, 0) = Unity(1.0, 0.5, 0.0)

---

## 🎯 Game Color Palette

### Power Cell (Orange)
**Use for**: Power cell, power-related objects, energy

| Format | Value |
|--------|-------|
| RGB (0-255) | (255, 128, 0) |
| Hex | #FF8000 |
| Unity (0-1) | R=1.0, G=0.5, B=0.0 |

**How to set in Unity**:
1. Click color picker
2. Set R=1.0, G=0.5, B=0.0
3. Or paste hex: #FF8000

---

### Power Bay (Cyan/Blue)
**Use for**: Power bay, electrical systems, tech

| Format | Value |
|--------|-------|
| RGB (0-255) | (0, 255, 255) |
| Hex | #00FFFF |
| Unity (0-1) | R=0.0, G=1.0, B=1.0 |

**Alternative - Electric Blue**:
| Format | Value |
|--------|-------|
| RGB (0-255) | (0, 150, 255) |
| Hex | #0096FF |
| Unity (0-1) | R=0.0, G=0.59, B=1.0 |

---

### Console Active (Green)
**Use for**: Console when active, success states, "go" signals

| Format | Value |
|--------|-------|
| RGB (0-255) | (0, 255, 0) |
| Hex | #00FF00 |
| Unity (0-1) | R=0.0, G=1.0, B=0.0 |

**Alternative - Softer Green**:
| Format | Value |
|--------|-------|
| RGB (0-255) | (0, 200, 100) |
| Hex | #00C864 |
| Unity (0-1) | R=0.0, G=0.78, B=0.39 |

---

### Console Inactive (Dark Green)
**Use for**: Console base color, inactive screens

| Format | Value |
|--------|-------|
| RGB (0-255) | (0, 100, 0) |
| Hex | #006400 |
| Unity (0-1) | R=0.0, G=0.39, B=0.0 |

---

### Sparks (Yellow-Orange)
**Use for**: Electrical sparks, warnings, energy bursts

| Format | Value |
|--------|-------|
| RGB (0-255) | (255, 200, 0) |
| Hex | #FFC800 |
| Unity (0-1) | R=1.0, G=0.78, B=0.0 |

**Alternative - Bright Yellow**:
| Format | Value |
|--------|-------|
| RGB (0-255) | (255, 255, 0) |
| Hex | #FFFF00 |
| Unity (0-1) | R=1.0, G=1.0, B=0.0 |

---

### Warning/Alert (Red)
**Use for**: Danger, errors, enemy indicators

| Format | Value |
|--------|-------|
| RGB (0-255) | (255, 0, 0) |
| Hex | #FF0000 |
| Unity (0-1) | R=1.0, G=0.0, B=0.0 |

**Alternative - Dark Red**:
| Format | Value |
|--------|-------|
| RGB (0-255) | (180, 0, 0) |
| Hex | #B40000 |
| Unity (0-1) | R=0.71, G=0.0, B=0.0 |

---

### UI Text (Cyan)
**Use for**: UI text, HUD elements, industrial feel

| Format | Value |
|--------|-------|
| RGB (0-255) | (0, 255, 255) |
| Hex | #00FFFF |
| Unity (0-1) | R=0.0, G=1.0, B=1.0 |

**Alternative - Soft Cyan**:
| Format | Value |
|--------|-------|
| RGB (0-255) | (100, 200, 255) |
| Hex | #64C8FF |
| Unity (0-1) | R=0.39, G=0.78, B=1.0 |

---

### Background/Ambient (Dark Gray)
**Use for**: UI backgrounds, panels, overlays

| Format | Value |
|--------|-------|
| RGB (0-255) | (30, 30, 30) |
| Hex | #1E1E1E |
| Unity (0-1) | R=0.12, G=0.12, B=0.12 |

**With transparency**: Set Alpha to 0.8 (80% opaque)

---

### Metal/Industrial (Gray)
**Use for**: Factory walls, machinery, metal surfaces

| Format | Value |
|--------|-------|
| RGB (0-255) | (128, 128, 128) |
| Hex | #808080 |
| Unity (0-1) | R=0.5, G=0.5, B=0.5 |

**Darker Metal**:
| Format | Value |
|--------|-------|
| RGB (0-255) | (60, 60, 60) |
| Hex | #3C3C3C |
| Unity (0-1) | R=0.24, G=0.24, B=0.24 |

---

### Rust/Wear (Brown-Orange)
**Use for**: Rust, dirt, wear on surfaces

| Format | Value |
|--------|-------|
| RGB (0-255) | (150, 75, 0) |
| Hex | #964B00 |
| Unity (0-1) | R=0.59, G=0.29, B=0.0 |

---

## 🎨 Quick Color Schemes

### Scheme 1: High Contrast (Recommended)
- **Power Cell**: Orange (#FF8000)
- **Power Bay**: Cyan (#00FFFF)
- **Console**: Green (#00FF00)
- **UI Text**: Cyan (#00FFFF)
- **Background**: Dark Gray (#1E1E1E)

### Scheme 2: Softer Industrial
- **Power Cell**: Orange (#FF8000)
- **Power Bay**: Electric Blue (#0096FF)
- **Console**: Soft Green (#00C864)
- **UI Text**: Soft Cyan (#64C8FF)
- **Background**: Dark Gray (#1E1E1E)

### Scheme 3: Dramatic
- **Power Cell**: Bright Orange (#FF8000)
- **Power Bay**: Bright Cyan (#00FFFF)
- **Console**: Bright Green (#00FF00)
- **UI Text**: White (#FFFFFF)
- **Background**: Black (#000000)

---

## 💡 How to Use in Unity

### For Lights:
1. Select Light component
2. Click Color field
3. **Option A**: Use sliders (R, G, B from 0-1)
4. **Option B**: Click hex field, paste hex code (e.g., #FF8000)

### For Materials:
1. Select Material
2. Click Base Color or Emission Color
3. **Option A**: Use sliders
4. **Option B**: Paste hex code
5. For emission: Adjust HDR intensity slider (0-10)

### For UI (TextMeshPro):
1. Select Text component
2. Click Color field
3. **Option A**: Use sliders
4. **Option B**: Paste hex code
5. Adjust Alpha for transparency

### For Particle Systems:
1. Select Particle System
2. Find Start Color
3. Click color field
4. Set color using sliders or hex

---

## 🎯 Emission Tips

**Emission Intensity Guide**:
- **0.5-1.0**: Subtle glow
- **1.0-2.0**: Noticeable glow (recommended)
- **2.0-5.0**: Strong glow
- **5.0-10.0**: Very bright, light source

**For best results**:
1. Enable Emission on material
2. Set Emission Color (same as base or brighter)
3. Set Emission Intensity to 2.0
4. Adjust based on scene lighting

---

## 🔍 Testing Colors

**In Unity Editor**:
1. Create test sphere
2. Apply material with color
3. Add light with same color
4. Check how it looks in:
   - Bright lighting
   - Dark lighting
   - With fog
   - From distance

**Adjust if**:
- Too bright: Lower intensity
- Too dark: Increase intensity or use brighter color
- Hard to see: Increase contrast with background
- Clashes: Choose complementary color

---

## 📋 Quick Reference Table

| Object | Color Name | Hex | Unity RGB |
|--------|-----------|-----|-----------|
| Power Cell | Orange | #FF8000 | (1.0, 0.5, 0.0) |
| Power Bay | Cyan | #00FFFF | (0.0, 1.0, 1.0) |
| Console Active | Green | #00FF00 | (0.0, 1.0, 0.0) |
| Console Inactive | Dark Green | #006400 | (0.0, 0.39, 0.0) |
| Sparks | Yellow-Orange | #FFC800 | (1.0, 0.78, 0.0) |
| Warning | Red | #FF0000 | (1.0, 0.0, 0.0) |
| UI Text | Cyan | #00FFFF | (0.0, 1.0, 1.0) |
| UI Background | Dark Gray | #1E1E1E | (0.12, 0.12, 0.12) |
| Metal | Gray | #808080 | (0.5, 0.5, 0.5) |
| Rust | Brown-Orange | #964B00 | (0.59, 0.29, 0.0) |

---

**Pro Tip**: Save this file and keep it open while working in Unity. Copy-paste hex codes directly into Unity's color picker!
