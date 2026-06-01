using UnityEngine;
using TMPro;

public class FinishHintUI : MonoBehaviour
{
    public static FinishHintUI Instance;

    public GameObject panel;               // Parent object yang di-hide/show
    public TextMeshProUGUI messageText;    // "Finish ada di atas!"

    void Awake()
    {
        Instance = this;
        if (panel != null) panel.SetActive(false);
    }

    public void Show()
    {
        if (panel != null) panel.SetActive(true);
        if (messageText != null) messageText.text = "Finish ada di atas! ⬆";
    }

    public void Hide()
    {
        if (panel != null) panel.SetActive(false);
    }
}