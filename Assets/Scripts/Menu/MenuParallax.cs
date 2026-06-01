using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layer;
        public float speedX = 1f;
        public float speedY = 0.5f;
    }

    [Header("Parallax")]
    public ParallaxLayer[] layers;

    [Header("Character Float")]
    public Transform characterTransform;
    public float floatSpeed = 1f;
    public float floatAmount = 0.2f;

    private Vector3 charStartPos;

    void Start()
    {
        if (characterTransform != null)
            charStartPos = characterTransform.position;
    }

    void Update()
    {
        // Mouse parallax
        float mouseX = (Input.mousePosition.x / Screen.width) - 0.5f;
        float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;

        foreach (var l in layers)
        {
            if (l.layer == null) continue;
            Vector3 target = new Vector3(
                mouseX * l.speedX,
                mouseY * l.speedY,
                l.layer.position.z
            );
            l.layer.position = Vector3.Lerp(
                l.layer.position,
                target,
                Time.deltaTime * 3f
            );
        }

        // Character float
        if (characterTransform != null)
        {
            float y = charStartPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            characterTransform.position = new Vector3(
                characterTransform.position.x,
                y,
                characterTransform.position.z
            );
        }
    }
}