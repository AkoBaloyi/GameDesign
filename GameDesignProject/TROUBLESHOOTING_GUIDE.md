# Troubleshooting Guide - Common Issues & Solutions

## 🚨 Enemy AI Issues

### Enemies Don't Move
**Symptoms**: Enemies spawn but stand still

**Solutions**:
1. **Bake NavMesh**:
   - Window → AI → Navigation
   - Select "Bake" tab
   - Click "Bake" button
   - Wait for it to complete (blue overlay appears on walkable surfaces)

2. **Check NavMeshAgent**:
   - Select enemy prefab
   - Verify NavMeshAgent component exists
   - Check "Agent Type" is set to "Humanoid"
   - Check "Base Offset" is 0

3. **Check Patrol Waypoints**:
   - Select enemy in scene
   - Check `patrolWaypoints[]` array is filled
   - Waypoints should be on NavMesh (blue areas)

### Enemies Walk Through Walls
**Symptoms**: Enemies ignore obstacles

**Solutions**:
1. **Check NavMesh Obstacles**:
   - Select walls/obstacles
   - Add "NavMesh Obstacle" component
   - Check "Carve" option
   - Rebake NavMesh

2. **Adjust NavMesh Settings**:
   - Window → AI → Navigation → Bake
   - Increase "Agent Radius" to 0.5
   - Rebake

### Enemies Don't Chase Player
**Symptoms**: Enemies patrol but ignore player

**Solutions**:
1. **Check Player Tag**:
   - Select Player GameObject
   - Set Tag to "Player"

2. **Check Detection Range**:
   - Select enemy
   - In SimpleEnemyAI script
   - Increase `detectionRange` to 20

3. **Check Player Reference**:
   - Enemy should auto-find player by tag
   - If not, manually assign in Inspector

---

## 🎯 Combat Issues

### Nailgun Doesn't Damage Enemies
**Symptoms**: Shooting enemies has no effect

**Solutions**:
1. **Check EnemyHealth Script**:
   - Select enemy prefab
   - Verify EnemyHealth component exists
   - Check `maxHealth` is set (default 100)

2. **Check Collider**:
   - Enemy must have Collider component
   - Collider must NOT be trigger
   - Check layer is not "Ignore Raycast"

3. **Check Nail Damage**:
   - Select nailgun
   - Check `damage` is set (default 10)
   - Check nail prefab has NailProjectile script

4. **Check IDamageable Interface**:
   - EnemyHealth should implement IDamageable
   - This is already done in the script

### Nails Don't Fire
**Symptoms**: Clicking does nothing

**Solutions**:
1. **Check Nailgun Equipped**:
   - Nailgun must be equipped first
   - Call `nailgun.EquipWeapon()`

2. **Check Ammo**:
   - Nailgun needs ammo to fire
   - Call `nailgun.LoadAmmo(50)`

3. **Check Fire Point**:
   - Nailgun needs `firePoint` assigned
   - Create empty child object in front of gun
   - Assign to `firePoint` field

4. **Check Nail Prefab**:
   - `nailPrefab` must be assigned
   - Prefab must have NailProjectile script

---

## 🔍 Inspection Issues

### Can't Inspect Console/Power Bay
**Symptoms**: E key does nothing

**Solutions**:
1. **Check InspectableObject Script**:
   - Select console/power bay
   - Verify InspectableObject component exists
   - Check `canInspect` is true

2. **Check Range**:
   - Stand closer to object
   - Check `inspectionRange` (default 3)
   - Increase if needed

3. **Check Player Tag**:
   - Player must have "Player" tag
   - InspectableObject finds player by tag

4. **Check UI**:
   - `promptUI` must be assigned
   - Canvas should be World Space
   - Text should be visible

### Inspection Doesn't Trigger Objective
**Symptoms**: Can inspect but objective doesn't change

**Solutions**:
1. **Check UnityEvent**:
   - Select inspectable object
   - Scroll to `onInspected` event
   - Should have EnhancedObjectiveManager method assigned

2. **Check EnhancedObjectiveManager**:
   - Verify it exists in scene
   - Check it's enabled
   - Check Console for debug messages

---

## 🎮 Spawner Issues

### Enemies Don't Spawn
**Symptoms**: No enemies appear

**Solutions**:
1. **Check Spawner Enabled**:
   - Select EnemySpawner
   - Check `spawnEnabled` is true
   - Or call `spawner.EnableSpawning()` from script

2. **Check Enemy Prefab**:
   - `enemyPrefab` must be assigned
   - Prefab must be valid

3. **Check Spawn Points**:
   - `spawnPoints[]` array must be filled
   - Spawn points must be on NavMesh
   - Check spawn points are not inside walls

4. **Check Max Enemies**:
   - If 5 enemies already alive, won't spawn more
   - Kill some enemies or increase `maxEnemiesAlive`

### Too Many Enemies Spawn
**Symptoms**: Overwhelming number of enemies

