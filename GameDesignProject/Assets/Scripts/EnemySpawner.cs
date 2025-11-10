using UnityEngine;
using System.Collections.Generic;



public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public int maxEnemiesAlive = 5;
    public bool spawnEnabled = false;
    
    [Header("Spawn Conditions")]
    public bool spawnOnStart = false;
    public bool stopWhenPowerRestored = true;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip spawnSound;
    
    [Header("Effects")]
    public GameObject spawnEffect;
    
    private float spawnTimer = 0f;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool powerRestored = false;

    void Start()
    {
        if (spawnOnStart)
        {
            EnableSpawning();
        }
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!spawnEnabled) return;
        if (powerRestored && stopWhenPowerRestored) return;
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0) return;

        spawnedEnemies.RemoveAll(enemy => enemy == null);

        if (spawnedEnemies.Count >= maxEnemiesAlive) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        if (spawnPoint == null)
        {
            Debug.LogWarning("[EnemySpawner] Spawn point is null!");
            return;
        }

        if (spawnEffect != null)
        {
            Instantiate(spawnEffect, spawnPoint.position, Quaternion.identity);
        }

        if (audioSource != null && spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedEnemies.Add(enemy);

        Debug.Log($"[EnemySpawner] Spawned enemy at {spawnPoint.name}. Total alive: {spawnedEnemies.Count}");
    }

    public void EnableSpawning()
    {
        spawnEnabled = true;
        spawnTimer = 0f;
        Debug.Log("[EnemySpawner] Spawning enabled!");
    }

    public void DisableSpawning()
    {
        spawnEnabled = false;
        Debug.Log("[EnemySpawner] Spawning disabled!");
    }

    public void OnPowerRestored()
    {
        powerRestored = true;
        
        if (stopWhenPowerRestored)
        {
            DisableSpawning();
            Debug.Log("[EnemySpawner] Power restored - spawning stopped!");
        }
    }

    public void KillAllEnemies()
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        
        spawnedEnemies.Clear();
        Debug.Log("[EnemySpawner] All enemies destroyed!");
    }

    public int GetAliveEnemyCount()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);
        return spawnedEnemies.Count;
    }

    void OnDrawGizmos()
    {
        if (spawnPoints == null) return;

        Gizmos.color = spawnEnabled ? Color.red : Color.gray;
        
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 1f);
                Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + spawnPoint.forward * 2f);
            }
        }
    }
}
