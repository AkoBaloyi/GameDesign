# Gameplay Flow Quick Reference

## 🎮 Complete Player Journey

```
START
  ↓
[TUTORIAL] - Learn Controls
  ↓
[OBJECTIVE BANNER] - "OBJECTIVE: Restore Power to the Factory"
  ↓
[TRACKER] - "Power Cells: 0/1"
  ↓
[FIND POWER CELL] - Orange glowing cube (rotating, bobbing)
  ↓
[PRESS E] - Pick up Power Cell
  ↓
[TRACKER UPDATE] - "Power Cells: 1/1 - Find Power Bay"
  ↓
[FIND POWER BAY] - Blue marker on minimap
  ↓
[WALK TO POWER BAY] - Prompt appears: "Press F to Insert Power Cell"
  ↓
[PRESS F] - Insert Power Cell
  ↓
[BANNER] - "Power Cell Inserted!"
  ↓
[LIGHTS ACTIVATE] - Sequential light activation across factory
  ↓
[TRACKER UPDATE] - "Power Cell Inserted ✓"
  ↓
[GLOWING PATH APPEARS] - Floor tiles light up showing path
  ↓
[TRACKER UPDATE] - "Follow the path"
  ↓
[FOLLOW PATH] - Walk along glowing floor
  ↓
[REACH CONSOLE] - Console light turns yellow (ready)
  ↓
[PROMPT APPEARS] - "Press F to Activate Console"
  ↓
[PRESS F] - Activate Console
  ↓
[CONSOLE ACTIVATES] - Light turns green, activation sequence plays
  ↓
[BANNER] - "FACTORY POWER RESTORED!"
  ↓
[WIN SCREEN] - "MISSION COMPLETE"
  ↓
[BUTTONS] - Restart or Quit
  ↓
END
```

---

## 🎯 Key Interactions

| Action | Key | What Happens |
|--------|-----|--------------|
| **Pick up Power Cell** | E | Cell attaches to hand, tracker updates |
| **Insert Power Cell** | F | Cell locks into bay, lights activate |
| **Activate Console** | F | Console powers on, win screen shows |

---

## 📊 Objective States

| Step | Objective Text | Tracker Text |
|------|----------------|--------------|
| 1 | "Learn the basic controls" | "Tutorial in progress..." |
| 2 | "OBJECTIVE: Restore Power to the Factory" | "Power Cells: 0/1" |
| 3 | "Insert the Power Cell into the Power Bay" | "Power Cells: 1/1 - Find Power Bay" |
| 4 | "Activating lights..." | "Power Cell Inserted ✓" |
| 5 | "Follow the glowing path to the console" | "Follow the path" |
| 6 | "Activate the console" | "Follow the path" |
| 7 | "Factory Power Restored!" | "Mission Complete!" |

---

## 🔗 Script Dependencies

```
ObjectiveManager (Central Hub)
  ├── PowerCell → OnPowerCellPicked()
  ├── PowerBay → OnPowerCellInserted()
  ├── LightsController → OnLightsActivated()
  ├── FactoryConsole → OnConsoleActivatedComplete()
  └── WinStateManager → ShowWinScreen()

LightsController
  ├── Activates Lights
  ├── Activates Glowing Path
  └── Enables FactoryConsole

PowerBay
  ├── Checks FPController.GetHeldObject()
  └── Calls FPController.DropHeldObject()

Player Input (F key)
  ├── PowerBay.OnInteract()
  └── FactoryConsole.OnInteract()
```

---

## 🎨 Visual Feedback Timeline

```
0:00 - Tutorial complete
0:01 - Objective banner fades in
0:04 - Banner fades out
     - Player finds power cell
     - Power cell rotates and bobs
     - Highlights when looked at
     - Player picks up with E
     - Player walks to Power Bay
     - Prompt appears: "Press F"
     - Player presses F
0:05 - Power cell snaps to socket
0:06 - Insert sound plays
0:07 - Banner: "Power Cell Inserted!"
0:08 - Lights start activating (0.2s delay each)
0:12 - All lights on
0:13 - Glowing path starts (0.3s delay each segment)
0:18 - Path complete, console enabled
     - Console light turns yellow
     - Player follows path
     - Player reaches console
     - Prompt: "Press F to Activate"
     - Player presses F
0:19 - Console activation sound
0:21 - Console fully activated
0:22 - Win banner appears
0:24 - Win screen fades in
0:25 - Win music plays
     - Player can restart or quit
```

---

## 🎯 Success Metrics

A successful implementation should have:
- ✅ Clear objective at all times
- ✅ Visual feedback for every action
- ✅ Audio feedback for key moments
- ✅ Smooth progression (no confusion)
- ✅ Satisfying win state
- ✅ ~2-3 minute gameplay loop
- ✅ No dead ends or soft locks

---

## 🔧 Customization Points

Easy things to adjust:
- **Banner duration**: `ObjectiveManager.bannerDisplayDuration`
- **Light activation speed**: `LightsController.lightActivationDelay`
- **Path activation speed**: `LightsController.pathActivationDelay`
- **Console activation time**: `FactoryConsole.activationDuration`
- **Win screen delay**: `WinStateManager.delayBeforeShow`
- **Detection ranges**: All scripts have `detectionRange` field

---

## 💡 Extension Ideas

Want to make it more complex?
1. **Multiple Power Cells**: Change tracker to "Power Cells: 0/3"
2. **Enemies**: Add robots that patrol and chase player
3. **Puzzles**: Add button sequences or codes
4. **Timer**: Add countdown for urgency
5. **Collectibles**: Add optional items for bonus
6. **Multiple Endings**: Different outcomes based on time/collectibles
7. **Dialogue**: Add NPC or AI voice giving instructions
8. **Hazards**: Add environmental dangers to avoid

---

This is your complete gameplay loop! 🚀
