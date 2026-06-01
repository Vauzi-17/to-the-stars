using UnityEngine;
using System.Collections;

public class ShootingStar : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 15f;
    public float angle = -45f; // diagonal ke bawah kanan

    [Header("Fade")]
    public float lifetime = 1.5f;

    private TrailRenderer trail;
    private SpriteRenderer sr;
    private Vector2 direction;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();

        // Random arah - kiri ke kanan atau kanan ke kiri
        float randomAngle = Random.value > 0.5f ?
            Random.Range(-60f, -30f) :   // kiri ke kanan
            Random.Range(-150f, -120f);  // kanan ke kiri

        float rad = randomAngle * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

        StartCoroutine(FadeAndDestroy());
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(lifetime * 0.7f);

        // Fade out
        float t = 0;
        Color startColor = trail.startColor;
        Color endColor = trail.endColor;

        while (t < 1f)
        {
            t += Time.deltaTime / (lifetime * 0.3f);
            Color sc = startColor;
            Color ec = endColor;
            sc.a = Mathf.Lerp(1, 0, t);
            ec.a = Mathf.Lerp(1, 0, t);
            trail.startColor = sc;
            trail.endColor = ec;
            yield return null;
        }

        Destroy(gameObject);
    }
}