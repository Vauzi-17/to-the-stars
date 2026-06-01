using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image overlay;
    public float duration = 1f;

    void Start()
    {
        // Fade in saat scene load
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        overlay.color = new Color(0, 0, 0, 1);
        while (t > 0)
        {
            t -= Time.deltaTime / duration;
            overlay.color = new Color(0, 0, 0, t);
            yield return null;
        }
        overlay.color = new Color(0, 0, 0, 0);
    }
}