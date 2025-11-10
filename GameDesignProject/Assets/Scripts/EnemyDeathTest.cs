using UnityEngine;




public class EnemyDeathTest : MonoBehaviour
{
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("[EnemyDeathTest] T key pressed - testing death system...");

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null)
            {
                Debug.LogError("[EnemyDeathTest] No player found with tag 'Player'!");
                return;
            }
            
            Debug.Log($"[EnemyDeathTest] Found player: {playerObj.name}");

            PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogError("[EnemyDeathTest] Player has no PlayerHealth component!");
                return;
            }
            
            Debug.Log("[EnemyDeathTest] Found PlayerHealth component - calling Die()...");

            playerHealth.Die();
            
            Debug.Log("[EnemyDeathTest] Die() called successfully!");
        }
    }
}
