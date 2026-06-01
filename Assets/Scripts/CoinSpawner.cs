using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform player;
    public float spawnAheadY = 8f;
    public float spawnRangeX = 5f;
    public float spawnInterval = 3f;

    private float nextSpawnY;

    void Start()
    {
        nextSpawnY = player.position.y + spawnAheadY;
    }

    void Update()
    {
        if (player.position.y + spawnAheadY >= nextSpawnY)
        {
            // Spawn 2-4 coin sekaligus bergerombol
            int count = Random.Range(2, 5);
            for (int i = 0; i < count; i++)
            {
                float randomX = player.position.x + Random.Range(-spawnRangeX, spawnRangeX);
                float randomY = nextSpawnY + Random.Range(-1f, 1f);
                Instantiate(coinPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
            }
            nextSpawnY += spawnInterval;
        }
    }
}