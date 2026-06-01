using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Parallax Settings")]
    public float parallaxSpeedX = 0.2f;  // horizontal follow
    public float parallaxSpeedY = 0.1f;  // vertical follow (penting untuk naik!)
    public bool infiniteScrollX = true;  // loop horizontal

    private Transform cam;
    private Vector3 lastCamPos;
    private float textureUnitSizeX;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;

        // Untuk infinite scroll
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        Vector3 deltaMove = cam.position - lastCamPos;

        // Gerak parallax
        transform.position += new Vector3(
            deltaMove.x * parallaxSpeedX,
            deltaMove.y * parallaxSpeedY,
            0
        );

        lastCamPos = cam.position;

        // Loop horizontal supaya tidak habis
        if (infiniteScrollX)
        {
            if (Mathf.Abs(cam.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetX = (cam.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(
                    cam.position.x + offsetX,
                    transform.position.y,
                    transform.position.z
                );
            }
        }
    }
}