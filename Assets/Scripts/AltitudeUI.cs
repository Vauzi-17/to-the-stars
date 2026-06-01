using UnityEngine;
using TMPro;

public class AltitudeUI : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI altitudeText;

    [Header("Settings")]
    public float groundY = -1.5f;      // posisi Y spawn player
    public float metersPerUnit = 10f;  // 1 unity unit = 10 meter

    private float maxAltitude = 0f;

    void Update()
    {
        float altitude = (player.position.y - groundY) * metersPerUnit;
        if (altitude < 0) altitude = 0;

        // Simpan ketinggian tertinggi
        if (altitude > maxAltitude)
            maxAltitude = altitude;

        altitudeText.text = $"↑ {altitude:F0}m";
    }
}