# SIMPLE SETUP - Do This Now!

## ✅ Code Changes Done!

I've updated these scripts to **automatically** connect to EnhancedObjectiveManager:
- ✅ PowerCell.cs - auto-notifies when picked up
- ✅ TutorialManager.cs - auto-notifies when tutorial completes
- ✅ PowerBay.cs - auto-notifies when power cell inserted
- ✅ FactoryConsole.cs - auto-notifies when console activated

**You don't need to manually connect these anymore!**

---

## 🔧 NavMesh Not Showing Blue Overlay?

**This is normal in Unity 6!** The NavMesh is baked even if you don't see blue.

**To verify it worked:**
1. Window → AI → Navigation
2. Look at "Bake" tab
3. If it says "Clear" button (not "Bake"), it's already baked!
4. **Test**: Put your enemy in the scene and press Play - if it moves, NavMesh works!

**If enemies don't move:**
- Make sure your floors have MeshRenderer and MeshCollider
- Try increasing Agent Radius to 0.5
- Click "Clear" then "Bake" again

---

## 🎮 Nailgun Slot Issue?

**The nailgun field is OPTIONAL!** 

**In EnhancedObjectiveManager Inspector:**
- If you have a nailgun → drag it in
- If you don't have a nailgun → **leave it empty (None)**

The script will work fine either way!

---

## 🖼️ Can't Edit Canvas Position/Scale?

**World Space Canvas Fix:**

When you create a World Space Canvas, Unity locks it. Here's how to edit it:

**Method 1 - Use Inspector:**
1. Select the Canvas
2. In Inspector, find "Rect Transform" component
3. Change values directly:
   - Pos X: 0
   - Pos Y: 2 (above object)
   - Pos Z: 0
   - Scale X: 0.01
   - Scale Y: 0.01
   - Scale Z: 0.01

**Method 2 - Use Rect Tool:**
1. Select Canvas
2. Press T key (Rect Tool)
3. Now you can drag it in scene view

**Method 3 - Skip World Space Canvas:**
Just use Screen Space canvas for now! The prompts will still work.

---

## 📋 SIMPLIFIED SETUP STEPS

### STEP 1: Create Enemy (15 min)

1. **Create Empty GameObject**: "EnemyBot"
2. **Add Cube child**: "Body", scale (0.5, 1, 0.5)
3. **Add Sphere child**: "Head", scale (0.4, 0.4, 0.4), position (0, 0.7, 0)
4. **Select EnemyBot parent**, add:
   - Capsule Collider
   - Rigidbody (Freeze Rotation X/Y/Z)
   - Nav Mesh Agent
   - SimpleEnemyAI script
   - EnemyHealth script
5. **Drag EnemyBot to Project** to make prefab
6. **Delete from scene**

---

### STEP 2: Bake NavMesh (2 min)

1. **Window → AI → Navigation**
2. **Click "Bake"**
3. **Done!** (Even if no blue overlay)

---

### STEP 3: Create Spawn Points (10 min)

1. **Create Empty**: "EnemySpawnPoints"
2. **Right-click it → Create Empty** (5 times)
3. **Name them**: SpawnPoint_1, SpawnPoint_2, etc.
4. **Move them** around your factory

---

### STEP 4: Create Spawner (5 min)

1. **Create Empty**: "EnemySpawner"
2. **Add Component**: EnemySpawner
3. **Assign**:
   - Enemy Prefab: Your EnemyBot prefab
   - Spawn Points: Drag all 5 spawn points
   - Spawn Interval: 5
   - Max Enemies Alive: 5
   - Spawn Enabled: UNCHECK
   - Spawn On Start: UNCHECK
   - Stop When Power Restored: CHECK

---

### STEP 5: Create Workshop Marker (1 min)

1. **Create Empty** at your workshop: "WorkshopMarker"
2. **Done!**

---

### STEP 6: Create EnhancedObjectiveManager (2 min)

1. **Create Empty**: "EnhancedObjectiveManager"
2. **Add Component**: EnhancedObjectiveManager
3. **Leave everything blank for now**

---

### STEP 7: Create Simple UI (5 min)

**Option A - Use Existing Canvas:**
1. Find your existing Canvas
2. Add Panel: "ObjectivePanel" (top of screen)
3. Add Text: "ObjectiveText" (cyan color)
4. Add Text: "DirectionText" (yellow color)

**Option B - Skip UI for now:**
- Just test without UI first
- Add it later when everything works

---

### STEP 8: Assign References (10 min)

**Select EnhancedObjectiveManager**, assign:

**UI (if you made it):**
- Objective Text → ObjectiveText
- Direction Text → DirectionText
- Objective Panel → ObjectivePanel

**Targets:**
- Console Transform → FactoryConsole
- Power Bay Transform → PowerBay
- Workshop Transform → WorkshopMarker

**References:**
- Factory Lights → Drag all your lights (or skip for now)
- Enemy Spawner → EnemySpawner
- Nailgun → **Leave empty if you don't have one!**

---

### STEP 9: Add Inspection (OPTIONAL - Skip for now!)

**You can skip this and test without inspection first!**

If you want to add it:
1. Select FactoryConsole
2. Add InspectableObject script
3. Set inspection message
4. Same for PowerBay

**But honestly, test without it first!**

---

## 🧪 TEST NOW!

**Press Play and check:**

1. [ ] Tutorial completes
2. [ ] Lights turn off
3. [ ] Enemies start spawning
4. [ ] Walk to power cell, pick it up
5. [ ] Walk to power bay, press F to insert
6. [ ] Lights turn on
7. [ ] Enemies stop spawning
8. [ ] Walk to console, press F to activate
9. [ ] Win!

---

## 🐛 If It Doesn't Work:

**Enemies don't spawn:**
- Check Console for errors
- Check EnemySpawner has prefab assigned
- Check spawn points are assigned

**Objectives don't update:**
- Check Console for "[EnhancedObjectiveManager]" messages
- The code changes I made should auto-connect everything

**Enemies don't move:**
- NavMesh might not be baked on your floors
- Try putting enemy directly in scene and press Play
- If it doesn't move, NavMesh issue

---

## 💡 Pro Tips:

1. **Test without UI first** - Get the flow working, add UI later
2. **Skip inspection for now** - Test basic flow first
3. **One thing at a time** - If something breaks, you know what caused it
4. **Check Console window** - All scripts print debug messages
5. **Nailgun is optional** - Leave it empty if you don't have one

---

## ⏱️ Time Estimate:

- Enemy: 15 min
- NavMesh: 2 min
- Spawn points: 10 min
- Spawner: 5 min
- Workshop marker: 1 min
- Enhanced manager: 2 min
- UI: 5 min (or skip)
- Assign references: 10 min
- **Total: 50 minutes!**

---

**The code is done. Just set up the GameObjects and test!** 🚀
