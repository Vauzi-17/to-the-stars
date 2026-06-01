using UnityEngine;

public class MoonParallax : MonoBehaviour
{
    [Header("Parallax")]
    public float parallaxSpeedX = 0.05f;
    public float parallaxSpeedY = 0.1f; // lebih lambat dari player = ketinggalan

    [Header("Fade")]
    public float fadeStartY = 5f;   // mulai fade di ketinggian ini
    public float fadeEndY = 20f;    // hilang total di ketinggian ini

    private Transform cam;
    private Vector3 lastCamPos;
    private SpriteRenderer sr;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Parallax
        Vector3 delta = cam.position - lastCamPos;
        transform.position += new Vector3(
            delta.x * parallaxSpeedX,
            delta.y * parallaxSpeedY,
            0
        );
        lastCamPos = cam.position;

        // Fade berdasarkan ketinggian kamera
        if (sr != null)
        {
            float alpha = 1f - Mathf.InverseLerp(fadeStartY, fadeEndY, cam.position.y);
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}