using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Coin : MonoBehaviour
{
    [Header("Collect Effect")]
    public float floatUpSpeed = 2f;
    public float fadeDuration = 0.5f;

    [Header("Audio")]
    public AudioClip coinSFX;
    public AudioMixerGroup sfxMixerGroup; // ← drag SFX group dari Audio Mixer
    public float sfxVolume = 1f;

    private bool collected = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (collected) return;
        if (col.CompareTag("Player"))
        {
            collected = true;
            CoinManager.instance.AddCoin();

            if (coinSFX != null)
                PlaySFX();

            StartCoroutine(CollectEffect());
        }
    }

    void PlaySFX()
    {
        // Buat temporary GameObject untuk play audio
        GameObject tempAudio = new GameObject("CoinSFX");
        tempAudio.transform.position = transform.position;

        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        audioSource.clip = coinSFX;
        audioSource.outputAudioMixerGroup = sfxMixerGroup; // ← masuk ke grup SFX
        audioSource.volume = sfxVolume;
        audioSource.Play();

        // Auto destroy setelah selesai
        Destroy(tempAudio, coinSFX.length);
    }

    IEnumerator CollectEffect()
    {
        GetComponent<Collider2D>().enabled = false;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 startPos = transform.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            transform.position = Vector3.Lerp(
                startPos,
                startPos + Vector3.up * floatUpSpeed,
                t
            );
            Color c = sr.color;
            c.a = Mathf.Lerp(1, 0, t);
            sr.color = c;
            yield return null;
        }

        Destroy(gameObject);
    }
}