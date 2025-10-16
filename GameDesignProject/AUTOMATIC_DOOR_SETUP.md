# Automatic Door Setup Guide

## ✨ What This Does
- Door opens automatically when player walks near it
- No button press needed (F key, E key, nothing!)
- Smooth sliding animation
- Works with any door structure (parent/child)

---

## 🚀 Quick Setup (5 Steps)

### Step 1: Remove Old Scripts
1. Select your Door GameObject (parent or child, whichever has the old scripts)
2. Remove these components if they exist:
   - `DoorInteractor`
   - `DoorDebugHelper`
   - Any trigger colliders (we don't need them anymore!)

### Step 2: Add AutomaticDoor Script
1. Select the **PARENT "Door" GameObject** (the empty parent)
2. Add Component → `AutomaticDoor`

### Step 3: Configure the Script

In the Inspector, set these values:

**Door Transform:**
- Drag your **CHILD "Door"** (the one that moves) into this field

**Door Positions:**
- **Close Pos**: Leave as (0, 0, 0) - this is where door starts
- **Open Pos**: Set based on how you want door to open:
  - Slide UP: `(0, 2.5, 0)` - door moves up 2.5 units
  - Slide RIGHT: `(2, 0, 0)` - door moves right 2 units
  - Slide LEFT: `(-2, 0, 0)` - door moves left 2 units
  - Slide BACK: `(0, 0, 2)` - door moves backward 2 units

**Detection Settings:**
- **Detection Distance**: `3` or `4` (how close player needs to be)
- **Trigger Layer Mask**: 
  - Click the dropdown
  - Check ONLY "Player" layer
  - (If Player doesn't have a layer, you can leave it as "Everything")

**Animation Settings:**
- **Animation Duration**: `1` (how many seconds to open/close)
- **Movement Curve**: Leave as default (smooth ease in/out)

### Step 4: Remove Old Player Scripts (Optional Cleanup)
If you added these for the old door system, you can remove them:
- `PlayerInteractionHandler` (on Player)
- `InteractionBroadcaster` (on Player)

The door doesn't need ANY input system setup now!

### Step 5: Test It!
1. Press Play
2. Walk toward the door
3. Door should open automatically when you get close!
4. Walk away
5. Door should close automatically!

---

## 🎨 Customization Options

### Different Door Types

**Sliding Door (Up):**
```
Close Pos: (0, 0, 0)
Open Pos: (0, 2.5, 0)
```

**Sliding Door (Sideways):**
```
Close Pos: (0, 0, 0)
Open Pos: (2, 0, 0)
```

**Double Doors:**
- Create two door objects
- Each has its own AutomaticDoor script
- Left door: Open Pos = (-1.5, 0, 0)
- Right door: Open Pos = (1.5, 0, 0)

**Rotating Door (Advanced):**
- This script moves position, not rotation
- For rotating doors, you'd need a different script

---

## 🔍 Troubleshooting

### Door doesn't open:
1. Check the yellow wire sphere in Scene view (shows detection range)
2. Make sure Player is within the sphere
3. Check "Trigger Layer Mask" includes Player's layer
4. Check Console for "[AutomaticDoor]" messages

### Door opens but doesn't move:
1. Make sure "Door Transform" field is assigned (the child door)
2. Check "Open Pos" is different from "Close Pos"
3. Try the Context Menu: Right-click script → "Test Open Door"

### Door moves to wrong position:
1. The door moves relative to its parent
2. Adjust "Open Pos" values
3. Use the Scene view gizmos (cyan line) to see movement path

### Door is too slow/fast:
- Adjust "Animation Duration" (lower = faster, higher = slower)

---

## 📊 Visual Setup

Your hierarchy should look like:
```
Door (Parent - Empty GameObject)
├── AutomaticDoor script HERE
└── Door (Child - the actual door mesh)
    └── This goes in "Door Transform" field
```

---

## ✅ Advantages Over Old System

| Old System | New System |
|------------|------------|
| Requires F key press | Automatic - no input needed |
| Needs Input System setup | No input system required |
| Needs trigger colliders | Uses sphere detection |
| Complex wiring | Just drag and drop |
| Parent/child issues | Works with any structure |

---

## 🎯 That's It!

No Input System setup needed!
No trigger colliders needed!
No F key, E key, or any key needed!

Just walk up to the door and it opens! 🚪✨
