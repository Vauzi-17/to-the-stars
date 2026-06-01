using UnityEngine;

public class ShootingStarSpawner : MonoBehaviour
{
    public GameObject shootingStarPrefab;
    public Transform player;

    [Header("Spawn Settings")]
    public float minInterval = 2f;
    public float maxInterval = 5f;
    public float spawnRangeX = 10f;
    public float spawnOffsetY = 6f;
    public float activeFromY = 8f;
    public int minCount = 1;  // minimal spawn sekaligus
    public int maxCount = 3;  // maksimal spawn sekaligus

    private float timer;

    void Start()
    {
        timer = Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        if (player.position.y < activeFromY) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            int count = Random.Range(minCount, maxCount + 1);
            for (int i = 0; i < count; i++)
                Spawn();
            timer = Random.Range(minInterval, maxInterval);
        }
    }

    void Spawn()
    {
        float randomX = player.position.x + Random.Range(-spawnRangeX, spawnRangeX);
        float spawnY = Camera.main.transform.position.y + spawnOffsetY;
        Vector3 pos = new Vector3(randomX, spawnY, 0);
        Instantiate(shootingStarPrefab, pos, Quaternion.identity);
    }
}