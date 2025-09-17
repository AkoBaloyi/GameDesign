using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Settings")]
    public string keycardID = "DOOR_01"; 
        public AudioClip pickupSound;
    public GameObject pickupEffect; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.CollectKeycard(keycardID);
                
                
                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                
                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);

                Destroy(gameObject); 
            }
        }
    }
}