using UnityEngine;

public class CloudLayer : MonoBehaviour
{
    [Header("Parallax")]
    public float parallaxSpeedY = 0.3f;
    public float parallaxSpeedX = 0.05f;
    public float offsetY = 5f; // berapa unit di atas kamera

    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        // Posisikan di atas kamera saat start
        transform.position = new Vector3(
            cam.position.x,
            cam.position.y + offsetY,
            transform.position.z
        );
    }

    void LateUpdate()
    {
        // Ikut kamera X penuh, Y lebih lambat (parallax)
        transform.position = new Vector3(
            cam.position.x,
            cam.position.y + offsetY - (cam.position.y * (1f - parallaxSpeedY)),
            transform.position.z
        );
    }
}