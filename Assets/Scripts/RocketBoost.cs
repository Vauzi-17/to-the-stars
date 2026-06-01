using UnityEngine;
using System.Collections;

public class RocketBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostForce = 20f;
    public float boostDuration = 2f;
    public float cooldownTime = 8f;

    [Header("Rocket Visual")]
    public GameObject rocketObject;

    // [MODIFIKASI AUDIO]
    [Header("Audio")]
    public AudioClip rocketSound;
    public AudioSource rocketAudioSource; // Kita pakai AudioSource khusus roket
    public float fadeDuration = 0.5f;     // Durasi suara mengecil (dalam detik)

    private bool isBoosting = false;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;
    private Rigidbody2D rb;
    private PlayerController pc;

    public static float CooldownProgress = 1f;
    private float originalVolume = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();

        if (rocketAudioSource != null)
        {
            // Simpan volume asli supaya setelah di-fade bisa dikembalikan
            originalVolume = rocketAudioSource.volume;
        }

        if (rocketObject != null)
            rocketObject.SetActive(false);
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            CooldownProgress = 1f - (cooldownTimer / cooldownTime);
            if (cooldownTimer <= 0)
            {
                isOnCooldown = false;
                CooldownProgress = 1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.J) && !isBoosting && !isOnCooldown)
        {
            StartCoroutine(DoBoost());
        }
    }

    IEnumerator DoBoost()
    {
        isBoosting = true;
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
        CooldownProgress = 0f;

        if (rocketObject != null)
            rocketObject.SetActive(true);

        // [MODIFIKASI AUDIO] Mainkan suara roket bukan dengan OneShot
        if (rocketSound != null && rocketAudioSource != null)
        {
            rocketAudioSource.volume = originalVolume; // Pastikan volume penuh
            rocketAudioSource.clip = rocketSound;
            rocketAudioSource.Play();
        }

        float elapsed = 0f;
        while (elapsed < boostDuration)
        {
            elapsed += Time.deltaTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, boostForce);
            yield return null;
        }

        if (rocketObject != null)
            rocketObject.SetActive(false);

        // [MODIFIKASI AUDIO] Mulai proses suara mengecil perlahan
        if (rocketAudioSource != null && rocketAudioSource.isPlaying)
        {
            StartCoroutine(FadeOutAudio());
        }

        isBoosting = false;
    }

    // Coroutine untuk memudarkan suara roket
    IEnumerator FadeOutAudio()
    {
        float startVolume = rocketAudioSource.volume;

        while (rocketAudioSource.volume > 0)
        {
            // Kurangi volume secara bertahap berdasarkan waktu
            rocketAudioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        rocketAudioSource.Stop(); // Matikan audio sepenuhnya
        rocketAudioSource.volume = originalVolume; // Kembalikan volume seperti semula untuk peluncuran roket berikutnya
    }
}