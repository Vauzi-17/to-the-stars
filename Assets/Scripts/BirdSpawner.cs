using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public Transform player;
    public float spawnInterval = 4f;
    public float spawnAheadY = 6f;
    public float minHeight = 10f; // burung mulai muncul setelah ketinggian ini

    private float nextSpawnY;
    private float timer;

    void Start()
    {
        nextSpawnY = minHeight;
        timer = spawnInterval;
    }

    void Update()
    {
        // Burung hanya spawn kalau player sudah cukup tinggi
        if (player.position.y < minHeight) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnBird();
            timer = spawnInterval;
        }
    }

    void SpawnBird()
    {
        float spawnY = player.position.y + Random.Range(-2f, spawnAheadY);
        Vector3 spawnPos = new Vector3(player.position.x, spawnY, 0);

        Instantiate(birdPrefab, spawnPos, Quaternion.identity);
    }
}