using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private float minTimeSpawnInterval = 2f;
    [SerializeField] private float maxTimeSpawnInterval = 4f;
    [SerializeField] private float firstSpawnDelay = 0f;
    [SerializeField] private Transform spawnPosition;

    private float spawnTimer;

    void Start()
    {
        // Initialize the spawn timer with the first spawn delay
        spawnTimer = firstSpawnDelay;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            spawnTimer -= Time.deltaTime; // Decrease timer by time elapsed

            // If the timer reaches zero, spawn the obstacle and reset the timer
            if (spawnTimer <= 0f)
            {
                SpawnObstacle();
                spawnTimer = Random.Range(minTimeSpawnInterval, maxTimeSpawnInterval); // Reset the timer with a new random interval
            }
        }
        else
        {
            // If the game is not playing (paused or game over), stop spawning
            spawnTimer = firstSpawnDelay; // Reset timer to prevent spawning while not playing
        }
    }

    private void SpawnObstacle()
    {
        // Spawn an obstacle at the specified spawn position
        EnumBasedPoolManager.Instance.SpawnRandom(spawnPosition.position);
    }
}
