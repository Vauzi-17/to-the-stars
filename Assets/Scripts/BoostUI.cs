using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoostUI : MonoBehaviour
{
    [Header("UI References")]
    public Image cooldownRadial;    // Image type: Filled, radial 360
    public Image iconImage;
    public TextMeshProUGUI labelText;
    public TextMeshProUGUI cooldownText;

    void Start()
    {
        labelText.text = "Press J to Boost";
        cooldownRadial.fillAmount = 1f;
    }

    void Update()
    {
        cooldownRadial.fillAmount = RocketBoost.CooldownProgress;

        if (RocketBoost.CooldownProgress >= 1f)
        {
            cooldownText.gameObject.SetActive(false);
            iconImage.color = Color.white; // icon normal
        }
        else
        {
            // Tampilkan sisa cooldown
            cooldownText.gameObject.SetActive(true);
            // Hitung dari progress
            cooldownText.color = Color.white;
            iconImage.color = new Color(0.5f, 0.5f, 0.5f, 1f); // icon gelap saat cooldown
        }
    }
}