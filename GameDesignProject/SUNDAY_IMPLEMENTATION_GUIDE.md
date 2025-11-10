# Sunday Implementation Guide - SIMPLIFIED

## 🎯 What's Ready

5 new scripts are already created and working:
1. ✅ **SimpleEnemyAI.cs** - Enemy patrol and chase
2. ✅ **EnemyHealth.cs** - Enemy takes damage
3. ✅ **EnemySpawner.cs** - Spawns enemies
4. ✅ **InspectableObject.cs** - Inspect objects with E key
5. ✅ **EnhancedObjectiveManager.cs** - New game flow

---

## 📋 DO THESE IN ORDER (Don't Skip Around!)

---

## STEP 1: Create Enemy Bot (30 min)

### Quick Version - Use Unity Primitives

1. **Create Empty GameObject** in scene: "EnemyBot"
2. **Add child Cube**: "Body"
   - Scale: (0.5, 1, 0.5)
   - Position: (0, 0, 0)
3. **Add child Sphere**: "Head"
   - Scale: (0.4, 0.4, 0.4)
   - Position: (0, 0.7, 0)
4. **Add child Cubes**: "Eye_Left" and "Eye_Right"
   - Scale: (0.1, 0.1, 0.1)
   - Position: (-0.1, 0.75, 0.15) and (0.1, 0.75, 0.15)
   - Material: Create new material, set Emission to red, intensity 2

5. **Select EnemyBot parent**, add these components:
   - **Capsule Collider**: Radius 0.3, Height 1.8
   - **Rigidbody**: Mass 50, Freeze Rotation X/Y/Z all checked
   - **Nav Mesh Agent**: Speed 3.5, Stopping Distance 2, Radius 0.3, Height 1.8
   - **SimpleEnemyAI script**: Detection Range 15, Chase Speed 4
   - **EnemyHealth script**: Max Health 100