**Solutions**:
1. **Adjust Spawn Rate**:
   - Increase `spawnInterval` (default 5 seconds)
   - Try 8-10 seconds for easier difficulty

2. **Reduce Max Alive**:
   - Decrease `maxEnemiesAlive` (default 5)
   - Try 3 for easier difficulty

3. **Check Multiple Spawners**:
   - Make sure you don't have multiple spawners active
   - Only one spawner should be enabled

---

## 📊 Objective Issues

### Objectives Don't Update
**Symptoms**: Objective text doesn't change

**Solutions**:
1. **Check EnhancedObjectiveManager**:
   - Verify it exists in scene
   - Check `objectiveText` is assigned
   - Check it's enabled

2. **Check Method Calls**:
   - Each trigger should call appropriate method
   - Check Console for debug messages
   - Look for "[EnhancedObjectiveManager]" logs

3. **Check UnityEvents**:
   - All inspection/pickup events should be connected
   - Tutorial → OnTutorialComplete()
   - Console → OnConsoleInspected()
   - Power Bay → OnPowerBayInspected()
   - Etc.

### Direction Indicator Wrong
**Symptoms**: Arrow points wrong direction

**Solutions**:
1. **Check Target Transforms**:
   - `consoleTransform` must be assigned
   - `powerBayTransform` must be assigned
   - `workshopTransform` must be assigned

2. **Check Player Reference**:
   - Player must have "Player" tag
   - EnhancedObjectiveManager finds player automatically

---

## 💡 Lights Issues

### Lights Don't Turn Off
**Symptoms**: Lights stay on after tutorial

**Solutions**:
1. **Check Lights Array**:
   - Select EnhancedObjectiveManager
   - Check `factoryLights[]` is filled with all 27 lights

2. **Check Tutorial Connection**:
   - Tutorial must call `OnTutorialComplete()`
   - Check this is connected

3. **Manual Test**:
   - In Play mode, select EnhancedObjectiveManager
   - Right-click script → OnTutorialComplete()
   - Lights should turn off

### Lights Don't Turn Back On
**Symptoms**: Lights stay off after power restored

**Solutions**:
1. **Check Power Cell Insertion**:
   - PowerBay must call `OnPowerCellInserted()`
   - Check this is connected to EnhancedObjectiveManager

2. **Check Lights Array**:
   - Same as above - must be assigned

---

## 🎨 Visual Issues

### Sparks Don't Stop
**Symptoms**: Sparks continue after power restored

**Solutions**:
1. **Check Spark Reference**:
   - PowerBay needs reference to spark particle system
   - Add field: `public ParticleSystem sparks;`
   - Assign in Inspector

2. **Stop Sparks in Code**:
   - In PowerBay.InsertPowerCell():
   - Add: `if (sparks != null) sparks.Stop();`

### Enemy Death Effect Missing
**Symptoms**: Enemies just disappear

**Solutions**:
1. **Check Death Effect**:
   - Select enemy prefab
   - In EnemyHealth script
   - Assign `deathEffect` prefab

2. **Create Death Effect**:
   - Create particle system
   - Make it explosion-like
   - Save as prefab
   - Assign to enemy

---

## 🔊 Audio Issues

### No Sound Effects
**Symptoms**: Game is silent

**Solutions**:
1. **Check AudioSource**:
   - Objects need AudioSource component
   - Check "Play On Awake" is OFF
   - Check volume is 1

2. **Check Audio Clips**:
   - Clips must be assigned in Inspector
   - Import audio files to project first

3. **Check Audio Listener**:
   - Player camera needs Audio Listener component
   - Only one Audio Listener in scene

---

## 🐛 General Debugging

### Check Console Messages
Always open Console (Ctrl+Shift+C) and look for:
- Red errors (must fix)
- Yellow warnings (should fix)
- Debug.Log messages (helpful info)

### Use Diagnostic Tool
Press F12 in Play mode to see:
- Component status
- Current game state
- Distances to objectives
- Setup verification

### Test Incrementally
Don't set up everything then test:
1. Set up one system
2. Test it
3. Fix issues
4. Move to next system

### Save Often
- Ctrl+S after each change
- Duplicate scene before big changes
- Use version control if possible

---

## 🆘 Still Stuck?

If you're still having issues:

1. **Check all references are assigned**
   - Every public field should have something assigned
   - Use Inspector to verify

2. **Check Console for errors**
   - Fix red errors first
   - Then yellow warnings

3. **Test each system individually**
   - Enemy AI alone
   - Combat alone
   - Objectives alone
   - Then combine

4. **Use Debug.Log**
   - Add Debug.Log to see what's happening
   - Check if methods are being called

5. **Restart Unity**
   - Sometimes Unity needs a restart
   - Save first!

---

Remember: Most issues are simple fixes like missing references or incorrect settings. Check the basics first! 🔍
