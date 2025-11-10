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

        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }
    }
    
    public void EquipNailgun(NailgunWeapon nailgun)
    {

        if (currentWeapon != null)
        {
            currentWeapon.UnequipWeapon();
        }

        currentWeapon = nailgun;

        if (gunSlot != null)
        {
            nailgun.transform.SetParent(gunSlot);
            nailgun.transform.localPosition = Vector3.zero;
            nailgun.transform.localRotation = Quaternion.identity;
        }

        nailgun.EquipWeapon();
        
        Debug.Log("Nailgun equipped in gun slot!");

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