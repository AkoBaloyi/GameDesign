# Quick Setup Checklist

Use this as a quick reference while setting up in Unity Inspector.

---

## ✅ Power Bay - 3 Critical Fields

1. **objectiveManager** 
   - Drag: ObjectiveManager GameObject from Hierarchy
   
2. **socketPoint**
   - Create: Right-click Power Bay → Create Empty → Name "SocketPoint"
   - Position: Inside/on the power bay where cell should snap
   - Drag: The SocketPoint child object to this field
   
3. **playerLayer**
   - Click dropdown → Select "Player"
   - If "Player" doesn't exist: Edit → Project Settings → Tags and Layers → Add "Player"

**Also set:**
- detectionRange = 5 (or higher)

---

## ✅ Factory Console - 2 Critical Fields

1. **objectiveManager**
   - Drag: ObjectiveManager GameObject from Hierarchy
   
2. **playerLayer**
   - Click dropdown → Select "Player"

**Also set:**
- detectionRange = 5 (or higher)

---

## ✅ Power Cell - 1 Critical Field

1. **objectiveManager**
   - Drag: ObjectiveManager GameObject from Hierarchy

---

## ✅ Player - 3 Critical Fields

1. **Layer** (top of Inspector)
   - Set to: "Player"
   
2. **holdPoint** (in FPController)
   - Create: Right-click Player → Create Empty → Name "HoldPoint"
   - Position: In front of camera (Z: 1.5, Y: -0.5)
   - Drag: HoldPoint to this field
   
3. **pickupRange** (in FPController)
   - Set to: 5

---

## ✅ Lights Controller - 4 Critical Fields

1. **lights[]**
   - Click + for each light in scene
   - Drag: All Light components into array
   
2. **pathSegments[]**
   - Click + for each path tile
   - Drag: All glowing path floor tiles into array
   
3. **objectiveManager**
   - Drag: ObjectiveManager GameObject
   
4. **factoryConsole**
   - Drag: FactoryConsole GameObject

---

## ✅ ObjectiveManager - UnityEvents

These are the most important connections!

1. **onPowerCellInserted**
   - Click + button
   - Drag: LightsController GameObject
   - Function: LightsController → ActivateLights()
   
2. **onStartGlowingPath**
   - Click + button
   - Drag: LightsController GameObject
   - Function: LightsController → ActivateGlowingPath()
   
3. **onWin**
   - Click + button
   - Drag: WinStateManager GameObject
   - Function: WinStateManager → ShowWinScreen()

---

## 🎯 Priority Order

Do these in order:

1. **Set Player Layer to "Player"** (30 seconds)
2. **Create HoldPoint for Player** (1 minute)
3. **Create SocketPoint for Power Bay** (1 minute)
4. **Assign ObjectiveManager to Power Cell, Power Bay, Console** (2 minutes)
5. **Set playerLayer on Power Bay and Console** (30 seconds)
6. **Set detection ranges to 5** (30 seconds)
7. **Connect ObjectiveManager UnityEvents** (3 minutes)
8. **Fill Lights Controller arrays** (5 minutes)

**Total time: ~15 minutes**

---

## 🧪 Test After Each Step

After setting up Power Bay:
1. Press Play
2. Pick up Power Cell (E)
3. Walk to Power Bay
4. Press F
5. Should insert!

After setting up Console:
1. Complete power cell insertion
2. Wait for lights
3. Follow path to console
4. Press F
5. Should activate!

---

## 🆘 Still Not Working?

### Power Cell won't pick up
- Check Player.pickupRange = 5
- Check Power Cell has PickUpObject script
- Check Power Cell has Collider

### F key doesn't work at Power Bay
- Check Power Bay.detectionRange = 5
- Check Power Bay.playerLayer = "Player"
- Check Player Layer = "Player"
- Check Power Bay.objectiveManager is assigned
- Check Power Bay.socketPoint is assigned

### F key doesn't work at Console
- Check Console.detectionRange = 5
- Check Console.playerLayer = "Player"
- Check Console.objectiveManager is assigned
- Check lights activated first (console won't work until lights are on)

### Lights don't activate
- Check ObjectiveManager.onPowerCellInserted → LightsController.ActivateLights()
- Check LightsController.lights[] array is filled
- Check LightsController.objectiveManager is assigned

### Path doesn't appear
- Check ObjectiveManager.onStartGlowingPath → LightsController.ActivateGlowingPath()
- Check LightsController.pathSegments[] array is filled

### Console doesn't enable
- Check LightsController.factoryConsole is assigned
- Wait for lights to fully activate first

---

## 📊 Diagnostic Tool

Press **F12** in Play mode to see:
- Which components are found ✓
- Which components are missing ✗
- Current game state
- Distances to objectives
- Setup verification

Use this to quickly identify what's not set up correctly!

---

## 💡 Pro Tips

1. **Save often** - Ctrl+S after each setup step
2. **Test incrementally** - Don't set up everything then test
3. **Use Console** - Watch for debug messages (Ctrl+Shift+C)
4. **Use Diagnostic** - F12 is your friend
5. **Check distances** - If F key doesn't work, you might be too far away

---

Good luck! Follow UNITY_INSPECTOR_SETUP.md for detailed instructions on each field.