6. **Drag EnemyBot to Project folder** to create prefab
7. **Delete EnemyBot from scene** (we'll spawn them later)

---

## STEP 2: Bake NavMesh (5 min)

**CRITICAL - Enemies can't move without this!**

1. **Window → AI → Navigation**
2. **Bake tab**
3. **Agent Radius**: 0.3
4. **Agent Height**: 1.8
5. **Click "Bake" button**
6. **Wait** - you'll see blue overlay on walkable floors
7. **Done!**

---

## STEP 3: Create Spawn Points (15 min)

1. **Create Empty GameObject** in scene: "EnemySpawnPoints"
2. **Right-click EnemySpawnPoints → Create Empty** (do this 5-10 times)
3. **Name them**: "SpawnPoint_1", "SpawnPoint_2", etc.
4. **Position them** around your factory:
   - Near workshop area
   - Between workshop and power bay
   - Between power bay and console
   - **Make sure they're on blue NavMesh areas!**

---

## STEP 4: Create Enemy Spawner (10 min)

1. **Create Empty GameObject** in scene: "EnemySpawner"
2. **Add Component**: EnemySpawner script
3. **In Inspector, assign:**
   - **Enemy Prefab**: Drag your EnemyBot prefab from Project
   - **Spawn Points**: Click + to add 5-10 slots, drag each SpawnPoint GameObject
   - **Spawn Interval**: 5
   - **Max Enemies Alive**: 5
   - **Spawn Enabled**: UNCHECK (starts disabled)
   - **Spawn On Start**: UNCHECK
   - **Stop When Power Restored**: CHECK

---

## STEP 5: Create Workshop Marker (2 min)

1. **Find your workshop area** (or any room with nailgun/power cell)
2. **Create Empty GameObject** at center of workshop: "WorkshopMarker"
3. **Position it** where you want the objective arrow to point
4. **Done!** (We'll use this later)

---

## STEP 6: Create EnhancedObjectiveManager (5 min)

**Do this BEFORE setting up inspection!**

1. **Create Empty GameObject** in scene: "EnhancedObjectiveManager"
2. **Add Component**: EnhancedObjectiveManager script
3. **Leave everything blank for now** - we'll assign references later
4. **Done!**

---

## STEP 7: Create Objective UI (10 min)

1. **Find your Canvas** (or create one: GameObject → UI → Canvas)
2. **Right-click Canvas → UI → Panel**: Name it "ObjectivePanel"
   - Position: Top of screen
   - Size: 400 width, 100 height
   - Color: Black with 50% transparency

3. **Right-click ObjectivePanel → UI → Text - TextMeshPro**: Name it "ObjectiveText"
   - Text: "Objective will appear here"
   - Font Size: 20
   - Color: Cyan #00FFFF
   - Alignment: Center

4. **Right-click ObjectivePanel → UI → Text - TextMeshPro**: Name it "DirectionText"
   - Text: "→ 0m"
   - Font Size: 24
   - Color: Yellow #FFFF00
   - Alignment: Center Right

---

## STEP 8: Assign EnhancedObjectiveManager References (10 min)

**Select EnhancedObjectiveManager**, now assign everything:

**UI Section:**
- **Objective Text**: Drag ObjectiveText
- **Direction Text**: Drag DirectionText
- **Objective Panel**: Drag ObjectivePanel

**Objective Targets:**
- **Console Transform**: Drag FactoryConsole GameObject
- **Power Bay Transform**: Drag PowerBay GameObject
- **Workshop Transform**: Drag WorkshopMarker GameObject

**References:**
- **Factory Lights**: Click + to add 27 slots, drag all your factory lights
- **Enemy Spawner**: Drag EnemySpawner GameObject
- **Nailgun**: Drag your nailgun GameObject (if you have one)

---

## STEP 9: Add Inspection to Console (15 min)

1. **Select FactoryConsole** GameObject
2. **Add Component**: InspectableObject script
3. **In Inspector, set:**
   - **Inspection Message**: "Press E to inspect console"
   - **Inspection Result**: "Console is offline. Check the power bay for issues."
   - **Inspection Range**: 3
   - **Can Inspect**: CHECK
   - **Inspect Once**: CHECK

4. **Create prompt UI:**
   - Right-click FactoryConsole → UI → Canvas
   - Name it "ConsolePromptCanvas"
   - Canvas component: Render Mode = World Space
   - Position: (0, 2, 0) - above console
   - Scale: (0.01, 0.01, 0.01)

5. **Add text:**
   - Right-click ConsolePromptCanvas → UI → Text - TextMeshPro
   - Name it "PromptText"
   - Text: "Press E to Inspect"
   - Font Size: 24
   - Color: White
   - Alignment: Center

6. **Back to InspectableObject component:**
   - **Prompt UI**: Drag ConsolePromptCanvas
   - **Prompt Text**: Drag PromptText

7. **Connect event:**
   - Scroll to **On Inspected** event
   - Click **+** button
   - Drag **EnhancedObjectiveManager** GameObject into the slot
   - Dropdown: Select **EnhancedObjectiveManager → OnConsoleInspected()**

---

## STEP 10: Add Inspection to Power Bay (15 min)

**Same as console, but different text:**

1. **Select PowerBay** GameObject
2. **Add Component**: InspectableObject script
3. **In Inspector, set:**
   - **Inspection Message**: "Press E to inspect power bay"
   - **Inspection Result**: "Sparks everywhere! The power cell is damaged. Get a replacement from the workshop."
   - **Inspection Range**: 3
   - **Can Inspect**: CHECK
   - **Inspect Once**: CHECK

4. **Create prompt UI** (same as console):
   - Right-click PowerBay → UI → Canvas: "PowerBayPromptCanvas"
   - World Space, Position (0, 2, 0), Scale (0.01, 0.01, 0.01)
   - Add Text - TextMeshPro: "Press E to Inspect"

5. **Assign to InspectableObject:**
   - **Prompt UI**: Drag PowerBayPromptCanvas
   - **Prompt Text**: Drag PromptText

6. **Connect event:**
   - **On Inspected** → Click +
   - Drag **EnhancedObjectiveManager**
   - Select: **EnhancedObjectiveManager → OnPowerBayInspected()**

---

## STEP 11: Connect Tutorial Complete (5 min)

**Find where your tutorial ends** (TutorialManager or similar)

**Option A - If you have TutorialManager:**
1. Select TutorialManager GameObject
2. Find the UnityEvent that fires when tutorial completes
3. Click + button
4. Drag EnhancedObjectiveManager
5. Select: EnhancedObjectiveManager → OnTutorialComplete()

**Option B - If no tutorial:**
1. Select EnhancedObjectiveManager
2. In Inspector, check "Start Immediately" (if that option exists)
3. Or call it from your game start script

---

## STEP 12: Connect Nailgun Pickup (5 min)

**If you have a nailgun:**
1. Find your nailgun GameObject
2. Find the script that handles pickup (PickUpObject or similar)
3. In the pickup event/method, add:
   - Click + on UnityEvent
   - Drag EnhancedObjectiveManager
   - Select: EnhancedObjectiveManager → OnNailgunPickedUp()

**If no nailgun yet:**
- Skip this for now, add later

---

## STEP 13: Connect Power Cell Pickup (5 min)

1. **Find your workshop power cell** GameObject
2. **Select it**, find PowerCell script
3. **Find OnPickedUp event** (or similar)
4. **Click +**
5. **Drag EnhancedObjectiveManager**
6. **Select**: EnhancedObjectiveManager → OnPowerCellPickedUp()

---

## STEP 14: Connect Power Bay Insert (5 min)

1. **Open PowerBay.cs** script in code editor
2. **Find InsertPowerCell() method**
3. **Add this line** after the power cell is inserted:
```csharp
// Notify enhanced objective manager
EnhancedObjectiveManager enhancedManager = FindObjectOfType<EnhancedObjectiveManager>();
if (enhancedManager != null)
{
    enhancedManager.OnPowerCellInserted();
}
```
4. **Save script**

---

## STEP 15: Connect Console Activation (5 min)

1. **Open FactoryConsole.cs** script
2. **Find ActivateConsole() method**
3. **Add this line** after console activates:
```csharp
// Notify enhanced objective manager
EnhancedObjectiveManager enhancedManager = FindObjectOfType<EnhancedObjectiveManager>();
if (enhancedManager != null)
{
    enhancedManager.OnConsoleActivated();
}
```
4. **Save script**

---

## STEP 16: TEST EVERYTHING! (30 min)

**Press Play and test each step:**

1. [ ] Tutorial completes (or game starts)
2. [ ] Lights turn off
3. [ ] Objective says "Check console"
4. [ ] Walk to console, see "Press E to Inspect"
5. [ ] Press E, see inspection message
6. [ ] Objective changes to "Check power bay"
7. [ ] Walk to power bay, press E to inspect
8. [ ] Objective changes to "Go to workshop"
9. [ ] **Enemies start spawning!**
10. [ ] Walk to workshop
11. [ ] Pick up nailgun (if you have one)
12. [ ] Pick up power cell
13. [ ] Objective says "Return to power bay"
14. [ ] **Shoot enemies with nailgun**
15. [ ] **Enemies die when shot**
16. [ ] Walk back to power bay
17. [ ] Press F to insert power cell
18. [ ] **Lights turn on!**
19. [ ] **Enemies stop spawning!**
20. [ ] Objective says "Go to console"
21. [ ] Walk to console, press F to activate
22. [ ] **Win screen appears!**

---

## 🐛 If Something Doesn't Work:

**Enemies don't spawn:**
- Check NavMesh is baked (blue overlay on floors)
- Check EnemySpawner has enemyPrefab assigned
- Check spawn points are assigned
- Check "spawnEnabled" gets set to true

**Inspection doesn't work:**
- Check InspectableObject script is on console/power bay
- Check "canInspect" is checked
- Check player is within range (increase to 5 if needed)
- Check prompt UI is assigned

**Objectives don't update:**
- Check EnhancedObjectiveManager has all references assigned
- Check UnityEvents are connected
- Check Console for error messages
- Check each trigger method is being called (add Debug.Log)

**Enemies don't move:**
- NavMesh not baked! Go to Window → AI → Navigation → Bake
- Enemy not on NavMesh (move spawn points to blue areas)
- NavMeshAgent not added to enemy prefab

---

## 🎨 Polish (If You Have Time)

**Only if you have extra time:**

- Add muzzle flash to nailgun
- Add hit sparks when shooting enemies
- Add death explosion for enemies
- Add enemy footstep sounds
- Add dramatic music when enemies spawn
- Make sparks on power bay more intense
- Add health pickups
- Add ammo pickups

**But get the core working first!**

---

## 📊 Time Estimate:

- Step 1 (Enemy Bot): 30 min
- Step 2 (NavMesh): 5 min
- Step 3 (Spawn Points): 15 min
- Step 4 (Spawner): 10 min
- Step 5 (Workshop Marker): 2 min
- Step 6 (Enhanced Manager): 5 min
- Step 7 (UI): 10 min
- Step 8 (Assign References): 10 min
- Step 9 (Console Inspection): 15 min
- Step 10 (Power Bay Inspection): 15 min
- Step 11-15 (Connections): 25 min
- Step 16 (Testing): 30 min

**Total: ~3 hours**

---

## 🎯 Summary:

**What you're building:**
1. Enemies that spawn and chase you
2. Inspection system (press E to inspect console/power bay)
3. New objective flow with directions
4. Combat with nailgun
5. Enemies stop spawning when power restored

**The order:**
1. Create enemy → Bake NavMesh → Spawn points → Spawner
2. Create EnhancedObjectiveManager → Create UI → Assign references
3. Add inspection to console and power bay
4. Connect all the triggers
5. Test!

**Follow the steps in order, don't skip around, and you'll be done in 3 hours!** 🚀
