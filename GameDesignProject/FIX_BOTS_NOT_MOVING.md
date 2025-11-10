# 🤖 Fix Bots Not Moving

## Problem: Bots spawn but don't move

This is a **NavMesh** issue. Here's how to fix it:

---

## ✅ Solution 1: Bake NavMesh Properly (Most Common Fix)

### Step 1: Check NavMesh Settings

1. **Window → AI → Navigation**
2. **Click "Bake" tab**
3. **Set these values:**
   - Agent Radius: **0.3**
   - Agent Height: **1.8**
   - Max Slope: **45**
   - Step Height: **0.4**

### Step 2: Select Your Floors

1. **Select ALL floor objects** in your scene (hold Ctrl and click each)
2. **In Inspector, check "Navigation Static"** (top-right, in Static dropdown)
3. **Or go to Navigation window → Object tab**
4. **Check "Navigation Static"**

### Step 3: Bake NavMesh

1. **Go back to Bake tab**
2. **Click "Clear"** (if already baked)
3. **Click "Bake"**
4. **Wait for it to finish**

### Step 4: Verify NavMesh

**In Scene view:**
- Enable Gizmos (top-right button)
- You might see blue overlay on floors (Unity 6 might not show it)
- That's OK - if "Bake" button changed to "Clear", it worked

---

## ✅ Solution 2: Check Floor Colliders

**Enemies need colliders to walk on floors!**

1. **Select each floor object**
2. **Check it has:**
   - MeshCollider (with mesh assigned)
   - Or BoxCollider
3. **If missing:**
   - Add Component → Mesh Collider
   - Check "Convex" if it's a complex mesh
   - Or Add Component → Box Collider

---

## ✅ Solution 3: Check Enemy Prefab Settings

### In Enemy Prefab:

1. **Select EnemyBot prefab**
2. **Check NavMeshAgent component:**
   - Agent Type: Humanoid
   - Base Offset: **0**
   - Speed: **3.5**
   - Angular Speed: **120**
   - Acceleration: **8**
   - Stopping Distance: **2**
   - Auto Braking: **Checked**
   - Radius: **0.3**
   - Height: **1.8**
   - **Obstacle Avoidance Type: High Quality**
   - **Avoidance Priority: 50**

3. **Check Rigidbody:**
   - Is Kinematic: **UNCHECKED**
   - Use Gravity: **Checked**
   - Constraints → Freeze Rotation: X, Y, Z all **CHECKED**

4. **Check Collider:**
   - Capsule Collider
   - Radius: 0.3
   - Height: 1.8
   - Center: (0, 0.9, 0)
   - Is Trigger: **UNCHECKED**

---

## ✅ Solution 4: Check Spawn Points

### Spawn points must be ON the NavMesh:

1. **Select a spawn point**
2. **Look at the floor below it in Scene view**
3. **Move it if needed** - should be on a walkable floor

### Quick Test:

1. **Drag one enemy into scene manually**
2. **Position it on a floor**
3. **Press Play**
4. **Watch Console for messages:**
   - Should say: "NavMeshAgent enabled. On NavMesh: True"
   - If says "False" → Enemy not on NavMesh!

---

## ✅ Solution 5: Debug Enemy in Scene

### Test with one enemy:

1. **Delete all spawn points temporarily**
2. **Drag EnemyBot prefab into scene**
3. **Position it on a floor**
4. **Press Play**
5. **Watch what happens:**

**If enemy doesn't move at all:**
- NavMesh not baked
- Or floor has no collider
- Or enemy not on NavMesh

**If enemy moves but sinks:**
- Increase Base Offset to 0.5 or 0.9

**If enemy moves correctly:**
- Problem is spawn point locations!
- Move spawn points to where enemy worked

---

## ✅ Solution 6: Simplify (No Waypoints)

If waypoints are causing issues, remove them:

1. **Select EnemyBot prefab**
2. **Find SimpleEnemyAI component**
3. **Leave "Patrol Waypoints" EMPTY**
4. **Enemy will just chase player** (no patrol)

This is actually fine for a speedrun game!

---

## 🧪 Testing Checklist:

After each fix, test:

1. [ ] Bake NavMesh (Window → AI → Navigation → Bake)
2. [ ] All floors have colliders
3. [ ] All floors marked "Navigation Static"
4. [ ] Enemy prefab has NavMeshAgent
5. [ ] Spawn one enemy manually - does it move?
6. [ ] Check Console for "On NavMesh: True"
7. [ ] Spawn points on walkable floors
8. [ ] Press Play - enemies spawn and move

---

## 🎯 Most Likely Causes:

1. **NavMesh not baked** (90% of cases)
2. **Floors not marked Navigation Static** (80% of cases)
3. **Floors missing colliders** (50% of cases)
4. **Spawn points in wrong location** (30% of cases)

---

## 💡 Quick Fix (If Nothing Works):

**Just make enemies chase player without patrol:**

1. Open SimpleEnemyAI.cs
2. In Start(), comment out waypoint code:
```csharp
// Start patrolling if we have waypoints
// if (patrolWaypoints != null && patrolWaypoints.Length > 0)
// {
//     GoToNextWaypoint();
// }
```

3. Enemies will stand still until player gets close
4. Then they chase!
5. Good enough for speedrun game!

---

## 📞 Still Not Working?

**Check Console window for errors:**
- "Failed to create agent" → NavMesh not baked
- "SetDestination failed" → Target not on NavMesh
- No messages → Script not running

**In Scene view:**
- Select enemy
- Look at NavMeshAgent component
- Is "On Nav Mesh" checked?
- If NO → Enemy not on NavMesh surface

---

**Follow these steps in order and your bots will move!** 🤖💨
