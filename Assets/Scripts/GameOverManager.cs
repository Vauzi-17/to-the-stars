using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("References")]
    public GameObject gameOverPanel;
    public Image fadeImage;          // Panel image untuk fade
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public Button mainMenuButton;

    [Header("Settings")]
    public float fadeDuration = 1f;
    public string mainMenuScene = "MainMenu";

    private bool isGameOver = false;

    void Start()
    {
        // Pastikan panel mati di awal
        gameOverPanel.SetActive(false);

        // Setup button listeners
        retryButton.onClick.AddListener(Retry);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Freeze player
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.enabled = false;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }

        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        gameOverPanel.SetActive(true);
        Image panelImage = gameOverPanel.GetComponent<Image>();

        // Sembunyikan text dan button pakai CanvasGroup
        CanvasGroup textCG = gameOverText.GetComponent<CanvasGroup>();
        if (textCG == null) textCG = gameOverText.gameObject.AddComponent<CanvasGroup>();
        textCG.alpha = 0;

        CanvasGroup retryCG = retryButton.GetComponent<CanvasGroup>();
        CanvasGroup mainMenuCG = mainMenuButton.GetComponent<CanvasGroup>();
        retryCG.alpha = 0;
        mainMenuCG.alpha = 0;

        // Fade in panel hitam
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            panelImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 0.85f, t));
            yield return null;
        }

        yield return StartCoroutine(FadeInCanvasGroup(textCG, 0.5f));
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(FadeInCanvasGroup(retryCG, 0.4f));
        yield return StartCoroutine(FadeInCanvasGroup(mainMenuCG, 0.4f));
    }

    IEnumerator FadeInText(TextMeshProUGUI text, float duration)
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            text.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
    }

    IEnumerator FadeInCanvasGroup(CanvasGroup cg, float duration)
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            cg.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
    }

    void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}