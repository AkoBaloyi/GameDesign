# Sunday Implementation Guide - New Narrative

## 🎯 What's Ready

I've created 5 new scripts for you:
1. ✅ **SimpleEnemyAI.cs** - Patrol and chase behavior
2. ✅ **EnemyHealth.cs** - Takes damage from nailgun
3. ✅ **EnemySpawner.cs** - Spawns enemies continuously
4. ✅ **InspectableObject.cs** - Console/Power Bay inspection
5. ✅ **EnhancedObjectiveManager.cs** - New narrative flow

All scripts compile with no errors!

---

## 📋 Your Tasks for Sunday Morning (4 hours)

### Task 1: Create Enemy Bot Model (1-2 hours)

**In Blender:**
1. Create simple robot/bot model
   - Doesn't need to be fancy
   - Box body + sphere head works
   - Add some details (antenna, lights, etc.)
2. Export as FBX
3. Import to Unity

**In Unity:**
1. Drag FBX into scene
2. Add components:
   - NavMeshAgent
   - SimpleEnemyAI script
   - EnemyHealth script
   - Capsule Collider
3. Set up materials (metallic, glowing eyes)
4. Save as Prefab: "EnemyBot"

**Quick Alternative if short on time:**
- Use Unity primitives (cubes + sphere)
- Make it look robotic with materials
- Add glowing red eyes (emissive material)

---

### Task 2: Set Up Enemy Spawn Points (30 min)

1. **Create spawn point containers:**
   - Create empty GameObject: "SpawnPoints_Workshop"
   - Create empty GameObject: "SpawnPoints_PowerBay"
   - Create empty GameObject: "SpawnPoints_Console"

2. **Add spawn points:**
   - Right-click each container → Create Empty
   - Name them "SpawnPoint_1", "SpawnPoint_2", etc.
   - Position them along paths between locations
   - 3-5 spawn points per area

3. **Visual markers (optional):**
   - Add small cube to each spawn point
   - Make it red and semi-transparent
   - Disable before building

---

### Task 3: Set Up Enemy Spawner (15 min)

1. **Create spawner GameObject:**
   - Create empty: "EnemySpawner"
   - Add EnemySpawner script

2. **Assign in Inspector:**
   - `enemyPrefab` → Your EnemyBot prefab
   - `spawnPoints[]` → Drag all spawn point GameObjects
   - `spawnInterval` → 5 (seconds between spawns)
   - `maxEnemiesAlive` → 5
   - `spawnEnabled` → Unchecked (starts disabled)
   - `spawnOnStart` → Unchecked
   - `stopWhenPowerRestored` → Checked

---

### Task 4: Set Up Workshop Location (30 min)

1. **Find/create workshop room**
   - Use existing room or create new area
   - Add some props (tables, shelves, crates)

2. **Place nailgun:**
   - Put nailgun on table
   - Make it glow (emissive material + point light)
   - Add PickUpObject script if not already there

3. **Place replacement power cell:**
   - Put power cell in workshop
   - Make it glow orange
   - Different from tutorial power cell

4. **Add workshop marker:**
   - Create empty GameObject at workshop center
   - Name it "WorkshopMarker"
   - This is for objective direction

---

### Task 5: Set Up Inspection System (30 min)

**Console Inspection:**
1. Select FactoryConsole GameObject
2. Add InspectableObject script
3. Set in Inspector:
   - `inspectionMessage` → "Press E to inspect console"
   - `inspectionResult` → "Console is offline. Check the power bay for issues."
   - `inspectionRange` → 3
   - `canInspect` → Checked
   - `inspectOnce` → Checked
4. Create UI for inspection:
   - Add Canvas (World Space) above console
   - Add TextMeshProUGUI for prompt
   - Assign to `promptUI` and `promptText`
5. In `onInspected` event:
   - Click +
   - Drag EnhancedObjectiveManager
   - Select: EnhancedObjectiveManager.OnConsoleInspected()

**Power Bay Inspection:**
1. Select PowerBay GameObject
2. Add InspectableObject script
3. Set in Inspector:
   - `inspectionMessage` → "Press E to inspect power bay"
   - `inspectionResult` → "Sparks everywhere! The power cell is damaged. Get a replacement from the workshop."
   - `inspectionRange` → 3
4. Create UI (same as console)
5. In `onInspected` event:
   - EnhancedObjectiveManager.OnPowerBayInspected()

---

### Task 6: Set Up EnhancedObjectiveManager (30 min)

1. **Create GameObject:**
   - Create empty: "EnhancedObjectiveManager"
   - Add EnhancedObjectiveManager script

