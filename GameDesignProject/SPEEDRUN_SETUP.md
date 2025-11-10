# 🏃 Speedrun Mode Setup

## New Game Mode: Permadeath Speedrun Challenge!

**Goal**: Complete the factory restoration as fast as possible without getting caught by security bots!

**Rules**:
- ⚡ Timer starts after tutorial
- 💀 If a bot touches you = instant death + restart
- 🏆 Best times saved to leaderboard
- 🎯 No combat - just RUN!

---

## ✅ New Scripts Created:

1. **PlayerHealth.cs** - Permadeath system
2. **SpeedrunTimer.cs** - Timer + leaderboard
3. **Updated SimpleEnemyAI.cs** - Kills player on contact
4. **Updated ClearObjectiveManager.cs** - Starts/stops timer

---

## 🎮 Setup Instructions:

### Step 1: Add PlayerHealth to Player (2 min)

1. **Select Player** GameObject
2. **Add Component**: PlayerHealth
3. **Assign in Inspector:**
   - Death Screen → Create UI panel (see below)
   - Death Text → TextMeshProUGUI saying "CAUGHT!"
   - Final Time Text → TextMeshProUGUI for time
   - Audio Source → Player's AudioSource
   - Death Sound → (optional) death sound effect

### Step 2: Create Death Screen UI (5 min)

**In your Canvas:**

1. **Create Panel**: "DeathScreen"
   - Background: Black, 80% alpha
   - Covers full screen
   - Disable it (script will enable)

2. **Add Text**: "DeathText"
   - Text: "CAUGHT BY SECURITY BOT!"
   - Font Size: 48
   - Color: Red
   - Alignment: Center
   - Position: Center of screen

3. **Add Text**: "FinalTimeText"
   - Text: "Survived: 00:00.00"
   - Font Size: 32
   - Color: Yellow
   - Below DeathText

4. **Add Text**: "RestartText"
   - Text: "Press R to Restart"
   - Font Size: 24
   - Color: White
   - Bottom of screen

### Step 3: Add SpeedrunTimer (2 min)

1. **Create Empty GameObject**: "SpeedrunTimer"
2. **Add Component**: SpeedrunTimer
3. **Assign in Inspector:**
   - Timer Text → Create TextMeshProUGUI in top-right corner
   - Leaderboard Panel → Create panel (see below)
   - Leaderboard Text → TextMeshProUGUI in panel
   - Start On Tutorial Complete: CHECK
   - Max Leaderboard Entries: 10

### Step 4: Create Timer UI (3 min)

**In your Canvas:**

1. **Create Text**: "TimerText"
   - Text: "00:00.00"
   - Font Size: 36
   - Color: Cyan
   - Position: Top-right corner
   - Anchor: Top-right

### Step 5: Create Leaderboard UI (5 min)

**In your Canvas:**

1. **Create Panel**: "LeaderboardPanel"
   - Background: Dark gray, 90% alpha
   - Size: 400 width, 500 height
   - Position: Center of screen
   - Disable it (script will enable)

2. **Add Text**: "LeaderboardTitle"
   - Text: "🏆 SPEEDRUN LEADERBOARD 🏆"
   - Font Size: 32
   - Color: Gold
   - Top of panel

3. **Add Text**: "LeaderboardText"
   - Text: (will be filled by script)
   - Font Size: 24
   - Color: White
   - Alignment: Left
   - Below title

4. **Add Button**: "CloseButton"
   - Text: "Continue"
   - Bottom of panel
   - OnClick → SpeedrunTimer.HideLeaderboard()

### Step 6: Tag Enemy as "Enemy" (1 min)

1. **Select EnemyBot** prefab
2. **Tag dropdown** (top of Inspector)
3. **Select "Enemy"** (or create new tag "Enemy")
4. **Save prefab**

### Step 7: Update ClearObjectiveManager (1 min)

1. **Select ClearObjectiveManager**
2. **Assign in Inspector:**
   - Speedrun Timer → SpeedrunTimer GameObject

### Step 8: Remove Nailgun (Optional)

Since this is now a speedrun challenge, you can:
- Remove nailgun from scene
- Or keep it but it's not needed
- Focus is on RUNNING, not fighting

---

## 🧪 Test It!

**Press Play:**

1. [ ] Complete tutorial
2. [ ] **Timer starts** (top-right corner)
3. [ ] Lights go out
4. [ ] Run to console, inspect
5. [ ] Run to power bay, inspect
6. [ ] **Enemies spawn**
7. [ ] Run to workshop
8. [ ] **Avoid enemies!**
9. [ ] Get power cell
10. [ ] Run back to power bay
11. [ ] **Don't get caught!**
12. [ ] Insert power cell
13. [ ] Run to console
14. [ ] Activate
15. [ ] **Timer stops**
16. [ ] **Leaderboard shows!**

**Test Death:**
1. [ ] Let an enemy touch you
2. [ ] Death screen appears
3. [ ] Shows your survival time
4. [ ] Press R to restart

---

## 🎯 Gameplay Tips for Players:

**Strategy:**
- Sprint everywhere (Shift key)
- Use map (M key) to plan route
- Enemies spawn after power bay inspection
- They chase you when close
- No combat - just RUN!
- Fastest route: Console → Power Bay → Workshop → Power Bay → Console

**Speedrun Tactics:**
- Skip inspection text (press E immediately)
- Sprint constantly
- Learn enemy patrol patterns
- Find optimal path
- Practice makes perfect!

---

## 🏆 Leaderboard Features:

- Saves top 10 times
- Shows your rank
- Highlights current run
- Medals for top 3 (🥇🥈🥉)
- Persists between sessions (PlayerPrefs)

---

## 💡 Polish Ideas:

**Audio:**
- Ticking clock sound
- Heartbeat when enemy is close
- Alarm when caught
- Victory fanfare

**Visual:**
- Screen shake when caught
- Red vignette when enemy is close
- Slow-motion on death
- Confetti on new record

**UI:**
- Show current rank during run
- "New Record!" message
- Split times for each objective
- Ghost replay of best run

---

## 🎮 This Makes Your Game:

✅ **Replayable** - Speedrun challenge
✅ **Competitive** - Leaderboard
✅ **Tense** - Permadeath
✅ **Simple** - Clear goal
✅ **Fun** - Fast-paced action

**Perfect for the assignment rubric:**
- NPC integration: ✅ Enemies chase and kill
- Gameplay impact: ✅ Core mechanic
- Environmental narrative: ✅ Factory escape
- Game feel: ✅ Tense and exciting
- Overall experience: ✅ Complete and polished

---

**Time to setup: 20 minutes**
**Time to test: 5 minutes**
**Total: 25 minutes**

**You're almost done! Set it up and test!** 🏃💨
