using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    [Header("Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI")]
    public GameObject[] heartIcons; // array icon hati di UI
    public RectTransform heartContainer; // drag GameObject "Heart" ke sini

    private GameOverManager gameOverManager;

    void Awake()
    {
        instance = this;
        currentHealth = maxHealth;
    }

    void Start()
    {
        gameOverManager = FindObjectOfType<GameOverManager>();
        UpdateHeartUI();
    }

    // Di HealthManager.cs
    public void TakeDamage()
    {
        int heartToShake = currentHealth - 1;
        if (heartToShake >= 0)
        {
            // Shake parent Heart container
            HeartShake hs = heartContainer.GetComponent<HeartShake>();
            if (hs != null) hs.Shake();
        }

        currentHealth--;
        UpdateHeartUI();

        if (currentHealth <= 0)
            gameOverManager.TriggerGameOver();
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].SetActive(i < currentHealth);
        }
    }
}