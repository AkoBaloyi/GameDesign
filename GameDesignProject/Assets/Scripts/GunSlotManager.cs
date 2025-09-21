using UnityEngine;

public class GunSlotManager : MonoBehaviour
{
    [Header("Gun Slot Transform")]
    public Transform gunSlot; // The transform where guns are held (from your old gun system)
    
    [Header("Current Weapon")]
    public NailgunWeapon currentWeapon;
    
    [Header("References")]
    public FPController playerController;
    public TutorialManager tutorialManager;
    
    void Start()
    {
        // Initially hide any weapon
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }
    }
    
    public void EquipNailgun(NailgunWeapon nailgun)
    {
        // Unequip current weapon
        if (currentWeapon != null)
        {
            currentWeapon.UnequipWeapon();
        }
        
        // Set new weapon
        currentWeapon = nailgun;
        
        // Position weapon in gun slot
        if (gunSlot != null)
        {
            nailgun.transform.SetParent(gunSlot);
            nailgun.transform.localPosition = Vector3.zero;
            nailgun.transform.localRotation = Quaternion.identity;
        }
        
        // Equip the weapon
        nailgun.EquipWeapon();
        
        Debug.Log("Nailgun equipped in gun slot!");
        
        // Notify tutorial
        if (tutorialManager != null)
        {
            tutorialManager.OnNailgunPickedUp();
        }
    }
    
    public void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.UnequipWeapon();
            currentWeapon = null;
        }
    }
    
    public bool HasWeaponEquipped()
    {
        return currentWeapon != null && currentWeapon.IsEquipped();
    }
    
    public NailgunWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}