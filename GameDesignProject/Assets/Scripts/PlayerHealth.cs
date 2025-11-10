using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;




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

        if (collision.gameObject.CompareTag("Enemy") && !isDead)
        {
            Die();
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy") && !isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return; // Already dead
        
        isDead = true;
        
        Debug.Log("[PlayerHealth] Player died! Restarting...");

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        float finalTime = 0f;
        if (timer != null)
        {
            timer.StopTimer();
            finalTime = timer.GetCurrentTime();
        }

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

        FPController controller = GetComponent<FPController>();
        if (controller != null)
        {
            controller.SetInputEnabled(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {

        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void RestartGame()
    {

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
