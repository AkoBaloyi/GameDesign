# 🤖 Combat System - Simple Step-by-Step Guide

## Overview
Add enemies that chase you and can be killed with the nailgun.

**Time needed**: 4-6 hours
**Difficulty**: Medium

---

## PART 1: Create Enemy Bot (1-2 hours)

### Step 1: Create Enemy Model

**Option A: Quick Primitive Bot (15 min)**
1. Create Empty GameObject: "EnemyBot"
2. Add child Cube: "Body" (scale 0.5, 1, 0.5)
3. Add child Sphere: "Head" (scale 0.4, position Y: 0.7)
4. Add child Cubes: "Eyes" (small, red, emissive)
5. Group and save as Prefab

**Option B: Blender Model (1-2 hours)**
1. Create simple robot in Blender
2. Export as FBX
3. Import to Unity
4. Save as Prefab

**My recommendation**: Start with Option A, upgrade later if time!

---

### Step 2: Add Components to Enemy Prefab

**Select EnemyBot prefab**, add these components:

1. **Capsule Collider**:
   - Radius: 0.3
   - Height: 1.8

2. **Rigidbody**:
   - Mass: 50
   - Drag: 1
   - Freeze Rotation: X, Y, Z (all checked)

3. **Nav Mesh Agent**:
   - Speed: 3.5
   - Angular Speed: 120
   - Acceleration: 8
   - Stopping Distance: 2
   - Radius: 0.3
   - Height: 1.8

4. **SimpleEnemyAI script**:
   - detectionRange: 15
   - chaseRange: 25
   - patrolSpeed: 2
   - chaseSpeed: 4

5. **EnemyHealth script**:
   - maxHealth: 100
   - (Other fields optional)

**Save prefab!**

---

### Step 3: Bake NavMesh (5 min)

**CRITICAL**: Enemies need NavMesh to move!

1. **Window → AI → Navigation**
2. **Bake tab**
3. **Settings**:
   - Agent Radius: 0.3
   - Agent Height: 1.8
   - Max Slope: 45
   - Step Height: 0.4
4. **Click "Bake"** button
5. **Wait** for blue overlay to appear on floors

**Result**: Enemies can now navigate your factory!

---

## PART 2: Set Up Spawning (30 min)

### Step 1: Create Spawn Points (15 min)

1. **Create empty GameObject**: "EnemySpawnPoints"
2. **Add children** (Right-click → Create Empty):
   - "SpawnPoint_1"
   - "SpawnPoint_2"
   - "SpawnPoint_3"
   - etc. (5-10 total)
3. **Position them** around your factory:
   - Near workshop
   - Between workshop and power bay
   - Between power bay and console
   - **Make sure they're on NavMesh** (blue areas)

---

### Step 2: Create Enemy Spawner (10 min)

1. **Create empty GameObject**: "EnemySpawner"
2. **Add EnemySpawner script**
3. **In Inspector**:
   ```
   enemyPrefab: [Drag EnemyBot prefab]
   spawnPoints[]: 
     - Size: 5-10
     - Drag each SpawnPoint GameObject
   spawnInterval: 5
   maxEnemiesAlive: 5
   spawnEnabled: ☐ Unchecked (starts disabled)
   spawnOnStart: ☐ Unchecked
   stopWhenPowerRestored: ☑ Checked
   ```

---

### Step 3: Create Patrol Waypoints (5 min)

**For each enemy spawn point**:
1. Create 2-3 child empty GameObjects
2. Name them "Waypoint_1", "Waypoint_2", etc.
3. Position them in a patrol path
4. These will be assigned to enemies when they spawn

**Or skip this**: Enemies will just stand at spawn point until they see player

---

## PART 3: Set Up Enhanced Objectives (30 min)

### Step 1: Create EnhancedObjectiveManager (5 min)

1. **Create empty GameObject**: "EnhancedObjectiveManager"
2. **Add EnhancedObjectiveManager script**

---

### Step 2: Set Up Objective UI (10 min)

1. **Create Canvas** (if not exists): Screen Space - Overlay
2. **Add Panel** at top of screen
3. **Add TextMeshProUGUI**: "ObjectiveText"
   - Font size: 24
   - Color: Cyan #00FFFF
   - Text: "Objective will appear here"
