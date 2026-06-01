using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlHintUI : MonoBehaviour
{
    public static ControlHintUI Instance;

    [Header("Panel")]
    public GameObject panel;

    [Header("Toggle Key")]
    public KeyCode toggleKey = KeyCode.Tab;

    [Header("Auto Hide")]
    public float autoHideDelay = 5f;   // Berapa detik sebelum fade out otomatis

    [Header("Fade")]
    public CanvasGroup canvasGroup;    // Attach CanvasGroup ke ControlHintPanel

    private bool isVisible = true;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        Instance = this;

        // Pastikan CanvasGroup ada
        if (canvasGroup == null)
            canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = panel.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isVisible = false;

        Show();
        fadeCoroutine = StartCoroutine(AutoHide());
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            Debug.Log("Tab pressed, isVisible: " + isVisible);
            if (isVisible)
                Hide();
            else
                Show();
        }
    }

    public void Show()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        isVisible = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        fadeCoroutine = StartCoroutine(FadeTo(1f, 0.3f));
    }

    public void Hide()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        isVisible = false;
        fadeCoroutine = StartCoroutine(FadeOutThenDisable());
    }


    IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(autoHideDelay);
        Hide();
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    IEnumerator FadeOutThenDisable()
    {
        yield return StartCoroutine(FadeTo(0f, 0.5f));
        // Hapus panel.SetActive(false)
        // Cukup alpha = 0, script tetap aktif
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}