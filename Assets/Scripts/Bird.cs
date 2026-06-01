using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed = 8f;
    public float destroyDistance = 3f;
    public float speedVariance = 3f;

    private Transform player;
    private int direction = 1;
    private bool hasHit = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        direction = Random.value > 0.5f ? 1 : -1;

        transform.position = new Vector3(
            player.position.x + (direction * -15f),
            transform.position.y,
            0
        );

        // Flip pakai scale, lebih proper untuk animated sprite
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -direction;
        transform.localScale = scale;
    }

    void Update()
    {
        // Terbang horizontal
        transform.position += Vector3.right * speed * direction * Time.deltaTime;

        // Destroy kalau sudah jauh
        if (Mathf.Abs(transform.position.x - player.position.x) > destroyDistance)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !hasHit)
        {
            hasHit = true;
            PlayerController pc = col.GetComponent<PlayerController>();
            if (pc != null)
            {
                Vector2 knockback = new Vector2(direction * 10f, 4f);
                pc.ApplyKnockback(knockback);
            }
            HealthManager.instance.TakeDamage();

            // Reset setelah 1 detik biar bisa hit lagi kalau player masih kena
            Invoke(nameof(ResetHit), 1f);
        }
    }

    void ResetHit()
    {
        hasHit = false;
    }
}