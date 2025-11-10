using UnityEngine;

/// <summary>
/// Simple test script - attach to enemy to test death system
/// Press T key to trigger death manually
/// </summary>
public class EnemyDeathTest : MonoBehaviour
{
    void Update()
    {
        // Press T to test death
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("[EnemyDeathTest] T key pressed - testing death system...");
            
            // Find player
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null)
            {
                Debug.LogError("[EnemyDeathTest] No player found with tag 'Player'!");
                return;
            }
            
            Debug.Log($"[EnemyDeathTest] Found player: {playerObj.name}");
            
            // Get PlayerHealth
            PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogError("[EnemyDeathTest] Player has no PlayerHealth component!");
                return;
            }
            
            Debug.Log("[EnemyDeathTest] Found PlayerHealth component - calling Die()...");
            
            // Kill player
            playerHealth.Die();
            
            Debug.Log("[EnemyDeathTest] Die() called successfully!");
        }
    }
}
