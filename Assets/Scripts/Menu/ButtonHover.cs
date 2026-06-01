using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float hoverScale = 1.1f;
    public float clickScale = 0.95f;
    public float duration = 0.1f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * hoverScale));
    }

    public void OnPointerExit(PointerEventData e)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));
    }

    public void OnPointerClick(PointerEventData e)
    {
        StopAllCoroutines();
        StartCoroutine(ClickEffect());
    }

    IEnumerator ScaleTo(Vector3 target)
    {
        Vector3 start = transform.localScale;
        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration; // ← ganti ini
            transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }

    IEnumerator ClickEffect()
    {
        yield return StartCoroutine(ScaleTo(originalScale * clickScale));
        yield return StartCoroutine(ScaleTo(originalScale));
    }
}