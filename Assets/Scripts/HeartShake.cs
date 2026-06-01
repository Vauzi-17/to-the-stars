using UnityEngine;
using System.Collections;

public class HeartShake : MonoBehaviour
{
    public float shakeDuration = 0.4f;
    public float shakeMagnitude = 15f;

    private Vector3 originalPos;
    private RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        originalPos = rt.anchoredPosition;
    }

    public void Shake()
    {
        StopAllCoroutines(); // stop kalau masih shake
        StartCoroutine(DoShake());
    }

    IEnumerator DoShake()
    {
        originalPos = rt.anchoredPosition;
        Debug.Log("Original pos: " + originalPos);

        float t = 0;
        while (t < shakeDuration)
        {
            t += Time.deltaTime;
            float x = originalPos.x + Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = originalPos.y + Random.Range(-shakeMagnitude, shakeMagnitude);
            rt.anchoredPosition = new Vector2(x, y);
            Debug.Log("Current pos: " + rt.anchoredPosition);
            yield return null;
        }
        rt.anchoredPosition = originalPos;
    }
}