4. **Add TextMeshProUGUI**: "DirectionText"
   - Font size: 36
   - Color: Yellow #FFFF00
   - Text: "→ 0m"

---

### Step 3: Assign References (15 min)

**Select EnhancedObjectiveManager**, assign:

**UI**:
- objectiveText → ObjectiveText
- directionText → DirectionText
- objectivePanel → Panel

**Objective Targets**:
- consoleTransform → FactoryConsole GameObject
- powerBayTransform → PowerBay GameObject
- workshopTransform → Create empty "WorkshopMarker" at workshop, drag it here

**References**:
- factoryLights[] → Drag all 27 lights
- enemySpawner → EnemySpawner GameObject
- nailgun → NailgunWeapon GameObject (if you have one)

---

## PART 4: Set Up Inspection (30 min)

### Step 1: Console Inspection (15 min)

1. **Select FactoryConsole**
2. **Add InspectableObject script**
3. **Settings**:
   ```
   inspectionMessage: "Press E to inspect console"
   inspectionResult: "Console is offline. Check the power bay."
   inspectionRange: 3
   canInspect: ☑
   inspectOnce: ☑
   ```

4. **Create World Space Canvas** above console:
   - Right-click Console → UI → Canvas
   - Canvas: World Space
   - Scale: (0.01, 0.01, 0.01)
   - Position: Above console
   - Add TextMeshProUGUI: "Press E to Inspect"

5. **Assign UI**:
   - promptUI → Canvas
   - promptText → TextMeshProUGUI

6. **Connect Event**:
   - onInspected → Click +
   - Drag EnhancedObjectiveManager
   - Function: OnConsoleInspected()

---

### Step 2: Power Bay Inspection (15 min)

**Same as console**:
1. Select PowerBay
2. Add InspectableObject script
3. inspectionResult: "Sparks! Power cell damaged. Get replacement from workshop."
4. Create World Space Canvas
5. Connect onInspected → EnhancedObjectiveManager.OnPowerBayInspected()

---

## PART 5: Connect Everything (30 min)

### Tutorial → Enhanced Manager:
1. Find where tutorial completes
2. Call: `enhancedObjectiveManager.OnTutorialComplete()`

### Nailgun Pickup:
1. When nailgun picked up
2. Call: `enhancedObjectiveManager.OnNailgunPickedUp()`

### Power Cell Pickup (Workshop):
1. Workshop power cell
2. Call: `enhancedObjectiveManager.OnPowerCellPickedUp()`

### Power Bay Insert:
1. PowerBay.InsertPowerCell()
2. Call: `enhancedObjectiveManager.OnPowerCellInserted()`

### Console Activate:
1. FactoryConsole.ActivateConsole()
2. Call: `enhancedObjectiveManager.OnConsoleActivated()`

---

## PART 6: Test (1 hour)

### Test Checklist:
- [ ] Tutorial completes → Lights turn off
- [ ] Objective says "Check console"
- [ ] Can inspect console with E
- [ ] Objective changes to "Check power bay"
- [ ] Can inspect power bay with E
- [ ] Objective changes to "Go to workshop"
- [ ] Enemies start spawning
- [ ] Can shoot enemies with nailgun
- [ ] Enemies die when shot
- [ ] Can pick up power cell
- [ ] Can insert power cell
- [ ] Enemies stop spawning
- [ ] Can activate console
- [ ] Win!

---

## Quick Reference

### Order of Setup:
1. Create enemy bot prefab
2. Bake NavMesh
3. Create spawn points
4. Create spawner
5. Create EnhancedObjectiveManager
6. Set up UI
7. Add inspection to console and power bay
8. Connect all triggers
9. Test!

---

## Time Breakdown:
- Enemy bot: 1-2 hours
- NavMesh: 5 min
- Spawn points: 15 min
- Spawner: 10 min
- Enhanced manager: 5 min
- UI: 10 min
- Inspection: 30 min
- Connections: 30 min
- Testing: 1 hour

**Total: 4-5 hours**

---

**This is the simplified version! Follow step-by-step and you'll have combat working!** 🤖🔫
