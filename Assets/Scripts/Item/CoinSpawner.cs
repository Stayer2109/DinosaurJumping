using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private float minIntervalSpawnTime = 1f;
    [SerializeField] private float maxIntervalSpawnTime = 3f;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject coinPrefab;

    private float spawnTimer;

    void Start()
    {
        // Initialize the spawn timer with a random interval
        spawnTimer = Random.Range(minIntervalSpawnTime, maxIntervalSpawnTime);
    }

    void Update()
    {
        // Only handle spawning if the game is playing
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            spawnTimer -= Time.deltaTime; // Decrease timer by time elapsed

            // If the timer reaches zero, spawn the coin and reset the timer
            if (spawnTimer <= 0f)
            {
                SpawnCoin();
                spawnTimer = Random.Range(minIntervalSpawnTime, maxIntervalSpawnTime);
            }
        }
        else
        {
            // Stop spawning when the game is paused (no need to reset timer here)
            spawnTimer = Random.Range(minIntervalSpawnTime, maxIntervalSpawnTime); // Optional: reset the timer to prevent unwanted spawn
        }
    }

    private void SpawnCoin()
    {
        // Instantiate the coin at the specified spawn position
        Instantiate(coinPrefab, spawnPosition.position, Quaternion.identity);
    }
}
