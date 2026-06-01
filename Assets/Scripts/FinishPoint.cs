using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class FinishPoint : MonoBehaviour
{
    [Header("Proximity Hint")]
    public float hintTriggerRadius = 6f;

    [Header("Cinemachine")]
    public CinemachineCamera vcamPlayer;   // Assign: VCam_Player
    public CinemachineCamera vcamFinish;   // Assign: VCam_Finish

    [Header("Timing")]
    public float holdDuration = 2f;

    private bool hintTriggered = false;
    private bool levelComplete = false;
    private Transform player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        if (vcamFinish != null)
            vcamFinish.Priority = 5;
    }

    void Update()
    {
        if (hintTriggered || levelComplete || player == null) return;

        float dist = Vector2.Distance(player.position, transform.position);
        if (dist <= hintTriggerRadius)
        {
            hintTriggered = true;
            StartCoroutine(DoFinishHint());
        }
    }

    IEnumerator DoFinishHint()
    {
        // Switch VCam, game masih jalan
        if (vcamFinish != null) vcamFinish.Priority = 20;
        FinishHintUI.Instance?.Show();

        // Tunggu blend selesai dulu (game masih jalan)
        yield return new WaitForSeconds(2f);

        // Baru pause saat sudah di finish point
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(holdDuration);

        // Resume, blend balik
        Time.timeScale = 1f;
        if (vcamFinish != null) vcamFinish.Priority = 5;

        yield return new WaitForSeconds(2f);
        FinishHintUI.Instance?.Hide();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !levelComplete)
        {
            levelComplete = true;
            FinishHintUI.Instance?.Hide();
            UnlockNewLevel();

            // Cek apakah ini level terakhir
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextIndex >= SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene("MainMenu"); // sesuaikan nama scene main menu kamu
            else
                SceneManager.LoadScene(nextIndex);
        }
    }

    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hintTriggerRadius);
    }
}