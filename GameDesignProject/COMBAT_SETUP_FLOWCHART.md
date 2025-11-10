# 🎯 Combat System Setup - Visual Flowchart

## Follow This Order (Don't Skip Steps!)

```
┌─────────────────────────────────────────┐
│ STEP 1: Create Enemy Bot Prefab        │
│ Time: 15 min (primitives) or 1-2 hours │
│                                         │
│ 1. Create GameObject: "EnemyBot"       │
│ 2. Add Capsule Collider                │
│ 3. Add Rigidbody                        │
│ 4. Add Nav Mesh Agent                  │
│ 5. Add SimpleEnemyAI script            │
│ 6. Add EnemyHealth script              │
│ 7. Save as Prefab                       │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 2: Bake NavMesh                   │
│ Time: 5 min                             │
│                                         │
│ 1. Window → AI → Navigation            │
│ 2. Bake tab                             │
│ 3. Click "Bake"                         │
│ 4. Wait for blue overlay                │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 3: Create Spawn Points            │
│ Time: 15 min                            │
│                                         │
│ 1. Create Empty: "EnemySpawnPoints"    │
│ 2. Add 5-10 child empties              │
│ 3. Position around factory              │
│ 4. Make sure on NavMesh (blue areas)   │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 4: Create Enemy Spawner           │
│ Time: 10 min                            │
│                                         │
│ 1. Create Empty: "EnemySpawner"        │
│ 2. Add EnemySpawner script             │
│ 3. Assign enemyPrefab                  │
│ 4. Assign spawnPoints[] array          │
│ 5. Set spawnInterval: 5                │
│ 6. Set maxEnemiesAlive: 5              │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 5: Create Enhanced Manager        │
│ Time: 5 min                             │
│                                         │
│ 1. Create Empty: "EnhancedManager"     │
│ 2. Add EnhancedObjectiveManager script │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 6: Set Up Objective UI            │
│ Time: 10 min                            │
│                                         │
│ 1. Create Canvas (Screen Space)        │
│ 2. Add Panel at top                    │
│ 3. Add 2 TextMeshProUGUI               │
│ 4. Assign to EnhancedManager           │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 7: Assign Enhanced Manager Refs   │
│ Time: 15 min                            │
│                                         │
│ Select EnhancedManager, assign:         │
│ - objectiveText                         │
│ - directionText                         │
│ - consoleTransform                      │
│ - powerBayTransform                     │
│ - workshopTransform                     │
│ - factoryLights[]                       │
│ - enemySpawner                          │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 8: Add Inspection to Console      │
│ Time: 15 min                            │
│                                         │
│ 1. Select FactoryConsole                │
│ 2. Add InspectableObject script        │
│ 3. Create World Space Canvas (prompt)  │
│ 4. Assign UI fields                    │
│ 5. Connect onInspected event           │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 9: Add Inspection to Power Bay    │
│ Time: 15 min                            │
│                                         │
│ Same as Step 8 but for PowerBay        │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 10: Test Basic Combat             │
│ Time: 30 min                            │
│                                         │
│ 1. Place one enemy in scene            │
│ 2. Press Play                           │
│ 3. Enemy should patrol/chase           │
│ 4. Shoot enemy with nailgun            │
│ 5. Enemy should die                    │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 11: Test Spawning                 │
│ Time: 30 min                            │
│                                         │
│ 1. Remove test enemy                   │
│ 2. Enable spawner manually              │
│ 3. Enemies should spawn                │
│ 4. Test combat with multiple enemies   │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 12: Connect Full Loop             │
│ Time: 30 min                            │
│                                         │
│ Connect all triggers:                   │
│ - Tutorial → OnTutorialComplete()      │
│ - Console inspect → OnConsoleInspected()│
│ - Power bay inspect → OnPowerBayInspected()│
│ - Nailgun pickup → OnNailgunPickedUp() │
│ - Power cell pickup → OnPowerCellPickedUp()│
│ - Power insert → OnPowerCellInserted() │
│ - Console activate → OnConsoleActivated()│
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│ STEP 13: Final Test                    │
│ Time: 1 hour                            │
│                                         │
│ Play through complete game:             │
│ Tutorial → Lights off → Investigate →  │
│ Enemies spawn → Get weapon → Fight →   │
│ Restore power → Victory!                │
└─────────────────────────────────────────┘
              ↓
            DONE! 🎉
```

---

## Quick Reference

### What You Need:
- ✅ EnemyBot prefab (with all components)
- ✅ NavMesh baked
- ✅ Spawn points positioned
- ✅ EnemySpawner set up
- ✅ EnhancedObjectiveManager created
- ✅ UI created and assigned
- ✅ Inspection added to console and power bay
- ✅ All triggers connected

### Time Estimate:
- Quick enemy (primitives): 4 hours total
- Blender enemy: 5-6 hours total

---

## Can't Do It All?

### Minimum Viable Combat (2 hours):
1. Quick primitive enemy (15 min)
2. Bake NavMesh (5 min)
3. Create 3 spawn points (5 min)
4. Set up spawner (10 min)
5. Test combat (30 min)
6. Skip inspection system
7. Just have enemies spawn when game starts

**Result**: Combat works, simpler story

---

**Follow this flowchart top to bottom and you'll have combat working!** 🤖
