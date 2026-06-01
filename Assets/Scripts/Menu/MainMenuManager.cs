using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Transition")]
    public Image transitionOverlay;
    public float transitionDuration = 1f;

    [Header("Title Animation")]
    public RectTransform titleText;
    public float titleBobSpeed = 1f;
    public float titleBobAmount = 10f;

    private Vector3 titleStartPos;

    void Start()
    {
        titleStartPos = titleText.anchoredPosition;
        ShowPanel(mainPanel);

        // Fade in saat menu muncul
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        // Title bob up down
        if (titleText != null)
        {
            float y = titleStartPos.y + Mathf.Sin(Time.time * titleBobSpeed) * titleBobAmount;
            titleText.anchoredPosition = new Vector2(titleStartPos.x, y);
        }
    }

    // Button functions
    public void PlayGame()
    {
        StartCoroutine(TransitionToScene("SampleScene"));
    }

    public void OpenSettings()
    {
        ShowPanel(settingsPanel);
    }

    public void OpenCredits()
    {
        ShowPanel(creditsPanel);
    }

    public void BackToMain()
    {
        ShowPanel(mainPanel);
    }

    public void QuitGame()
    {
        StartCoroutine(QuitSequence());
    }

    void ShowPanel(GameObject panel)
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        panel.SetActive(true);
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        // Fade out
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0)
        {
            t -= Time.deltaTime / transitionDuration;
            transitionOverlay.color = new Color(0, 0, 0, t);
            yield return null;
        }
        transitionOverlay.color = new Color(0, 0, 0, 0);
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / transitionDuration;
            transitionOverlay.color = new Color(0, 0, 0, t);
            yield return null;
        }
    }

    IEnumerator QuitSequence()
    {
        yield return StartCoroutine(FadeOut());
        Application.Quit();
    }
}