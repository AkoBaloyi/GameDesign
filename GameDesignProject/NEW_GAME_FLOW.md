# New Game Flow - Quick Reference

## 🎮 Complete Player Journey

```
┌─────────────────────────────────────────────────────────────┐
│                        TUTORIAL                              │
│  Learn: Move, Look, Jump, Sprint, Crouch, Pickup, Throw    │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                     LIGHTS GO OUT!                           │
│              "What happened?!"                               │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                  CHECK MAIN CONSOLE                          │
│  Walk to console → Press E to inspect                        │
│  Result: "Console is offline. Check power bay."             │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                   CHECK POWER BAY                            │
│  Walk to Power Grid Chamber → Press E to inspect            │
│  Result: "Sparks! Power cell damaged. Get replacement."     │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                  GO TO WORKSHOP                              │
│  ⚠️ ENEMIES START SPAWNING! ⚠️                              │
│  Navigate to workshop while avoiding/fighting bots          │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                  GRAB NAILGUN                                │
│  Press E to pick up nailgun from table                      │
│  "Bots are going rogue! Defend yourself!"                   │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│              GET REPLACEMENT POWER CELL                      │
│  Press E to pick up power cell                              │
│  Objective: Return to Power Grid Chamber                    │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│              FIGHT BACK TO POWER BAY                         │
│  Shoot enemies with nailgun (Left Click)                    │
│  Navigate back to Power Grid Chamber                        │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                INSERT POWER CELL                             │
│  Press F at Power Bay to insert cell                        │
│  ✨ LIGHTS TURN ON! ✨                                      │
│  🛑 ENEMIES STOP SPAWNING! 🛑                               │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│              RETURN TO MAIN CONSOLE                          │
│  Navigate to Assembly Line Corridor                         │
│  Press F to activate console                                │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                   VICTORY!                                   │
│              "FACTORY SECURED!"                              │
│         [Restart] [Main Menu] [Quit]                        │
└─────────────────────────────────────────────────────────────┘
```

---

## 🎯 Key Locations

| Location | Purpose | Marker Color |
|----------|---------|--------------|
| **Main Console** | Start/End point (Assembly Line Corridor) | Green |
| **Power Bay** | Insert power cell (Power Grid Chamber) | Blue |
| **Workshop** | Get nailgun + power cell | Orange |

---

## 🎮 Controls

| Action | Key | When |
|--------|-----|------|
| **Inspect** | E | Near console/power bay |
| **Pick Up** | E | Near objects |
| **Interact** | F | Near power bay/console |
| **Shoot** | Left Click | When holding nailgun |
| **Move** | WASD | Always |
| **Sprint** | Shift | While moving |
| **Jump** | Space | Always |
| **Crouch** | Ctrl | Always |

---

## 🤖 Enemy Behavior

**Before Power Restored:**
- Spawn every 5 seconds
- Max 5 enemies alive at once
- Patrol waypoints
- Chase player when spotted (15m range)
- Don't shoot (just chase)

**After Power Restored:**
- Stop spawning immediately
- Existing enemies remain until killed

**Combat:**
- Nailgun damage: 10 per shot
- Enemy health: 100
- 10 shots to kill one enemy
- Enemies drop items when killed (optional)

---

## 📊 Objective Progression

| Step | Objective Text | Direction Indicator |
|------|----------------|---------------------|
| 1 | Complete the tutorial | - |
| 2 | The lights went out! What happened? | - |
| 3 | Check the main console | → Console (distance) |
| 4 | Check the power bay | → Power Bay (distance) |
| 5 | Get replacement from workshop | → Workshop (distance) |
| 6 | Grab the nailgun - bots going rogue! | → Workshop (distance) |
| 7 | Get the replacement power cell | → Workshop (distance) |
| 8 | Return to the power bay | → Power Bay (distance) |
| 9 | Insert the power cell | → Power Bay (distance) |
| 10 | Return to the main console | → Console (distance) |
| 11 | Activate the console | → Console (distance) |
| 12 | FACTORY SECURED! | - |

---

## 🎨 Visual Feedback

**Lights Off:**
- All 27 factory lights turn off
- Dark, ominous atmosphere
- Emergency lighting only

**Sparks at Power Bay:**
- Particle system active
- Orange/yellow sparks
- Electrical sound

**Enemy Spawn:**
- Spawn effect (optional)
- Spawn sound (optional)
- Red glowing eyes

**Power Restored:**
- All lights turn on sequentially
- Sparks stop
- Triumphant sound

**Console Activation:**
- Screen boots up
- Progress bar
- Success sound

---

## ⏱️ Estimated Playtime

- **Tutorial**: 1-2 minutes
- **Investigation**: 1 minute
- **Combat + Workshop**: 2-3 minutes
- **Return + Restore**: 1-2 minutes
- **Console Activation**: 30 seconds

**Total**: 5-8 minutes (perfect length!)

---

## 🎯 Success Metrics

Game is successful when:
- ✅ Player never asks "what do I do?"
- ✅ Objective always shows clear direction
- ✅ Combat feels satisfying
- ✅ Tension builds (enemies spawning)
- ✅ Relief when power restored (enemies stop)
- ✅ Victory feels earned
- ✅ Can be completed in one try
- ✅ Replayable and fun

---

## 🔧 Difficulty Tuning

**Too Easy?**
- Increase enemy speed
- Increase spawn rate
- Decrease nailgun damage
- Add enemy shooting

**Too Hard?**
- Decrease enemy speed
- Decrease spawn rate
- Increase nailgun damage
- Add health pickups
- Add ammo pickups

**Just Right:**
- Player feels challenged but not frustrated
- Uses most of nailgun ammo
- Takes some damage but doesn't die
- Feels relief when power restored

---

This is going to be EPIC! 🚀
