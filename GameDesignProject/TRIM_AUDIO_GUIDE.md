# ✂️ How to Trim/Cut Audio Files

## Best Tool: Audacity (Free!)

**Download**: https://www.audacityteam.org/download/

---

## Quick Guide (5 minutes)

### Step 1: Install Audacity (2 min)
1. Download from link above
2. Install (just click Next)
3. Open Audacity

### Step 2: Open Your Sound (30 sec)
1. **File → Open**
2. Select your sound file (generator, etc.)
3. You'll see the waveform

### Step 3: Find the Good Part (1 min)
1. **Press Space** to play
2. **Listen** for the part you want
3. For generator: Find the "rev up" part (probably 10-30 seconds in)

### Step 4: Select the Part You Want (1 min)
1. **Click and drag** on the waveform to select
2. **Or use timestamps**:
   - Click at start time
   - Shift+Click at end time

### Step 5: Delete Everything Else (30 sec)

**Option A: Keep Selection, Delete Rest**
1. Select the part you WANT
2. **Ctrl+Shift+I** (Invert selection)
3. **Delete** key
4. Done!

**Option B: Delete Unwanted Parts**
1. Select part you DON'T want
2. **Delete** key
3. Repeat for other unwanted parts

### Step 6: Export (1 min)
1. **File → Export → Export Audio**
2. **Format**: WAV or MP3
   - WAV: Better quality, larger file
   - MP3: Smaller file, good enough
3. **Name it**: "Generator_Short.wav"
4. **Save**

### Step 7: Import to Unity
1. Drag trimmed file into Unity Assets/Audio
2. Use this instead of original

---

## Specific Recommendations

### Generator Sound (8+ minutes)
**What to keep**: The "rev up" part
- Start: When engine starts
- End: When it reaches full power
- **Duration**: 10-20 seconds is perfect

**How to find it**:
1. Play the sound
2. Listen for the dramatic rev-up
3. That's probably 10-60 seconds into the file
4. Select just that part

### Factory Ambient (34 seconds)
**Already perfect!** No need to trim.

### Transformer (varies)
**If it's long**: Keep just 5-10 seconds, it will loop anyway

### PlayStation (18 seconds)
**Already good!** Maybe trim if there's silence at start/end

---

## Quick Audacity Tips

### Playback:
- **Space**: Play/Pause
- **Home**: Go to start
- **End**: Go to end

### Selection:
- **Click + Drag**: Select region
- **Ctrl+A**: Select all
- **Ctrl+Shift+I**: Invert selection

### Editing:
- **Delete**: Remove selection
- **Ctrl+X**: Cut
- **Ctrl+C**: Copy
- **Ctrl+V**: Paste

### Zoom:
- **Ctrl+1**: Zoom to fit
- **Ctrl+3**: Zoom to selection
- **Scroll wheel**: Zoom in/out

---

## Alternative: Online Tools (No Install)

### TwistedWave Online
**URL**: https://twistedwave.com/online
- Free, no install
- Upload sound
- Select and delete parts
- Download trimmed version

### AudioTrimmer
**URL**: https://audiotrimmer.com/
- Very simple
- Upload, select start/end time
- Download

### MP3Cut
**URL**: https://mp3cut.net/
- Works with MP3 and WAV
- Drag sliders to select
- Download

---

## Recommended Lengths

For your game:

```
Generator: 10-20 seconds
  - Just the rev-up part
  - Dramatic and punchy

Factory Ambient: 34 seconds ✓
  - Already perfect

Transformer: 5-10 seconds
  - Will loop anyway
  - Just need one good buzz cycle

PlayStation: 18 seconds ✓
  - Already good
  - Maybe trim silence at start
```

---

## Pro Tips

### For Looping Sounds (Factory, Transformer):
- Make sure start and end match
- No clicks or pops
- Test loop in Audacity: **Effect → Repeat**

### For One-Shot Sounds (Generator, PlayStation):
- Trim silence at start (for instant impact)
- Can leave silence at end (doesn't matter)
- Make sure it's loud enough

### Volume Adjustment:
If sound is too quiet:
1. Select all (Ctrl+A)
2. **Effect → Amplify**
3. Increase by 3-6 dB
4. Export

---

## Quick Workflow

### For Generator (Most Important):

1. **Open in Audacity**
2. **Play and find the rev-up** (probably 10-60 seconds in)
3. **Select 10-20 seconds** of the dramatic part
4. **Ctrl+Shift+I** (invert)
5. **Delete**
6. **File → Export → WAV**
7. **Name**: "Generator_Short.wav"
8. **Import to Unity**
9. **Assign to LightsController**

**Time**: 5 minutes total

---

## If You Don't Want to Trim

**Alternative**: Use the full sound!
- It will play the whole 8 minutes
- Adds realism (full startup sequence)
- Players might like it
- Or fade it out in code after 10 seconds

**Your choice!** Both work fine.

---

**Audacity is the easiest - download it and you'll be trimming in 5 minutes!** ✂️🎵