2. **Assign UI:**
   - `objectiveText` → TextMeshProUGUI showing current objective
   - `directionText` → TextMeshProUGUI showing direction arrow
   - `objectivePanel` → Panel containing objective UI

3. **Assign Targets:**
   - `consoleTransform` → FactoryConsole GameObject
   - `powerBayTransform` → PowerBay GameObject
   - `workshopTransform` → WorkshopMarker GameObject

4. **Assign References:**
   - `factoryLights[]` → Drag all 27 lights
   - `enemySpawner` → EnemySpawner GameObject
   - `nailgun` → NailgunWeapon GameObject

---

### Task 7: Connect Everything (30 min)

**Tutorial Manager:**
- Find TutorialManager
- In `OnTutorialComplete()` or similar:
  - Call EnhancedObjectiveManager.OnTutorialComplete()

**Nailgun Pickup:**
- Select nailgun GameObject
- Add trigger to call: EnhancedObjectiveManager.OnNailgunPickedUp()

**Power Cell Pickup (Workshop):**
- Select workshop power cell
- In PowerCell script `OnPickedUp()`:
  - Call EnhancedObjectiveManager.OnPowerCellPickedUp()

**Power Bay:**
- In PowerBay script `OnPowerCellInserted()`:
  - Call EnhancedObjectiveManager.OnPowerCellInserted()

**Console:**
- In FactoryConsole script `OnConsoleActivated()`:
  - Call EnhancedObjectiveManager.OnConsoleActivated()

---

## 🧪 Testing Checklist

After setup, test each step:

1. [ ] Tutorial completes
2. [ ] Lights turn off
3. [ ] Objective says "Check console"
4. [ ] Can inspect console with E
5. [ ] Objective changes to "Check power bay"
6. [ ] Can inspect power bay with E
7. [ ] Objective changes to "Go to workshop"
8. [ ] Enemies start spawning
9. [ ] Can pick up nailgun
10. [ ] Can shoot enemies
11. [ ] Enemies take damage and die
12. [ ] Can pick up power cell
13. [ ] Objective changes to "Return to power bay"
14. [ ] Can insert power cell
15. [ ] Lights turn back on
16. [ ] Enemies stop spawning
17. [ ] Objective changes to "Go to console"
18. [ ] Can activate console
19. [ ] Win screen appears

---

## 🎨 Polish Tasks (If Time Allows)

### Visual Polish:
- [ ] Add muzzle flash to nailgun
- [ ] Add hit sparks on enemies
- [ ] Add death explosion for enemies
- [ ] Make sparks on power bay more dramatic
- [ ] Add glowing path from workshop to power bay

### Audio Polish:
- [ ] Enemy footstep sounds
- [ ] Enemy alert sound when they spot you
- [ ] Ambient factory sounds
- [ ] Dramatic music when enemies spawn
- [ ] Victory music

### Gameplay Polish:
- [ ] Add health system for player
- [ ] Add ammo pickups around map
- [ ] Add more enemy variety
- [ ] Add boss enemy at workshop
- [ ] Add time bonus for fast completion

---

## 🚨 Common Issues & Solutions

### Enemies don't move:
- Check NavMesh is baked (Window → AI → Navigation)
- Bake NavMesh for your factory floor
- Make sure enemies have NavMeshAgent component

### Enemies don't take damage:
- Check EnemyHealth script is attached
- Check enemies have Collider
- Check nailgun damage is set (default 10)

### Spawner doesn't spawn:
- Check `spawnEnabled` is true when it should be
- Check `enemyPrefab` is assigned
- Check `spawnPoints[]` array is filled
- Check spawn points are positioned correctly

### Inspection doesn't work:
- Check InspectableObject script is attached
- Check `canInspect` is true
- Check player is within `inspectionRange`
- Check E key input is working

### Objectives don't update:
- Check EnhancedObjectiveManager is in scene
- Check all UnityEvents are connected
- Check Console for debug messages
- Check each trigger is calling the right method

---

## 💡 Pro Tips

1. **Test incrementally** - Don't set up everything then test
2. **Use Console window** - Watch for debug messages
3. **Save often** - Ctrl+S after each major change
4. **Backup your scene** - Duplicate scene before big changes
5. **Use prefabs** - Make enemy a prefab so you can update all instances
6. **NavMesh first** - Bake NavMesh before testing enemy movement
7. **Start simple** - Get basic flow working, then add polish

---

## 📞 When You're Ready

Once you've completed the setup:
1. Test the complete flow
2. Note any issues or bugs
3. Tell me what's working and what needs fixing
4. We'll polish together!

---

You've got this! The hard part (coding) is done. Now it's just assembly and testing. 🚀
