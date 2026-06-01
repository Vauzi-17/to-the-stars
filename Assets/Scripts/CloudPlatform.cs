using UnityEngine;

public class CloudPlatform : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1f;
    public float moveRange = 2f;
    public bool movesHorizontal = true;

    [Header("Type")]
    public bool isCrumbling = false;
    public float crumbleDelay = 0.5f;

    [Header("Drift")]
    public float driftSpeed = 0.1f;

    [Header("Spawn Effect")]
    public float spawnDuration = 0.4f;

    private Vector3 startPos;
    private SpriteRenderer sr;
    private bool isFalling = false;

    private Vector3 lastPos;
    private Rigidbody2D playerRb;
    private bool playerOnCloud = false;

    void Start()
    {
        startPos = transform.position;
        lastPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(SpawnEffect());
    }

    System.Collections.IEnumerator SpawnEffect()
    {
        // Mulai dari kecil dan transparan
        Vector3 originalScale = transform.localScale;
        transform.localScale = Vector3.zero;

        Color c = sr.color;
        c.a = 0;
        sr.color = c;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / spawnDuration;

            // Scale up dengan efek bounce
            transform.localScale = Vector3.Lerp(
                Vector3.zero,
                originalScale,
                Mathf.SmoothStep(0, 1, t)
            );

            // Fade in
            c.a = Mathf.Lerp(0, 1, t);
            sr.color = c;

            yield return null;
        }

        transform.localScale = originalScale;
        c.a = 1;
        sr.color = c;
    }

    void Update()
    {
        if (isFalling) return;

        // Drift selalu jalan (semua cloud)
        transform.position += Vector3.left * driftSpeed * Time.deltaTime;

        // Hanya cloud yang movesHorizontal
        if (movesHorizontal)
        {
            float x = startPos.x + Mathf.Sin(Time.time * moveSpeed) * moveRange;
            Vector3 newPos = new Vector3(x, transform.position.y, 0);
            float deltaX = newPos.x - transform.position.x;
            transform.position = newPos;

            if (playerOnCloud && playerRb != null)
            {
                PlayerController pc = playerRb.GetComponent<PlayerController>();
                if (pc != null)
                    pc.platformVelocityX = deltaX / Time.deltaTime;
            }
        }

        // Destroy check selalu jalan (semua cloud)
        if (Camera.main != null)
        {
            float distY = Camera.main.transform.position.y - transform.position.y;
            float distX = Mathf.Abs(Camera.main.transform.position.x - transform.position.x);
            if (distY > 20f || distX > 25f)
                Destroy(gameObject);
        }

        lastPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {      
            playerRb = col.gameObject.GetComponent<Rigidbody2D>();
            playerOnCloud = true;
            if (isCrumbling) StartCoroutine(Crumble());
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // Reset platform velocity saat player lompat
            PlayerController pc = col.gameObject.GetComponent<PlayerController>();
            if (pc != null)
                pc.platformVelocityX = 0f;

            playerRb = null;
            playerOnCloud = false;
        }
    }

    System.Collections.IEnumerator Crumble()
    {
        isFalling = true;

        for (int i = 0; i < 4; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(crumbleDelay);

        // Tambah Rigidbody2D saat runtime biar jatuh
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 2f;

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}