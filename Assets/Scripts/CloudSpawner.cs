using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;
    public Transform player;
    public float spawnInterval = 3f;
    public float spawnAheadY = 10f;  // spawn berapa unit di atas player
    public float spawnRangeX = 6f;   // sebaran horizontal
    public GameObject coinPrefab;

    private float nextSpawnY;

    void Start()
    {
        nextSpawnY = player.position.y + 2f;
    }

    void Update()
    {
        // Spawn kalau player udah deket
        if (player.position.y + spawnAheadY >= nextSpawnY)
        {
            SpawnCloud();
            nextSpawnY += spawnInterval;
        }
    }

    void SpawnCloud()
    {
        // Pakai posisi kamera, bukan player
        float camX = Camera.main.transform.position.x;
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);

        Vector3 spawnPos = new Vector3(
            camX + randomX,
            nextSpawnY,
            0
        );

        GameObject cloud = Instantiate(cloudPrefab, spawnPos, Quaternion.identity);
        CloudPlatform cp = cloud.GetComponent<CloudPlatform>();

        float rand = Random.value;
        if (rand < 0.4f)
            cp.movesHorizontal = false;
        else if (rand < 0.8f)
        {
            cp.movesHorizontal = true;
            cp.moveSpeed = Random.Range(0.5f, 2f);
            cp.moveRange = Random.Range(1f, 3f);
        }
        else
            cp.isCrumbling = true;

        if (coinPrefab != null && Random.value > 0.5f)
        {
            Vector3 coinPos = spawnPos + Vector3.up * 1f;
            Instantiate(coinPrefab, coinPos, Quaternion.identity);
        }
    }
}