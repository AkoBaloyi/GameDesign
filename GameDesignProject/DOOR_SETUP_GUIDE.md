# Door Interaction Setup Guide (F Key)

## ✅ Scripts Updated
- `DoorInteractor.cs` - Uses `OnInteract()` for F key
- `SecurityDoor.cs` - Uses `OnInteract()` for F key
- `InteractionBroadcaster.cs` - NEW! Broadcasts F key to nearby doors

## 🎮 Unity Editor Setup (Choose ONE method)

### METHOD 1: Using InteractionBroadcaster (RECOMMENDED - Easiest)

1. **Add InteractionBroadcaster to Player:**
   - Select your Player GameObject
   - Add Component → `InteractionBroadcaster`
   - Set `Interaction Range` to 3-5 (how far you can interact)

2. **Configure Player Input:**
   - On Player GameObject, find `Player Input` component
   - Set `Behavior` to `Invoke Unity Events`
   - Under `Events` → `Player` → `Interact`:
     - Click `+` to add event
     - Drag Player GameObject into the field
     - Select `InteractionBroadcaster` → `OnInteract`

3. **Done!** All doors will now respond to F key automatically when you look at them.

---

### METHOD 2: Manual Wiring (More Control)

1. **For Each Door:**
   - Select the Door GameObject
   - Make sure it has `DoorInteractor` or `SecurityDoor` script
   - Make sure it has a Collider with `Is Trigger` checked
   - Make sure the door's trigger layer is not ignored by raycasts

2. **Configure Player Input:**
   - Select Player GameObject
   - Find `Player Input` component
   - Set `Behavior` to `Send Messages`
   - The system will automatically call `OnInteract()` on scripts that have this method

3. **Test:**
   - Walk up to a door (enter its trigger)
   - Press F key
   - Door should open

---

## 🔍 Troubleshooting

### Door doesn't open when pressing F:

1. **Check Player Input Component:**
   - Is it enabled?
   - Is `Actions` set to `Controls`?
   - Is `Default Map` set to `Player`?

2. **Check Door Setup:**
   - Does door have a Collider with `Is Trigger` checked?
   - Is the door tagged correctly (if using tags)?
   - Is `playerInRange` being set to true? (Add debug log in `OnTriggerEnter`)

3. **Check Input Actions:**
   - Open `Controls.inputactions` file
   - Find `Interact` action
   - Verify it's bound to `<Keyboard>/f`

4. **Add Debug Logs:**
   ```csharp
   // In DoorInteractor.OnTriggerEnter():
   Debug.Log("Player entered door trigger!");
   
   // In DoorInteractor.OnInteract():
   Debug.Log("F key pressed, opening door!");
   ```

### F key does nothing:

1. Check Console for errors
2. Make sure Player Input component is enabled
3. Make sure you're using the NEW Input System (not old Input Manager)
4. Verify `Interact` action exists in Controls.inputactions

### Door opens but animation doesn't play:

1. Check `doorAnimator` is assigned in Inspector
2. Check Animator has "Open" trigger/bool parameter
3. Check `animatorOpenTrigger` name matches your Animator parameter
4. Check `useBoolToOpen` setting matches your Animator setup

---

## 📝 Key Bindings Summary

| Action | Keyboard | Gamepad | Purpose |
|--------|----------|---------|---------|
| **Interact** | F | Button South (A/Cross) | Open doors |
| **Pickup** | E | Button East (B/Circle) | Pick up objects |
| **Jump** | Space | Button South (A/Cross) | Jump |
| **Sprint** | Left Shift | Left Stick Press | Sprint |
| **Crouch** | Left Ctrl | Right Shoulder | Crouch |
| **Throw** | G | Left Shoulder | Throw held object |

---

## 🎯 Next Steps

1. Add `InteractionBroadcaster` to your Player
2. Wire up the Interact action in Player Input
3. Test with a door
4. If it works, you're done!
5. If not, check Troubleshooting section above

---

## 💡 Tips

- **Prompt Messages**: All door prompts now say "Press F to open"
- **E key is for pickups**: Objects, weapons, etc.
- **F key is for interactions**: Doors, buttons, levers, etc.
- **Separation of concerns**: This prevents accidentally picking up objects when trying to open doors

---

## 🔧 Advanced: Adding More Interactables

To add new interactable objects (buttons, levers, etc.):

1. Create a script with `public void OnInteract(InputAction.CallbackContext context)`
2. Add the script to your interactable object
3. Add a trigger collider
4. Add this to `InteractionBroadcaster.OnInteract()`:
   ```csharp
   YourScript interactable = hit.collider.GetComponent<YourScript>();
   if (interactable != null)
   {
       interactable.OnInteract(context);
       return;
   }
   ```

Done!
