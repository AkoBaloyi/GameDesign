using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Player health - if enemy touches you, you die and restart
/// Simple permadeath for speedrun challenge
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("UI")]
    public GameObject deathScreen;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI finalTimeText;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;
    
    public bool isDead = false; // Public so enemies can check
    private SpeedrunTimer timer;

    void Start()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
        
        timer = FindObjectOfType<SpeedrunTimer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if hit by enemy
        if (collision.gameObject.CompareTag("Enemy") && !isDead)
        {
            Die();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Also check triggers in case enemies use triggers
        if (other.CompareTag("Enemy") && !isDead)
        {
            Die();
        }
    }

    // Public method so enemies can call it directly
    public void Die()
    {
        if (isDead) return; // Already dead
        
        isDead = true;
        
        Debug.Log("[PlayerHealth] Player died! Restarting...");
        
        // Play death sound
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        
        // Stop timer
        float finalTime = 0f;
        if (timer != null)
        {
            timer.StopTimer();
            finalTime = timer.GetCurrentTime();
        }
        
        // Show death screen
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            
            if (deathText != null)
            {
                deathText.text = "CAUGHT BY SECURITY BOT!\nPress R to Restart";
            }
            
            if (finalTimeText != null && finalTime > 0)
            {
                finalTimeText.text = $"Survived: {FormatTime(finalTime)}";
            }
        }
        
        // Freeze player
        FPController controller = GetComponent<FPController>();
        if (controller != null)
        {
            controller.SetInputEnabled(false);
        }
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Press R to restart after death
        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);
        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
}
